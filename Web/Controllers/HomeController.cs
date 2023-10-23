using Domain.PriceListAggregate;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Web.Helpers;
using Web.Models;
using Web.ViewModels.PriceListViewModels;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly FetchApi _apiHelper;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;

            string apiUrl = "https://assignments.novater.com/v1/bus/schedule";
            string username = "karmo";
            string password = "71b4b3253c40c6f59cc4af5b9005d105";
            _apiHelper = new FetchApi(apiUrl, username, password);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> PriceList(SearchRoutesInputModel input)
        {
            var apiData = await _apiHelper.FetchPriceListAsync();

            var priceListViewModel = MapToViewModel(apiData, input.SelectedFrom, input.SelectedTo);

            return View("PriceList", priceListViewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private PriceListViewModel MapToViewModel(PriceList apiData, string selectedFrom, string selectedTo)
        {
            var priceListViewModel = new PriceListViewModel
            {
                Id = apiData.Id,
                Expires = apiData.Expires.Date,
                Routes = apiData.Routes
                .Where(route =>
                (string.IsNullOrEmpty(selectedFrom) || route.From.Name == selectedFrom) &&
                (string.IsNullOrEmpty(selectedTo) || route.To.Name == selectedTo))
                .Select(route => new RouteViewModel
                {
                    Id = route.Id,
                    From = route.From.Name,
                    To = route.To.Name,
                    Distance = route.Distance,
                    Schedule = route.Schedule.Select(schedule => new ScheduleViewModel
                    {
                        Id = schedule.Id,
                        Price = schedule.Price,
                        StartDate = schedule.StartEndToDate(schedule.Start.Date),
                        StartTime = schedule.StartEndToTime(schedule.Start.Date),
                        EndDate = schedule.StartEndToDate(schedule.Start.Date),
                        EndTime = schedule.StartEndToTime(schedule.Start.Date),
                        CompanyName = schedule.Company.State
                    }).ToList()
                }).ToList()
            };

            return priceListViewModel;
        }
    }

    public record SearchRoutesInputModel
    {
        public required string SelectedFrom { get; init; }
        public required string SelectedTo { get; init;}
    }
}