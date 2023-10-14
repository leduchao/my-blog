namespace MyBlog.Areas.Identity.Services;

public class SendMailService
{
    public string? MailFrom { get; set; }
    public string? Password { get; set; }
    public string? Host { get; set; }
    public int Port { get; set; }
    public string? Body { get; set; }
    public string? Subject { get; set; }

    private readonly IConfiguration _configuration;

    public SendMailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
}