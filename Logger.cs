using System;
using System.IO;

namespace GTAVWebhook
{
    public static class Logger
    {
        public static void Log(string message)
        {
            try
            {
                File.AppendAllText("GTAVWebhook.log", DateTime.Now + ": " + message + Environment.NewLine);
            } catch(Exception)
            {

            }
        }
    }
}
