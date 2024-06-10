using System;

namespace RivTech.CABoxTruck.WebService.DTO.Report.Filters
{
    public abstract class FilterBase
    {
        public string SortField { get; set; }
        public string ThenSortField { get; set; }
        public bool Ascending { get; set; }
        public int PageNumber { get; set; }

        private int _pageSize = 1000;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > _pageSize || value <= 0) ? _pageSize : value;
        }
    }
}