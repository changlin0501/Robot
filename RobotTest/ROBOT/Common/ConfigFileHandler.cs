﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Configuration;

namespace ROBOT.Common
{
   public static class ConfigFileHandler
    {
        /// <summary>
        /// 读取配置节点对应值
        /// </summary>
        /// <param name="strKey"></param>
        /// <returns></returns>
        public static string GetAppConfig(string strKey)
        {
            return ConfigurationManager.AppSettings[strKey].ToString();
        }
    }
}
