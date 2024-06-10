import { AbstractControl, FormGroup } from '@angular/forms';
import { DocumentsConstants } from '@app/shared/constants/documents.constants';

export function invalidFileTypeValidator(control: AbstractControl) {
    if (!(control.value instanceof File)) { return null; }

    const fileInfo = control.value as File;
    const extension = `.${fileInfo.name.split('.')?.pop()?.toLowerCase()}` ?? '';
    return !DocumentsConstants.acceptedFileTypes.includes(extension) ? { 'invalidFileType': true } : null;
}

export function invalidFileSizeValidator(control: AbstractControl) {
    if (!(control.value instanceof File)) { return null; }

    const fileInfo = control.value as File;
    return fileInfo.size > 22000000 ? { 'invalidFileSize': true } : null;
}

export function fileTypeValidatorMultiple(control: AbstractControl) {
    const fileList = control?.value?.fileList;
    if (!(fileList instanceof FileList)) { return null; }

    control.value.invalidTypeNames.length = 0;
    const fileInfo = Array.from(fileList as FileList);
    fileInfo.forEach(f => {
        const ext = `.${f.name.split('.')?.pop()?.toLowerCase()}` ?? '';
        if (!DocumentsConstants.acceptedFileTypes.includes(ext)) {
            control.value.invalidTypeNames.push(f.name);
        }
    });
    return control.value.invalidTypeNames.length > 0 ? { 'invalidFileType': true } : null;
}

export function fileSizeValidatorMultiple(control: AbstractControl) {
    const fileList = control?.value?.fileList;
    if (!(fileList instanceof FileList)) { return null; }

    const fileInfo = Array.from(fileList as FileList);
    const invalidSizeNames = control.value.invalidSizeNames;
    invalidSizeNames.length = 0;
    fileInfo.forEach(f => {
        if (f.size > 22000000) { invalidSizeNames.push(f.name); }
    });
    const validationInfo = invalidSizeNames.length > 0 ? { 'invalidFileSize': true } : null;
    if (fileInfo.length < 2) { invalidSizeNames.length = 0; }

    return validationInfo;
}

export function customFileTypeValidatorMultiple(control: AbstractControl) {
    const fileList = control?.value?.fileList;
    if (!(fileList instanceof FileList)) { return null; }

    control.value.invalidTypeNames.length = 0;
    const fileInfo = Array.from(fileList as FileList);
    fileInfo.forEach(f => {
        const ext = `.${f.name.split('.')?.pop()?.toLowerCase()}` ?? '';
        if (!['.pdf'].includes(ext)) {
            control.value.invalidTypeNames.push(f.name);
        }
    });
    return control.value.invalidTypeNames.length > 0 ? { 'invalidFileType': true } : null;
}

export function filenameLengthValidator(control: AbstractControl) {
    const fileList = control?.value?.fileList;
    if (!(fileList instanceof FileList)) { return null; }

    let totalFilenameLength = 0;
    const fileInfo = Array.from(fileList as FileList);
    fileInfo.forEach(f => {
       totalFilenameLength += f.name.length;
    });
    return totalFilenameLength > 500 ? { 'invalidFilenameLength': true } : null;
}
