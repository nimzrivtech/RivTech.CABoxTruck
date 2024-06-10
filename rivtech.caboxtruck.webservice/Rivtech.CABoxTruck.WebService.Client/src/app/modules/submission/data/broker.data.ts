import { Injectable } from '@angular/core';
import { FieldConfig } from '../../../shared/models/dynamic/field.interface';
import { SelectItem } from '../../../shared/models/dynamic/select-item';
import { FormGroup } from '@angular/forms';
import { BROKER_FIELDS, BrokerFieldConfig } from '../../submission/fields/broker.field-config';
import { Broker } from '../../../shared/models/submission/broker';

@Injectable()
export class BrokerData {
  regConfig: FieldConfig[];

  agencyList: SelectItem[] = [
    {
      label: 'Auto Club Insurance Agency, LLC',
      value: '004912'
    },
    {
      label: 'Lighthouse Risk & Ins Solutions, Inc',
      value: '009984'
    },
    {
      label: 'J E Brown & Associates',
      value: '014135'
    },
  ];

  fullSubAgencyList: any[] = [
    {
      name: 'ACRISURE OF CALIFORNIA LLC',
      value: '000174',
      agencyId: '004912'
    },
    {
      name: 'TRUMAN CHRIST INSURANCE AGENCY INC',
      value: '004437',
      agencyId: '009984'
    },
    {
      name: 'VIRGINIA DAOANG',
      value: '006407',
      agencyId: '014135'
    }];

  constructor(public brokerTemplate: BrokerFieldConfig) { }

  defaultValues: Broker = new Broker({
    AgencyFax: 'default Agency'
  });

  initiateFormValues(fb: FormGroup) {
    this.regConfig = BROKER_FIELDS;
    this.brokerTemplate.Agencies = this.agencyList;

    fb.controls['Agency'].valueChanges.subscribe((res) => {
      console.log(res);
      const subAgencyItems = this.fullSubAgencyList.filter((item) => {
        if (res === item.agencyId) {
          return item;
        }
      }).map((item) => {
        return { label: item.name, value: item.value };
      });
      this.brokerTemplate.SubAgencies = subAgencyItems;
      // this.brokerTemplate.SubAgencies = this.agencyList;
    });
  }

  retrieveDropDownValues() {

  }
}
