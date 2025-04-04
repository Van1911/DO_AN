using DO_AN.ViewModel;

namespace DO_AN.Services
{

    public interface IEmailSendertTest
    {

        Task SendEmailAsync(string email, string subject, string htmlMessage);
    }

}
