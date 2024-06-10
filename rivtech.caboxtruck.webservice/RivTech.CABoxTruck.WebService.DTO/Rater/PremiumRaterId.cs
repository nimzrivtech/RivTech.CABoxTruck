using System;

namespace RivTech.CABoxTruck.WebService.DTO.Rater
{
    public class PremiumRaterId
    {
        public PremiumRaterId(string submissionNumber, string optionId)
        {
            SubmissionNumber = submissionNumber;
            OptionId = optionId;
        }

        public string SubmissionNumber { get; }
        public string OptionId { get; }

        public override bool Equals(object obj)
        {
            return obj is PremiumRaterId other
                && SubmissionNumber.Equals(other.SubmissionNumber)
                && OptionId.Equals(other.OptionId);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(SubmissionNumber, OptionId);
        }
    }
}
