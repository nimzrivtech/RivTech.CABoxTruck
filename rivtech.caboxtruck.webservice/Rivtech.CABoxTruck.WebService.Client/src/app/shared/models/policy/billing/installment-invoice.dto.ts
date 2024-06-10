export class InstallmentInvoiceDTO implements IInstallmentInvoiceDTO {
  invoiceId?: string;  
  installmentId?: string;
  status?: string;
  installmentType?: string;
  premium?: number;
  tax?: number;
  fee?: number;
  totalBilled?: number;
  balance?: number;
  totalDue?: number;
  billDate?: Date;
  dueDate?: Date;
  invoiceNumber?: string | undefined;

  constructor(data?: IInstallmentInvoiceDTO) {
      if (data) {
          for (const property in data) {
              if (data.hasOwnProperty(property)) {
                (<any>this)[property] = (<any>data)[property];
              }
          }
      }
  }
}

export interface IInstallmentInvoiceDTO {
  invoiceId?: string;
  installmentId?: string;
  status?: string;
  installmentType?: string;
  premium?: number;
  tax?: number;
  fee?: number;
  totalBilled?: number;
  balance?: number;
  totalDue?: number;
  billDate?: Date;
  dueDate?: Date;
  invoiceNumber?: string | undefined;
}