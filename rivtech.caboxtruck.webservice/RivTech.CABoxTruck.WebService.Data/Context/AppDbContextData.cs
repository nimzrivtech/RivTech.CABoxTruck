using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.Data.Entity.DataSets;
using RivTech.CABoxTruck.WebService.Data.Entity.Submission.Risk_Specifics;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Data.Context
{
    public static class AppDbContextData
    {
        public static async Task Initialize(AppDbContext context)
        {
            context.ChangeTracker.DetectChanges();
            await context.SaveChangesAsync();
        }

        public static async Task SeedDataSets(AppDbContext context)
        {
            SeedTableShort<AdditionalInterestType>(context);
            SeedTableString<RiskStatus>(context);
            SeedTableShort<BackgroundCheckOption>(context);
            SeedTableShort<DriverTrainingOption>(context);
            SeedTableShort<MvrPullingFrequencyOption>(context);
            SeedTableShort<ChameleonIssuesOption>(context);
            SeedTableShort<SaferRatingOption>(context);
            SeedTableShort<TrafficLightRatingOption>(context);
            SeedTableShort<NoteCategoryOption>(context);
            //SeedTableShort<PricingOption>(context);
            SeedTableShort<BindOption>(context);
            SeedTableShort<BindStatus>(context);
            SeedTableShort<Subjectivities>(context);
            SeedTableShort<BusinessType>(context);
            SeedTableShort<Symbol>(context);
            SeedTableShort<CreditedOffice>(context);
            SeedTableShort<BindRequirementsOption>(context);
            SeedTableShort<QuoteConditionsOption>(context);
            SeedTableShort<PaymentTypes>(context);
            SeedTableShort<MainUse>(context);
            SeedUpdateTableShort<FileCategory>(context);
            SeedUpdateTableShort<CancellationReasons>(context);
            SeedUpdateTableShort<PolicyContactTypes>(context);
            await context.SaveChangesAsync();
        }

        private static void SeedTableString<T>(AppDbContext context) where T : Enumeration<string>, new()
        {
            var valuesToSeed = Enumeration<string>.GetAll<T>();
            foreach (var value in valuesToSeed)
            {
                if (!context.Set<T>().Any(x => x.Id == value.Id))
                {
                    context.Set<T>().Add(value);
                }
            }
        }

        private static void SeedTableShort<T>(AppDbContext context) where T : Enumeration<short>, new()
        {
            var valuesToSeed = Enumeration<short>.GetAll<T>();
            foreach (var value in valuesToSeed)
            {
                if (!context.Set<T>().Any(x => x.Id == value.Id))
                {
                    context.Set<T>().Add(value);
                }
            }
        }

        public static void SeedUpdateTableShort<T>(AppDbContext context) where T : Enumeration<short>, new()
        {
            var valuesToSeed = Enumeration<short>.GetAll<T>();
            foreach (var value in valuesToSeed)
            {
                var data = context.Set<T>().Where(x => x.Id == value.Id).FirstOrDefault();
                if (data == null)
                {
                    context.Set<T>().Add(value);
                }
                else
                {
                    if (data.Description != value.Description)
                    {
                        data.Description = value.Description;
                        context.Set<T>().Update(data);
                    }
                }
            }
        }

        public static IEnumerable<LvAdditionalTerm> LvAdditionalTerms => new LvAdditionalTerm[]
            {
                new LvAdditionalTerm
                { 
                    IsActive = true,
                    Description = "1 Year Term",
                },
                new LvAdditionalTerm
                {
                    IsActive = true,
                    Description = "2 Year Term",
                },
                new LvAdditionalTerm
                {
                    IsActive = true,
                    Description = "3 Year Term",
                },
                new LvAdditionalTerm
                {
                    IsActive = true,
                    Description = "4 Year Term",
                },
                new LvAdditionalTerm
                {
                    IsActive = true,
                    Description = "5 Year Term",
                },
                new LvAdditionalTerm
                {
                    IsActive = true,
                    Description = "6 Year Term",
                },
                new LvAdditionalTerm
                {
                    IsActive = true,
                    Description = "7 Year Term",
                },
            };

        public static IEnumerable<LvRateType> LvRateTypes => new LvRateType[]
          {
                new LvRateType
                {
                    IsActive = true,
                    Description = "Flat/Non-Auditable",
                },
                new LvRateType
                {
                    IsActive = true,
                    Description = "Specific",
                },
          };

        public static IEnumerable<LvRetroDateType> LvRetroDateTypes => new LvRetroDateType[]
          {
                new LvRetroDateType
                {
                    IsActive = true,
                    Description = "None",
                },
                new LvRetroDateType
                {
                    IsActive = true,
                    Description = "Not Applicable",
                },
                new LvRetroDateType
                {
                    IsActive = true,
                    Description = "Inception Date",
                },
                new LvRetroDateType
                {
                    IsActive = true,
                    Description = "Specific Date",
                },
                new LvRetroDateType
                {
                    IsActive = true,
                    Description = "Beginning Of Time",
                },
          };

        public static IEnumerable<LvQuestionCategory> LvQuestionCategories => new LvQuestionCategory[]
        {
                new LvQuestionCategory
                {
                    Id = 1,
                    Description = "Coverage",
                },
                new LvQuestionCategory
                {
                    Id = 1,
                    Description = "Risk specific",
                }
        };

        public static IEnumerable<LvQuestionSection> LvQuestionSections => new LvQuestionSection[]
        {
            new LvQuestionSection
            {
                Id = 1,
                Description = "Requested Coverage",
            },
            new LvQuestionSection
            {
                Id = 2,
                Description = "Historical Coverage",
            },
            new LvQuestionSection
            {
                Id = 3,
                Description = "Coverage Question",
            },
            new LvQuestionSection
            {
                Id = 4,
                Description = "General Liability Coverage",
            },
            new LvQuestionSection
            {
                Id = 5,
                Description = "Cargo Coverage",
            },
            new LvQuestionSection
            {
                Id = 6,
                Description = "General Information",
            },
            new LvQuestionSection
            {
                Id = 7,
                Description = "Driver Information",
            },
            new LvQuestionSection
            {
                Id = 1,
                Description = "Driver Hiring Criteria",
            },
            new LvQuestionSection
            {
                Id = 8,
                Description = "Maintenance Question",
            },
            new LvQuestionSection
            {
                Id = 9,
                Description = "Underwriting Question",
            }
        };

        public static IEnumerable<LvQuestionType> LvQuestionTypes => new LvQuestionType[]
        {
            new LvQuestionType
            {
                Id = 1,
                Description = "Checkbox",
            },
            new LvQuestionType
            {
                Id = 2,
                Description = "Date",
            },
            new LvQuestionType
            {
                Id = 3,
                Description = "Text input",
            },
            new LvQuestionType
            {
                Id = 4,
                Description = "Text area",
            },
            new LvQuestionType
            {
                Id = 5,
                Description = "Select dropdown",
            },
            new LvQuestionType
            {
                Id = 6,
                Description = "Multi-select dropdown",
            },
            new LvQuestionType
            {
                Id = 7,
                Description = "Switch",
            },
            new LvQuestionType
            {
                Id = 8,
                Description = "Radiobutton",
            }
        };

        public static IEnumerable<LvValidationType> LvQuestionValidationTypes => new LvValidationType[]
        {
            new LvValidationType
            {
                Id = 1,
                Description = "Min",
            },
            new LvValidationType
            {
                Id = 2,
                Description = "Max",
            },
            new LvValidationType
            {
                Id = 3,
                Description = "Format",
            }
        };

        public static IEnumerable<LvRiskResponseType> LvRiskResponseTypes => new LvRiskResponseType[]
        {
            new LvRiskResponseType
            {
                Id = 1,
                Description = "boolean",
            },
            new LvRiskResponseType
            {
                Id = 2,
                Description = "int",
            },
            new LvRiskResponseType
            {
                Id = 3,
                Description = "decimal",
            },
            new LvRiskResponseType
            {
                Id = 4,
                Description = "string",
            },
            new LvRiskResponseType
            {
                Id = 5,
                Description = "date",
            }
        };

        public static IEnumerable<LvRiskSpecificSafetyDeviceCategory> LvRiskSpecificSafetyDeviceCategory => new LvRiskSpecificSafetyDeviceCategory[]
        {
            new LvRiskSpecificSafetyDeviceCategory
            {
                Id = 1,
                Description = "Cameras",
            },
            new LvRiskSpecificSafetyDeviceCategory
            {
                Id = 2,
                Description = "Accident Event Recorders (AER`s)",
            },
            new LvRiskSpecificSafetyDeviceCategory
            {
                Id = 3,
                Description = "Location Tracking Device",
            },
            new LvRiskSpecificSafetyDeviceCategory
            {
                Id = 4,
                Description = "Geographic Driving History Data",
            },
            new LvRiskSpecificSafetyDeviceCategory
            {
                Id = 5,
                Description = "Mileage Tracking Device",
            },
            new LvRiskSpecificSafetyDeviceCategory
            {
                Id = 5,
                Description = "Brake Warning System",
            },
            new LvRiskSpecificSafetyDeviceCategory
            {
                Id = 5,
                Description = "Distracted Driving Warning System",
            },
            new LvRiskSpecificSafetyDeviceCategory
            {
                Id = 5,
                Description = "Speed Warning System",
            },
            new LvRiskSpecificSafetyDeviceCategory
            {
                Id = 5,
                Description = "Monitoring of Harsh Braking/Speeding/Accelerating via Telematics Device",
            },
            new LvRiskSpecificSafetyDeviceCategory
            {
                Id = 5,
                Description = "Any Active Accident Avoidance Technology",
            },
            new LvRiskSpecificSafetyDeviceCategory
            {
                Id = 5,
                Description = "Reflective Tape",
            },
            new LvRiskSpecificSafetyDeviceCategory
            {
                Id = 5,
                Description = "Step Stools",
            },
            new LvRiskSpecificSafetyDeviceCategory
            {
                Id = 5,
                Description = "1-800-HOWSMYDRIVING Program",
            },
            new LvRiskSpecificSafetyDeviceCategory
            {
                Id = 5,
                Description = "No Texting/Cell Phone/Handheld Device Usage While Driving",
            },
            new LvRiskSpecificSafetyDeviceCategory
            {
                Id = 5,
                Description = "Electronic Logging Devices (ELDs)",
            }
        };
    }
}