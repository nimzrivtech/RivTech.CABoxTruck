import { ValidationListModel } from '../../../../shared/models/validation-list.model';
import { PathConstants } from '../../../../shared/constants/path.constants';
import { NavData } from '../../../../_nav';

const setBadge = (validation?) => {
  let badge = { text: '', variant: '' };
  if (validation !== undefined) {
    if (!validation) {
      badge = {
        text: '!',
        variant: 'warning'
      };
      return badge;
    } else {
      return badge;
    }
  } else {
    return badge;
  }
};

export const createSubmissionDetailsMenuItems = (id?: string, validation?: ValidationListModel, hiddenNavItems?: string[]): NavData[] => {
  let paramId = '/new';
  // If id has value, concat id to submission details url path.
  if (id != null && id.length > 0) {
    paramId = `/${id}`;
  }
  // Url path of submission details. '/submission/details'
  const path = '/' + PathConstants.Submission.Index + paramId;
  const submissionMenu = [
    {
      name: 'APPLICANT',
      url: path + '/' + PathConstants.Submission.Applicant.Index,
      icon: 'icon-user',
      badge: setBadge(
        validation?.applicantNameAndAddress &&
        validation?.applicantNameAndAddressBinding &&
        validation?.businessDetails &&
        validation?.businessDetailsBinding &&
        validation?.filingsInfo &&
        validation?.additionalInterestBinding),
      children: [
        {
          name: 'Name and Address',
          icon: 'icon-user',
          badge: setBadge(validation?.applicantNameAndAddress && validation?.applicantNameAndAddressBinding),
          url: path
            + '/' + PathConstants.Submission.Applicant.Index
            + '/' + PathConstants.Submission.Applicant.NameAndAddress,
        },
        {
          name: 'Business Details',
          icon: 'icon-info',
          badge: setBadge(validation?.businessDetails && validation?.businessDetailsBinding),
          url: path
            + '/' + PathConstants.Submission.Applicant.Index
            + '/' + PathConstants.Submission.Applicant.BusinessDetails,
        },
        {
          name: 'Filings Information',
          icon: 'icon-info',
          badge: setBadge(validation?.filingsInfo),
          url: path
            + '/' + PathConstants.Submission.Applicant.Index
            + '/' + PathConstants.Submission.Applicant.FilingsInformation
        },
        {
          name: 'Additional Interest',
          icon: 'icon-docs',
          badge: setBadge(validation?.additionalInterestBinding),
          url: path
            + '/' + PathConstants.Submission.Applicant.Index
            + '/' + PathConstants.Submission.Applicant.AdditionalInterest,
        },
        {
          name: 'Manuscripts',
          icon: 'icon-docs',
          url: path
            + '/' + PathConstants.Submission.Applicant.Index
            + '/' + PathConstants.Submission.Applicant.Manuscripts,
          attributes: { hidden: hiddenNavItems?.includes('Manuscripts') ? true : null }
        }
      ]
    },
    {
      name: 'BROKER',
      url: path + '/' + PathConstants.Submission.Broker.Index,
      icon: 'icon-people',
      badge: setBadge(validation?.broker && validation?.brokerBinding),
    },
    {
      name: 'LIMITS',
      url: path + '/' + PathConstants.Submission.Limits.Index,
      icon: 'icon-shield',
      badge: setBadge(validation?.limits),
    },
    {
      name: 'VEHICLE',
      icon: 'fa fa-car',
      url: path + '/' + PathConstants.Submission.Vehicle.Index,
      badge: setBadge(validation?.vehicle),
    },
    {
      name: 'DRIVER',
      icon: 'fa fa-user',
      url: path + '/' + PathConstants.Submission.Driver.Index,
      badge: setBadge(validation?.driver),
    },
    {
      name: 'HISTORICAL COVERAGE',
      icon: 'icon-notebook',
      url: path + '/' + PathConstants.Submission.Coverages.Index,
      badge: setBadge(validation?.historicalCoverages && !hiddenNavItems?.includes('Historical Coverage')),
      attributes: { hidden: hiddenNavItems?.includes('Historical Coverage') ? true : null }
    },
    {
      name: 'CLAIMS HISTORY',
      icon: 'icon-folder-alt',
      url: path + '/' + PathConstants.Submission.Claims.Index,
      badge: setBadge(validation?.claimsHistory && !hiddenNavItems?.includes('Claims History')),
      attributes: { hidden: hiddenNavItems?.includes('Claims History') ? true : null }
    },
    {
      name: 'RISK SPECIFICS',
      icon: 'icon-target',
      url: path + '/' + PathConstants.Submission.RiskSpecifics.Index,
      badge: setBadge((validation?.riskSpecifics || ((validation?.destinationInfo ?? true) && (validation?.driverInfo ?? true)
                      && (validation?.dotInfo ?? true) && (validation?.maintenanceSafety ?? true) && (validation?.generalLiabilityCargo ?? true))) 
                      && (validation?.underwritingQuestions && validation?.uwAnswersConfirmed.badge)),
      children: [
        {
          name: 'Destination Information',
          url: path + '/' + PathConstants.Submission.RiskSpecifics.Index
            + '/' + PathConstants.Submission.RiskSpecifics.DestinationInfo,
          icon: 'icon-location-pin',
          badge: setBadge(validation?.destinationInfo),
        },
        {
          name: 'Driver Information',
          url: path + '/' + PathConstants.Submission.RiskSpecifics.Index
            + '/' + PathConstants.Submission.RiskSpecifics.DriverInfo,
          icon: 'icon-user',
          badge: setBadge(validation?.driverInfo),
        },
        {
          name: 'DOT Information',
          url: path + '/' + PathConstants.Submission.RiskSpecifics.Index
            + '/' + PathConstants.Submission.RiskSpecifics.DotInfo,
          icon: 'icon-user',
          badge: setBadge(validation?.dotInfo),
        },
        {
          name: 'Maintenance / Safety',
          url: path + '/' + PathConstants.Submission.RiskSpecifics.Index
            + '/' + PathConstants.Submission.RiskSpecifics.MaintenanceSafety,
          icon: 'icon-support',
          badge: setBadge(validation?.maintenanceSafety),
        },
        {
          name: 'General Liability & Cargo',
          url: path + '/' + PathConstants.Submission.RiskSpecifics.Index
            + '/' + PathConstants.Submission.RiskSpecifics.GeneralLiabilityCargo,
          icon: 'icon-support',
          badge: setBadge(validation?.generalLiabilityCargo && !hiddenNavItems?.includes('General Liability & Cargo')),
          attributes: { hidden: hiddenNavItems?.includes('General Liability & Cargo') ? true : null }
        },
        {
          name: 'Underwriting Questions',
          url: path + '/' + PathConstants.Submission.RiskSpecifics.Index
            + '/' + PathConstants.Submission.RiskSpecifics.UWQuestions,
          icon: 'icon-question',
          badge: setBadge(validation?.underwritingQuestions && validation?.uwAnswersConfirmed.badge),

        },
      ]
    },
    {
      name: 'QUOTE OPTIONS',
      url: path + '/' + PathConstants.Submission.Quote.Index,
      icon: 'icon-docs',
      badge: setBadge(validation?.quoteOptions),
      // w/o "hidden" next/back button breaks, revisit impl
      attributes: { hidden: null, disabled: !validation?.uwAnswersConfirmed.enable }
    },
    {
      name: 'PREMIUM',
      url: path + '/' + PathConstants.Submission.Premiums.Index,
      icon: 'icon-docs',
      attributes: { hidden: null, disabled: !validation?.uwAnswersConfirmed.enable }
    },
    {
      name: 'BIND REQUEST',
      url: path + '/' + PathConstants.Submission.Bind.Index,
      icon: 'fa fa-book',
      badge: setBadge(validation?.bind),
      attributes: { hidden: null, disabled: !validation?.uwAnswersConfirmed.enable }
    },
    {
      name: 'FORMS SELECTION',
      url: path + '/' + PathConstants.Submission.FormsSelection.Index,
      icon: 'icon-docs',
      attributes: { hidden: null, disabled: !validation?.uwAnswersConfirmed.enable }
    },
    {
      name: 'DOCUMENTS',
      icon: 'icon-folder-alt',
      url: path + '/' + PathConstants.Submission.Documents,
    },
    {
      name: 'NOTES',
      icon: 'fa fa-sticky-note-o',
      url: path + '/' + PathConstants.Submission.Notes.Index,
    }
  ];

  return submissionMenu;
};

