namespace GymManagementSystem.Config;

public class JwtOption
{
    public const string SectionName = "JwtConfig";
    public string ValidationAudiences { get; set; }
    public string ValidIssuer { get; set; }
    public string Secret { get; set; }
}