using TicketGo.Application.DTOs;
using TicketGo.Domain.Entities;
using TicketGo.Domain.Interfaces;

namespace TicketGo.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IDiscountRepository _discountRepository;
        private readonly ICoachRepository _coachRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly ISeatRepository _seatRepository;
        private readonly ITicketRepository _ticketRepository;
        private readonly IOrderTicketRepository _orderTicketRepository;

        public OrderService(
            ICoachRepository coachRepository,
            IOrderRepository orderRepository,
            ISeatRepository seatRepository,
            ITicketRepository ticketRepository,
            IOrderTicketRepository orderTicketRepository,
            ICustomerRepository customerRepository,
            IDiscountRepository discountRepository)
        {
            _coachRepository = coachRepository;
            _orderRepository = orderRepository;
            _seatRepository = seatRepository;
            _ticketRepository = ticketRepository;
            _orderTicketRepository = orderTicketRepository;
            _customerRepository = customerRepository;
            _discountRepository = discountRepository;
        }
        public async Task<List<OrderDto>> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetAllAsync();
            return orders.Select(o => new OrderDto
            {
                IdOrder = o.IdOrder,
                UnitPrice = o.UnitPrice,
                DateOrder = o.DateOrder,
                IdTicket = o.IdTicket,
                IdDiscount = o.IdDiscount,
                DiscountName = o.IdDiscountNavigation?.IdDiscount.ToString(), // Có thể thay bằng thuộc tính phù hợp của Discount
                NameCus = o.NameCus,
                Phone = o.Phone,
                IdCus = o.IdCus,
                CustomerName = o.IdCusNavigation?.IdCus.ToString() // Có thể thay bằng thuộc tính phù hợp của Customer
            }).ToList();
        }
        public async Task<OrderDto> GetOrderByIdAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
            {
                return null;
            }

            return new OrderDto
            {
                IdOrder = order.IdOrder,
                UnitPrice = order.UnitPrice,
                DateOrder = order.DateOrder,
                IdTicket = order.IdTicket,
                IdDiscount = order.IdDiscount,
                DiscountName = order.IdDiscountNavigation?.IdDiscount.ToString(), // Có thể thay bằng thuộc tính phù hợp của Discount
                NameCus = order.NameCus,
                Phone = order.Phone,
                IdCus = order.IdCus,
                CustomerName = order.IdCusNavigation?.IdCus.ToString() // Có thể thay bằng thuộc tính phù hợp của Customer
            };
        }
        public async Task<OrderTicketDto> GetOrderTicketDetailsAsync(int idCoach)
        {
            var coach = await _coachRepository.GetCoachWithRelatedDataAsync(idCoach);
            if (coach == null)
            {
                return null;
            }

            var occupiedSeats = coach.Seats.Where(s => s.State).ToList();
            var coachCategory = coach.Category;
            var ticketPrice = CalculateTicketPrice(coach.IdTrainNavigation);

            return CreateOrderTicketDto(coach, occupiedSeats, coachCategory, ticketPrice);
        }

        public async Task CreateOrderAsync(CreateOrderDto createOrderDto)
        {
            // Tạo đơn hàng
            var order = new Order
            {
                UnitPrice = (double)createOrderDto.TotalPrice,
                DateOrder = DateTime.Now,
                NameCus = createOrderDto.Fullname,
                Phone = createOrderDto.Phone,
                IdCus = createOrderDto.IdCustomer
            };

            await _orderRepository.AddAsync(order);

            // Xử lý từng ghế
            foreach (var seatName in createOrderDto.ListSeats)
            {
                var seat = await _seatRepository.GetByNameAndCoachIdAsync(seatName, createOrderDto.IdCoach);
                if (seat != null)
                {
                    // Cập nhật trạng thái ghế
                    seat.State = true;
                    await _seatRepository.UpdateAsync(seat);

                    // Tạo vé
                    var ticket = new Ticket
                    {
                        Date = DateTime.Now,
                        Price = (double)createOrderDto.TotalPrice,
                        IdSeat = seat.IdSeat,
                        IdTrain = (int)_coachRepository.GetCoachWithRelatedDataAsync(seat.IdCoach).Result.IdTrain
                    };

                    await _ticketRepository.AddAsync(ticket);

                    // Tạo mối quan hệ đơn hàng - vé
                    var orderTicket = new OrderTicket
                    {
                        IdOrder = order.IdOrder,
                        IdTicket = ticket.IdTicket
                    };

                    await _orderTicketRepository.AddAsync(orderTicket);
                }
            }
        }

        private decimal CalculateTicketPrice(Train train)
        {
            var basicPrice = (decimal)(train.Coaches.FirstOrDefault()?.BasicPrice ?? 0);
            return (decimal)(train.CoefficientTrain ?? 1) * basicPrice;
        }

        private OrderTicketDto CreateOrderTicketDto(Coach coach, List<Seat> occupiedSeats, string coachCategory, decimal ticketPrice)
        {
            var train = coach.IdTrainNavigation;
            return new OrderTicketDto
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
        public async Task UpdateOrderAsync(int id, CreateUpdateOrderDto orderDto)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
            {
                throw new Exception("Order not found");
            }

            order.UnitPrice = orderDto.UnitPrice;
            order.DateOrder = orderDto.DateOrder;
            order.IdTicket = orderDto.IdTicket;
            order.IdDiscount = orderDto.IdDiscount;
            order.NameCus = orderDto.NameCus;
            order.Phone = orderDto.Phone;
            order.IdCus = orderDto.IdCus;

            await _orderRepository.UpdateAsync(order);
        }

        public async Task DeleteOrderAsync(int id)
        {
            await _orderRepository.DeleteAsync(id);
        }

        public async Task<List<CustomerDto>> GetAllCustomersAsync()
        {
            var customers = await _customerRepository.GetAllAsync();
            return customers.Select(c => new CustomerDto
            {
                IdCus = c.IdCus,
                CustomerName = c.IdCus.ToString() // Có thể thay bằng thuộc tính phù hợp của Customer
            }).ToList();
        }

       
    }
}