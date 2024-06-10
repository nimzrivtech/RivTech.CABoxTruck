export interface IPaymentPortalRequest {
  id: string;
  riskId: string;
  effectiveDate: Date;
  amount: number;
  paymentDetail: string;
  instrument: number;
  instrumentId?: string;

  // CASH
  payer?: string;
  reference?: number;
  email?: string;
  transactionDetail?: string;

  // CREDIT CARD
  firstName?: string;
  lastName?: string;
  address?: string;
  zipCode?: string;
  city?: string;
  state?: string;
  creditCardTypeId?: number;
  creditCardNumber?: string;
  expirationMonth?: number;
  expirationYear?: number;
  creditCardFee?: number;
  totalChangeAmount?: number;
  cvv?: string;

  //EFT
  bankName?: string;
  bankBranch?: string;
  routingNumber?: number;
  accountNumber?: number;
  comments?: string;
  submissionId?: string;
  createdDate?: Date;
  createdBy?: string;
  voidOfPayment?: string;
  voidDate?: Date;
  voidedBy?: string;
  premium?: number;
  tax?: number;
  fee?: number;
  postDate: Date;
  type: string;
  isWriteOff: boolean;
  paymentType: string;
  accessCode: string;
  isPreBind: boolean;
  insuredName: string;
  accountType: number;
  accountUsageType: number;
}

export class PaymentPortalRequest implements IPaymentPortalRequest {
  id: string;
  riskId: string;
  effectiveDate: Date;
  amount: number;
  paymentDetail: string;
  instrument: number;
  instrumentId: string;

  // CASH
  payer: string;
  reference: number;
  email?: string;

  transactionDetail: string;

  // CREDIT CARD
  firstName: string;
  lastName: string;
  address: string;
  zipCode: string;
  city: string;
  state: string;
  creditCardTypeId: number;
  creditCardNumber: string;
  expirationMonth: number;
  expirationYear: number;
  creditCardFee: number;
  totalChangeAmount: number;
  cvv: string;

  //EFT
  bankName: string;
  bankBranch: string;
  routingNumber: number;
  accountNumber: number;
  accountType: number;
  accountUsageType: number;

  comments: string;

  submissionId: string;
  createdDate: Date;
  createdBy: string;
  voidOfPayment: string;
  voidDate: Date;
  voidedBy: string;
  premium: number;
  tax: number;
  fee: number;
  postDate: Date;
  type: string;
  isWriteOff: boolean;
  paymentType: string;
  accessCode: string;
  isPreBind: boolean;
  insuredName: string;

  constructor(init?: Partial<IPaymentPortalRequest>) {
    Object.assign(this, init);
  }
}
