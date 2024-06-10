import { Injectable } from '@angular/core';
import { BaseComponent } from '../../../shared/base-component';
import { GenericConstants } from '../../../shared/constants/generic.constants';

@Injectable()
export class MgtSubmissionNavSavingService extends BaseComponent {
  public genericConstants = GenericConstants;
  isRetiredGreaterOrEqualTo60: boolean;
  formType: string;
  propertyUsage: string;

  constructor(
  ) {
    super();
  }
}
