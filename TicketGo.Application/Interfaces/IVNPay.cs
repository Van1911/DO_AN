using DO_AN.ViewModel;

namespace DO_AN.Services
{
    public interface IVNPay
    {
        string CreatePaymentUrl(HttpContext httpContext, VnPayRequestModel model);
        VnPaymentResponseModel PaymentExecute(IQueryCollection collections);
    }
}
