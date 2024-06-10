using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RivTech.CABoxTruck.WebService.DomainEntities.Endorsement
{
    /// <summary>
    /// Collections of current risk endorsement points using Stack data structure.
    /// </summary>
    public class RiskEndorsementPoints : Stack<EndorsementPoint>, ICloneable
    {
        public RiskEndorsementPoints() : base(new List<EndorsementPoint>()){ 

        }

        public RiskEndorsementPoints(IEnumerable<EndorsementPoint> endorsementPoints) : base(endorsementPoints)
        { 
        }

        /// <summary>
        /// Returns the <c>EndorsementPoint</c> which will be merged with changes by given date.  
        /// </summary>
        public EndorsementPoint FindPointBeforeDate(DateTime date)
        {
            var stackCopy = (RiskEndorsementPoints) Clone();
            EndorsementPoint currentPoint = stackCopy.Pop();
            while (stackCopy.Any() && currentPoint.EffectiveDate > date)
            {
                currentPoint = stackCopy.Pop();
            }
            return currentPoint;
        }

        public object Clone()
        {
            var clone = new RiskEndorsementPoints();
            foreach(var point in this.Reverse())
            {
                clone.Push(point);
            }
            return clone;
        }

        public int GetNextEndorsementNumber()
        {
            return this.Max(x => x.EndorsementNumber) + 1;
        }
    }
}
