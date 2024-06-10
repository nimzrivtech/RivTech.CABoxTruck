import { Directive, ElementRef, Renderer2, OnInit } from '@angular/core';

@Directive({
  selector: '[appReadOnly]'
})
export class ReadOnlyDirective implements OnInit {
  private originalValue: string;

  constructor(private el: ElementRef, private renderer: Renderer2) {}

  ngOnInit() {
    const input = this.el.nativeElement;

    // Set the input as read-only
    this.renderer.setAttribute(input, 'readonly', 'true');
    this.renderer.setStyle(input, 'cursor', 'not-allowed');
    this.originalValue = input.value;

    input.addEventListener('keyup', () => this.revertValue(input)); 
  }

  private revertValue(input: HTMLInputElement) {
    if (input.value !== this.originalValue) {
      this.renderer.setProperty(input, 'value', this.originalValue);
    }
  }
}