import { Component, OnInit, ViewChild } from '@angular/core';
import { SubmissionData } from '../../../../../../../modules/submission/data/submission.data';
import { ToggleCollapseComponent } from '../../../../../../../shared/components/toggle-collapse/toggle-collapse.component';
import { BrokerGenInfoData } from '../../../../../data/broker/general-information.data';

@Component({
  selector: 'app-policy-contacts',
  templateUrl: './policy-contacts.component.html',
  styleUrls: ['./policy-contacts.component.scss']
})
export class PolicyContactsComponent implements OnInit {
  @ViewChild(ToggleCollapseComponent) toggleCollapse: ToggleCollapseComponent;
  submissionSpecialistContacts: any[];
  psrContacts: any[];
  teamLeaderContacts: any[];
  internalUWContacts: any[];
  productUWContacts: any[];

  constructor(public brokerData: BrokerGenInfoData,
    public submissionData: SubmissionData
  ) { }

  ngOnInit() {
    this.initializePolicyContacts();
  }

  initializePolicyContacts() {
    this.brokerData.onPolicyContactsLoaded$.subscribe(data => {
      this.submissionSpecialistContacts = this.brokerData.contactsList.filter(contact => contact.position === 'Assistant Underwriter');
      this.psrContacts = this.brokerData.contactsList.filter(contact => contact.position === 'PSR');
      this.teamLeaderContacts = this.brokerData.contactsList.filter(contact => contact.position === 'Team Leader');
      this.internalUWContacts = this.brokerData.contactsList.filter(contact => contact.position === 'Internal Underwriter');
      this.productUWContacts = this.brokerData.contactsList.filter(contact => contact.position === 'Product Underwriter');
    });
  }
}
