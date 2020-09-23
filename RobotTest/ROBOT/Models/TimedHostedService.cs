using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ROBOT.Models
{
    public class TimedHostedService:IHostedService,IDisposable
    {

        /// <summary>
        /// 所有机器列表
        /// </summary>
        public List<Model.RobotEntity> MC_AllMachineList = new List<Model.RobotEntity>();

        /// <summary>
        /// 默认机器人个数
        /// </summary>
        public int MC_AllMachineCount = 0;

        /// <summary>
        /// 机器人状态检测触发器
        /// </summary>
        public Common.RobotStatusTrigger MC_RobotStatusTrigger;

        /// <summary>
        /// 更新机器状态事件委托
        /// </summary>
        delegate void UpdateMachineStatusDelegate(string robotID);
        private void UpdateMachineStatus(string robotID)
        {
            if (InvokeRequired)
            {
                try
                {
                    //两种写法是一样的
                    UpdateMachineStatusDelegate kmsgCallback = new UpdateMachineStatusDelegate(UpdateMachineStatus);
                    // this.Invoke(kmsgCallback, new object[] { robotID });

                    //两种写法是一样的
                    //this.Invoke(new UpdateMachineStatusDelegate(delegate (string s)
                    //{                        
                    //    ChangeMachineStatusControl(machineNumber);                       
                    //}), machineNumber);
                }
                catch (Exception ex)
                {

                }
            }
            else
            {

                try
                {
                    ChangeMachineStatusControl(robotID);
                }
                catch (Exception ex)
                {

                }
            }
        }
        private int executionCount = 0;
        private readonly ILogger<TimedHostedService> _logger;
        private Timer _timer;

        public bool InvokeRequired { get; private set; }

        public TimedHostedService(ILogger<TimedHostedService> logger)
        {
            _logger = logger;
            StartTimerFunction();
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service running.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(5));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            var count = Interlocked.Increment(ref executionCount);

            _logger.LogInformation(
                "Timed Hosted Service is working. Count: {Count}", count);
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        public void StartTimerFunction()
        {

            if (!Common.CommonFunction.ServiceRunning(Common.MongodbHandler.MongodbServiceName) && Common.ConfigFileHandler.GetAppConfig("CheckMongoDBService") == "1")
            {
                throw new Exception("MongoDB服务未安装或未运行，请关闭应用后并确认MongoDB服务状态再重新启动");
            }
            SettingDefaultRobotList();

            //机器人状态查询触发器
            MC_RobotStatusTrigger = new Common.RobotStatusTrigger(MC_AllMachineList);
           // MC_RobotStatusTrigger.UpdateMachineStatusDelegate += UpdateMachineStatus;
            MC_RobotStatusTrigger.StartTrigger();
        }

        private void SettingDefaultRobotList()
        {
            try
            {
                //读取数据库中的机器列表
                MC_AllMachineList = Common.RobotMasterHelper.GetRobotMasterList();

                //设置完成后，需要设置总数
                MC_AllMachineCount = MC_AllMachineList.Count;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 修改机器状态界面
        /// </summary>
        /// <param name="machineNumber"></param>
        private void ChangeMachineStatusControl(string robotID)
        {
            #region 机器状态

            //通过机器编号找到按钮
            //Button button = Common.CommonFunction.FindMachineStatusControl(this.tableLayoutPanelMachine, robotID) as Button;
            
            

            //找到机器
            Model.RobotEntity robotResult = MC_RobotStatusTrigger.MC_AllMachineList.Find(
                delegate (Model.RobotEntity robotEntity)
                {
                    return robotEntity.id.Equals(robotID);
                });

            //处理连接时文本
            // button.Text = ProcessMachineStatusStringForMain(robotResult);

            //处理颜色


            #endregion
        }

    }
}
