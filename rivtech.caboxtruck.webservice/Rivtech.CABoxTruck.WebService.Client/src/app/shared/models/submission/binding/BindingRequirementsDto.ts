import { FileUploadDocumentDto } from './FileUploadDocumentDto';

export class BindingRequirementsDto {
  id?: string = null;
  //Subjectivities is a description
  bindRequirementsOptionId?: string | undefined = null;
  isPreBind?: boolean | undefined = null;
  bindStatusId?: string | undefined = null;
  describe?: string | undefined = null;
  comments?: string | undefined = null;
  relevantDocumentDesc?: string | undefined = null;
  filePath?: string | undefined = null;
  bindRequirementsOption?: string | undefined = null;
  bindStatus?: string | undefined = null;

  riskDetailId?: string = null;

  createdBy?: number | undefined = null;
  createdDate?: Date | undefined = null;
  updatedDate?: Date | undefined = null;
  IsActive?: boolean | undefined = null;
  isDefault?: boolean | undefined = null;
  deletedDate?: Date | undefined = null;

  fileUploads?: Array<FileUploadDocumentDto> | undefined = null;

  constructor(init?: Partial<BindingRequirementsDto>) {
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
