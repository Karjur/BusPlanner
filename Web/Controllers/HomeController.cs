using Domain.ClientAggregate;
using Domain.ClientAggregate.Valueobjects;
using Domain.PriceListAggregate;
using Microsoft.AspNetCore.Mvc;
using Web.Helpers;
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
            _apiHelper = new FetchApi();
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Reserve(ReserveInputModel input)
        {
            var client = Client.Create(Name.Create("test", "klient").Value).Value;
            Reservation reservation = Reservation.Create(input.SelectedExpires, client.Id, client.Name.FirstName, client.Name.LastName, 
                input.SelectedFrom, input.SelectedTo, input.SelectedDate, input.SelectedStart, input.SelectedEnd, input.SelectedPrice, input.SelectedCarrierCompany).Value;

            client.AddReservation(DateTime.Parse(input.SelectedExpires), reservation);

            return RedirectToAction("PriceList");
        }

        [HttpGet]
        public async Task<IActionResult> PriceList(SearchRoutesInputModel input)
        {
            var apiData = await _apiHelper.FetchPriceListAsync();

            var priceListViewModel = MapToViewModel(apiData, input.SelectedFrom, input.SelectedTo);
                
            return View("PriceList", priceListViewModel);
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
                        EndTime = schedule.StartEndToTime(schedule.End.Date),
                        CarrierCompany = schedule.Company.State
                    }).ToList()
                }).ToList()
            };

            return priceListViewModel;
        }
    }

    public record ReserveInputModel
    {
        public required string SelectedExpires { get; init; }
        public required string SelectedFrom { get; init; }
        public required string SelectedTo { get; init; }
        public required string SelectedDate { get; init; }
        public required string SelectedStart { get; init; }
        public required string SelectedEnd { get; init; }
        public required double SelectedPrice { get; init; }
        public required string SelectedCarrierCompany { get; init; }
    }
    public record SearchRoutesInputModel
    {
        public required string SelectedFrom { get; init; }
        public required string SelectedTo { get; init;}
    }
}