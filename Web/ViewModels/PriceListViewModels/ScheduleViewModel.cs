namespace Web.ViewModels.PriceListViewModels
{
    public class ScheduleViewModel
    {
        public required string Id { get; init; }
        public required double Price { get; init; }
        public required string StartDate { get; init; }
        public required string StartTime { get; init; }
        public required string EndDate { get; init; }
        public required string EndTime { get; init; }
        public required string CarrierCompany { get; init; }
    }
}