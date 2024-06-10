using Newtonsoft.Json;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Submission;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeComparer;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItems;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeComparers
{
    public interface IChangeComparerContext
    {
        Task CompareChanges(Guid riskId);
        Task CompareChangesForRatingFactors(Guid riskId);

        /// <summary>
        /// Comapares last issued RiskDetail with for endorsement RiskDetail and returns the resulting change items. 
        /// Returned change items are not yet persisted.
        /// </summary>
        Task<IEndorsementChangeCollection> CompareChangesWithComparer(Guid riskId, Type comparer);
        Task<IEndorsementChangeCollection> CompareChanges(Guid newRiskDetailId, Guid oldRiskDetailId);
    }

    public class ChangeComparerContext : IChangeComparerContext
    {
        private readonly IRiskRepository _riskRepository;
        private readonly IEnumerable<IChangeComparer> _changeComparers;
        private readonly IRiskDetailRepository _riskDetailRepository;
        private readonly IEndorsementChangesService _endorsementChangesService;
        private readonly IRiskService _riskService;

        public ChangeComparerContext(
            IRiskRepository riskRepository,
            IEnumerable<IChangeComparer> changeComparers,
            IRiskDetailRepository riskDetailRepository,
            IEndorsementChangesService endorsementChangesService,
            IRiskService riskService
        )
        {
            _riskRepository = riskRepository;
            _changeComparers = changeComparers;
            _riskDetailRepository = riskDetailRepository;
            _endorsementChangesService = endorsementChangesService;
            _riskService = riskService;
        }

        public async Task CompareChanges(Guid riskId)
        {
            var changeCollection = new EndorsementChangeCollection();
            var risk = await _riskRepository.GetRiskByIdAsync(riskId);

            var oldRisDTO = await _riskService.GetByIdIncludeAsync(risk.CurrentRiskDetailId.Value);
            var newRiskDTO = await _riskService.GetByIdIncludeAsync(risk.ForEndorsementRiskDetailId.Value);

            foreach (var comparer in _changeComparers)
            {
                var changeItems = await comparer.Compare(newRiskDTO, oldRisDTO);
                if (changeItems != null)
                {
                    foreach (var item in changeItems)
                    {
                        if (item != null)
                            changeCollection.Add(item);
                    }
                }
            }

            await _endorsementChangesService.UpdateEndorsementChangesAsync(changeCollection, riskId, risk.NextEndorsementNumber());
        }
 
        public async Task<IEndorsementChangeCollection> CompareChangesWithComparer(Guid riskId, Type comparerType)
        {
            var changeCollection = new EndorsementChangeCollection();
            var risk = await _riskRepository.GetRiskByIdAsync(riskId);

            var oldRisDTO = await _riskService.GetByIdIncludeAsync(risk.CurrentRiskDetailId.Value);
            var newRiskDTO = await _riskService.GetByIdIncludeAsync(risk.ForEndorsementRiskDetailId.Value);

            IChangeComparer comparer = _changeComparers.FirstOrDefault(x => x.GetType().Equals(comparerType));
            if (comparer is null)
                throw new InvalidOperationException($"No change comparer found for {comparerType.Name}.");

            var changeItems = await comparer.Compare(newRiskDTO, oldRisDTO);
            if (changeItems != null)
            {
                foreach (var item in changeItems)
                {
                    if (item != null)
                        changeCollection.Add(item);
                }
            }

            return changeCollection;
        }

        public async Task CompareChangesForRatingFactors(Guid riskId)
        {
            var ratingFactorComparer = _changeComparers.FirstOrDefault(x => x is RatingFactorsComparer);
            if (ratingFactorComparer is null)
                return;

            var risk = await _riskRepository.GetRiskByIdAsync(riskId);
            var changeCollection = await _endorsementChangesService.GetChangesByRiskAndEndorsement(riskId, risk.NextEndorsementNumber());

            var oldRisDTO = await _riskService.GetByIdIncludeAsync(risk.CurrentRiskDetailId.Value);
            var newRiskDTO = await _riskService.GetByIdIncludeAsync(risk.ForEndorsementRiskDetailId.Value);

            var existingChangeItem = changeCollection.FirstOrDefault(x => x is RatingFactorsChangeItem); 
            changeCollection.Remove(existingChangeItem);

            var changeItems = await ratingFactorComparer.Compare(newRiskDTO, oldRisDTO);
            if (changeItems != null)
            {
                foreach (var item in changeItems)
                {
                    if (item != null)
                        changeCollection.Add(item);
                }
            }

            await _endorsementChangesService.UpdateEndorsementChangesAsync(changeCollection, riskId, risk.NextEndorsementNumber());
        }

        public async Task<IEndorsementChangeCollection> CompareChanges(Guid newRiskDetailId, Guid oldRiskDetailId)
        {
            var changeCollection = new EndorsementChangeCollection();

            var oldRisDTO = await _riskService.GetByIdIncludeAsync(oldRiskDetailId);
            var newRiskDTO = await _riskService.GetByIdIncludeAsync(newRiskDetailId);

            foreach (var comparer in _changeComparers)
            {
                var changeItems = await comparer.Compare(newRiskDTO, oldRisDTO);
                if (changeItems != null)
                {
                    foreach (var item in changeItems)
                    {
                        if (item != null)
                            changeCollection.Add(item);
                    }
                }
            }

            return changeCollection;
        }
    }
}
