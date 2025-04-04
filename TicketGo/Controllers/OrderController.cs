using DO_AN.Models;
using DO_AN.Services;
using DO_AN.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DO_AN.Controllers
{
    public class OrderController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DOANContext _context;
        private readonly IVNPayService _vnPayService;
        private bool IsUserLoggedIn()
        {
            return HttpContext.Session.GetString("UserSession") != null;
        }
        public OrderController(DOANContext context, IVNPayService vnPayService, IHttpContextAccessor httpContext)
        {
            _httpContextAccessor = httpContext;
            _context = context;
            _vnPayService = vnPayService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? idCoach)
        {



            if (!IsUserLoggedIn())
            {
                return RedirectToAction("Login", "Access");
            }
            HttpContext.Session.SetInt32("idCoach", (int)idCoach);

            if (idCoach == null)
            {
                return NotFound();
            }

            var coach = await GetCoachWithRelatedDataAsync(idCoach.Value);
            if (coach == null)
            {
                return NotFound();
            }

            var occupiedSeats = GetOccupiedSeats(coach.Seats);
            var coachCategory = coach.Category;
            var ticketPrice = CalculateTicketPrice(coach.IdTrainNavigation);

            var viewModel = CreateOrderTicketViewModel(coach, occupiedSeats, coachCategory, ticketPrice);

            return View(viewModel);
        }

        private async Task<Coach> GetCoachWithRelatedDataAsync(int idCoach)
        {
            return await _context.Coaches
                                 .Include(c => c.Seats)
                                 .Include(c => c.IdTrainNavigation)
                                    .ThenInclude(t => t.IdTrainRouteNavigation)
                                 .FirstOrDefaultAsync(c => c.IdCoach == idCoach);
        }

        private List<Seat> GetOccupiedSeats(IEnumerable<Seat> seats)
        {
            return seats.Where(s => s.State).ToList();
        }

        private decimal CalculateTicketPrice(Train train)
        {
            var basicPrice = (decimal)(train.Coaches.FirstOrDefault()?.BasicPrice ?? 0);
            return (decimal)(train.CoefficientTrain ?? 1) * basicPrice;
        }

        private OrderTicketViewModel CreateOrderTicketViewModel(Coach coach, List<Seat> occupiedSeats, string coachCategory, decimal ticketPrice)
        {
            var train = coach.IdTrainNavigation;
            return new OrderTicketViewModel
            {
                Train = train,
                IdTrain = train.IdTrain,
                OccupiedSeats = occupiedSeats,
                PointStart = train.IdTrainRouteNavigation.PointStart,
                PointEnd = train.IdTrainRouteNavigation.PointEnd,
                DateStart = train.DateStart?.ToShortDateString(),
                Price = ticketPrice,
                VehicleType = coachCategory
            };
        }


        [HttpPost]
        public async Task<IActionResult> Index(List<string> listSeats, string Fullname, string Phone, string Email, decimal TotalPrice)
        {
            if (!IsUserLoggedIn())
            {
                return RedirectToAction("Login", "Access");
            }
            if (!ModelState.IsValid)
            {
                return NotFound();          // Trả về view nếu dữ liệu không hợp lệ
            }

            //Lấy id người dùng từ session
            var httpContext = _httpContextAccessor.HttpContext;
            var idcus = httpContext.Session.GetInt32("UserID");

            if (idcus == null)
            {
                return NotFound();
            }
            // Lưu danh sách ghế vào Session khi người dùng đặt vé
            string jsonSeats = JsonConvert.SerializeObject(listSeats);
            HttpContext.Session.SetString("SelectedSeats", jsonSeats);

            HttpContext.Session.SetString("Fullname", Fullname);
            HttpContext.Session.SetString("Phone", Phone);
            HttpContext.Session.SetString("Email", Email);
            HttpContext.Session.SetString("TotalPrice", TotalPrice.ToString());



            // Chuẩn bị dữ liệu cho VNPay
            string idOrder = DateTime.Now.ToString("yyyyMMdd");
            int orderId = Convert.ToInt32(idOrder);
            var vnPayModel = new VnPayRequestModel
            {
                Amount = (double)TotalPrice * 1000,
                CreatedDate = DateTime.Now,
                Description = $"Thanh toán",
                Fullname = Fullname,
                OrderId = orderId
            };

            return Redirect(_vnPayService.CreatePaymentUrl(HttpContext, vnPayModel));

        }

        public IActionResult Pay_Fail()
        {
            return View();
        }

        public IActionResult Pay_Success()
        {
            return View();
        }

        public IActionResult PaymentCallBack()
        {
            // Lấy thông tin từ Session

            var fullname = HttpContext.Session.GetString("Fullname");
            var idcus = HttpContext.Session.GetInt32("UserID");
            var phone = HttpContext.Session.GetString("Phone");
            var email = HttpContext.Session.GetString("Email");
            var totalPrice = decimal.Parse(HttpContext.Session.GetString("TotalPrice"));
            var idCoach = HttpContext.Session.GetInt32("idCoach");

            // Tạo đơn hàng và lưu vào cơ sở dữ liệu
            var order = new Order
            {
                UnitPrice = (double)totalPrice,
                DateOrder = DateTime.Now,
                NameCus = fullname,
                Phone = phone,
                IdCus = idcus
                // Thêm các thuộc tính khác của đơn hàng nếu có
            };

            _context.Orders.Add(order);
            _context.SaveChanges();

            string jsonSeats = HttpContext.Session.GetString("SelectedSeats");
            List<string> listSeats = JsonConvert.DeserializeObject<List<string>>(jsonSeats);
            string listSeatsFinal = listSeats.First();
            List<string> seats = JsonConvert.DeserializeObject<List<string>>(listSeatsFinal);

            // Lặp qua từng seatName trong danh sách
            foreach (string seatName in seats)
            {

                // Tìm ghế dựa vào NameSeat và IdCoach
                var seat = _context.Seats.FirstOrDefault(s => s.NameSeat == seatName && s.IdCoach == idCoach);

                if (seat != null)
                {
                    // Cập nhật trạng thái ghế thành đã đặt
                    seat.State = true;

                    var ticket = new Ticket
                    {
                        Date = DateTime.Now,
                        Price = (double)totalPrice,         // Giá vé (có thể khác nhau nếu có giảm giá)
                        IdSeat = seat.IdSeat,               // Lấy Id ghế từ cơ sở dữ liệu
                        IdTrain = (int)_context.Coaches
                                        .Where(c => c.IdCoach == seat.IdCoach)
                                        .Select(c => c.IdTrain)
                                        .FirstOrDefault()   // Lấy IdTrain từ bảng Coach
                    };

                    _context.Tickets.Add(ticket);
                    _context.SaveChanges();
                    // Tạo mối quan hệ đơn hàng - vé
                    var orderTicket = new OrderTicket
                    {
                        IdOrder = order.IdOrder,
                        IdTicket = ticket.IdTicket
                    };

                    _context.OrderTickets.Add(orderTicket);
                }
            }
            // Lưu các thay đổi vào cơ sở dữ liệu
            _context.SaveChanges();


            TempData["Message"] = "Thanh toán thành công";
            return RedirectToAction("Pay_Success");
        }
    }
}
