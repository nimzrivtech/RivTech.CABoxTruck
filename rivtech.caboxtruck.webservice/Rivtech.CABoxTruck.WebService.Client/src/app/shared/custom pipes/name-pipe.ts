import { Pipe, PipeTransform } from '@angular/core';

// Pipe used in QQ Module and Applicant Module
@Pipe({ name: 'nameCase' })
export class NameCasePipe implements PipeTransform {
    public transform(input: string): string {
        return (!input) ? '' : input.replace(/\b\w/g, first => first.toLocaleUpperCase());
    }
}
