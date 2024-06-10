import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { BrokerGenInfoData } from '../../../../../data/broker/general-information.data';
import { ToggleCollapseComponent } from '../../../../../../../shared/components/toggle-collapse/toggle-collapse.component';
import { DatePickerComponent } from '../../../../../../../shared/components/dynamic/date-picker/date-picker.component';
import { SubmissionData } from '../../../../../../../modules/submission/data/submission.data';

@Component({
  selector: 'app-general-information',
  templateUrl: './general-information.component.html',
  styleUrls: ['./general-information.component.scss']
})
export class GeneralInformationComponent implements OnInit, AfterViewInit {
  @ViewChild(ToggleCollapseComponent) toggleCollapse: ToggleCollapseComponent;
  @ViewChild('dtpeffectiveDate') dtpeffectiveDate: DatePickerComponent;
  @ViewChild('dtpexpirationDate') dtpexpirationDate: DatePickerComponent;
  constructor(public Data: BrokerGenInfoData,
    public submissionData: SubmissionData
  ) { }

  ngAfterViewInit() {
    setTimeout(() => {
      const elementReference = document.querySelector('[id="agencyId"]');
      if (elementReference instanceof HTMLElement) {
        elementReference.focus();
      }
    }, 1100);

    // this.fg.valueChanges.subscribe((res) => {
    //   this.submissionNavValidationService.validateCurrentCategory(PathConstants.Submission.Broker.Index);
    //   this.layoutService.updateMenu(createSubmissionDetailsMenuItems(this.submissionData.risk.id, this.submissionNavValidationService.validationList));
    // });
  }

  ngOnInit() {
    this.Data.getReasonMove();
    this.Data.initiateBrokerFields();
  }

  setTooltip(field, dataList, type) {
    if (field && dataList) {
      if (type == 'company') {
        const data = dataList.find(x => x.id == field);
        if (data) {
          return data.companyName;
        }
      }
      if (type == 'name') {
        const data = dataList.find(x => x.id == field);
        if (data) {
          return data.Name;
        }
      }
    }
  }
}
