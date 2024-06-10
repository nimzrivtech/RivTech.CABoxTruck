import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { take } from 'rxjs/operators';
import { ApplicantData } from '../../../../../../../../modules/submission/data/applicant/applicant.data';
import { ApplicantAdditionalInterestService } from '../../../../../../../../modules/submission/services/applicant-additional-interest.service';
import { SubmissionData } from '../../../../../../../../modules/submission/data/submission.data';
import { ApplicantLabelsConstants } from '../../../../../../../../shared/constants/applicant.labels.constants';
import Utils from '../../../../../../../../shared/utilities/utils';
import NotifUtils from '../../../../../../../../shared/utilities/notif-utils';
import { ZipcodeService } from '../../../../../../../../core/services/generics/zipcode.service';
import { IAngularMyDpOptions } from 'angular-mydatepicker';
import * as moment from 'moment';
import { TableConstants } from '@app/shared/constants/table.constants';

@Component({
    selector: 'app-additional-interest-form',
    templateUrl: './additional-interest-form.component.html',
    styleUrls: ['./additional-interest-form.component.scss']
})
export class AdditionalInterestFormComponent implements OnInit {

    @Output() formSubmitted: EventEmitter<any> = new EventEmitter();
    riskDetailId: string = '';
    LabelMessage = ApplicantLabelsConstants;
    cityList: string[];
    modalTitle: string = 'Add Additional Interest';
    modalButton: string = 'Add Additional Interest';
    isEdit: boolean = false;
    cityDisabled: boolean = false;
    datepickerOptions: IAngularMyDpOptions;
    TableConstants = TableConstants;
    today: string = new Date().toISOString().substring(0, 10);
    constructor(public applicantData: ApplicantData,
        public modalRef: BsModalRef,
        public additionalInterestService: ApplicantAdditionalInterestService,
        private zipcodeService: ZipcodeService,
        public submissionData: SubmissionData
    ) { }

    ngOnInit() {
        this.riskDetailId = this.submissionData.riskDetail?.id;
        if (this.isEdit && this.fc.zipCode.value != null) { this.searchZipCode(true); }
        const defaultPolicyEffectiveDate = moment(new Date(this.submissionData.currentServerDateTime)).toDate();
        const defaultPolicyExpirationDate = moment(new Date(this.submissionData.currentServerDateTime)).add(1, 'year').toDate();
        const policyEffectiveDate = moment(this.submissionData.riskDetail?.brokerInfo?.effectiveDate ?? defaultPolicyEffectiveDate).subtract(1, 'day').toDate();
        const policyExpirationDate = moment(this.submissionData.riskDetail?.brokerInfo?.expirationDate ?? defaultPolicyExpirationDate).add(1, 'day').toDate();
        this.datepickerOptions = {
            dateRange: false,
            dateFormat: 'mm/dd/yyyy',
            disableUntil: { year: policyEffectiveDate.getFullYear() , month: policyEffectiveDate.getMonth() + 1, day: policyEffectiveDate.getDate() },
            disableSince: { year: policyExpirationDate.getFullYear() , month: policyExpirationDate.getMonth() + 1, day: policyExpirationDate.getDate() },
        };
    }

    get form() { return this.applicantData.applicantForms.additionalInterestForm; }
    get fc() { return this.applicantData.applicantForms.additionalInterestForm.controls; }

    get entityTypeList() {
        const currentEntityTypes = this.applicantData.additionalInterests?.map(c => +c.additionalInterestTypeId).filter(c => c != this.fc['additionalInterestTypeId'].value);
        return this.applicantData.entityTypesList?.filter(cl => !currentEntityTypes.includes(cl.value));
    }

    saveForm() {
        const formValue = this.form.getRawValue();
        formValue.effectiveDate = formValue.effectiveDate?.singleDate?.jsDate?.toLocaleDateString();
        formValue.expirationDate = formValue.expirationDate?.singleDate?.jsDate?.toLocaleDateString();
        this.modalRef.hide();
        this.applicantData.saveAdditionalInterest(formValue);
    }

    private getEntityTypeName(id: number) {
        let entitType = this.applicantData.entityTypesList.find(et => et.value == id);
        return entitType?.label ?? '';
    }

    searchZipCode(fromEdit?: boolean) {
        this.resetCityState();
        const zipCode = this.fc.zipCode.value;
        if (zipCode === '') return;
        Utils.blockUI()
        this.zipcodeService.getZipCodes(zipCode).pipe(take(1)).subscribe(
            data => {
                Utils.unblockUI()
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
        )
    }

    private resetCityState() {
        this.cityList = [];
        this.cityDisabled = false;
        this.fc.state.setValue('');
    }

    public onChangeDate(startDate, element) {
        if (startDate?.jsDate && element === 'effectiveDate') {
            const expirationDate = {
                isRange: false,
                singleDate: {
                    jsDate: new Date(startDate?.jsDate.getFullYear() + 1, startDate?.jsDate.getMonth(), startDate?.jsDate.getDate())
                }
            };
            this.form.get('expirationDate').setValue(expirationDate);
        }
    }

}


