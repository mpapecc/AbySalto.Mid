namespace AbySalto.Mid.Application.Models.GridProcessors
{
    public class GridRquest
    {
        public GridPagination? Pagination { get; set; }
        public IList<GridSort>? Sort { get; set; }
    }
}
