namespace Common.API
{
    public class AppSettings
    {
        public Logging Logging { get; set; }
        public ConnectionStrings ConnectionStrings { get; set; }

    }
    public class ConnectionStrings
    {
        public string ApiDatabase { get; set; }
        
    }
    public class Logging
    {
        public bool IncludeScopes { get; set; }
        public Loglevel LogLevel { get; set; }
    }

    public class Loglevel
    {
        public string Default { get; set; }
        public string System { get; set; }
        public string Microsoft { get; set; }
    }
}
