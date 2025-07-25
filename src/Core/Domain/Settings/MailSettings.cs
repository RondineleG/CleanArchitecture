﻿namespace Domain.Settings;

public class MailSettings
{
    public string DisplayName { get; set; }

    public string EmailFrom { get; set; }

    public string SmtpHost { get; set; }

    public string SmtpPass { get; set; }

    public int SmtpPort { get; set; }

    public string SmtpUser { get; set; }
}