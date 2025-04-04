using DO_AN.ViewModel;

namespace DO_AN.Services
{

    public interface IEmailSender
    {

        Task SendEmailAsync(string email, string subject, string htmlMessage);
    }

}
