import { promise } from 'protractor';

export abstract class BaseResizer {
  constructor() { }
  public resizeObject(): Promise<unknown> {
    // tslint:disable-next-line: no-shadowed-variable
    const promise = new Promise((resolve, reject) => {
    });
    return promise;
  }
}

//
export class SubmissionSummaryResizer extends BaseResizer {
  public static Initialize() {
    return new this();
  }

  constructor() {
    super();
  }

  resizeObject(): Promise<unknown> {
    // selector must be existed on the UI.
    // tslint:disable-next-line: no-shadowed-variable
    const promise = new Promise((resolve, reject) => {
      try {
        const main = document.querySelector('main.main .container-fluid') as any;
        const para = document.querySelector('app-submission-summary .card') as any;

        const widthAllowance = 5;
        para.style.width = (parseFloat(window.getComputedStyle(main).getPropertyValue('width')) - widthAllowance).toString() + "px";
        resolve();
      } catch {
        reject();
      }
    });
    return promise;
  }
}
//
export class SubmissionDetailResizer extends BaseResizer {
  public static Initialize() {
    return new this();
  }
  constructor() {
    super();
  }
  resizeObject(): Promise<unknown> {
    // selector must be existed on the UI.
    // tslint:disable-next-line: no-shadowed-variable
    const promise = new Promise((resolve, reject) => {
      try {
        let para = document.querySelector('app-submission-summary .card') as any;
        let compStyles = window.getComputedStyle(para);

        let _height = parseFloat(compStyles.getPropertyValue('height'));
        let _paddingTop = parseFloat(compStyles.getPropertyValue('padding-top'));
        let _paddingBottom = parseFloat(compStyles.getPropertyValue('padding-bottom'));
        let _marginTop = parseFloat(compStyles.getPropertyValue('margin-top'));
        let _marginBottom = parseFloat(compStyles.getPropertyValue('margin-bottom'));
        let _heightAllowance = 0;
        let height = _height + _paddingTop + _paddingBottom + _marginTop + _marginBottom + _heightAllowance;

        para = document.querySelector('.page-position');
        para.style.paddingTop = height + "px";

        resolve();
      } catch {
        reject();
      }
    });
    return promise;
  }
}
//
export class nextbackButtonResizer extends BaseResizer {
  public static Initialize() {
    return new this();
  }
  constructor() {
    super();
  }

  resizeObject(): Promise<unknown> {
    // selector must be existed on the UI.
    // tslint:disable-next-line: no-shadowed-variable
    const promise = new Promise((resolve, reject) => {
      try {
        let para = document.querySelector('app-submission-details .page-position') as any;
        let compStyles = window.getComputedStyle(para);

        let _width = parseFloat(compStyles.getPropertyValue('width'));
        let _paddingLeft = parseFloat(compStyles.getPropertyValue('padding-left'));
        let _paddingRight = parseFloat(compStyles.getPropertyValue('padding-right'));
        let _marginLeft = parseFloat(compStyles.getPropertyValue('margin-left'));
        let _marginRight = parseFloat(compStyles.getPropertyValue('margin-right'));
        let _widthAllowance = 0;
        let width = _width + _paddingLeft + _paddingRight + _marginLeft + _marginRight + _widthAllowance;

        para = document.querySelector('app-next-back-button > div > div');
        para.style.width = width + "px";
        resolve();
      } catch {
        reject();
      }
    });
    return promise;
  }

  public scrollVisible(objName: string): Promise<unknown> {
    // tslint:disable-next-line: no-shadowed-variable
    const promise = new Promise((resolve, reject) => {
      let isScroll = false;
      try {
        let element = document.querySelector(objName) as any;
        isScroll = element.scrollHeight > element.clientHeight;
        element = document.querySelector('app-next-back-button > div > div');

        if (!isScroll) {
          if (element.style.position != "inherit") element.style.position = "inherit";
        } else {
          if (element.style.position != "fixed") element.style.position = "fixed";
        }

        resolve(isScroll);
      } catch {
        reject(isScroll);
      }
    });
    return promise;
  }

  public scrollBottom(objName: string): Promise<unknown> {
    // tslint:disable-next-line: no-shadowed-variable
    const promise = new Promise((resolve, reject) => {
      let isScrollBottom = false;
      try {
        let element = document.querySelector(objName) as any;
        isScrollBottom = (((element.scrollHeight - element.scrollTop) - element.clientHeight) < 13) || (element.scrollHeight - element.scrollTop === element.clientHeight);
        resolve(isScrollBottom);
      } catch {
        reject(isScrollBottom);
      }
    });
    return promise;
  }


}

