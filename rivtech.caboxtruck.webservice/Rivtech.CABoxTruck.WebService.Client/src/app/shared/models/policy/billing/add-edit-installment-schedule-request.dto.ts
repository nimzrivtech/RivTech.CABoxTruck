export class AddEditInstallmentScheduleRequestDTO implements IAddEditInstallmentScheduleRequestDTO {
  installmentId: string;
  invoiceDate: string;
  dueDate: string;

  constructor(data?: IAddEditInstallmentScheduleRequestDTO) {
    if (data) {
      for (const property in data) {
        if (data.hasOwnProperty(property)) {
          (<any>this)[property] = (<any>data)[property];
        }
      }
    }
  }
}

export interface IAddEditInstallmentScheduleRequestDTO {
  installmentId: string;
  invoiceDate: string;
  dueDate: string;
}