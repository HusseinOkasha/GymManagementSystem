namespace GymManagementSystem.Config;

public class EmailConfig
{
    public static readonly string SectionName = "EmailSettings";
    public string SendGridApiKey { get; set; }
    public string FromEmail { get; set; }
    public string FromName { get; set; }
}