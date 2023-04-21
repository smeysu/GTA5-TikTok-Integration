using System;
using System.Collections.Generic;
using System.IO;

namespace GTAVWebhook
{
    public static class Logger
    {
        private readonly static string LogfileName = "GTAVWebhook.log";
        private static List<string> currentSessionLogs = new List<string>();
        public static void Clear()
        {
            try
            {
                if (File.Exists(LogfileName))
                    File.Delete(LogfileName);
            }
            catch (Exception)
            {

            }
        }
        public static void Log(string message)
        {
            try
            {
                if (currentSessionLogs.Count > 200)
                    currentSessionLogs.RemoveAt(0);

                string logData = DateTime.Now + ": " + message;

                currentSessionLogs.Add(logData);

                File.AppendAllText(LogfileName, logData + Environment.NewLine);
            }
            catch (Exception)
            {

            }
        }

        public static string GetLogContents()
        {
            string logContents = "";

            if (currentSessionLogs.Count > 0)
            {
                foreach (string logEntry in currentSessionLogs)
                {
                    logContents += logEntry + "\n";
                }
            }
            else
            {
                logContents = "No logs available";
            }

            return logContents;
        }
    }
}
