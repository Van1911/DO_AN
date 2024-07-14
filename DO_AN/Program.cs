﻿using DO_AN.Models;
using DO_AN.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddSession();
builder.Services.AddSingleton<IVNPayService, VNPayService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<ReportService>();
// Inject IConfiguration to access appsettings.json
var configuration = builder.Configuration;

//builder.Services.AddAuthentication()
//    //.AddGoogle(googleOptions =>
//    //{
//    //    IConfigurationSection googleAuthNSection = configuration.GetSection("Authentication:Google");

//    //    googleOptions.ClientId = googleAuthNSection["ClientId"];
//    //    googleOptions.ClientSecret = googleAuthNSection["ClientSecret"];
//    //    googleOptions.CallbackPath = "/dang-nhap-tu-google";
//    //});

builder.Services.AddDbContext<DOANContext>(opts =>
{
    opts.UseSqlServer(configuration.GetConnectionString("DoAnConnection"));
});

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<DOANContext>();

builder.Services.AddTransient<IEmailSender, EmailSender>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Sales}/{action=Sales}/{id?}");

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=TrangChu}/{id?}");
});

app.Run();
