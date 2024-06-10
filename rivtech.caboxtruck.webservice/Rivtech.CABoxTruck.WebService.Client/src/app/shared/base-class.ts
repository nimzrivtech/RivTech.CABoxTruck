/// https://stackoverflow.com/a/43367791/1035039
// https://medium.com/@benlesh/rxjs-dont-unsubscribe-6753ed4fda87

import { OnDestroy } from '@angular/core';
import { Subject } from 'rxjs';
import { ComponentListConstants } from './constants/component-list.constants';
import { CurrencyMaskConstants } from './constants/currency-mask.constants';
import { DropdownOptionSettingsConstants } from './constants/dropdown-option-settings.constants';
import { DropdownSettingsConstants } from './constants/dropdown-settings.constants';
import { GenericLabelConstants } from './constants/generic.labels.constants';

export abstract class BaseClass implements OnDestroy {
  public CurrencyMaskConstants = CurrencyMaskConstants;
  public DropdownSettingsConstants = DropdownSettingsConstants;
  public ComponentListConstants = ComponentListConstants;
  public DropdownOptionSettingsConstants = DropdownOptionSettingsConstants;
  public GenericLabelConstants = GenericLabelConstants;

  protected stop$: Subject<boolean>;
  constructor() {
    this.stop$ = new Subject<boolean>();
    const f = this.ngOnDestroy;
    this.ngOnDestroy = () => {
      f.bind(this)();
      this.stop$.next(true);
      this.stop$.complete();
    };
  }
  /// placeholder of ngOnDestroy. no need to do super() call of extended class.
  ngOnDestroy() { }
}
