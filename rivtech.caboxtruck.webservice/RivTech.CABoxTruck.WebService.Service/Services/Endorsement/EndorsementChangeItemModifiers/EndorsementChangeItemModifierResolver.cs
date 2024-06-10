using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using System;
using System.Collections.Generic;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItemModifiers
{
    public class EndorsementChangeItemModifierResolver : IEndorsementChangeItemModifierResolver 
    {
        private readonly IEnumerable<IEndorsementChangeItemModifier> _endorsementChangeItemModifiers;

        public EndorsementChangeItemModifierResolver(
            IEnumerable<IEndorsementChangeItemModifier> endorsementChangeItemModifiers
        ) {
            _endorsementChangeItemModifiers = endorsementChangeItemModifiers;
        }

        public IEndorsementChangeItemModifier Resolve(IEndorsementChangeItem changeItem) 
        {
            foreach(var modifier in _endorsementChangeItemModifiers)
            {
                var type = modifier.GetType();
                if(type.BaseType.GenericTypeArguments.Length == 1)
                {
                    var genericArgs = type.BaseType.GenericTypeArguments[0];
                    if (genericArgs.Equals(changeItem.GetType()))
                        return modifier;    
                }
            }
            throw new ArgumentException($"No suitable modifier found for changeItem {changeItem.Type}.");
        }
    }
}
