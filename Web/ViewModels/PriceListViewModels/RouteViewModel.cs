namespace Web.ViewModels.PriceListViewModels
{
    public class RouteViewModel
    {
        public required string Id { get; init; }
        public required string From { get; init; }
        public required string To { get; init; }
        public required int Distance { get; init; }
        public required List<ScheduleViewModel> Schedule { get; init; }
    }
}