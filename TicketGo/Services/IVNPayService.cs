using DO_AN.ViewModel;

namespace DO_AN.Services
{
    public interface IVNPayService
    {
        string CreatePaymentUrl(HttpContext httpContext, VnPayRequestModel model);
        VnPaymentResponseModel PaymentExecute(IQueryCollection collections);
    }
}
