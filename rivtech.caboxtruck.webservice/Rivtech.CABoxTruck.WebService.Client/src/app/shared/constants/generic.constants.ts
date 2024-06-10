//import { RiskStatus, RiskStatusCode } from 'app/shared/models/data/dto/quick-quote/risk.dto';
//import { UWActionType } from 'app/shared/models/data/dto/quick-quote/risk.dto';

export type UserType = 'internal' | 'external' | null;

export const GenericConstants = {
  userType: {
    internal: <UserType>'internal',
    external: <UserType>'external',
  },
  //https://www.gbmb.org/mb-to-bytes
  //22 MB = 23068672 Bytes (in binary)
  fileSizeLimit: 23068672,
  acceptedFileTypes: [
    'pdf',
    'doc',
    'docx',
    'msg',
    'jpg',
    'jpeg',
    'bmp',
    'png',
    'xls',
    'xlsx',
    'txt',
    'zip',
  ],
  acceptedFileTypesForSubAgencyBatchUpload: [
    'xls',
    'xlsx',
  ],
  notes: {
    maxFileSize: 'Maximum upload file size: 22MB',
  },
  fileValidationMessage: {
    maxFileSizeExceeded: 'The file being uploaded exceeds the maximum allowable size of 22MB.',
    invalidFileName: 'File name that contains % or # is not allowed.'
  },
  submissionsListUrl: '/submissions/list',
  formStatus: {
    disabled: 'DISABLED'
  },
  isPolicy: 'isPolicy',
  quoteStatus: {
    /* status: <RiskStatus>'Status',
    quo: <RiskStatus>'Quoted',
    app: <RiskStatus>'Application',
    ruw: <RiskStatus>'Referred to UW',
    auw: <RiskStatus>'Approved by UW',
    duw: <RiskStatus>'Declined by UW',
    air: <RiskStatus>'Additional Information Required',
    qexp: <RiskStatus>'Quote Expired',
    act: <RiskStatus>'Active',
    pen: <RiskStatus>'Pending',
    can: <RiskStatus>'Cancelled',
    pexp: <RiskStatus>'Policy Expired', */
  },
  quoteStatusCode:  {
   /*  quo: <RiskStatusCode>'QUO',
    app: <RiskStatusCode>'APP',
    ruw: <RiskStatusCode>'RUW',
    auw: <RiskStatusCode>'AUW',
    duw: <RiskStatusCode>'DUW',
    air: <RiskStatusCode>'AIR',
    qexp: <RiskStatusCode>'QEXP',
    act: <RiskStatusCode>'ACT',
    pen: <RiskStatusCode>'PEN',
    can: <RiskStatusCode>'CAN',
    pexp: <RiskStatusCode>'PEXP' */
  },
  uwStatus: {
    /* approve: <UWActionType>'Approve',
    reject: <UWActionType>'Reject',
    air: <UWActionType>'AIR',
    pending: <UWActionType>'Pending' */
  },
  claimsDefault: {
    addClaims: 'Add Claims',
    editClaims: 'Edit Claims',
    noDescription: 'No Description',
    noCATCode: 'No CAT Code',
    newCATCode: 'New CAT Code',
    newPolicyType: 'Policy Type',
    newClaimNumber: '01',
    newIndividualMatchType: 'Individual Match Type',
    isDispute: 'Yes',
    notDispute: 'No',
    isChargeable: 'Yes',
    notChargeable: 'No',
    dateFormat: 'mm/dd/yyyy',
    fields: {
      lossDate: 'lossDate',
      claimType: 'claimType',
      additionalDescription: 'additionalDescription',
      catCode: 'catCode',
      claimAmount: 'claimAmount',
      claimChargeableVsNonChargeable: 'claimChargeableVsNonChargeable',
      claimStatus: 'claimStatus',
      policyType: 'policyType',
      dispute: 'dispute',
      claimSource: 'claimSource',
      claimNumber: 'claimNumber',
      individualMatchType: 'individualMatchType'
    }
  },
  CustomAmountChecking: {
    withDollarSign: '$ ',
    withNegativeDollarSign: '-$ ',
    deleteKey: 'Delete',
    backspaceKey: 'Backspace',
    exemptedKeys: [
      'Delete',
      'Backspace',
      'Tab',
      'PageUp',
      'PageDown',
      'Home',
      'End',
      'ArrowLeft',
      'ArrowRight'
    ]
  },
  backUpType: {
    submission: 'Submission Backup',
    policy: 'Policy Backup'
  },
  userTypeLabel: {
    internal: 'Internal',
    external: 'External'
  },
  covDPercentage: {
    covDHO4: .2,
    covDHO6: .4
  },
  newSubmissionRouteUrl: '/submissions/new',
  invalidDate: 'Invalid Date',
  socialMediaLinks: {
    facebook: 'https://www.facebook.com/centauriinsurance',
    twitter: 'https://twitter.com/CENTAURI_INSURE',
    linkedIn: 'https://www.linkedin.com/company/centauri-specialty-insurance-holdings-inc-',
    globe: 'https://www.centauriinsurance.com/'
  },
  moratoriumAPIError: `Moratorium Checking Cannot Be Reached , Please try again later.`,
  closure: 'Closure',
  moratorium: 'Moratorium',
  emergency: 'Emergency',
  quoteURL: {
    submissions: '/submissions/',
    policies: '/policies/'
  }
};
