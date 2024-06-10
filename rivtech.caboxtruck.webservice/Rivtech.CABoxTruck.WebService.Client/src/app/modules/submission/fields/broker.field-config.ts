import {Injectable} from '@angular/core';
import {SelectItem} from '../../../shared/models/dynamic/select-item';
import {FieldConfig} from '../../../shared/models/dynamic/field.interface';
import {Validators} from '@angular/forms';

@Injectable()
export class BrokerFieldConfig {
  FieldConfigs = BROKER_FIELDS;

  constructor() { }

  get Agencies() {
    return this.getControlByName('Agency').options;
  }

  set Agencies(value: SelectItem[]) {
    this.getControlByName('Agency').options = value;
  }

  set SubAgencies(value: SelectItem[]) {
    this.getControlByName('SubAgency').options = value;
  }

  getControlByName(controlName: string): FieldConfig {
    return this.FieldConfigs.find(x => x.name === controlName);
  }
}

export const BROKER_FIELDS: FieldConfig[] = [
  {
    type: 'select',
    label: 'Agency',
    name: 'Agency',
    options: []
  },
  {
    type: 'input',
    label: 'testPhone',
    inputType: 'text',
    name: 'testPhone',
  },
  {
    type: 'select',
    label: 'Sub-Agency',
    name: 'SubAgency',
    options: []
  },
  {
    type: 'input',
    label: 'Agency Phone',
    inputType: 'text',
    name: 'AgencyPhone',
    validations: [
      {
        name: 'required',
        validator: Validators.required,
        message: 'Name Required'
      },
      {
        name: 'pattern',
        validator: Validators.pattern('^[a-zA-Z]+$'),
        message: 'Accept only text'
      }
    ]
  },
  {
    type: 'input',
    label: 'Agency Fax',
    inputType: 'text',
    name: 'AgencyFax',
    validations: [
      {
        name: 'required',
        validator: Validators.required,
        message: 'Name Required'
      },
      {
        name: 'pattern',
        validator: Validators.pattern('^[a-zA-Z]+$'),
        message: 'Accept only text'
      }
    ]
  },
  {
    type: 'input',
    label: 'How long has your agency written this applicant?',
    inputType: 'text',
    name: 'ApplicantLength',
    validations: [
      {
        name: 'required',
        validator: Validators.required,
        message: 'Name Required'
      },
      {
        name: 'pattern',
        validator: Validators.pattern('^[a-zA-Z]+$'),
        message: 'Accept only text'
      }
    ]
  },
  {
    type: 'checkbox',
    label: 'Are you the incumbent agency?',
    name: 'IncumbentAgency',
    value: true
  },
  {
    type: 'checkbox',
    label: 'Is this a broker account?',
    name: 'brokerAccount',
    value: true
  },
  {
    type: 'date',
    label: 'Date',
    name: 'EffectiveDate',
    validations: [
      {
        name: 'required',
        validator: Validators.required,
        message: 'effective date required'
      }
    ]
  },
  {
    type: 'input',
    label: 'Producers Name',
    inputType: 'text',
    name: 'ProducersName',
    validations: [
      {
        name: 'required',
        validator: Validators.required,
        message: 'Name Required'
      },
      {
        name: 'pattern',
        validator: Validators.pattern('^[a-zA-Z]+$'),
        message: 'Accept only text'
      }
    ]
  },
  {
    type: 'input',
    label: 'Email Address',
    inputType: 'text',
    name: 'ProducersEmailAddress',
    value: 'sherwin.jalla@rivtechglobal.com',
    validations: [
      {
        name: 'required',
        validator: Validators.required,
        message: 'Name Required'
      },
      {
        name: 'pattern',
        validator: Validators.pattern('^[a-zA-Z]+$'),
        message: 'Accept only text'
      }
    ]
  },
  {
    type: 'input',
    label: 'Producers Phone',
    inputType: 'text',
    name: 'ProducerPhone',
    validations: [
      {
        name: 'required',
        validator: Validators.required,
        message: 'Name Required'
      },
      {
        name: 'pattern',
        validator: Validators.pattern('^[a-zA-Z]+$'),
        message: 'Accept only text'
      }
    ]
  },
  {
    type: 'input',
    label: 'Sub-Producer Name (if applicable):',
    inputType: 'text',
    name: 'SubProducerName',
    validations: [
      {
        name: 'required',
        validator: Validators.required,
        message: 'Name Required'
      },
      {
        name: 'pattern',
        validator: Validators.pattern('^[a-zA-Z]+$'),
        message: 'Accept only text'
      }
    ]
  }];
