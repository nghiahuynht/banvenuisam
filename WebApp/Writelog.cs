//using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using WebApp.Infrastructure.Utilities;

namespace WebApp
{
    public static class WriteLog
    {
        public static void writeToLogFile(String request)
        {
            try
            {
                string logMessage = request;
                string strLogMessage = string.Empty;
                string path = Path.GetFullPath("Logs//LogRequest//");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                string strLogFile = path + string.Format("{0}-{1}-{2}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day) + ".txt";
                StreamWriter swLog;

                strLogMessage = string.Format("{0}: {1}", DateTime.Now, ConfigureContext.GetRequestPath() + "      \r\n" + logMessage);

                if (!File.Exists(strLogFile))
                {
                    swLog = new StreamWriter(strLogFile);
                }
                else
                {
                    swLog = File.AppendText(strLogFile);
                }

                swLog.WriteLine(strLogMessage);
                swLog.WriteLine();

                swLog.Close();
            }
            catch (Exception)
            {

            }
        }
    }
}