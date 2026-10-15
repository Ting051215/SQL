using System.Configuration;

namespace CourseCompass.Services
{
    public class BaseService
    {
        protected string connStr =
            ConfigurationManager
            .ConnectionStrings["CourseDB"]
            .ConnectionString;
    }
}