import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { ZipcodeService } from '@app/core/services/generics/zipcode.service';
import { PolicyContactsData } from '@app/modules/submission/data/broker/policy-contacts.data';
import { SubmissionData } from '@app/modules/submission/data/submission.data';
import NotifUtils from '@app/shared/utilities/notif-utils';
import Utils from '@app/shared/utilities/utils';
import { BsModalRef } from 'ngx-bootstrap';
import { take } from 'rxjs/operators';

@Component({
    selector: 'app-policy-contacts-form',
    templateUrl: './policy-contacts-form.component.html',
    styleUrls: ['./policy-contacts-form.component.scss']
})
export class PolicyContactFormComponent implements OnInit {

    riskDetailId: string = '';
    cityList: string[];
    modalTitle: string = 'Add Policy Contact';
    modalButton: string = 'Add Policy Contact';
    isEdit: boolean = false;
    cityDisabled: boolean = false;

    constructor(public policyContactData: PolicyContactsData,
        public modalRef: BsModalRef,
        // public additionalInterestService: ApplicantAdditionalInterestService,
        private zipcodeService: ZipcodeService,
        private submissionData: SubmissionData
    ) { }

    ngOnInit() {
        this.riskDetailId = this.submissionData.riskDetail?.id;
        if (this.isEdit) { this.searchZipCode(true); }
    }

    get form() { return this.policyContactData.formInfo; }
    get fc() { return this.policyContactData.formInfo.controls; }

    saveForm() {
        const formValue = this.form.getRawValue();
        this.modalRef.hide();
        this.policyContactData.savePolicyContacts(formValue);
    }

    searchZipCode(fromEdit?: boolean) {
        this.resetCityState();
        const zipCode = this.fc.zipCode.value;
        if (zipCode === '') {
            return;
        }
        Utils.blockUI();
        this.zipcodeService.getZipCodes(zipCode).pipe(take(1)).subscribe(
            data => {
                Utils.unblockUI();
                this.cityList = data.map(z => z.city);
                if (this.cityList.length > 0) {
                    this.fc.state.setValue(data[0].stateCode);
                    if (this.cityList.length === 1) {
                        this.fc.city.setValue(data[0].city);
                        this.cityDisabled = true;
                    } else {
                        this.fc.city.setValue(fromEdit ? this.fc.city.value : null);
                    }
                } else {
                    NotifUtils.showError('Zip code not found, contact Underwriting');
                }
            }
        );
    }

    private resetCityState() {
        this.cityList = [];
        this.cityDisabled = false;
        this.fc.state.setValue('');
    }

}


