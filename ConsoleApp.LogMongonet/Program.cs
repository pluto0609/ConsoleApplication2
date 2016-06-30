using System;
using System.IO;
using log4net;
using log4net.Config;

namespace ConsoleApp.LogMongonet
{
    class Program
    {
        static void Main(string[] args)
        {
            var logCfg = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config");
            XmlConfigurator.ConfigureAndWatch(logCfg);

            ILog log = LogManager.GetLogger(typeof(Program));

            //log.Info(string.Format("test_{0:yy-MM-dd,HH:mm:ss-fff}",DateTime.Now));
            log.Info(new{Message="sdfsdflsdf",Title="testTitle"});

        }
    }
}
