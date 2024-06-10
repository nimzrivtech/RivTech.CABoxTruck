// eslint-disable-next-line no-shadow
export enum PageSections {
  QuickQuoteApplicant = 1,
  ApplicantInformation = 2,
  CoApplicant = 3,
  Interest = 4,
  Billing = 5,
  CreditCardPayment = 6,
  EFTPayment = 7,
  CheckPayment = 8,
  SuspendedPayment = 9,
  AddSuspendedPayment = 10,
  AddTaskModal = 11
}

// eslint-disable-next-line no-shadow
export enum PageSectionsValidations {
  IsFromQuickQuote = 'Is From QQ Page',
  ConfirmMessage = 'There are still missing Required to Quote fields.<br>Would you like to proceed?',
  ConfirmMessageForSubmission = 'Any changes will not be saved.<br>Would you like to proceed?',
  ConfirmMessageForCoApplicant = 'There are still missing fields for the Co-applicant.<br>If you wish to leave this page, the changes will be disregard.<br><br>Are you sure you want to continue?',
  CurrentPage = 'Current'
}
