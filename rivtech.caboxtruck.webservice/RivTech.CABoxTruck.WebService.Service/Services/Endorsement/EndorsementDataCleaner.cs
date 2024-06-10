using Microsoft.EntityFrameworkCore;
using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Submission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement
{
    public interface IEndorsementDataCleaner
    {
        Task CleanUpAsync(Guid riskId);
        Task DeleteForEndorsementDataAsync(Guid riskId);
        Task DeleteForEndorsementDataRiskDetailIdAsync(Guid riskId, Guid riskDetailId);
    }

    public class EndorsementDataCleaner : IEndorsementDataCleaner
    {
        private readonly AppDbContext _context;
        private readonly IRiskRepository _riskRepository;
        private readonly IRiskDetailRepository _riskDetailRepository;

        public EndorsementDataCleaner(
            AppDbContext context,
            IRiskRepository riskRepository,
            IRiskDetailRepository riskDetailRepository
        )
        {
            _context = context;
            _riskRepository = riskRepository;
            _riskDetailRepository = riskDetailRepository;
        }

        public async Task CleanUpAsync(Guid riskId)
        {
            // Clean RiskDetail 
            var risk = await _riskRepository.FindAsync(riskId);
            var riskDetails = await _riskDetailRepository.GetAsync(x => x.RiskId == riskId);
            riskDetails = riskDetails.Where(x => x.Id != risk.ForEndorsementRiskDetailId && x.IsProcessed != true)
                            .OrderByDescending(x => x.CreatedDate);

            foreach (var riskDetail in riskDetails)
            {
                await DeleteCurrentPolicyDetailAsync(riskDetail.Id, riskDetail.SourceRiskDetailId ?? Guid.Empty);
            }
        }

        public async Task DeleteForEndorsementDataAsync(Guid riskId)
        {
            // Clean RiskDetail 
            var risk = await _riskRepository.FindAsync(riskId);
            await DeleteCurrentPolicyDetailAsync(risk.ForEndorsementRiskDetailId.Value, risk.CurrentRiskDetailId ?? Guid.Empty);
        }

        public async Task DeleteForEndorsementDataRiskDetailIdAsync(Guid riskId, Guid riskDetailId)
        {
            // Clean RiskDetail 
            var risk = await _riskRepository.FindAsync(riskId);
            await DeleteCurrentPolicyDetailAsync(riskDetailId, risk.CurrentRiskDetailId ?? Guid.Empty);
        }

        private async Task DeleteCurrentPolicyDetailAsync(Guid riskDetailId, Guid previousRiskDetailId)
        {
            var toDeleteRiskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(riskDetailId);

            if (toDeleteRiskDetail == null) { return; }

            _riskDetailRepository.Detach(toDeleteRiskDetail);

            var executionStrategy = _context.Database.CreateExecutionStrategy();
            executionStrategy.Execute(() =>
            {
                using var transaction = _context.Database.BeginTransaction();
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
                        DELETE FROM [dbo].[RiskCoveragePremiumAvg] WHERE [RiskCoveragePremiumId] IN 
                            (SELECT [Id] FROM [RiskCoveragePremium] WHERE [RiskCoverageId] IN (SELECT [Id] FROM [RiskCoverage] WHERE [RiskDetailId] = '{0}'))
                        DELETE FROM [dbo].[RiskCoveragePremium] WHERE [RiskCoverageId] IN (SELECT [Id] FROM [RiskCoverage] WHERE [RiskDetailId] = '{0}')
                        DELETE FROM [dbo].[RiskCoverage] WHERE [RiskDetailId] = '{0}'
                        DELETE FROM [dbo].[RiskHistory] WHERE [RiskDetailId] = '{0}'
                        DELETE FROM [dbo].[RiskSpecificSafetyDevice] WHERE [RiskDetailId] = '{0}'
                        DELETE FROM [dbo].[RiskSpecificRadiusOfOperation] WHERE [Id] = '{0}'
                        DELETE FROM [dbo].[RiskFiling] WHERE [RiskDetailId] = '{0}'
                        DELETE FROM [dbo].[RiskForm] WHERE [RiskDetailId] = '{0}'
                        DELETE FROM [dbo].[FileUploadDocument] WHERE [RiskDetailId] = '{0}'

                        UPDATE  [dbo].[Driver]
                        SET     [PreviousDriverVersionId] = NULL
                        WHERE   [PreviousDriverVersionId] IS NOT NULL 
                                AND [PreviousDriverVersionId] NOT IN (SELECT [DriverId] FROM [RiskDetailDriver] WHERE [RiskDetailId] = '{3}') 
                                AND [PreviousDriverVersionId] IN (SELECT [DriverId] FROM [RiskDetailDriver] WHERE [RiskDetailId] = '{0}')

                        DELETE FROM [dbo].[Driver] 
                        WHERE   [Id] NOT IN (SELECT [DriverId] FROM [RiskDetailDriver] WHERE [RiskDetailId] = '{3}') 
                                AND [Id] IN (SELECT [DriverId] FROM [RiskDetailDriver] WHERE [RiskDetailId] = '{0}')

                        UPDATE  [dbo].[Vehicle]
                        SET     [PreviousVehicleVersionId] = NULL
                        WHERE   [PreviousVehicleVersionId] IS NOT NULL 
                                AND [PreviousVehicleVersionId] NOT IN (SELECT [VehicleId] FROM [RiskDetailVehicle] WHERE [RiskDetailId] = '{3}') 
                                AND [PreviousVehicleVersionId] IN (SELECT [VehicleId] FROM [RiskDetailVehicle] WHERE [RiskDetailId] = '{0}')

                        DELETE FROM [dbo].[Vehicle]
                        WHERE   [Id] NOT IN (SELECT [VehicleId] FROM [RiskDetailVehicle] WHERE [RiskDetailId] = '{3}') 
                                AND [Id] IN (SELECT [VehicleId] FROM [RiskDetailVehicle] WHERE [RiskDetailId] = '{0}')

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

                    ", riskDetailId, toDeleteRiskDetail.Insured.EntityId, toDeleteRiskDetail.InsuredId, previousRiskDetailId);

                    _context.Database.ExecuteSqlRaw(deletePolicyDetail);
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
