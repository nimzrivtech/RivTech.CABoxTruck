import { Component, OnInit } from '@angular/core';
import { AgencyData } from '../../../../../modules/management/data/agency.data';
import { BaseComponent } from '../../../../../shared/base-component';
import { GenericAddressData } from '../../../../../shared/components/generic-address/data/generic-address.data';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { SubAgencyData } from '../../../../../modules/management/data/sub-agency.data';
import { TableConstants } from '../../../../../shared/constants/table.constants';
import { GenericLabel } from '../../../../../shared/constants/generic.labels.constants';
import { SubAgencyModalComponent } from './sub-agency-modal/sub-agency-modal.component';
import { AgencyLabelConstants } from '../../../../../shared/constants/agency.label.constants';
import { SearchDTO } from '../../../../../shared/models/management/search.dto';
import { environment } from '../../../../../../environments/environment';
import Utils from '../../../../../shared/utilities/utils';
import NotifUtils from '../../../../../shared/utilities/notif-utils';
import { AgencySubAgencyService } from '../../../../../core/services/management/agency-sub-agency.service';
import { GenericConstants } from '../../../../../shared/constants/generic.constants';
import { UploadAgentsThruSubAgencyDTOErrors } from '../../../../../shared/models/management/uploadAgentsThruSubAgency.dto';

@Component({
  selector: 'app-agency-subagency',
  templateUrl: './agency-subagency.component.html',
  styleUrls: ['./agency-subagency.component.scss']
})
export class AgencySubagencyComponent extends BaseComponent implements OnInit {

  modalRef: BsModalRef;

  isListHidden: boolean = false;

  TableConstants = TableConstants;
  genericLabel = GenericLabel;
  AgencyLabelConstants = AgencyLabelConstants;

  constructor(
    private modalService: BsModalService,
    public agencyData: AgencyData,
    public subAgencyData: SubAgencyData,
    private genericAddressData: GenericAddressData,
    private subAgencyService: AgencySubAgencyService
  ) {
    super();
  }

  ngOnInit() {
    this.subAgencyData.getSubAgencies();
    this.genericAddressData.getAddressType();
  }


  addSubAgency(): void {
    this.subAgencyData.addressList = [];
    this.subAgencyData.subAgencyFlatCommission = [];
    this.agencyData.activeTab = this.AgencyLabelConstants.subAgencyTab;
    this.agencyData.subAgencyActiveTab = this.AgencyLabelConstants.detailsTab;
    this.modalRef = this.modalService.show(SubAgencyModalComponent, {
      initialState: {
        modalTitle: AgencyLabelConstants.addSubAgencyMessageModal,
      },
      class: 'modal-lg',
      backdrop: 'static',
      keyboard: false,
      ignoreBackdropClick: true,
    });

    this.modalRef.content.event.subscribe((res) => { });
    localStorage.setItem('subAgencyId', '');
    this.subAgencyData.showingOfSaveUpdateNext(false);
  }


  editSubAgency(item?: any): void {
    this.modalRef = this.modalService.show(SubAgencyModalComponent, {
      initialState: {
        modalTitle: AgencyLabelConstants.editSubAgencyMessageModal,
        isEdit: true,
      },
      class: 'modal-lg',
      backdrop: 'static',
      keyboard: false,
      ignoreBackdropClick: true,
    });

    this.agencyData.subAgencyActiveTab = this.AgencyLabelConstants.detailsTab;
    localStorage.setItem(`subAgencyId`, item.id);
    this.subAgencyData.getAgencyDetails(item.id);

    this.modalRef.content.event.subscribe(() => { });
    this.subAgencyData.showingOfSaveUpdateNext(true);
  }

  deleteSubAgency(item?: any): void {
    this.subAgencyData.deleteSubAgency(item.id);
  }

  toggleAgencyList(): void {
    this.isListHidden = !this.isListHidden;
  }

  searchRecords(input: string): void {
    const payload: SearchDTO = {
      term: input,
      appCode: environment.ApplicationId,
      agencyId: this.agencyData.agencyId
    };
    Utils.unblockUI();
    this.subAgencyData.subAgencyListData = [];
    this.subAgencyService.postSearchSubAgencyList(payload).subscribe((data: any) => {
      this.subAgencyData.subAgencyListData = data.map(x => x.subAgency);
      Utils.unblockUI();
    }, error => {
      Utils.unblockUI();
      NotifUtils.showError(error.message);
    });
  }


  get acceptedFileTypesForSubAgencyBatchUpload(): string {
    let acceptedFileTypes: string = '';

    GenericConstants.acceptedFileTypesForSubAgencyBatchUpload.forEach(item => {
      acceptedFileTypes += `.${item}, `;
    });

    return acceptedFileTypes;
  }

  public handleFileInput(files: FileList,subAgencyIdFromClick: string): void {

    if (files.length <= 0) return;

    if (files?.item(0)?.size > GenericConstants.fileSizeLimit) {
      NotifUtils.messages.push({html: GenericConstants?.notes?.maxFileSize});
      NotifUtils.showQueue();
      return;
    }

    Utils.blockUI();

    const programIdFromEnvi =  environment?.ApplicationId;
    const agencyId = this?.agencyData?.agencyId;

    this.subAgencyService.postUploadAgentsThruSubAgency({ program_Id: programIdFromEnvi, subagency_Id: subAgencyIdFromClick?.toString()?.trim() , file: files[0] , agency_Id: agencyId?.toString()?.trim()  }).subscribe((results) => {
      NotifUtils.showSuccessWithConfirmationFromUser(`Upload Successful. ${results?.rows ?? 'Unknown'} Records Inserted. <br> Page will now refresh.`, () => {
        window.location.reload();
      });
    }, error => {
       const listOfErrors: UploadAgentsThruSubAgencyDTOErrors = error?.error;

        let stringBuilder = '';

        if (!('errorDuplicate' in listOfErrors) && !('errorExist' in listOfErrors) && !('errorRequired' in listOfErrors) &&
              !('errorLengthExceeded' in listOfErrors) && !('errorInvalidFormat' in listOfErrors)) {
          stringBuilder += `<li> ${error?.error} </li>`;
          NotifUtils.showErrorHugeBox(stringBuilder);
          return;
        }

      listOfErrors?.errorDuplicate?.forEach(x => {
          stringBuilder += `<li class="text-left"> ${x} </li>`;
        });

        listOfErrors?.errorExist?.forEach(x => {
          stringBuilder += `<li class="text-left"> ${x} </li>`;
        });

        listOfErrors?.errorRequired?.forEach(x => {
          stringBuilder += `<li class="text-left"> ${x} </li>`;
        });

        listOfErrors?.errorLengthExceeded?.forEach(x => {
          stringBuilder += `<li class="text-left"> ${x} </li>`;
        });

        listOfErrors?.errorInvalidFormat?.forEach(x => {
          stringBuilder += `<li class="text-left"> ${x} </li>`;
        });

        NotifUtils.showErrorHugeBox(stringBuilder);
    });

  }

}
