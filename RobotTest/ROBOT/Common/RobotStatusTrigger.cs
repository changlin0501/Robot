using System;
using System.Collections.Generic;
using System.Text;

using System.Threading;
using KRcc;
using Microsoft.AspNetCore.Http;
using NLog.Web;


using Newtonsoft.Json;
using ROBOT.Model;
using System.Threading.Tasks;

namespace ROBOT.Common
{
    public class RobotStatusTrigger
    {

        //机器列表
        public List<Model.RobotEntity> MC_AllMachineList;

        //更新
        public delegate void UpdateMachineStatusFunction(string robotID);
        public UpdateMachineStatusFunction UpdateMachineStatusDelegate;

        /// <summary>
        /// 检测机器人状态命令
        /// </summary>
        public string MC_DefaultCheckRobotStatusCommand = Common.ConfigFileHandler.GetAppConfig("CheckRobotStatusCommand");


        /// <summary>
        /// 检测机器人状态间隔时间（毫秒）
        /// </summary>
        public int MC_DefaultCheckRobotStatusIntervalMilliseconds = int.Parse(Common.ConfigFileHandler.GetAppConfig("CheckRobotStatusIntervalMilliseconds"));


        List<Thread> threads = new List<Thread>();

        /// <summary>
        /// 实例化
        /// </summary>
        /// <param name="robotEntities"></param>
        public RobotStatusTrigger(List<Model.RobotEntity> robotEntities)
        {
            if (string.IsNullOrEmpty(MC_DefaultCheckRobotStatusCommand)) throw new Exception("未知检测机器人状态命令[SWITCH ???]！");

            this.MC_AllMachineList = robotEntities;
        }

        /// <summary>
        /// 开始监控
        /// </summary>
        /// <returns></returns>
        public bool StartTrigger()
        {
            Random rd = new Random();
            int RandomNumber = rd.Next(1000, 2000);
            return StartTrigger(MC_DefaultCheckRobotStatusIntervalMilliseconds + RandomNumber);
        }

        /// <summary>
        /// 开始监控（设定时间）
        /// </summary>
        /// <param name="IntervalMilliseconds"></param>
        /// <returns></returns>
        public bool StartTrigger(int IntervalMilliseconds)
        {
            try
            {

                // 读取指定位置的配置文件
                var logger = NLogBuilder.ConfigureNLog("XmlConfig/nlog.config").GetCurrentClassLogger();
                if (MC_AllMachineList == null || MC_AllMachineList.Count == 0) return false;

                //循环所有机器人创建线程进行监控
                foreach (var item in MC_AllMachineList)
                {
                    string robotID = item.id;

                    var t = new Thread(new ThreadStart(() =>
                    {
                        //一直运行
                        while (true)
                        {
                            Thread.Sleep(IntervalMilliseconds);

                            KRcc.Commu commu = null;
                            try
                            {
                                //默认状态
                                item.Status = Common.Enum.Robot.RobotStatus.Default;


                                commu = Common.RobotOperationHandler.GetCommu(item.IPAddress, "TCP", "23");

                              


                                if (Common.RobotOperationHandler.CheckCommu(commu))
                                {
                                    //解析状态
                                    string commandResult = Common.RobotOperationHandler.RunCommand(commu, "SWITCH REPEAT,CS,POWER,EMERGENCY,ERROR");

                                    //连接状态
                                    item.Status = Common.Enum.Robot.RobotStatus.Link;
                                    //var redisManger = new RedisManagerPool("127.0.0.1:6379");
                                    //var _redis = redisManger.GetClient();
                                    //var redisTodos = _redis.As<Model.RobotStatus>();
                                    //var newTodo = new RobotStatus
                                    //{

                                    //    id = item.id,
                                    //    IPAddress = item.IPAddress,
                                    //    Status = item.Status,
                                    //    DateTime = DateTime.Now.Date
                                    //};
                                    //redisTodos.Store(newTodo);
                                
                                }

                            }
                            catch (Exception ex)
                            {
                                //错误状态
                                item.Status = Common.Enum.Robot.RobotStatus.Error;


                                //foreach (var intm in MC_AllMachineList)
                                //{
                                //    if (item.Status == Common.Enum.Robot.RobotStatus.Error)
                                //    {
                                //        item.IsConnect = false;
                                //        Common.RobotStatusHelper.UpdateRobotMasterEntity(intm);
                                //    }
                                
                                //}

                              
                               
                                logger.Error(ex, "机器人连接错误");
                            }
                            finally
                            {

                                //关闭连接
                                if (Common.RobotOperationHandler.CheckCommu(commu))
                                {
                                    commu.disconnect();
                                }

                                //清除变量
                                if (commu != null) commu.Dispose();

                            }
                        }

                    }))
                    {
                        Name = robotID
                    };


                    t.Start();

                    threads.Add(t);
                }

                return true;
            }
            catch (Exception ex)
            {
                //Common.LogHandler.WriteLog("机器人状态错误 原因是：" + ex.Message, ex);
                throw ex;
            }

        }

        /// <summary>
        /// 停止
        /// </summary>
        /// <returns></returns>
        public bool StopTrigger()
        {
            try
            {
                foreach (var item in threads)
                {
                    //取消线程
                    //item.Abort();

                    //当IsBackground为true的线程会随主线程退出而退出
                    item.IsBackground = true;
                    //item.Join();
                }

                threads.Clear();

                return true;
            }
            catch (Exception ex)
            {
                //Common.LogHandler.WriteLog("机器人线程停止错误 原因是：" + ex.Message, ex);
                throw ex;
            }
        }

        /// <summary>
        /// 机器列表改变
        /// </summary>
        /// <param name="robotEntities"></param>
        public void ChangeMachineList(List<Model.RobotEntity> robotEntities)
        {
            try
            {
                if (StopTrigger())
                {
                    //重设机器人列表
                    this.MC_AllMachineList = robotEntities;

                    //重新开始处理
                    StartTrigger();
                }
            }
            catch (Exception ex)
            {
                // Common.LogHandler.WriteLog("机器人列表改变错误 原因是:" + ex.Message, ex);
                throw ex;
            }
        }



    }
}
