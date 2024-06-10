import { BillingSummaryDetailsDTO } from './billing-summary-details.dto';

export class BillingSummaryDTO implements IBillingSummaryDTO {
  totalPremium?: number;
  billed?: number;
  paid?: number;
  balance?: number;
  paymentPlan?: string;
  pastDueAmount?: number;
  pastDueDate?: Date;
  currentDueDate?: Date;
  payoffAmount?: number;
  equityDate?: Date;
  minimumToBindRenewal?: number;
  payoffRenewal?: number;
  nextActivity?: string;
  billingSummaryDetails?: BillingSummaryDetailsDTO[];
  totalCommissionWritten?: number;
  totalCommissionBilled?: number;
  totalCommissionPaid?: number;
  totalCommissionBalance?: number;
  effectiveDate: Date;
  expirationDate: Date;
  premiumAmount?: number;
  feeAmount?: number;
  taxAmount?: number;

  constructor(data?: IBillingSummaryDTO) {
      if (data) {
          for (const property in data) {
              if (data.hasOwnProperty(property)) {
                (<any>this)[property] = (<any>data)[property];
              }
          }
      }
  }
}

export interface IBillingSummaryDTO {
  totalPremium?: number;
  billed?: number;
  paid?: number;
  balance?: number;
  paymentPlan?: string;
  pastDueAmount?: number;
  pastDueDate?: Date;
  currentDueDate?: Date;
  payoffAmount?: number;
  equityDate?: Date;
  minimumToBindRenewal?: number;
  payoffRenewal?: number;
  nextActivity?: string;
  BillingSummaryDetails?: BillingSummaryDetailsDTO[];
  totalCommissionWritten?: number;
  totalCommissionBilled?: number;
  totalCommissionPaid?: number;
  totalCommissionBalance?: number;
  effectiveDate: Date;
  expirationDate: Date;
  premiumAmount?: number;
  feeAmount?: number;
  taxAmount?: number;
}