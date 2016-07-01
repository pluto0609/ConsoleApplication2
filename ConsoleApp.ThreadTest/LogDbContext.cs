using Jaap.Common.Mongodb;

namespace ConsoleApp.ThreadTest
{
    public class LogDbContext : MongoBaseDbContext
    {
        public LogDbContext(string connectionStringName = "mongolog") : base(connectionStringName) { }
    }
}