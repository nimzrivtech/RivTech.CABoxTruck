export const CoveragesValidatorErrorConstants = {
  floodCovCGreaterThanBaseErrorMessage: {
    key: 'floodCovCGreaterThanBase',
    value: 'Flood Coverage C limit cannot be greater than policy Coverage C limit.'
  },
  floodCovAGreaterThanBaseErrorMessage: {
    key: 'floodCovAGreaterThanBase',
    value: 'Flood Coverage A limit cannot be greater than policy Coverage A limit when Flood Coverage is selected.'
  },
  floodCovABaseFloodZoneBCXErrorMessage: {
    key: 'floodCovABaseFloodZoneBCX',
    value: 'Flood Coverage A limit cannot be less than $20,000 when Flood Coverage is selected.'
  },
  floodCovCBaseFloodZoneBCXErrorMessage: {
    key: 'floodCovCBaseFlodZoneBCX',
    value: 'Flood Coverage C limit cannot be less than $8,000 when Flood Coverage is selected.'
  },
  floodCovABaseFloodZoneErrorMessage: {
    key: 'floodCovABaseFloodZone',
    value: 'Flood Coverage A limit cannot be less than $100,000 when Flood Coverage is selected.'
  },
  floodCovCBaseFloodZoneErrorMessage: {
    key: 'floodCovCBaseFloodZone',
    value: 'Flood Coverage C limit cannot be less than $35,000 when Flood Coverage is selected.'
  },
  participationStatusIdShouldBeRegularProgram: {
    key: 'participationStatusId',
    value: 'Flood Coverage is not available due to participation status.'
  },
  floodCovAInfoMessage: {
    key: 'floodCovAInfoMessage',
    value: 'Flood Coverage A defaulted to maximum allowed of $2,000,000.'
  },
  floodCovCInfoMessage: {
    key: 'floodCovCInfoMessage',
    value: 'Flood Coverage C defaulted to maximum allowed of $500,000.'
  },
  fireProtectionCannotBeNoneMessage: {
    key: 'fireProtectionCannotBeNone',
    value: 'Fire protection device cannot be none.'
  },
  floodZoneVErrorMessage: {
    key: 'floodZoneVError',
    value: 'Flood Coverage Endorsement is not available due to flood zone.'
  },
  firmDateErrorMessage: {
    key: 'firmDateError',
    value: 'Year should be in range 1970 to current year.'
  },
  hurricaneDeductibleExcludedInfoMessage: {
    key: 'hurricaneDeductibleExcludedInfoMessage',
    value: 'Policy must be ex-wind due to distance to coast'
  },
  hurricaneDeductible5InfoMessage: {
    key: 'hurricaneDeductible5InfoMessage',
    value: 'Minimum deductible is 5% due to distance to coast'
  },
  hurrDedExcludedDueToWaterInfoMessage: {
    key: 'hurrDedExcludedDueToWaterInfoMessage',
    value: 'Policy must be ex-wind due to distance to water'
  },
  hurrDed5DueToWaterInfoMessage: {
    key: 'hurrDed5DueToWaterInfoMessage',
    value: 'Minimum deductible is 5% due to distance to water'
  },
  hurricaneDedCannotBeLessThanAOP: {
    key: 'hurricaneDedCannotBeLessThanAOP',
    value: 'Hurricane deductible cannot be less than AOP'
  }
};
