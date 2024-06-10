using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Wordprocessing;
using NPOI.SS.Formula.Functions;
using RivTech.CABoxTruck.WebService.DALDapper.IDataAccess;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.Data.Entity.DataSets;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Submission;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Submission.DataSets;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Constants;
using RivTech.CABoxTruck.WebService.DTO.Submission;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.IServices.Risk;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Risk
{
    public class RiskSummaryService : IRiskSummaryService
    {
        private readonly ISubmissionPolicyDataAccess _submissionPolicyDataAccess;

        public RiskSummaryService(ISubmissionPolicyDataAccess submissionPolicyDataAccess)
        {
            _submissionPolicyDataAccess = submissionPolicyDataAccess;
        }

        public async Task<SummaryListPagingDTO> GetFilteredSummaryListAsync(SubmissionFilterDto filter)
        {
            var submissionPolicyData = await _submissionPolicyDataAccess.GetSubmissionPolicy(filter);

            var count = await _submissionPolicyDataAccess.GetSubmissionPolicyTotalCount(filter);

            var totalPages = (int)Math.Ceiling(count / (float)filter.PageSize);
            var paginationDetails = new PaginationDetails(filter.PageNumber, totalPages, count);
            return new SummaryListPagingDTO()
            {
                PaginationDetails = paginationDetails,
                RiskSummaryDtos = submissionPolicyData.ToList(),

            };
        }
    }
}
