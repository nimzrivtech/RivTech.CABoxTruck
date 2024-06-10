using Microsoft.EntityFrameworkCore;
using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Submission;
using RivTech.CABoxTruck.WebService.DTO.Constants;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Submission
{
    public interface ISubmissionService
    {
        Task DeleteSubmissionAsync(string submissionNumber);
    }

    public class SubmissionService : ISubmissionService
    {
        private readonly IRiskRepository _riskRepository;
        private readonly AppDbContext _dbContext;
        private readonly IRiskDetailRepository _riskDetailRepository;

        public SubmissionService(
            IRiskRepository riskRepository,
            AppDbContext dbContext,
            IRiskDetailRepository riskDetailRepository
        )
        {
            _riskRepository = riskRepository;
            _dbContext = dbContext;
            _riskDetailRepository = riskDetailRepository;
        }


        public async Task DeleteSubmissionAsync(string submissionNumber)
        {
            var risk = await _riskRepository.GetBySubmissionAsync(submissionNumber);
            if (risk is null || risk.Status != RiskStatus.Pending)
                return;

            var riskDetail = await _riskDetailRepository.FindAsync(x => x.SubmissionNumber == submissionNumber);
            var toDeleteRiskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(riskDetail.Id);
            if (toDeleteRiskDetail is null)
                return;

            _riskDetailRepository.Detach(toDeleteRiskDetail);

            var executionStrategy = _dbContext.Database.CreateExecutionStrategy();
            executionStrategy.Execute(() =>
            {
                using var transaction = _dbContext.Database.BeginTransaction();
                // total clean up on data upon reset, to avoid data duplication
                try
                {
                    string deletePolicyDetail = string.Format(@"
                        DELETE FROM [dbo].[EntityAddress] WHERE [EntityId] = '{1}'
                        DELETE FROM [dbo].[EntitySubsidiary] WHERE [EntityId] = '{1}'
                        DELETE FROM [dbo].[RiskSpecificCommoditiesHauled] WHERE [RiskDetailId] = '{0}'
                        DELETE FROM [dbo].[RiskSpecificDriverInfo] WHERE [Id] = '{0}'
                        DELETE FROM [dbo].[DriverHeader] WHERE [RiskDetailId] = '{0}'
                        DELETE FROM [dbo].[RiskSpecificDriverHiringCriteria] WHERE [Id] = '{0}'
                        DELETE FROM [dbo].[RiskSpecificDotInfo] WHERE [Id] = '{0}'
                        DELETE FROM [dbo].[RiskSpecificDestination] WHERE [RiskDetailId] = '{0}'
                        DELETE FROM [dbo].[ClaimsHistory] WHERE [RiskDetailId] = '{0}'
                        DELETE FROM [dbo].[RiskManuscripts] WHERE [RiskDetailId] = '{0}'
                        DELETE FROM [dbo].[RiskSpecificUnderwritingQuestion] WHERE [Id] = '{0}'
                        DELETE FROM [dbo].[RiskCoveragePremium] WHERE [RiskCoverageId] IN (SELECT [Id] FROM [RiskCoverage] WHERE [RiskDetailId] = '{0}')
                        DELETE FROM [dbo].[RiskCoverage] WHERE [RiskDetailId] = '{0}'
                        DELETE FROM [dbo].[RiskHistory] WHERE [RiskDetailId] = '{0}'
                        DELETE FROM [dbo].[RiskSpecificSafetyDevice] WHERE [RiskDetailId] = '{0}'
                        DELETE FROM [dbo].[RiskSpecificRadiusOfOperation] WHERE [Id] = '{0}'
                        DELETE FROM [dbo].[RiskFiling] WHERE [RiskDetailId] = '{0}'
                        DELETE FROM [dbo].[RiskForm] WHERE [RiskDetailId] = '{0}'
                        DELETE FROM [dbo].[FileUploadDocument] WHERE [RiskDetailId] = '{0}'

                        DELETE FROM [dbo].[Driver] 
                        WHERE   [Id] IN (SELECT [DriverId] FROM [RiskDetailDriver] WHERE [RiskDetailId] = '{0}')

                        DELETE FROM [dbo].[Vehicle]
                        WHERE   [Id] IN (SELECT [VehicleId] FROM [RiskDetailVehicle] WHERE [RiskDetailId] = '{0}')

                        DELETE FROM [dbo].[RIskDetailVehicle] WHERE [RiskDetailId] = '{0}'
                        DELETE FROM [dbo].[RiskDetailDriver] WHERE [RiskDetailId] = '{0}'
                        DELETE FROM [dbo].[VehiclePremiumRatingFactor] WHERE [RiskDetailId] = '{0}'
                        DELETE FROM [dbo].[VehiclePremium] WHERE [RiskDetailId] = '{0}'
                        DELETE FROM [dbo].[RiskResponse] WHERE [RiskDetailId] = '{0}'
                        DELETE FROM [dbo].[RiskAdditionalInterest] WHERE [RiskDetailId] = '{0}'                     
                        DELETE FROM [dbo].[RiskDetail] WHERE [Id] = '{0}'
                        DELETE FROM [dbo].[Insured] WHERE [Id] = '{2}'
                        DELETE FROM [dbo].[Entity] WHERE [Id] = '{1}'

                        DELETE FROM [dbo].[RiskVehiclePremiumCoverage] WHERE [Id] not in (SELECT CoveragePremiumId FROM RiskVehiclePremium) 
                        DELETE FROM [dbo].[RiskVehiclePremium] WHERE [RiskDetailId] = '{0}'
                        DELETE FROM [dbo].[RiskVehiclePremiumTransaction] WHERE [RiskDetailId] = '{0}'

                        DELETE FROM [dbo].[RiskPolicyPremiumCoverage] WHERE [Id] not in (SELECT CoveragePremiumId FROM RiskPolicyPremium) 
                        DELETE FROM [dbo].[RiskPolicyPremium] WHERE [RiskDetailId] = '{0}'
                        DELETE FROM [dbo].[RiskPolicyPremiumTransaction] WHERE [RiskDetailId] = '{0}'

                        DELETE FROM [dbo].[Binding] where RiskId = '{3}'
                        DELETE FROM [dbo].[BrokerInfo] where RiskId = '{3}'
                        DELETE FROM [dbo].[Risk] WHERE [Id] = '{3}'
                    ", 
                    /* 0 */toDeleteRiskDetail.Id, 
                    /* 1 */toDeleteRiskDetail.Insured.EntityId, 
                    /* 2 */toDeleteRiskDetail.InsuredId, 
                    /* 3 */risk.Id);

                    _dbContext.Database.ExecuteSqlRaw(deletePolicyDetail);
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw e;
                }
                transaction.Commit();
            });
        }

    }
}
