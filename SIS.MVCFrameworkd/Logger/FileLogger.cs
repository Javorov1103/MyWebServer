namespace SIS.MVCFrameworkd.Logger
{
    using SIS.MVCFrameworkd.Logger.Contracts;
    using System;
    using System.IO;

    public class FileLogger : ILogger
    {
        private static object LockObject = new object();

        private readonly string fileName;

        public FileLogger(string fileName)
        {
            this.fileName = fileName;
        }

        public void Log(string message)
        {
            lock(LockObject)
            {
                File.AppendAllText(this.fileName, message + Environment.NewLine);
            }
        }
    }
}
