export class ValidationListModel {
  constructor(
    public applicantNameAndAddress?: boolean,
    public applicantNameAndAddressBinding?: boolean,
    public businessDetails?: boolean,
    public businessDetailsBinding?: boolean,
    public filingsInfo?: boolean,
    public additionalInterest?: boolean,
    public additionalInterestBinding?: boolean,
    public broker?: boolean,
    public brokerBinding?: boolean,
    public appliedContactBinding?: boolean,
    public limits?: boolean,
    public vehicle?: boolean,
    public driver?: boolean,
    public historicalCoverages?: boolean,
    public claimsHistory?: boolean,
    public riskSpecifics?: boolean,
    public destinationInfo?: boolean,
    public driverInfo?: boolean,
    public dotInfo?: boolean,
    public maintenanceSafety?: boolean,
    public generalLiabilityCargo?: boolean,
    public underwritingQuestions?: boolean,
    public quoteOptions?: boolean,
    public bind?: boolean,
    public uwAnswersConfirmed?: any
  ) {
    this.applicantNameAndAddress = applicantNameAndAddress;
    this.applicantNameAndAddressBinding = applicantNameAndAddressBinding;
    this.businessDetails = businessDetails;
    this.businessDetailsBinding = businessDetailsBinding;
    this.filingsInfo = filingsInfo;
    this.additionalInterest = additionalInterest;
    this.additionalInterestBinding = additionalInterestBinding;
    this.broker = broker;
    this.brokerBinding = brokerBinding;
    this.limits = limits;
    this.vehicle = vehicle;
    this.driver = driver;
    this.historicalCoverages = historicalCoverages;
    this.riskSpecifics = riskSpecifics;
    this.claimsHistory = claimsHistory;
    this.destinationInfo = destinationInfo;
    this.driverInfo = driverInfo;
    this.dotInfo = dotInfo;
    this.maintenanceSafety = maintenanceSafety;
    this.generalLiabilityCargo = generalLiabilityCargo;
    this.underwritingQuestions = underwritingQuestions;
    this.quoteOptions = quoteOptions;
    this.uwAnswersConfirmed = uwAnswersConfirmed;
    this.bind = bind;
  }
}
