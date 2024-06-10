export const raterMappingTemplate = {
  SubmissionNo: "",
  riskDetailId: "",
  RiskCoverageId: "",
  OptionId: null,
  RatingFactors: {
    LiabilityScheduleRatingFactor: null,
    LiabilityExperieceRatingFactor: null,
    GLScheduleRatingFactor: null,
    GLExperienceRatingFactor: null,
    CargoScheduleRatingFactor: null,
    CargoExperienceRatingFactor: null,
    APDScheduleRatingFactor: null,
    APDExperienceRatingFactor: null
  },
  Broker: {
    EffectiveDate: null,
    CommissionAL: null,
    CommissionPD: null
  },
  Applicant: {
    BusinessDetails: {
      UseClass: null,
      AccountCategory: null,
      YearsInBusiness: null,
      BusinessType: null,
      ZipCode: null,
      OperatingZipCode: null,
      NewVenturePreviousExperience: null,
      MainGVW: null,
      SecondaryBusinessGroup: null,
    }
  },
  Policy: {
    InceptionDate: null
  },
  Driver: {
    UseDriverRatingTab: null,
    UseAccidentsOrViolationInfo: null,
    KOUnder25: null,
    KOAFOrMovingViolations: null,
    DriverList: []
  },
  Vehicle: {
    TotalVehicles: null,
    RiskManagementFeeAL: null,
    RiskManagementFeePD: null,
    VehicleDropdowns: { //ignore this on mapping. used only in datasets
      VehicleType: null,
      Description: null,
      CompDeductible: null,
      CollDeductible: null,
      UseClass: null,
      BusinessClass: null,
    },
    VehicleList: [
    ]
  },
  RequestedCoverages: {
    AutoLiability: {
      AutoBodilyInjury: null, // need list
      AutoPropertyDamage: null,// need list
      AutoLiabilityDeductible: null,// need list
      MedicalPaymentsLimit: null,// need list
      PIPLimit: null,// need list
      UMSCLLimit: null,// need list
      UMPDLimit: null,// need list
      UIMSCLLimit: null,// need list
      UIMPDLimit: null,// need list
      UMStacking: null,
      SymbolsRequested: null// need list
    },
    PhysicalDamage: {
      FireTheftSpecPerils: null,// need list
      COMPDeductible: null,// need list
      COLLDeductible: null,// need list
      PdSymbol: null// need list
    },
    OtherCoverages: {
      GLBodilyInjuryLimits: null,// need list
      CargoCollLimit: null,// need list
      RefrigeratedCargoLimits: null,// need list
      TrailerInterchangeLimit: null,
      NonOwnedTrailerLimit: null,
      TrailerType: null,
      PollutionCoverageLimit: null
    }
  },
  RiskSpecifics: {
    DriverInfo: {
      TotalDrivers: null,
      AnyVehiclesOperatedForPersonalUse: null,
      AnnualCostOfHire: null
    },
    GeneralLiability: {
      IsOperationAtStorageLot: null,
      Payroll: null
    },
    SafetyDevices: {
      SafetyDeviceCategory: {
        Cameras: {
          CameraDiscount: null
        },
        BrakeWarningSystem: null,
        SpeedWarningSystem: null,
        MonitoringViaTelematicsDevice: null,
        AnyActiveAccidentAvoidanceTechnology: null,
        AnyPassiveAccidentAvoidanceTechnology: null,
        DistractedDrivingEquipmentDiscount: null,
        MileageTrackingEquipment: null,
        NavigationOrGPSDataAvailable: null
      }
    },
    DriverHiringCriteria: {
      BackgroundCheckIncluded: {
        WrittenApplication: null,
        RoadTest: null,
        WrittenTest: null,
        FullMedical: null,
        DrugTesting: null,
        CurrentMVR: null,
        ReferenceChecks: null,
        CriminalBackgroundCheck: null
      },
      ReportAllDriversToRivington: null,
      IsDisciplinaryPlanDocumentedForAllDrivers: null,
      AllDriversProperlyLicensedAndDOTCompliant: null,
      AllDriversDrivingSimilarVehicleCommerciallyFor2YrsPlus: null,
      AllDriversHaveAtLeast5YrsUSDrivingExperience: null
    },
    MaintenanceQuestions: {
      VehicleMaintenanceProgram: {
        ServiceRecordForEachVehicle: null,
        ControlledAndFrequentInspections: null,
        VehicleDailyConditionReports: null,
      },
      WrittenMaintenanceProgram: null,
      WrittenDriverTrainingProgram: null,
      WrittenSafetyProgram: null,
      WrittenAccidentReportingProcedures: null,
      IsMaintenanceProgramManagedByCompany: null,
      DoYouProvideCompleteMaintenanceOnAllVehicles: null,
      DriverFilesAvailableForReview: null,
      AccidentFilesAvailableForReview: null,
    },
    DOTMiscellaneous: {
      PolicyLevelAccicdents: null
    },
    DestinationInformation: {
      OwnerOperator: null,
      Radius0To50Miles: null,
      Radius50to200Miles: null,
      RadiusAbove200Miles: null,
      AvgMilesPerVehicle: null,
      Destination1: null,
      Destination2: null,
      Destination3: null,
      Destination4: null,
      Destination5: null,
      Destinationpct1: null,
      Destinationpct2: null,
      Destinationpct3: null,
      Destinationpct4: null,
      Destinationpct5: null,
      RatingTypes: null,
    },
    DOTInformation: {
      RiskRequiresDOTNumber: null,
      RiskHasDOTNumber: null,
      RiskHasInterstateAuthority: null,
      RiskPlansToStayIntrastate: null,
      DOTRegistrationDate: null,
      DOTCurrentlyActive: null,
      ChameleonIssues: null,
      CurrentSaferRating: null,
      ISSCAB: null,
      BasicAlert: {
        UnsafeDrivingBasicAlertOrWorse: null,
        HoursOfServiceBasicAlertOrWorse: null,
        DriverFitnessBasicAlertOrWorse: null,
        ControlledSubstanceBasicAlertOrWorse: null,
        VehicleMaintenanceBasicAlertOrWorse: null,
        CrashBasicAlertOrWorse: null,
      }
    },
    Cargo: {
      CommoditiesHauled: {
        Commodity1: null,
        CarryPercentage1: null,
        Commodity2: null,
        CarryPercentage2: null,
        Commodity3: null,
        CarryPercentage3: null,
        Commodity4: null,
        CarryPercentage4: null,
        Commodity5: null,
        CarryPercentage5: null
      }
    }
  },
  HistoricalCoverage: {
    HistoricalInsuranceCoverage: {
      NoOfVehiclesCurrent: null,
      PolicyInceptionStartDateCurrent: null,
      PolicyEndDateCurrent: null,
      LiabilityLimitsCurrent: null,
      LiabilityDeductibleCurrent: null,
      PowerUnitsCurrent: null,
      PolicyInceptionStartDateY1: null,
      PolicyEndDateY1: null,
      LiabilityLimitsY1: null,
      LiabilityDeductibleY1: null,
      PowerUnitsY1: null,
      PolicyInceptionStartDateY2: null,
      PolicyEndDateY2: null,
      LiabilityLimitsY2: null,
      LiabilityDeductibleY2: null,
      PowerUnitsY2: null,
      PolicyInceptionStartDateY3: null,
      PolicyEndDateY3: null,
      LiabilityLimitsY3: null,
      LiabilityDeductibleY3: null,
      PowerUnitsY3: null,
      PolicyInceptionStartDateY4: null,
      PolicyEndDateY4: null,
      LiabilityLimitsY4: null,
      LiabilityDeductibleY4: null,
      PowerUnitsY4: null
    },
    ProjectionsAndHistoricalFigures: {
      GrossFleetMileage: null
    }
  },
  AdditionalInterest: {
    NoOfAdditionalInsured: null,
    WaiverOfSubrogation: null,
    PrimaryNonContributoryCoverage: null,
    AdditionalInsuredPremium: null,
    PrimaryNonContributoryPremium: null,
    WaiverOfSubrogationPremium: null
  },
  ClaimsHistory: {
    LiabilityClaimsInfo: {
      LossRunDateCurrent: null,
      LossRunDateY1: null,
      LossRunDateY2: null,
      LossRunDateY3: null,
      LossRunDateY4: null,
      LossesGroundUpOrNetCurrent: null,
      LossesGroundUpOrNetY1: null,
      LossesGroundUpOrNetY2: null,
      LossesGroundUpOrNetY3: null,
      LossesGroundUpOrNetY4: null,
      BIPIPMPIncLossCurrent: null,
      BIPIPMPIncLossY1: null,
      BIPIPMPIncLossY2: null,
      BIPIPMPIncLossY3: null,
      BIPIPMPIncLossY4: null,
      PDIncLossCurrent: null,
      PDIncLossY1: null,
      PDIncLossY2: null,
      PDIncLossY3: null,
      PDIncLossY4: null,
      BIPIPClaimCountCurrent: null,
      BIPIPClaimCountY1: null,
      BIPIPClaimCountY2: null,
      BIPIPClaimCountY3: null,
      BIPIPClaimCountY4: null,
      PDClaimCountCurrent: null,
      PDClaimCountY1: null,
      PDClaimCountY2: null,
      PDClaimCountY3: null,
      PDClaimCountY4: null,
      OpenClaimsUnder400Current: null,
      OpenClaimsUnder400Y1: null,
      OpenClaimsUnder400Y2: null,
      OpenClaimsUnder400Y3: null,
      OpenClaimsUnder400Y4: null
    }
  },
  EndorsementPremiums: {
    HiredPhysicalDamage: null,
    TrailerInterchange: null,
    ALManuscriptPremium: null,
    PDManuscriptPremium: null
  }
};
