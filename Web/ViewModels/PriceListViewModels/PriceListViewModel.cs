namespace Web.ViewModels.PriceListViewModels
{
    public class PriceListViewModel
    {
        public required string Id { get; init; }
        public required string Expires { get; init; }
        public required List<RouteViewModel> Routes { get; init; }
    }
}
