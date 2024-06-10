import { Component, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { ManuscriptsData } from '@app/modules/submission/data/manuscripts/manuscripts.data';
import { PolicySummaryData } from '@app/modules/policy/data/policy-summary.data';

@Component({
    selector: 'app-manuscript-dialog',
    templateUrl: './manuscript-dialog.component.html',
    styleUrls: ['./manuscript-dialog.component.scss']
})
export class ManuscriptDialogComponent implements OnInit {
    modalTitle: string;
    modalButton: string;
    currencySettings: any = {
        align: 'left',
        allowNegative: true,
        precision: 0,
        nullable: true
    };

    constructor(
        public data: ManuscriptsData,
        private policySummaryData: PolicySummaryData,
        public modalRef: BsModalRef
    ) { }

    ngOnInit() {
    }

    get form() { return this.data.manuscriptForm; }
    get fc() { return this.form.controls; }

    saveForm() {
        this.modalRef.hide();
        this.data.save(this.form.value, true);
    }
}


