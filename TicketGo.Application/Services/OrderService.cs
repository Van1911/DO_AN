using TicketGo.Application.DTOs;
using TicketGo.Domain.Entities;
using TicketGo.Domain.Interfaces;
using TicketGo.Application.Interfaces;

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
                TotalPrice = o.UnitPrice, // Đồng bộ với tên TotalPrice
                DateOrder = o.DateOrder,
                IdTicket = o.IdTicket,
                IdDiscount = o.IdDiscount,
                DiscountName = o.IdDiscountNavigation?.IdDiscount.ToString(),
                NameCus = o.NameCus,
                Phone = o.Phone,
                IdCus = o.IdCus,
                CustomerName = o.IdCusNavigation?.FullName
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
                TotalPrice = order.UnitPrice,
                DateOrder = order.DateOrder,
                IdTicket = order.IdTicket,
                IdDiscount = order.IdDiscount,
                DiscountName = order.IdDiscountNavigation?.IdDiscount.ToString(),
                NameCus = order.NameCus,
                Phone = order.Phone,
                IdCus = order.IdCus,
                CustomerName = order.IdCusNavigation?.FullName
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

        public async Task CreateOrderAsync(CreateUpdateOrderDto orderDto)
        {
            var order = new Order
            {
                UnitPrice = orderDto.TotalPrice ?? 0,
                DateOrder = orderDto.DateOrder ?? DateTime.Now,
                NameCus = orderDto.NameCus,
                Phone = orderDto.Phone,
                IdCus = orderDto.IdCus,
                IdDiscount = orderDto.IdDiscount ?? 0
            };

            await _orderRepository.AddAsync(order);

            foreach (var seatName in orderDto.ListSeats)
            {
                if (orderDto.IdCoach == null)
                    throw new ArgumentException("IdCoach is required");

                var seat = await _seatRepository.GetByNameAndCoachIdAsync(seatName, orderDto.IdCoach.Value);

    
                if (seat != null)
                {
                    seat.State = true;
                    await _seatRepository.UpdateAsync(seat);
                    var coach = await _coachRepository.GetCoachWithRelatedDataAsync(seat.IdCoach);

                    var ticket = new Ticket
                    {
                        Date = DateTime.Now,
                        Price = orderDto.TotalPrice ?? 0,
                        IdSeat = seat.IdSeat.Value,
                        IdTrain = coach.IdTrain.Value
                    };

                    await _ticketRepository.AddAsync(ticket);

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
                throw new Exception("Đơn hàng không tồn tại");
            }

            order.UnitPrice = orderDto.TotalPrice ?? 0;
            order.DateOrder = orderDto.DateOrder ?? DateTime.Now;
            order.IdTicket = orderDto.IdTicket ?? 0;
            order.IdDiscount = orderDto.IdDiscount ?? 0;
            order.NameCus = orderDto.NameCus;
            order.Phone = orderDto.Phone;
            order.IdCus = orderDto.IdCus ?? 0; // Sử dụng orderDto, không phải Coach

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
                CustomerName = c.FullName
            }).ToList();
        }
    }
}