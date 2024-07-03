namespace Server.Contracts.Settings;

public class EmailSetting
{
    public const string SectionName = "EmailSetting";
    public string EmailHost { get; set; }
    public string EmailUsername { get; set; }
    public string EmailPassword { get; set; }
}