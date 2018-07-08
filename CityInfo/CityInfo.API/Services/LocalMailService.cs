namespace CityInfo.API.Services
{
    using System.Diagnostics;
    using CityInfo.API.Common;

    public class LocalMailService : IMailService
    {
        private readonly string _mailTo = Startup.Configuration["mailSettings:mailToAddress"];
        private readonly string _mailFrom = Startup.Configuration["mailSettings:mailFromAddress"];

        public void Send(string subject, string message)
        {
            Guard.ArgumentNotNullOrEmpty(subject, nameof(subject));
            Guard.ArgumentNotNullOrEmpty(message, nameof(message));

            Debug.WriteLine($"Mail from {_mailFrom} to {_mailTo}, with LocalMailService.");
            Debug.WriteLine($"Subject: {subject}");
            Debug.WriteLine($"Message: {message}");
        }
    }
}
