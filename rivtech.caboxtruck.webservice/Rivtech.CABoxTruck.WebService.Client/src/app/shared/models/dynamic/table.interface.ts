export interface ITableTr {
  id: any;
  tr: ITableTd[];
  edit?: boolean;
  files?: IFile[];
  itemId?: string;
  checked?: boolean;
  isAPlus?: boolean;
  file?: any;
  isUploaded?: boolean;
  riskUWApprovalId?: string;
  riskId?: string;
  currentRiskId?: string;
  createdBy?: number;
  isFromParent?: boolean;
  isUnder24Hrs?: boolean;
  item?: any;
  riskDetailId?: string;
  createdDate?: Date;
  scheduleId?: number | undefined;
  isAdd?: boolean;
  disableUpdateIcon?: boolean;
  disableDeleteIcon?: boolean;
  isComparative?: boolean;
}

export interface IFile {
  id: any;
  fileName: string;
  filePath: string;
  dateAdded: string;
  file?: any;
  isUploaded?: boolean;
  fileId?: string;
  riskUWApprovalId?: string;
}

export interface ITableTh {
  value: any;
}

export interface ITableTd {
  id: any;
  value: any;
  display: string;
  type?: 'number' | 'string';
  formControlName?: string;
}

export interface ITableFormControl {
  name: string;
  type: 'number' | 'string';
}

export interface IHideTableItems {
  addButton?: boolean;
  updateButton?: boolean;
  cancelButton?: boolean;
  saveButton?: boolean;
  controlButtons?: boolean;
  updateIcon?: boolean;
  deleteIcon?: boolean;
  fileIcon?: boolean;
  checkboxButton?: boolean;
  viewIcon?: boolean;
}

export interface IDisableTableItems extends IHideTableItems { }

export interface IDisableUWApprovalTableItems extends IDisableTableItems {
  uwApprovalOptions?: boolean;
}
