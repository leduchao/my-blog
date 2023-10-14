namespace MyBlog.Areas.Identity.Services;

public class MailSettings
{
    public string? MailFrom { get; }
    public string? Password { get; }
    public string? Host { get; }
    public int Port { get; }

    public MailSettings(string mailFrom, string password, string host, int port)
    {
        MailFrom = mailFrom;
        Password = password;
        Host = host;
        Port = port;
    }
}