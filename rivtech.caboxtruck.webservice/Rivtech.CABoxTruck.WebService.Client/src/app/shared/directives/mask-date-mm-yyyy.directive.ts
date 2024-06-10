import { Directive, ElementRef, Input, OnDestroy } from '@angular/core';
import * as textMask from 'vanilla-text-mask/dist/vanillaTextMask.js';

@Directive({
  selector: '[appMaskDateMMYYYY]'
})
export class MaskDateMMYYYYDirective implements OnDestroy {
  mask = [/\d/, /\d/, '/', /\d/, /\d/, /\d/, /\d/]; // mm/yyyy
  maskedInputController;

  constructor(private element: ElementRef) {
    this.maskedInputController = textMask.maskInput({
      inputElement: this.element.nativeElement,
      mask: this.mask
    });
  }

  ngOnDestroy() {
    this.maskedInputController.destroy();
  }

}
