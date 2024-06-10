import { Component, ElementRef, EventEmitter, OnInit, Output, ViewChild } from '@angular/core';
import { SubmissionNavSavingService } from '../../../../../../../../core/services/navigation/submission-nav-saving.service';
import { IDropdownSettings } from 'ng-multiselect-dropdown';
import { finalize, map, switchMap, take, takeUntil, tap } from 'rxjs/operators';
import { QuoteLimitsData } from '../../../../../../../../modules/submission/data/quote/quote-limits.data';
import { ExperienceRaterDto, RaterApiData } from '../../../../../../../../modules/submission/data/rater-api.data';
import { SubmissionData } from '../../../../../../../../modules/submission/data/submission.data';
import Utils from '@app/shared/utilities/utils';
import { RiskSpecificsData } from '../../../../../../../../modules/submission/data/risk-specifics.data';
import { DriverHiringCriteriaData } from '../../../../../../../../modules/submission/data/riskspecifics/driver-hiring-criteria.data';
import { SubmissionNavValidateService } from '../../../../../../../../core/services/navigation/submission-nav-validate.service';
import { PathConstants } from '../../../../../../../../shared/constants/path.constants';
import { ReportData } from '../../../../../../../../modules/submission/data/report/report.data';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Subject } from 'rxjs';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-quote-section',
  templateUrl: './quote-section.component.html',
  styleUrls: ['./quote-section.component.scss']
})
export class QuoteSectionComponent implements OnInit {
  private readonly destroy$ = new Subject();
  @ViewChild('UploadFileInput', { static: false }) uploadFileInput: ElementRef;
  @ViewChild('btnShowLoadingUploader') btnShowLoadingUploader;
  @ViewChild('fileSelection') fileSelection: ElementRef;
  @Output() public onUploadFinished = new EventEmitter();
  dropdownSettings: IDropdownSettings = {};
  public message: string;
  public progress: number;
  public styleLoading: string;
  hideMe: boolean = false;

  public experienceRaterDto: ExperienceRaterDto = null;
  public isPreloading: boolean = true;
  public preloadingTitle: string = 'Preloading rater...';
  constructor(
    public quoteLimitsData: QuoteLimitsData,
    public raterApiData: RaterApiData,
    public submissionData: SubmissionData,
    public submissionNavSavingService: SubmissionNavSavingService,
    private riskSpecificsData: RiskSpecificsData,
    private driverHiringCriteriaData: DriverHiringCriteriaData,
    private submissionValidationService: SubmissionNavValidateService,
    private reportData: ReportData,
    private router: Router,
    private toastr: ToastrService,
    private datePipe: DatePipe
  ) { }

  ngOnInit() {
    this.dropdownSettings = {
      singleSelection: false,
      idField: 'value',
      textField: 'label',
      selectAllText: 'Select All',
      unSelectAllText: 'Unselect All',
      itemsShowLimit: 3,
      allowSearchFilter: true
    };

    // if these screens are skipped. Should include in validation.
    this.riskSpecificsData.isInitialMaintenanceAndSatefy = false;
    this.driverHiringCriteriaData.isInitial = false;
    this.riskSpecificsData.isInitialGeneralLiabAndCargo = false;
    this.submissionValidationService.validateCurrentCategory(PathConstants.Submission.RiskSpecifics.MaintenanceSafety);
    this.submissionValidationService.validateCurrentCategory(PathConstants.Submission.RiskSpecifics.DriverInfo);
    this.submissionValidationService.validateCurrentCategory(PathConstants.Submission.RiskSpecifics.GeneralLiabilityCargo);
    this.submissionValidationService.validateCurrentCategory(PathConstants.Submission.Claims.Index);
    // ---------------------------------------------------------------
    this.preloadRater();
    this.raterApiData.experienceRaterDto.pipe(takeUntil(this.destroy$))
      .subscribe(dto => this.experienceRaterDto = dto);
  }

  get subjectivitiesList() { return this.quoteLimitsData.subjectivities; }

  get xpRaterButtonDisabled() {
    return ((!this.requiredModulesValid));
  }

  get xpRaterTooltip() {
    if (this.xpRaterButtonDisabled && !this.requiredModulesValid) {
      return this.raterApiData.showRequiredtoRateToolTip;
    }
    return null;
  }

  get requiredModulesValid() {
    return this.raterApiData.validateRequiredToRateModules();
  }

  get newBusiness() {
    return this.submissionData.riskDetail.businessDetails.isNewVenture;
  }

  get hasLiabilityRF() {
    var riskCov = this.submissionData?.riskDetail?.riskCoverages[0];
    var hasRF = riskCov.liabilityExperienceRatingFactor != null
      && riskCov.liabilityScheduleRatingFactor != null;
    return hasRF;
  }

  get raterVersionText() {
    const riskDetail = this.submissionData.riskDetail;
    var text = 'Rater version: ' + riskDetail.raterVersionId;
    if (riskDetail.isRaterVersionLocked) {
      text += ' (Locked)';
    }
    return text;
  }

  get xpRaterUploadTooltip() {
    var text = '';
    if(this.experienceRaterDto != null) {
      text = `Last uploaded on ${this.datePipe.transform(this.experienceRaterDto.dateModified, 'MM/dd/yyyy')} 
              (version ${this.experienceRaterDto.raterVersionId })`;
    } else {
      text = 'No experience rater uploaded yet.';
    }

    var tooltip = !this.raterApiData.validateRequiredToRateModules() ? this.raterApiData.showRequiredtoRateToolTip : '';
    return tooltip += `\n${text}`;
  }

  public ToggleHiding(): void {
    this.hideMe = !this.hideMe;
  }

  clickUpload(f) {
    this.fileSelection.nativeElement.value = null;
    this.fileSelection.nativeElement.click();
  }

  public uploadFile = (f) => {
    if (!this.raterApiData.validateRequiredToRateModules()) { return; }
    this.raterApiData.btnShowLoadingUploader = this.btnShowLoadingUploader;
    this.raterApiData.onUploadFinished = this.onUploadFinished;

    try {
      this.raterApiData.upload(f)
      .then((result) => {
        this.preloadRater();
      })
      .catch((error) => {
        this.isPreloading = false;
        this.preloadingTitle = '';
      });
    } catch (error) {
        this.isPreloading = false;
        this.preloadingTitle = '';
    }
  }

  clickDownload(obj) {
    Utils.blockUIWithMessage("Saving current page...");
    obj.preventDefault();
    obj.stopPropagation();

    obj.target.disabled = true;

    this.submissionNavSavingService.saveCurrentPage(this.router.url).then(() => {
      Utils.unblockUI();
      if (!this.raterApiData.validateRequiredToRateModules()) { obj.preventDefault(); return; }
      this.raterApiData.btnShowLoadingUploader = this.btnShowLoadingUploader;
      this.raterApiData.onUploadFinished = this.onUploadFinished;
      if (this.submissionNavSavingService.isSaving) {
        this.submissionNavSavingService.saveCategoryEnded.first().pipe(take(1)).subscribe(() => {
          this.triggerExpRaterDownload(obj);
        });
      } else {
        this.triggerExpRaterDownload(obj);
      }
    }).catch(err => {
      Utils.unblockUI();
      console.error(err);
      this.toastr.error("Error saving page.");
    })
  }

  private triggerExpRaterDownload(obj): void {
    this.quoteLimitsData.saveOptions()
      .pipe(tap(() => this.raterApiData.Download(obj)))
      .subscribe();
  }

  get disableCopySubmission() {
    if (this.submissionData.riskDetail.policyNumber) { return true; }
    return false;
  }

  get copySubmissionTooltip() {
    return 'Copy Submission';
  }
  
  copySubmission() {
    this.submissionData.CopySubmission();
  }

  private preloadRater() {
    this.isPreloading = true;
    this.preloadingTitle = 'Preloading rater...';
    
    this.raterApiData.preloadRater().subscribe(
      (success) => {
        this.isPreloading = false;
        this.preloadingTitle = '';
      },
      (error) => {
        this.isPreloading = false;
        this.preloadingTitle = '';
      }
    );
  }
}
