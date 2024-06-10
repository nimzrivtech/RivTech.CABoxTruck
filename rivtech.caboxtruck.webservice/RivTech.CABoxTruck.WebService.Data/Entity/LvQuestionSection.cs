using System;

namespace RivTech.CABoxTruck.WebService.Data.Entity
{
    public class LvQuestionSection : Enumeration<short>
    {
        public static LvQuestionSection MaintenanceQuestion => new LvQuestionSection(11, "Maintenance Question");
        public static LvQuestionSection SafetyDevice => new LvQuestionSection(14, "Safety Device");
        public static LvQuestionSection GeneralLiability => new LvQuestionSection(12, "General Liability");
        public static LvQuestionSection Cargo => new LvQuestionSection(13, "Cargo");
        
        public LvQuestionSection() { IsActive = true; }
        public LvQuestionSection(Int16 id, string description) : base(id, description) { IsActive = true;  }
    }
}

