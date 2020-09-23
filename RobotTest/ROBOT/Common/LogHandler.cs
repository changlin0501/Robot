//using System;
//using System.Collections.Generic;
//using System.Text;

//using System.IO;

//namespace ROBOT.Common
//{
//    public static class LogHandler
//    {
//        public static readonly log4net.ILog loginfo = log4net.LogManager.GetLogger("loginfo");
//        public static readonly log4net.ILog logerror = log4net.LogManager.GetLogger("logerror");


//        /// <summary>
//        /// 写入日志（无异常）
//        /// </summary>
//        /// <param name="info">日志信息</param>
//        public static void WriteLog(string info)
//        {
//            if (loginfo.IsInfoEnabled)
//            {
//                loginfo.Info(info);
//            }
//        }

//        /// <summary>
//        /// 写入日志（有异常）
//        /// </summary>
//        /// <param name="info">日志信息</param>
//        /// <param name="ex">异常实体</param>
//        public static void WriteLog(string info, Exception ex)
//        {
//            if (logerror.IsErrorEnabled)
//            {
//                logerror.Error(info, ex);
//            }
//        }
//    }
//}
