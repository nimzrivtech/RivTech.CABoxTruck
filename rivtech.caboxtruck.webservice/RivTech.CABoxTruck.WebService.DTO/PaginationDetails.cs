using RivTech.CABoxTruck.WebService.Data.Entity.DataSets;
using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.DTO
{
    public class PaginationDetails
    {
        private int _currentPage;
        private int _totalPages;
        private int _totalCount;

        public PaginationDetails(int currentPage, int totalPages, int count)
        {
            _currentPage = currentPage;
            _totalPages = totalPages;
            _totalCount = count;
        }

        public int FirstPage => 1;
        public int LastPage => _totalPages;
        public int CurrentPage => _currentPage;
        public int PreviousPage => _currentPage > FirstPage ? _currentPage - 1 : FirstPage;
        public int NextPage => _currentPage < LastPage ? _currentPage + 1 : LastPage;
        public int TotalPages => _totalPages;
        public int TotalCount => _totalCount;
    }
}
