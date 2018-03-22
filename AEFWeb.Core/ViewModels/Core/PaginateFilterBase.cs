namespace AEFWeb.Core.ViewModels.Core
{
    public class PaginateFilterBase
    {
        public PaginateFilterBase(string skip, string take, string search)
        {
            Skip = skip;
            Take = take;
            Search = search;
        }

        public PaginateFilterBase()
        { }

        public string Skip { get; set; }
        public string Take { get; set; }
        public string Search { get; set; }
    }
}
