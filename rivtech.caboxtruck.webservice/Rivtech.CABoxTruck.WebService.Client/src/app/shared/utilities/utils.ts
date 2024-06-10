import { Injectable } from '@angular/core';
import swal, { SweetAlertOptions } from 'sweetalert2';
import { SelectItem } from '../models/dynamic/select-item';

@Injectable()
export default class Utils {
    static blockUI(): void {
        swal.fire({
            title: "Please wait",
            allowOutsideClick: false,
            allowEscapeKey: false,
            showConfirmButton: false
        });
        swal.showLoading();
    }

    static blockUIWithMessage(message: string): void {
        swal.fire({
            title: message,
            allowOutsideClick: false,
            allowEscapeKey: false,
            showConfirmButton: false
        });
        swal.showLoading();
    }

    static unblockUI(): void {
        swal.close();
    }

    static convertEnumerableListToSelectItemList(list: any[]): SelectItem[] {
        let convertedList: SelectItem[] = [];

        if(list){
            for (const item of list) {
                convertedList.push({
                    label: item.name,
                    value: item.id
                });
            }
        }

        return convertedList;
    }
}