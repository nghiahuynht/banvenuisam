using System.Collections.Generic;

namespace GM.MODEL.ViewModel
{
    public class BrowseListViewModel<T>
    {
        public int TotalItem { get; set; }
        public IEnumerable<T> DataList { get; set; }
    }
}