namespace AuthService.SyncDataServices.Smtp
{
    public interface IEmailService
    {
        void SendEmail(string subject, string body, string receiverEmail);
    }
}