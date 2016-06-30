using System.IO;
using log4net.Core;
using log4net.Layout;
using log4net.Layout.Pattern;

namespace ConsoleApp.LogMongonet
{
    public class TitlePatternConvert : PatternLayoutConverter
    {
        protected override void Convert(TextWriter writer, LoggingEvent loggingEvent)
        {
            var logMessage = loggingEvent.MessageObject;
            writer.Write(logMessage);
        }
    }

    public class TitleLayout : PatternLayout
    {
        public TitleLayout()
        {
            this.AddConverter("title",typeof(TitlePatternConvert));
        }
    }
}