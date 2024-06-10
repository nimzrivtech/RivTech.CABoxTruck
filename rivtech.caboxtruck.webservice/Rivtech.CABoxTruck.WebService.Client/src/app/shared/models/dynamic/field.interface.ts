import {SelectItem} from './select-item';

export interface Validator {
  name: string;
  validator: any;
  message: string;
}
export interface Field {
  name: string;
  value: any;
}

export interface FieldConfig {  
  type: string;
  label?: string;
  name?: string;  
  value?: any;
  required? : boolean;
  validations?: Validator[];
  parentField?: Field;
  childField?: Field;
  isVisible?: boolean;

  //#region input
  size?: string;
  inputType?: string;
  placeholder?: string,
  prependLabel?: string,
  appendLabel?: string,
  maxValue?: number,
  //#endregion

  //#region dropdown
  options?: SelectItem[];
  //#endregion

  //#region multiselectdropdown
  dropdownList?: any[];
  selectedItems?: any[];
  //#endregion
  
  collections?: any;
}
