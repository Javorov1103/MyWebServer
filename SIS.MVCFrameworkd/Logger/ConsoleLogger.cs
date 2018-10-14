namespace SIS.MVCFrameworkd.Logger
{
    using SIS.MVCFrameworkd.Logger.Contracts;
    using System;

    public class ConsoleLogger : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
        }
    }
}
