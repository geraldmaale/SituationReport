namespace SR.Shared.Options;

public class ApplicationSettings : ClientClass
{
    public const string SettingsName = "SrAppSettings";
    //public Dictionary<int, ClientClass>? Client { get; set; }
}

public class ClientClass
{
    public string UriConnection { get; set; }
    public Dictionary<string, DbTypeClass> Database { get; set; }
    public Dictionary<string, string> SendGrid { get; set; }
}

public class DbTypeClass
{
    public string DbConnection { get; set; }
}
