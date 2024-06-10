using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Submission;
using RivTech.CABoxTruck.WebService.DTO.Enum;
using RivTech.CABoxTruck.WebService.Service.IServices.Submission;
using System;
using System.Linq;

namespace RivTech.CABoxTruck.WebService.Service.Services.Submission
{
    public class RiskSequenceService : IRiskSequenceService
    {
        private readonly string policyNumberFormat = "BRP{0}T{1}01{2}{3}00_08{4}_01";
        private readonly string policyNumberUpdateFormat = "BRP{0}T{1}01{2}{3}00_{4}";
        private readonly IRiskRepository _riskRepository;
        private readonly ISequenceRepository _sequenceRepository;

        public RiskSequenceService(IRiskRepository riskRepository, ISequenceRepository sequenceRepository)
        {
            _riskRepository = riskRepository;
            _sequenceRepository = sequenceRepository;
        }
        public string GenerateSubmissionNumber()
        {
            //Q - 2021000578

            var nextValue = _sequenceRepository.GetNextSequence(SequenceType.NextSubmissionNumber);
            return $"S-{DateTime.Today.Year}{nextValue.ToString().PadLeft(6, '0')}";
        }

        public string GenerateQuoteNumber()
        {
            var randomGenerator = new Random();
            var nextValue = _sequenceRepository.GetNextSequence(SequenceType.NextQuoteNumber);
            return $"Q-{DateTime.Today.Year}{nextValue.ToString().PadLeft(6, '0')}";
        }

        public string GeneratePolicyNumber(string state, DateTime expirationDate, DateTime inceptionDate)
        {
            var policies = _riskRepository.GetAsync(x => x.PolicyNumber != null).Result;

            string policyNumberPrefix = "CBL";
            int term = getPolicyTerm(expirationDate, inceptionDate);
            string effectiveYear = getEffectiveYear(inceptionDate);

            string nextSequence = _sequenceRepository.GetNextPolicyNumber();

            return string.Format(policyNumberFormat, policyNumberPrefix, state, term, effectiveYear, nextSequence);
        }

        public string UpdatePolicyNumber(string policyNumber, DateTime expirationDate, DateTime inceptionDate, string state)
        {
            if (!validatePolicyNumber(policyNumber))
                throw new Exception("Invalid Policy Number");
            string policyNumberPrefix = "CBL";
            string effectiveYear = getEffectiveYear(inceptionDate);
            var splitPolicyNumber = policyNumber.Split(new[] { '_' }, 2);
            int term = getPolicyTerm(expirationDate, inceptionDate);

            return string.Format(policyNumberUpdateFormat, policyNumberPrefix, state, term, effectiveYear, splitPolicyNumber[1]);
        }

        public string GenerateRenewalPolicyNumber(string policyNumber)
        {
            if (!validatePolicyNumber(policyNumber))
                throw new Exception("Invalid Policy Number");
            var splitPolicyNumber = policyNumber.Split(new[] { '_' });
            var lastRenewalNum = Int32.Parse(splitPolicyNumber[2]);
            lastRenewalNum++;

            splitPolicyNumber[2] = lastRenewalNum.ToString("D2");
            var renewalPolicyNumber = string.Join("_", splitPolicyNumber);
            var isExist = _riskRepository.AnyAsync(x => x.PolicyNumber == renewalPolicyNumber).Result;
            if (isExist)
                throw new Exception("Renewal Policy Number Already Exist");

            return renewalPolicyNumber;
        }

        public string GenerateRenewalPolicyNumber(string policyNumber, string state, DateTime expirationDate, DateTime inceptionDate)
        {
            if (!validatePolicyNumber(policyNumber))
                throw new Exception("Invalid Policy Number");
            var splitPolicyNumber = policyNumber.Split(new[] { '_' });
            var lastRenewalNum = Int32.Parse(splitPolicyNumber[2]);
            lastRenewalNum++;

            splitPolicyNumber[2] = lastRenewalNum.ToString("D2");
            var renewalPolicyNumber = string.Join("_", splitPolicyNumber);
            var isExist = _riskRepository.AnyAsync(x => x.PolicyNumber == renewalPolicyNumber).Result;
            if (isExist)
            {
                // Generate new policynumber retain Renewal sequence
                var newPolicyNum = GeneratePolicyNumber(state, expirationDate, inceptionDate);
                var splitNewPolicyNumber = newPolicyNum.Split(new[] { '_' });
                splitNewPolicyNumber[2] = lastRenewalNum.ToString("D2");
                return string.Join("_", splitNewPolicyNumber);
            }

            return renewalPolicyNumber;
        }

        public bool validatePolicyNumber(string policyNumber)
        {
            if (string.IsNullOrEmpty(policyNumber))
                return false;

            var splitPolicyNumber = policyNumber.Split(new[] { '_' });

            // Policynumber should be split into 3
            if (splitPolicyNumber.Count() != 3)
                return false;

            var firstPart = splitPolicyNumber[0];
            // 15 characters
            if (firstPart.Length != 15)
                return false;
            // Validate first index format
            if (firstPart.Substring(0, 3) != "BRP")
                return false;
            if (firstPart.Substring(6, 1) != "T")
                return false;
            if (firstPart.Substring(9, 2) != "01")
                return false;
            if (firstPart.Substring(13) != "00")
                return false;

            int a;
            if (Int32.TryParse(firstPart.Substring(3, 3), out a) != false)
                return false;
            
            var secondPart = splitPolicyNumber[1];
            // 6 characters
            if (secondPart.Length != 6)
                return false;
            // Validate second index format
            if (secondPart.Substring(0, 2) != "08")
                return false;

            var thirdPart = splitPolicyNumber[2];
            // 2 characters
            if (thirdPart.Length != 2)
                return false;
            int b;
            if (Int32.TryParse(thirdPart, out b) != true)
                return false;

            return true;
        }

        private int getPolicyTerm(DateTime expirationDate, DateTime inceptionDate)
        {
            int policyTerm = -1;
            if (expirationDate < inceptionDate.AddYears(1)) policyTerm = 0;
            if (expirationDate < inceptionDate.AddMonths(13) && expirationDate >= inceptionDate.AddYears(1)) policyTerm = 1;
            if (expirationDate >= inceptionDate.AddMonths(13) && expirationDate <= inceptionDate.AddYears(2)) policyTerm = 2;
            if (expirationDate > inceptionDate.AddYears(2) && expirationDate <= inceptionDate.AddYears(3)) policyTerm = 3;
            if (expirationDate > inceptionDate.AddYears(3) && expirationDate <= inceptionDate.AddYears(4)) policyTerm = 4;
            if (expirationDate > inceptionDate.AddYears(4) && expirationDate <= inceptionDate.AddYears(5)) policyTerm = 5;
            if (expirationDate > inceptionDate.AddYears(5) && expirationDate <= inceptionDate.AddYears(6)) policyTerm = 6;
            if (expirationDate > inceptionDate.AddYears(6) && expirationDate <= inceptionDate.AddYears(7)) policyTerm = 7;
            if (expirationDate > inceptionDate.AddYears(7) && expirationDate <= inceptionDate.AddYears(8)) policyTerm = 8;
            if (expirationDate > inceptionDate.AddYears(8) && expirationDate <= inceptionDate.AddYears(9)) policyTerm = 9;

            return policyTerm;
        }

        private string getEffectiveYear(DateTime inceptionDate)
        {
            int currentYear = Convert.ToInt32(inceptionDate.ToString("yy"));
            currentYear = currentYear - 20;
            if (currentYear <= 15)
            {
                return currentYear.ToString("X"); // In year 2035 the value will be F.
            }
            else
            {
                switch (currentYear)
                {
                    case 16: return "G";
                    case 17: return "H";
                    case 18: return "I";
                    case 19: return "J";
                    case 20: return "K";
                    case 21: return "L";
                    case 22: return "M";
                    case 23: return "N";
                    case 24: return "O";
                    case 25: return "P";
                    case 26: return "Q";
                    case 27: return "R";
                    case 28: return "S";
                    case 29: return "T";
                    case 30: return "U";
                    case 31: return "V";
                    case 32: return "W";
                    case 33: return "X";
                    case 34: return "Y";
                    case 35: return "Z"; // In year 2055
                    default: return "";
                }
            }
        }
    }
}
