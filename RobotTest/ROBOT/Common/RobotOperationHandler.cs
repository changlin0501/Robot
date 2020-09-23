using System;
using System.Collections.Generic;
using System.Text;

using KRcc;
using System.Collections;
using NLog.Web;

namespace ROBOT.Common
{
    public static class RobotOperationHandler
    {
        public static KRcc.Commu GetCommu(string ip)
        {
            return GetCommu(ip, "TCP");
        }

        public static KRcc.Commu GetCommu(string ip, string protocol)
        {
            return GetCommu(ip, protocol, "23");
        }

        public static KRcc.Commu GetCommu(string ip, string protocol, string port)
        {
            try
            {
                return new Commu(string.Format("{0} {1} {2}", protocol, ip, port));
            }
            catch (Exception ex)
            {
                // 读取指定位置的配置文件
                var logger = NLogBuilder.ConfigureNLog("XmlConfig/nlog.config").GetCurrentClassLogger();
                // Common.LogHandler.logerror.Error(string.Format("连接到机器人错误 {0} {1} {2}", protocol, ip, port), ex);
                logger.Error(string.Format("连接到机器人错误 {0} {1} {2}", protocol, ip, port), ex);
                throw ex;
            }
        }


        /// <summary>
        /// 检查机器人连接是否已经正常
        /// </summary>
        /// <param name="commu"></param>
        /// <returns></returns>
        public static bool CheckCommu(KRcc.Commu commu)
        {
            return commu != null && commu.IsConnected;
        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        /// <param name="commu"></param>
        /// <returns></returns>
        public static bool CloseCommu(KRcc.Commu commu)
        {
            try
            {
                bool result = true;
                if (commu != null && commu.IsConnected)
                {
                    commu.disconnect();
                    commu.Dispose();
                }
                return result;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 运行命令
        /// </summary>
        /// <param name="commu"></param>
        /// <param name="commandString"></param>
        /// <returns></returns>
        public static string RunCommand(KRcc.Commu commu, string commandString)
        {
            try
            {
                //默认回车
                ArrayList arrayList = commu.command(commandString + "\n");
                int returnStatusCode = 0;
                int.TryParse(arrayList[0].ToString(), out returnStatusCode);

                if (returnStatusCode == 0) return arrayList[1].ToString();

                return string.Empty;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
