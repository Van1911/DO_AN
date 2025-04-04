using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DO_AN.Models;
using DO_AN.Areas.Admin.ViewModels;
using DO_AN.Areas.Admin.Models;
using Newtonsoft.Json;

namespace DO_AN.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SalesController : Controller
    {
        private readonly DOANContext _context;

        public SalesController(DOANContext context)
        {
            _context = context;
        }

        public IActionResult RevenueChart()
        {
            var inputYear = 2024;
            // Truy vấn dữ liệu doanh thu từ bảng Order (ví dụ tính tổng doanh thu theo tháng)
            var revenueData = _context.Orders
                                .Where(o => o.DateOrder != null && o.DateOrder.Value.Year == inputYear)
                                .GroupBy(o => o.DateOrder.Value.Month)
                                .Select(g => new { Month = g.Key, TotalRevenue = g.Sum(o => o.UnitPrice) })
                                .ToList();


            // Chuẩn bị dữ liệu cho biểu đồ (ví dụ, chuyển sang định dạng JSON)
            var labels = revenueData.Select(r => $"Tháng {r.Month}");
            var revenueValues = revenueData.Select(r => r.TotalRevenue);

            var chartData = new
            {
                labels = labels,
                datasets = new[]
                {
                new
                {
                    label = "Doanh thu",
                    data = revenueValues,
                    backgroundColor = "rgba(54, 162, 235, 0.2)",
                    borderColor = "rgba(54, 162, 235, 1)",
                    borderWidth = 1
                }
            }
            };

            ViewData["ChartData"] = JsonConvert.SerializeObject(chartData); // Truyền dữ liệu biểu đồ sang view

            return View();
        }


        public IActionResult DailyRevenueChart(int year, int month)
        {
            if (year == 0 || month == 0)
            {
                year = DateTime.Now.Year;
                month = DateTime.Now.Month;
            }
            // Validate year and month input (optional, based on your requirements)
            if (year < 2000 || year > DateTime.Today.Year || month < 1 || month > 12)
            {
                // Handle invalid input
                return BadRequest("Invalid year or month.");
            }

            // Lấy số ngày trong tháng
            int daysInMonth = DateTime.DaysInMonth(year, month);

            // Tính toán doanh thu theo từng ngày trong tháng
            var dailyRevenueData = _context.Orders
                        .Where(o => o.DateOrder.HasValue && o.DateOrder.Value.Year == year && o.DateOrder.Value.Month == month)
                        .GroupBy(o => new
                        {
                            Day = o.DateOrder.Value.Day
                        })
                        .Select(g => new
                        {
                            Day = g.Key.Day,
                            TotalRevenue = g.Sum(o => o.UnitPrice)
                        })
                        .OrderBy(g => g.Day)
                        .ToList();

            // Chuẩn bị dữ liệu cho biểu đồ (theo ngày)
            string[] labels = new string[daysInMonth];
            int[] revenueValues = new int[daysInMonth];

            for (int day = 1; day <= daysInMonth; day++)
            {
                labels[day - 1] = day.ToString();
                var revenue = dailyRevenueData.FirstOrDefault(d => d.Day == day);
                revenueValues[day - 1] = (int)(revenue?.TotalRevenue ?? 0); // Đảm bảo giá trị không null và chuyển về kiểu int
            }

            var chartData = new
            {
                Labels = labels,
                RevenueValues = revenueValues
            };

            ViewData["DailyChartData"] = JsonConvert.SerializeObject(chartData);

            return View();
        }

        public IActionResult PopularTravelTimes(DateTime? selectedDate)
        {
            // Nếu ngày không được chọn, mặc định là ngày hiện tại
            var date = selectedDate ?? DateTime.Today;

            var travelTimes = _context.Tickets
                .Where(t => t.Date.Value.Date == date.Date) // Lọc theo ngày được chọn
                .GroupBy(t => t.Date.Value.Hour) // Nhóm theo giờ của ngày khởi hành
                .Select(g => new
                {
                    Hour = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(g => g.Count) // Sắp xếp giảm dần theo số lượng vé
                .ToList();

            string[] labels = travelTimes.Select(t => t.Hour.ToString()).ToArray();
            int[] data = travelTimes.Select(t => t.Count).ToArray();

            var chartData = new { labels, data };

            ViewData["PopularTravelTimesData"] = JsonConvert.SerializeObject(chartData);

            ViewData["SelectedDate"] = date.ToString("dd-MM-yyyy");

            return View();
        }


        public IActionResult AllRoutesWithTripCounts()
        {
            var allRoutes = _context.TrainRoutes
                .Select(route => new
                {
                    Route = route.PointStart + " - " + route.PointEnd,
                    RouteId = route.IdTrainRoute,
                    TicketsCount = _context.Tickets
                        .Count(ticket => ticket.IdTrainNavigation.IdTrainRoute == route.IdTrainRoute)
                })
                .OrderByDescending(r => r.TicketsCount)
                .ToList();

            ViewData["AllRoutes"] = allRoutes;

            return View();
        }
    }
}