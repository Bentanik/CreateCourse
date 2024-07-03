namespace Server.Contracts.Settings;

public class EmailRegisterSetting
{
    public const string SectionName = "EmailRegisterSetting";
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string SecretToken { get; set; }
    public double EmailExpMinute { get; set; }

}