export class FileUploadDocumentDto {
  id?: string = null;
  tableRefId?: string | undefined = null;

  fileCategoryId?: number | undefined = null;
  description?: string | undefined = null;

  fileName?: string | undefined = null;
  filePath?: string | undefined = null;
  fileInfo?: any;

  fileExtension?: string | undefined = null;
  mimeType?: string | undefined = null;

  isConfidential?: boolean | undefined = null;
  isUploaded?: boolean | undefined = null;

  surplusLIneNum?: string | undefined = null;

  riskDetailId?: string | undefined = null;

  createdBy?: number | undefined = null;
  createdDate?: Date | undefined = null;
  updatedDate?: Date | undefined = null;
  deletedDate?: Date | undefined = null;

  isActive?: boolean | undefined = null;
  removedBy?: number | undefined = null;
  isUserUploaded?: boolean | undefined = false;

  constructor(init?: Partial<FileUploadDocumentDto>) {
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
