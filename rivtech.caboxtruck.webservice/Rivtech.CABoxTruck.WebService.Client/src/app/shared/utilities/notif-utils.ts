import { Injectable } from '@angular/core';
import swal, { SweetAlertOptions } from 'sweetalert2';

@Injectable()
export default class NotifUtils {
  public static messages = [];

  static showNotice(
    options: string | SweetAlertOptions,
    defaults: SweetAlertOptions,
    callback: () => any = null,
    cancelCallBack: () => any = null): void {

    const opts: any = {};
    if (options && typeof options === 'string') {
      opts.html = options as string;
      // this fix the issue appearing on browser..
      // this append the option as array per character..
      // since the value has been parse to opt.html. this needs to be empty.
      options = '';
    }

    Object.assign(opts, defaults, options || {});

    swal.fire(opts).then((result) => {
      if (callback && result.value) {
        setTimeout(callback);
      } else if (cancelCallBack && !result.value) {
        setTimeout(cancelCallBack);
      }
    });
  }

  static showConfirmMessage(options: string | SweetAlertOptions, callback: () => any = null, cancelCallback: () => any = null): void {
    const defaults: any = {
      title: 'Confirmation',
      icon: 'question',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Yes',
      cancelButtonText: 'No',
    };
    this.showNotice(options, defaults, callback, cancelCallback);
  }

  static showSuccess(options: string | SweetAlertOptions, callback: () => any = null): void {
    const defaults: any = {
      icon: 'success'
    };
    this.showNotice(options, defaults, callback);
  }

  static showError(options: string | SweetAlertOptions, callback: () => any = null): void {
    const defaults: any = {
      // title: 'Oops...',
      icon: 'error'
    };
    this.showNotice(options, defaults, callback);
  }

  static showWarning(options: string | SweetAlertOptions, callback: () => any = null): void {
    const defaults: any = {
      icon: 'warning'
    };
    this.showNotice(options, defaults, callback);
  }

  static showInfo(options: string | SweetAlertOptions, callback: () => any = null): void {
    const defaults: any = {
      icon: 'info'
    };
    this.showNotice(options, defaults, callback);
  }

  static showSuccessWithConfirmationFromUser(options: string | SweetAlertOptions, callback: () => any = null): void {
    const defaults: any = {
      icon: 'success',
      allowOutsideClick: false,
      allowEscapeKey: false,
      stopKeydownPropagation: true,
      keydownListenerCapture: false,
      allowEnterKey: true
    };
    this.showNotice(options, defaults, callback);
  }

  static showMultiLineError(message: string): void {
    this.showError(message?.replace(new RegExp('\r?\n','g'), '<br />'));
  }

  static showQueue(): void {
    if (!swal.isVisible() || swal.isLoading()) {
      swal.mixin({
        icon: 'info',
        allowOutsideClick: false,
        allowEscapeKey: false,
        stopKeydownPropagation: true,
        keydownListenerCapture: false,
        allowEnterKey: true
      }).queue(NotifUtils.messages).then((result) => { NotifUtils.messages = []; });
    }
  }

  static showErrorHugeBox(options: string | SweetAlertOptions, callback: () => any = null): void {
    const defaults: any = {
      title: 'Oops...',
      icon: 'error',
      allowOutsideClick: false,
      width: '700px'
    };
    this.showNotice(options, defaults, callback);
  }

}
