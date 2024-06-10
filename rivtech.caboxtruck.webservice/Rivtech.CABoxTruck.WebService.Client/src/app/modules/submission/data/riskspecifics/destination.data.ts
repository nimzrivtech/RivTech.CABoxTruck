import { Injectable } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { SelectItem } from '@app/shared/models/dynamic/select-item';
import { take } from 'rxjs/operators';
import { DestinationDto } from '../../../../shared/models/submission/riskspecifics/destinationDto';
import { FormsService } from '../../services/forms.service';

@Injectable()
export class DestinationData {
  destinationForm: FormGroup;
  stateList: any = [];

  constructor(private formBuilder: FormBuilder, private formsService: FormsService) { }

  initiateFormGroup(obj: DestinationDto): FormGroup {
    this.destinationForm = this.formBuilder.group({
      id: [this.setInitialValue(obj.id, null)],
      city: [this.setInitialValue(obj.city, ''), Validators.required],
      state: [this.setInitialValue(obj.state, null), Validators.required],
      zipCode: [this.setInitialValue(obj.zipCode, ''), [Validators.required, Validators.min(1)]],
      percentageOfTravelGiven: [this.setInitialValue(obj.percentageOfTravelGiven, ''), [Validators.required, Validators.min(0)]],
      percentageOfTravelActual: [this.setInitialValue(obj.percentageOfTravelActual, ''), [Validators.required, Validators.min(0.1)]],
      totalPercentageOfTravelGiven: [obj.totalPercentageOfTravelGiven],
      totalPercentageOfTravelActual: [obj.totalPercentageOfTravelActual],
      currentDestinations: [obj.currentDestinations]
    }, { validators: [this.validatePercentageOfTravelGiven, this.validatePercentageOfTravelActual, this.validateDestination] });

    this.getStates();
    return this.destinationForm;
  }

  validatePercentageOfTravelGiven(form: FormGroup) {
    let totalPercentageOfTravel: number;
    const percentageOfTravel = form.controls['percentageOfTravelGiven'].value;
    const currentPercentageOfTravel = form.controls['totalPercentageOfTravelGiven'].value;

    totalPercentageOfTravel = +(Number(currentPercentageOfTravel) + Number(percentageOfTravel)).toFixed(1);
    return totalPercentageOfTravel > 100.0 ? { 'invalidPercentageOfTravelGiven': true } : null;
  }

  validatePercentageOfTravelActual(form: FormGroup) {
    let totalPercentageOfTravel: number;
    const percentageOfTravel = form.controls['percentageOfTravelActual'].value;
    const currentPercentageOfTravel = form.controls['totalPercentageOfTravelActual'].value;

    totalPercentageOfTravel = +(Number(currentPercentageOfTravel) + Number(percentageOfTravel)).toFixed(1);
    return totalPercentageOfTravel > 100.0 ? { 'invalidPercentageOfTravelActual': true } : null;
  }

  validateDestination(form: FormGroup) {
    const currentDestinations = form.controls['currentDestinations']?.value as Array<string>;
    const city = form.controls['city']?.value;
    const state = form.controls['state']?.value;
    const zipCode = form.controls['zipCode']?.value;

    const selectedDestination = city?.toLowerCase()?.trim() + state + zipCode?.trim();
    return currentDestinations?.includes(selectedDestination) ? { 'duplicateDestination': true } : null;
  }

  initiateFormValues(fb: FormGroup) {
  }

  public setInitialValue(obj: any, val: any) {
    return (obj == null || obj.length === 0) ? val : obj;
  }

  getStates() {
    this.formsService.getStates().pipe(take(1)).subscribe(data => {
      this.stateList = data.map(s => ({
        value: s.stateCode?.toString(),
        label: s.fullStateName?.toString()?.replace('[E]', '')
      })) as SelectItem[];
    });
  }
}
