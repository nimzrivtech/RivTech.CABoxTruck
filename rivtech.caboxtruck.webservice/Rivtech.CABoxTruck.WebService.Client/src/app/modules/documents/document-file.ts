export class DocumentFile {
    id?: string;
    tableRefId: string;
    riskDetailId: string;
    fileCategoryId: number;
    category: string;
    description: string;
    fileName: string;
    filePath: string;
    createdDate: any;
    dateAdded: any;
    checked?: boolean = false;
    isSystemGenerated?: boolean = false;

    public static CreateNew(riskDetailId?: string): DocumentFile {
        return  {
            id: null,
            tableRefId: riskDetailId,
            riskDetailId: riskDetailId,
            fileCategoryId: null,
            category: null,
            description: null,
            fileName: null,
            filePath: null,
            createdDate: null,
            dateAdded: null,
            checked: false
        };
    }

    private static formatDateForPicker(dateValue?: any) {
        const dateSet = dateValue ? new Date(dateValue) : new Date();
        return { isRange: false, singleDate: { jsDate: dateSet } }
    }
}
export class DocumentPaginationResultDTO implements IDocumentaginationResultDTO {
    currentPage?: number | undefined;
    firstPage?: number | undefined;
    lastPage?: number | undefined;
    previousPage?: number | undefined;
    totalCount?:  number | undefined;
    totalPages?: number | undefined;
  }
  
  export interface IDocumentaginationResultDTO  {
    currentPage?: number | undefined;
    firstPage?: number | undefined;
    lastPage?: number | undefined;
    previousPage?: number | undefined;
    totalCount?:  number | undefined;
    totalPages?: number | undefined;
  }
  export class PolicyDocumentResultDTO implements IPolicyDocumentResultDTO {
    paginationDetails: DocumentPaginationResultDTO | undefined;
    fileUploadDocuments: DocumentFile[] | undefined;
  }
  
  export interface IPolicyDocumentResultDTO {
    paginationDetails: DocumentPaginationResultDTO | undefined;
    fileUploadDocuments: DocumentFile[] | undefined;
  }
  export class PolicyDocumentListRequestDTO implements IPolicyDocumentListRequestDTO {
    pageNumber?: number;
    pageSize?: number;
    riskDetailId?: string | undefined;
    isPolicy?: boolean;
    isBilling?: boolean;
    keyword?: string;
  }
  export interface IPolicyDocumentListRequestDTO {
    pageNumber?: number;
    pageSize?: number;
    riskDetailId?: string | undefined;
    isPolicy?: boolean;
    isBilling?: boolean;
    keyword?: string;
    sortField?: string;
    order?: string;
  }