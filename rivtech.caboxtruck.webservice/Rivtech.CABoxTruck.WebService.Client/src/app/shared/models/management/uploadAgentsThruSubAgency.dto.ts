export interface UploadAgentsThruSubAgencyDTO {
  subagency_Id: string;
  file: FileList | File | any;
  program_Id: number | string;
  agency_Id: string;
}

export interface UploadAgentsThruSubAgencyDTOErrors {
  errorDuplicate: string[];
  errorExist: string[];
  errorRequired: string[];
  errorLengthExceeded: string[];
  errorInvalidFormat: string[];
}
