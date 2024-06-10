import {
  Directive,
  EventEmitter,
  HostListener,
  Input,
  OnDestroy,
  OnInit,
  Output
} from '@angular/core';
import { Subject, Subscription } from 'rxjs';
import { debounceTime, takeUntil } from 'rxjs/operators';
import { BaseClass } from 'app/shared/base-class';

@Directive({
  selector: '[appPreventDoubleClick]'
})
export class PreventDoubleClickDirective extends BaseClass implements OnInit, OnDestroy {
  @Input()
  debounceTime = 500;

  @Output()
  debounceClick = new EventEmitter();

  private clicks = new Subject();

  constructor() {
    super();
  }

  ngOnInit() {
    this.clicks.pipe(
      takeUntil(this.stop$),
      debounceTime(this.debounceTime)
    ).subscribe(e => {
      this.debounceClick.emit(e);
    });
  }

  @HostListener('click', ['$event'])
  clickEvent(event) {
    event.preventDefault();
    event.stopPropagation();
    this.clicks.next(event);
  }
}
