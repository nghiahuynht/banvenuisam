using System;

namespace GM.MODEL.ViewModel
{
    public class PaginationFilterViewModel
    {
        public int? PageIndex { get; set; } = 1;

        public int? PageSize { get; set; } = 10;

        public string SearchString { get; set; }
    }

    public class BaseFilterViewModel : PaginationFilterViewModel
    {
        public string SortColumn { get; set; }
        public string SortDir { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }

    }


}