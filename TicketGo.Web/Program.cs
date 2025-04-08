using Microsoft.EntityFrameworkCore;
using TicketGo.Application.Interfaces;
using TicketGo.Application.Services;
using TicketGo.Domain.Interfaces;
using TicketGo.Infrastructure.Data;
using TicketGo.Infrastructure.Repositories;
using TicketGo.Infrastructure.Services;
using TicketGo.Web.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(); // Sử dụng MVC thay vì Razor Pages

// Cấu hình DbContext với SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Cấu hình session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Thời gian timeout của session
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Đăng ký các repository
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ITicketRepository, TicketRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<ITrainRepository, TrainRepository>();
builder.Services.AddScoped<ICoachRepository, CoachRepository>();
builder.Services.AddScoped<ISeatRepository, SeatRepository>();
builder.Services.AddScoped<IOrderTicketRepository, OrderTicketRepository>();
builder.Services.AddScoped<ITrainRouteRepository, TrainRouteRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();
// Đăng ký các repository khác nếu cần...

// Đăng ký các service
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ITicketService, TicketService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ITrainService, TrainService>();
builder.Services.AddScoped<ICoachService, CoachService>();
builder.Services.AddScoped<ITrainRouteService, TrainRouteService>();

// Đăng ký các service khác nếu cần...

// Đăng ký dịch vụ gửi email
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddScoped<IMailSettings, MailSettings>();
builder.Services.AddScoped<IEmailSender, EmailSender>();

// Đăng ký dịch vụ thanh toán VnPay
builder.Services.AddScoped<IVnPayService, VnPayService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseErrorHandling(); // Sử dụng middleware xử lý lỗi
    app.UseHsts();
}

app.UseCustomLogging(); // Sử dụng middleware ghi log

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession(); // Thêm middleware session

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();