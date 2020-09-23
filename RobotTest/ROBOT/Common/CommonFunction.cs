using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace ROBOT.Common
{
    public class CommonFunction
    {

        public enum ServiceStatus
        {
            NoInstall = 0,
            Stopped = 1,
            StartPending = 2,
            StopPending = 3,
            Running = 4,
            ContinuePending = 5,
            PausePending = 6,
            Paused = 7
        }
        /*-------------------------------------------------------------------------------------*/
        /*服务状态*/

        /// <summary>
        /// 确认服务是否在运行
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public static bool ServiceRunning(string serviceName)
        {
            bool returnFlag = true;
            ServiceController[] services = ServiceController.GetServices();
            var service = services.FirstOrDefault(s => s.ServiceName == serviceName);

            if (service == null)
            {
                returnFlag = false;
            }
            else if (service.Status != ServiceControllerStatus.Running)
            {
                returnFlag = false;
            }


            return returnFlag;
        }

        /// <summary>
        /// 获取服务状态
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public static ServiceStatus GetServiceStatus(string serviceName)
        {
            ServiceStatus returnFlag = ServiceStatus.NoInstall;

            ServiceController[] services = ServiceController.GetServices();
            var service = services.FirstOrDefault(s => s.ServiceName == serviceName);

            if (service == null)
            {
                returnFlag = ServiceStatus.NoInstall;
            }
            else
            {
                returnFlag = (ServiceStatus)service.Status;
            }

            return returnFlag;
        }
    }
}
