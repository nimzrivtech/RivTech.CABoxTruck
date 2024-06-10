export class BankAccountDTO implements IBankAccountDTO {
  payer?: string;
  accountType?: string;
  routingNumber?: string;
  accountNumber?: string;
  nameOnAccount?: string;
  bankName?: string;

  constructor(data?: IBankAccountDTO) {
    if (data) {
      for (const property in data) {
        if (data.hasOwnProperty(property)) {
          (<any>this)[property] = (<any>data)[property];
        }
      }
    }
  }
}

export interface IBankAccountDTO {
  payer?: string;
  accountType?: string;
  routingNumber?: string;
  accountNumber?: string;
  nameOnAccount?: string;
  bankName?: string;
}