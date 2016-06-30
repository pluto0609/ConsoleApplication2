using System;
using System.IO;
using log4net;
using log4net.Config;
using MongoDB.Bson;

namespace ConsoleApp.LogMongonet
{
    public class LogHelper
    {
        private static readonly ILog Log;

        static LogHelper()
        {
            var logCfg = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config");
            XmlConfigurator.ConfigureAndWatch(logCfg);

            Log = LogManager.GetLogger(typeof(LogHelper));
        }

        public static void Info(object msg)
        {
            Log.Info(msg.ToJson());

        }

        public static void Error(object msg)
        {
            Log.Error(msg.ToJson());
        }

        public static void Debug(object msg)
        {
            Log.Debug(msg.ToJson());
        }

        public static void Fatal(object msg)
        {
            Log.Fatal(msg.ToJson());
        }

        public static void Warn(object msg)
        {
            Log.Warn(msg.ToJson());
        }


        public static void ErrorFormat(string format, params object[] args)
        {
            Log.ErrorFormat(format, args);
        }

        public static void WarnFormat(string format, params object[] args)
        {
            Log.WarnFormat(format, args);
        }

        public static void DebugFormat(string format, params object[] args)
        {
            Log.DebugFormat(format, args);
        }

        public static void InfoFormat(string format, params object[] args)
        {
            Log.InfoFormat(format, args);
        }

        public static void FatalFormat(string format, params object[] args)
        {
            Log.FatalFormat(format, args);
        }
    }
}