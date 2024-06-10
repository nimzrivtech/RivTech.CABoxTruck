import { Injectable } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { FormDTO } from '../../../../shared/models/submission/forms/FormDto';
import { SubmissionData } from '../submission.data';
import { ReportService } from '../../services/report.service';
import Utils from '../../../../shared/utilities/utils';
import { RaterApiData } from '../rater-api.data';
import { SubmissionNavSavingService } from '../../../../core/services/navigation/submission-nav-saving.service';
import { QuoteLimitsData } from '../quote/quote-limits.data';
import { Router } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { QuoteProposalComponent } from '../../components/submission-details/sections/quote-options/quote-proposal/quote-proposal.component';
import NotifUtils from '../../../../shared/utilities/notif-utils';
import { FileUploadDocumentDto } from '@app/shared/models/submission/binding/FileUploadDocumentDto';
import { DocumentApiService } from '@app/modules/documents/document-api.service';
import { LayoutService } from '@app/core/services/layout/layout.service';
import { createSubmissionDetailsMenuItems } from '../../components/submission-details/submission-details-navitems';
import { ToastrService } from 'ngx-toastr';
import { ErrorMessageConstant } from '../../../../shared/constants/error-message.constants';
import { SummaryData } from '../summary.data';
import { Observable, of } from 'rxjs';
import { ApplicantData } from '../applicant/applicant.data';
import { BindingData } from '../binding/binding.data';
import { SubmissionNavValidateService } from '../../../../core/services/navigation/submission-nav-validate.service';
import { PathConstants } from '../../../../shared/constants/path.constants';
import { RiskSpecificsData } from '../risk-specifics.data';
import { catchError, tap } from 'rxjs/operators';
import { SubmissionSummaryComponent } from '../../components/submission-details/submission-summary/submission-summary/submission-summary.component';

@Injectable()
export class ReportData {
  forms: FormDTO[];
  uploadForm: FormGroup;
  modalRef: BsModalRef;

  constructor(
    private submissionData: SubmissionData,
    private summaryData: SummaryData,
    private raterApiData: RaterApiData,
    private quoteLimitsData: QuoteLimitsData,
    private submissionService: SubmissionNavSavingService,
    private submissionNavValidationService: SubmissionNavValidateService,
    private reportService: ReportService,
    private documentService: DocumentApiService,
    private layoutService: LayoutService,
    private router: Router,
    private modalService: BsModalService,
    private toastr: ToastrService,
    private applicantData: ApplicantData,
    private bindingData: BindingData,
    private riskSpecificsData: RiskSpecificsData,
    private submissionSummaryComponent: SubmissionSummaryComponent,
  ) { }

  viewQuoteProposal() {
    Utils.blockUI();
    this.submissionService.saveCurrentPage(this.router.url)
      .then((resultData) => {
        // Preload rater first before calculating.
        //this.modalRef.content.message = 'Preloading rater...';
        return this.raterApiData.preloadRater().toPromise()
          .then(() => Promise.resolve(resultData))
          .catch(() => Promise.reject("Failed to preload rater."));
      })
      .then(resultData => {
        console.log(resultData);
        Utils.blockUIWithMessage("Rating Option 1...");
        return this.raterApiData.calculateForQuote(1); // 2. Calculate Premium for quote
      })
      .then(resultData => {
        console.log(resultData);
        Utils.blockUIWithMessage("Rating Option 2...");
        return this.raterApiData.calculateForQuote(2); // Will just skip calc if Option 2 not present
      })
      .then(resultData => {
        console.log(resultData);
        Utils.blockUIWithMessage("Rating Option 3...");
        return this.raterApiData.calculateForQuote(3); // Will just skip calc if Option 3 not present
      })
      .then(resultData => {
        console.log(resultData);
        Utils.blockUIWithMessage("Generating Quote Proposal...");
        this.bindingData.saveBindingDefault();
        return this.generateQuoteProposal().toPromise();
      })
      .then(resultData => {
        if (resultData != null) {
          console.log('Success', resultData);
          this.submissionSummaryComponent.setBrokerCommissionsValue("Quote");
          Utils.unblockUI();
        } else {
          NotifUtils.showError(ErrorMessageConstant.quoteProposalErrorMsg);
          console.error('Error', new Error('Failed generating Quote Proposal file.'));
        }
      })
      .catch(error => {
        console.error('Error', error);
        Utils.unblockUI();
        NotifUtils.showError('There was an error generating quote proposal.');
      })
      .finally(() => {
        this.raterApiData.preloadRater().subscribe();
      });
  }

  generateQuoteProposal(): Observable<any> {
    return this.reportService.get(this.submissionData.riskDetail.id)
      .pipe(
        tap(result => {
          if (result !== null) {
            this.summaryData.updateSummaryStatus();
            this.submissionData.isQuoteIssued = true;
            this.submissionData.riskDetail.isRaterVersionLocked = true;
            this.submissionData.riskDetail.requestedQuoteDate = this.submissionData.currentServerDateTime;
            this.enableBindRequired();
            this.validateCategories();
            this.saveQuoteInfo(result);
            // const quoteFileUrl = decodeURIComponent(result);
            this.documentService.getDocumentSASURL(result).pipe().take(1).subscribe(data => {
              const win = window.open(data, '_blank');
              win.focus();
            });
          } else {
            NotifUtils.showError(ErrorMessageConstant.quoteProposalErrorMsg);
          }
        }),
        catchError((error) => {
            console.log(error);
            NotifUtils.showError(ErrorMessageConstant.quoteProposalErrorMsg);
            throw error;
          })
      );
  }

  enableBindRequired() {
    this.applicantData.enableBindRequiredFields(true);
    this.bindingData.enableBindRequiredFields(true);
  }

  validateCategories(): void {
    this.submissionNavValidationService.validateCurrentCategory(PathConstants.Submission.Applicant.NameAndAddress);
    this.submissionNavValidationService.validateCurrentCategory(PathConstants.Submission.Applicant.BusinessDetails);
    this.submissionNavValidationService.validateCurrentCategory(PathConstants.Submission.Applicant.FilingsInformation);
    this.submissionNavValidationService.validateCurrentCategory(PathConstants.Submission.Applicant.AdditionalInterest);
    this.submissionNavValidationService.validateCurrentCategory(PathConstants.Submission.Broker.Index);
    this.submissionNavValidationService.validateCurrentCategory(PathConstants.Submission.Limits.LimitsPage);
    this.submissionNavValidationService.validateCurrentCategory(PathConstants.Submission.Vehicle.Index);
    this.submissionNavValidationService.validateCurrentCategory(PathConstants.Submission.Driver.Index);
    this.submissionNavValidationService.validateCurrentCategory(PathConstants.Submission.Coverages.HistoricalCoverage);
    this.submissionNavValidationService.validateCurrentCategory(PathConstants.Submission.RiskSpecifics.DestinationInfo);
    this.submissionNavValidationService.validateCurrentCategory(PathConstants.Submission.RiskSpecifics.DriverInfo);
    this.submissionNavValidationService.validateCurrentCategory(PathConstants.Submission.RiskSpecifics.DotInfo);
    this.submissionNavValidationService.validateCurrentCategory(PathConstants.Submission.RiskSpecifics.MaintenanceSafety);
    this.submissionNavValidationService.validateCurrentCategory(PathConstants.Submission.RiskSpecifics.GeneralLiabilityCargo);
    this.submissionNavValidationService.validateCurrentCategory(PathConstants.Submission.RiskSpecifics.UWQuestions);
    this.submissionNavValidationService.validateCurrentCategory(PathConstants.Submission.Quote.Index);
    this.layoutService.updateMenu(createSubmissionDetailsMenuItems(this.submissionData.riskDetail.id, this.submissionData.validationList,
      this.riskSpecificsData.hiddenNavItems));
  }

  get canGenerateQuote() {
    return this.submissionData.riskDetail?.riskCoverages?.some(c => c.riskCoveragePremium?.premium > 0);
  }

  get canCalculatePremium() {
    return !this.raterApiData.disableCalculatePremium && this.raterApiData.validateRequiredToRateModules();
  }

  calculatePremium(optionNumber: number) {
    this.submissionService.saveCurrentPage(this.router.url)
      .then(resultData => {
        console.log(resultData);
        return this.raterApiData.calculateForQuote(optionNumber);
      })
      .then(resultData => {
        console.log(resultData);
        this.modalRef.content.currentTaskMessage = 'Updating data';
        this.toastr.success('Premium calculated successfully.', 'Success!');
        this.raterApiData.appendCalculatedData(resultData);
      })
      .catch(error => {
        console.log(error);
        this.toastr.error('Error encountered while calculating premium.', 'Failed!');
      })
      .finally(() => {
        setTimeout(() => {
          this.raterApiData.disableCalculatePremium = false;
        }, 6000);
      });
  }

  calculatePremiumObservable(optionNumber: number) {
    var returnPromise = this.submissionService.saveCurrentPage(this.router.url)
      .then(resultData => {
        console.log(resultData);
        this.modalRef.content.currentTaskMessage = 'Calculating Premium';
        return this.raterApiData.calculateForQuote(optionNumber);
      })
      .then(resultData => {
        console.log(resultData);
        this.toastr.success('Premium calculated successfully.', 'Success!');
        return Promise.resolve({ data: resultData });
      })
      .catch(error => {
        console.log(error);
        this.toastr.error('Error encountered while calculating premium.', 'Failed!');
        return Promise.resolve({ error: error });
      })
      .finally(() => {
        this.modalRef.hide();
        setTimeout(() => {
          this.raterApiData.disableCalculatePremium = false;
        }, 6000);
      });

    return Observable.from(returnPromise);
  }

  private saveQuoteInfo(filePath: string) {
    const document = new FileUploadDocumentDto({
      riskDetailId: this.submissionData.riskDetail?.id,
      tableRefId: this.submissionData.riskDetail?.id,
      fileName: 'Pre-Bind Quote',
      filePath: filePath,
      isConfidential: true,
      createdBy: 0,
      createdDate: new Date(this.submissionData.currentServerDateTime),
      updatedDate: new Date(this.submissionData.currentServerDateTime)
    });

    this.documentService.saveQuoteInfo(document).subscribe(() => {
      if (this.submissionData.riskDetail) {
        if (!this.submissionData.riskDetail.riskDocuments) this.submissionData.riskDetail.riskDocuments = [];
        this.submissionData.riskDetail.riskDocuments.push(document);
        this.layoutService.updateMenu(createSubmissionDetailsMenuItems(this.submissionData.riskDetail.id, this.submissionData.validationList));
      }
    });
  }

}
