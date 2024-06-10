import { ValidationListModel } from '@app/shared/models/validation-list.model';
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

export function createPolicyDetailsMenuItems (riskId: string, riskDetailId: string, validation?: ValidationListModel, hiddenNavItems: any = []): NavData[] {
  let paramId = '';

  // If id has value, concat id to submission details url path.
  if (riskId != null && riskId.length > 0 && riskDetailId != null && riskDetailId.length > 0) {
    paramId = `/${riskId}/${riskDetailId}`;
  }

  // Url path of submission details. '/submission/details'
  const path = '/' + PathConstants.Policy.Index +  paramId;
  const disableIssuancePage = !(validation?.applicantNameAndAddress && validation?.applicantNameAndAddressBinding && validation?.limits
                              && validation?.vehicle && validation?.driver
                              && validation?.historicalCoverages && validation?.claimsHistory
                              && validation?.generalLiabilityCargo);

  return [
    {
      name: 'SUMMARY',
      icon: 'fa fa-list',
      url: path + '/' + PathConstants.Policy.Summary,
    },
    {
      name: 'HISTORY',
      icon: 'fa fa-history',
      url: path + '/' + PathConstants.Policy.History,
    },
    {
      name: 'BILLING',
      icon: 'fa fa-calculator',
      url: path + '/' + PathConstants.Policy.Billing,
    },
    {
      name: 'APPLICANT',
      icon: 'icon-user',
      url: path + '/' + PathConstants.Submission.Applicant.Index,
      badge: setBadge(validation?.applicantNameAndAddress && validation?.applicantNameAndAddressBinding),
      children: [
        {
          name: 'Name and Address',
          icon: 'icon-user',
          url: path
            + '/' + PathConstants.Submission.Applicant.Index
            + '/' + PathConstants.Submission.Applicant.NameAndAddress,
          badge: setBadge(validation?.applicantNameAndAddress && validation?.applicantNameAndAddressBinding),
        },
        {
          name: 'Business Details',
          icon: 'icon-info',
          url: path
            + '/' + PathConstants.Submission.Applicant.Index
            + '/' + PathConstants.Submission.Applicant.BusinessDetails,
        },
        {
          name: 'Filings Information',
          icon: 'icon-info',
          url: path
            + '/' + PathConstants.Submission.Applicant.Index
            + '/' + PathConstants.Submission.Applicant.FilingsInformation
        }
      ]
    },
    {
      name: 'ADDITIONAL INTEREST',
      icon: 'icon-docs',
      url: path + '/' + PathConstants.Policy.AdditionalInterest,
    },
    {
      name: 'MANUSCRIPTS',
      icon: 'icon-docs',
      url: path + '/' + PathConstants.Policy.Manuscript,
    },
    {
      name: 'BROKER',
      icon: 'icon-people',
      url: path + '/' + PathConstants.Submission.Broker.Index,
      badge: setBadge(validation?.broker),
    },
    {
      name: 'LIMITS',
      icon: 'icon-shield',
      url: path + '/' + PathConstants.Submission.Limits.Index,
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
      badge: setBadge((validation?.riskSpecifics || (validation?.generalLiabilityCargo ?? true))),
      children: [
        {
          name: 'Destination Information',
          url: path + '/' + PathConstants.Submission.RiskSpecifics.Index
            + '/' + PathConstants.Submission.RiskSpecifics.DestinationInfo,
          icon: 'icon-location-pin'
        },
        {
          name: 'Driver Information',
          url: path + '/' + PathConstants.Submission.RiskSpecifics.Index
            + '/' + PathConstants.Submission.RiskSpecifics.DriverInfo,
          icon: 'icon-user'
        },
        {
          name: 'DOT Information',
          url: path + '/' + PathConstants.Submission.RiskSpecifics.Index
            + '/' + PathConstants.Submission.RiskSpecifics.DotInfo,
          icon: 'icon-user'
        },
        {
          name: 'Maintenance / Safety',
          url: path + '/' + PathConstants.Submission.RiskSpecifics.Index
            + '/' + PathConstants.Submission.RiskSpecifics.MaintenanceSafety,
          icon: 'icon-support'
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
          icon: 'icon-question'
        },
      ]
    },
    // {
    //   name: 'QUOTE OPTIONS',
    //   url: path + '/' + PathConstants.Submission.Quote.Index,
    //   icon: 'icon-docs'
    // },
    {
      name: 'BIND REQUEST',
      url: path + '/' + PathConstants.Submission.Bind.Index,
      icon: 'fa fa-book',
      badge: setBadge(validation?.bind),
    },
    {
      name: 'FORMS SELECTION',
      icon: 'icon-docs',
      url: path + '/' + PathConstants.Submission.FormsSelection.Index,
    },
    {
      name: 'RATING',
      icon: 'icon-calculator',
      url: path + '/' + PathConstants.Policy.Rating,
    },
    {
      name: 'PREMIUM',
      icon: 'icon-calculator',
      url: path + '/' + PathConstants.Policy.Premium,
    },
    {
      name: 'ISSUANCE',
      icon: 'fa fa-calendar-check-o',
      url: path + '/' + PathConstants.Policy.Issuance,
      attributes: { hidden: null, disabled: disableIssuancePage }
    },
    {
      name: 'DOCUMENTS',
      icon: 'icon-folder-alt',
      url: path + '/' + PathConstants.Policy.Documents,
    },
    {
      name: 'NOTES',
      icon: 'fa fa-sticky-note-o',
      url: path + '/' + PathConstants.Policy.Notes,
    }
  ];
}

