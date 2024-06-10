import { Pipe, PipeTransform } from '@angular/core';
import { SelectItem } from '../../models/dynamic/select-item';

@Pipe({
  name: 'selectItemIdToLabel'
})
export class SelectItemIdToLabelPipe implements PipeTransform {

  transform(value: any, array: SelectItem[]): any {
    try {
      return array.find(f => f.value == value)?.label;
    } catch {
      return 'not found';
    }
  }
}
