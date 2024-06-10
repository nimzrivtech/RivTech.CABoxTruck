export const PolicyBillingLabelsConstants = {
  billingSummary: {
    header: 'Billing Summary',
    totalPremium: 'Total Premium',
    billed: 'Billed',
    paid: 'Paid',
    balance: 'Balance',
    paymentPlan: 'Payment Plan',
    pastDueAmount: 'Past Due Amount',
    pastDueDate: 'Past Due Date',
    payoffAmount: 'Payoff Amount',
    equityDate: 'Equity Date',
    nextActivity: 'Next Activity',
    minimumToBindRenewal: 'Minimum To Bind Renewal',
    payoffRenewal: 'Payoff Renewal',
    changePlan: 'Change Plan',
    equityDateNotShown: 'Equity date cannot be shown',
    written: 'Written'
  },

  installmentSchedule: {
    header: 'Installment Schedule',
    installmentNumber: '#',
    status: 'Status',
    type: 'Type',
    premium: 'Premium',
    tax: 'Tax',
    fee: 'Fee',
    totalBilled: 'Total Billed',
    balance: 'Balance',
    totalDue: 'Total Due',
    billDate: 'Bill Date',
    dueDate: 'Due Date',
    invoiceNumber: 'Invoice #',
    view: 'View',
    addInstallmentSchedule: 'Add New Installment Schedule'
  },

  installmentScheduleModal: {
    header: 'Installment Schedule',
    invoiceDate: 'Print Invoice On',
    dueDate: 'Installment Due Date',
    save: 'Submit',
    cancel: 'Cancel'
  },

  payments: {
    header: 'Payments',
    postDate: 'Post Date',
    amount: 'Amount',
    type: 'Type',
    method: 'Payment Method',
    referenceNumber: 'Reference #',
    comments: 'Comments',
    reversalDate: 'Reversal Date',
    clearDate: 'Clear Date',
    postedBy: 'Posted By',
    premium: 'Premium',
    tax: 'Surcharge',
    fee: 'Fee',

    adjustment: 'Correction',
    writeOff: 'Write-Off',
    refund: 'Refund',
    postPayment: 'Make A Payment',
  },

  postPayment: {
    title: 'Make A Payment',
    titleRefund: 'Refund',
    postDate: 'Post Date',
    amount: 'Amount',
    method: 'Payment Method',
    comments: 'Comments',
    agreeEnrollAutoPay: 'I Agree',
    paymentDetails: 'Payment Details',
    save: 'Save',
    cancel: 'Cancel'
  },

  makePayment: {
    title: 'Make A Payment',
    policyNumber: 'Policy Number',
    insuredLastName: 'Insured Last Name',
    policyPayoffAmount: 'Policy Payoff Amount',
    amountDue: 'Amount Due',

    postDate: 'Post Date',
    amount: 'Payment Amount',
    method: 'Payment Method',
    email: 'Email',
    comments: 'Comments',
    agreeEnrollAutoPay: 'I agree to pay...',
    paymentDetails: 'Payment Details',
    continue: 'Continue',
    completePayment: 'Complete Payment',
    cancel: 'Cancel'
  },

  searchPolicy: {
    title: 'Make A Payment',
    policyNumber: 'Policy Number',
    insuredLastName: 'Insured Last Name',
    billingDataNotFound: 'Policy billing data not found.',
    continue: 'Continue',
    close: 'Close',
  },

  paymentDetails: {
    postDate: 'Post Date',
    amount: 'Amount',
    postedBy: 'Posted By',
    premium: 'Premium',
    tax: 'Tax',
    fee: 'Fee',
    method: 'Payment Method',
    refundMethod: 'Refund Method',
    referenceNumber: 'Reference',
    comments: 'Comments',

    reversalType: 'Reversal Type',
    reversalDate: 'Reversal Date',
    reversalProcessedBy: 'Reversal Processed By',

    clearDate: 'Clear Date',
    escheatDate: 'Escheat Date',

    voidConfirmation: 'Please enter your comments and confirm Void',
    nsfConfirmation: 'Please enter your comments and confirm NSF',
    stopPayConfirmation: 'Please enter your comments and confirm Stop Pay',
    escheatConfirmation: 'Please enter the escheat date/comment and confirm Escheatment',

    paymentModalTitle: 'Payment Details',
    adjustmentModalTitle: 'Correction Details',
    writeOffModalTitle: 'Write-Off Details',

    voidType: 'Void',
    stopPayType: 'Stop',
    nsfType: 'NSF',

    successMessage: 'has been completed',
    save: 'Save',

    transferPayment: {
      success: 'Successfully transferred',
      invalidPolicyNumberError: 'Invalid policy number'
    },
  },

  fees: {
    title: 'Add Fee',
    header: 'Fees',
    addDate: 'Add Date',
    type: 'Type',
    description: 'Description',
    amount: 'Amount',
    voidDate: 'Void Date',
    void: 'Void',
    addFee: 'Add Fee',

    addFeeSuccess: 'Successfully added transaction fee.',
    voidFeeSuccess: 'Successfully voided transaction fee.',
    voidFeeConfirmation: 'Are you sure you want to void this fee?',
    noResultsFound: 'No results found'
  },  
  
  taxes: {
    title: 'Add Tax',
    header: 'Taxes',
    addDate: 'Add Date',
    type: 'Type',
    description: 'Description',
    amount: 'Amount',
    voidDate: 'Void Date',
    void: 'Void',
    addFee: 'Add Tax',

    addFeeSuccess: 'Successfully added transaction tax.',
    voidFeeSuccess: 'Successfully voided transaction tax.',
    voidFeeConfirmation: 'Are you sure you want to void this tax?',
    noResultsFound: 'No results found'
  },

  paymentDocument: {
    header: 'Related Documents',
    upload: 'Upload',
    view: 'View',
    delete: 'Delete',
    description: 'Description',
    uploadDate: 'Upload Date',
    uploadedBy: 'Uploaded By',
    noDocument: 'No Documents'
  },

  fee: {
    addDate: 'Date',
    description: 'Description',
    amount: 'Amount',
    Save: 'Save',
    Cancel: 'Cancel'
  },

  cashPayment: {
    payerName: 'Payer Name',
    referenceNumber: 'Reference Number',
  },

  checkPayment: {
    header: 'Payee Information',
    name: 'Name',
    firstName: 'First Name',
    lastName: 'Last Name',
    suffix: 'Suffix',
    address: 'Address',
    city: 'City',
    state: 'State',
    zip: 'Zip Code',
    IsIndividual: 'Is this an individual',
    checkNumber: 'Check Number'
  },

  creditCard: {
    cardHolder: 'Card Holder Details',
    firstName: 'First Name',
    lastName: 'Last Name',
    address: 'Address',
    city: 'City',
    state: 'State',
    zip: 'Zip Code',
    creditCard: 'Credit Card Details',
    type: 'Credit Card Type',
    cardNumber: 'Credit Card Number',
    expirationMonth: 'Expiration Month',
    expirationYear: 'Expiration Year',
    cardCodeCVV: 'CVV',
    cardCodeCID: 'CID',
    email: 'Email'
  },

  eft: {
    billingAddress: 'Billing Address',
    firstName: 'First Name',
    lastName: 'Last Name',
    address: 'Address',
    city: 'City',
    state: 'State',
    zip: 'Zip Code',
    bankAccount: 'Bank Account Information',
    accountType: 'Account Type',
    routingNumber: 'Routing Number',
    accountNumber: 'Account Number',
    nameOnAccount: 'Name on Account',
    bankName: 'Bank Name',
    email: 'Email',
  },

  recurrentPaymentEnrollment: {
    header: 'Automatic Payment Enrollment',
    agreeEnrollAutoPay: 'I agree to pay...',
    accountHolderInfo: 'Account Holder Information',
    firstName: 'First Name',
    lastName: 'Last Name',
    phoneNumber: 'Phone Number',
    email: 'Email',
    paymentAccounts: 'Payment Accounts',
    addPaymentAccount: 'Add',
    editPaymentAccount: 'Edit',
    accountDescription: 'Description',
    accountType: 'Type',
    accountDefault: 'Default',
    addAccount: 'Add Payment Account',
    method: 'Payment Method',
    addProfile: 'Save',
    editProfile: 'Edit'
  },

  adjustment: {
    title: 'Correction',
    adjustmentMethod: 'Correction',
    method: 'Payment Method',
    amount: 'Amount',
    adjustmentDetails: 'Correction Details',
    premium: 'Premium',
    tax: 'Surcharge',
    fee: 'Fee',
    comment: 'Comments',
    save: 'Save',
    cancel: 'Cancel',
    notMatchAmountError: 'The sum of Correction Details should be equal to the Amount.'
  },

  writeOff: {
    title: 'Write-Off',
    writeOffMethod: 'Write-Off',
    method: 'Payment Method',
    amount: 'Amount',
    writeOffDetails: 'Write-Off Details',
    premium: 'Premium',
    tax: 'Tax',
    fee: 'Fee',
    comment: 'Comments',
    save: 'Save',
    cancel: 'Cancel',
    notMatchAmountError: 'The sum of Write-Off Details should be equal to the Amount.'
  },

  transferPayment: {
    title: 'Transfer Payment',
    transferToPolicyNumber: 'Transfer to Policy',
    amount: 'Amount',
    comment: 'Comments',
    transferToSelfError: 'Cannot transfer to same policy number',
    transfer: 'Transfer',
    cancel: 'Cancel'
  },

  successPayment: 'Payment has been posted',
  writeOffSuccess: 'Write-Off has been successful',
  successRefund: 'Refund has been successful',
  successPaymentAdjustment: 'Successfully posted payment correction',
  successAddInstallmentSchedule: 'Adding of installment schedule has been successful',
  successEditInstallmentSchedule: 'Installment schedule update has been successful',

  changePaymentPlan: {
    title: 'Change Payment Plan',
    currentPlan: 'Current Plan',
    newPlan: 'New Plan',
    paymentRequired: 'Payment Required',
    makeAPaymentHeader: 'Make a Payment',
    paymentMethod: 'PaymentMethod',
    amount: 'Amount'
  },

  changePayplanConfirmation: 'Are you sure you want to change the payment plan?',
  successChangePayplan: 'Payment plan has been successfully changed',

  outstandingReinstatementRequirements: {
    title: 'Outstanding Reinstatement Requirements',
    payBalance: 'Pay Balance',
    outstandingBalance: 'Oustanding Balance',
    notApprovedByUW: 'Not approved by underwriter for reinstatement',
    exit: 'Exit',
  },

  reinstatementRequirementsMet: {
    title: '',
    payToReinstate: 'Pay to Reinstate',
    payBalance: 'Pay Balance',
    exit: 'Exit',
  },
};
