import { PayeeInfoDTO } from './payee-info.dto';

export class RefundRequestDTO implements IRefundRequestDTO {
  riskId: string;
  riskBindId: string;
  instrumentId: string;
  amount: number;
  comments: string;
  payeeInfo: PayeeInfoDTO;

  constructor(data?: IRefundRequestDTO) {
    if (data) {
      for (const property in data) {
        if (data.hasOwnProperty(property)) {
          (<any>this)[property] = (<any>data)[property];
        }
      }
    }
  }
}

export interface IRefundRequestDTO {
  riskId: string;
  riskBindId: string;
  instrumentId: string;
  amount: number;
  comments: string;
  payeeInfo: PayeeInfoDTO;
}