  export interface IBanner {
    id: string;
    description: string;
    fileName: string;
    filePath: string;
    displayOrder: string;
    pageLink: string;
    isActive: boolean;
    createdDate: Date | string;
  }