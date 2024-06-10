import { Pipe, PipeTransform } from '@angular/core';
import { DriverDto } from '../../../shared/models/submission/Driver/DriverDto';

@Pipe({
  name: 'driverFilter',
  // pure: true
})
export class DriverFilterPipe implements PipeTransform {
  transform(_array: DriverDto[], filter: string): any {
    if (!_array || !filter) {
      return _array;
    }
    return _array.filter(o => o.firstName?.trim()?.toLowerCase()?.indexOf(filter?.trim()?.toLowerCase()) > -1 || o.lastName?.trim()?.toLowerCase()?.indexOf(filter?.trim()?.toLowerCase()) > -1 || o.licenseNumber?.trim()?.toLowerCase()?.indexOf(filter?.trim()?.toLowerCase()) > -1);
  }
}
