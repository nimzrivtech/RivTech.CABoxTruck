using System;

namespace RivTech.CABoxTruck.WebService.Data.Entity
{
    public class LvQuestionCategory : Enumeration<short>    
    {
        public static LvQuestionCategory Coverage => new LvQuestionCategory(1, "Coverage");
        public static LvQuestionCategory RiskSpecific => new LvQuestionCategory(2, "Risk specific");
        public static LvQuestionCategory TestCategory => new LvQuestionCategory(2, "Test Category");

        public LvQuestionCategory() { IsActive = true; }
        public LvQuestionCategory(Int16 id, string description) : base(id, description) { IsActive = true; }
    }
}