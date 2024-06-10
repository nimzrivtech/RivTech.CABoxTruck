using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using System.Reflection;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItemModifiers
{
    public abstract class ChangeModifierBase<T> : IEndorsementChangeItemModifier where T : IEndorsementChangeItem
    {
        public abstract Task ModifyAsync(IEndorsementData endorsementData, IEndorsementChangeItem changeItem);

        protected T CastOriginalType(IEndorsementChangeItem endorsementChangeItem)
        {
            return (T) endorsementChangeItem;
        }

        protected bool IsChangeItemPropertiesEmpty(IEndorsementChangeItem changeItem)
        {
            var changeItemProperties = changeItem.GetType().GetProperties();

            //Checks each input if at least one property in the change item is not set to null
            foreach (PropertyInfo changeItemProperty in changeItemProperties)
            {
                if (changeItemProperty.Name == "Type" ||
                    changeItemProperty.Name == "DisplayText")
                    continue;

                if (changeItemProperty.GetValue(changeItem) != null)
                    return false;
            }

            return true;
        }
    }
}