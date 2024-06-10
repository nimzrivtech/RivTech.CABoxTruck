using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems
{
    /// <summary>
    ///  Collection interface of IEndorsementChangeItem.
    /// </summary>
    public class EndorsementChangeCollection : IEndorsementChangeCollection
    {
        private ICollection<IEndorsementChangeItem> _items;


        public EndorsementChangeCollection()
        {
            _items = new List<IEndorsementChangeItem>();
        }

        public EndorsementChangeCollection(IEnumerable<IEndorsementChangeItem> items)
        {
            _items = items?.ToList() ?? new List<IEndorsementChangeItem>();
        }

        public void Add(IEndorsementChangeItem changeItem)
        {
            if (changeItem is null)
                return;
            _items.Add(changeItem);
        }

        public void Remove(IEndorsementChangeItem changeItem)
        {
            if(_items.Any(x => x == changeItem ))
            {
                _items.Remove(changeItem);
            }
        }

        public IEnumerator<IEndorsementChangeItem> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }
    }
}
