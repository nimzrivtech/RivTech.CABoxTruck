import { FileUploadDocumentDto } from './FileUploadDocumentDto';

export class QuoteConditionsDto {
  id?: string = null;
  quoteConditionsOptionId?: string | undefined = null;
  quoteConditionsOption?: string | undefined = null;

  describe?: string | undefined = null;
  comments?: string | undefined = null;

  relevantDocumentDesc?: string | undefined = null;
  filePath?: string | undefined = null;

  riskDetailId?: string = null;

  createdBy?: number | undefined = null;
  createdDate?: Date | undefined = null;
  updatedDate?: Date | undefined = null;
  IsActive?: boolean | undefined = null;
  deletedDate?: Date | undefined = null;
  isDefault?: boolean | undefined = null;

  fileUploads?: Array<FileUploadDocumentDto> | undefined = null;

  sortableText?: string | undefined = null;
  constructor(init?: Partial<QuoteConditionsDto>) {
    // Object.assign(this, init);
    if (init) {
      for (const property in init) {
        if (init.hasOwnProperty(property)) {
          (<any>this)[property] = (<any>init)[property];
        }
      }
    }
  }
}
