import { Injectable } from '@angular/core';
import { SubmissionData } from '../data/submission.data';
import { ToastrService } from 'ngx-toastr';
import NotifUtils from '../../../shared/utilities/notif-utils';
import { RaterApiService } from '../services/rater-api.service';
import { HttpEventType } from '@angular/common/http';
import { formatDate } from '@angular/common';
import { SummaryData } from '../data/summary.data';

import { LvAccountCategory, LvBusinessType, LvGVWCategoryBeforeV7, LvGVWCategoryV7, LvMainUse, LvNewVenturePreviousExperience, LvSecondaryBusinessGroup } from '../../../shared/constants/business-details.options.constants';
import { ApplyFilter } from '../../../shared/constants/vehicle-driver.options.constants';

import { VehicleData } from '../data/vehicle/vehicle.data';
import { raterMappingTemplate } from '../../../shared/constants/raterMappingTemplate';
import { LimitsData } from './limits/limits.data';
import Utils from '../../../shared/utilities/utils';
import { QuoteLimitsData } from './quote/quote-limits.data';
import { CoverageData } from './coverages/coverages.data';
import { BehaviorSubject, EMPTY, Observable, observable, of, throwError } from 'rxjs';
import { catchError, map, switchMap, tap } from 'rxjs/operators';
import { RiskCoverageDTO } from '@app/shared/models/submission/limits/riskCoverageDto';
import { PreloadingStatus } from '@app/modules/rating/models/preloading-status';
import { PolicyRatingFactorsDTO } from '@app/modules/policy/components/policy-details/sections/policy-rating/policy-rating-factors-dto';
import { DestinationData } from './riskspecifics/destination.data';
import { RadiusOfOperationsData } from './riskspecifics/radius-of-operations.data';
import { DatePipe } from '@angular/common';
import { AgencyData } from '../../management/data/agency.data'
import { LimitsDefaultValues } from '@app/shared/constants/limits-defaults.constants';
import { ApplicantData } from '../data/applicant/applicant.data';

@Injectable()
export class RaterApiData {

  private _ratingData: BehaviorSubject<any> = new BehaviorSubject<any>(null);
  get ratingData(): Observable<any> {
    return this._ratingData.asObservable();
  }

  private _preloadingStatus: BehaviorSubject<PreloadingStatus> = new BehaviorSubject<PreloadingStatus>(null);
  get preloadingStatus(): Observable<PreloadingStatus> {
    return this._preloadingStatus.asObservable();
  }

  private _areModulesValidToRate: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  get areModulesValidToRate() {
    return this._areModulesValidToRate.asObservable();
  }

  private _areModulesValidToRatePremium: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  get areModulesValidToRatePremium() {
    return this._areModulesValidToRatePremium.asObservable();
  }

  private _ratingFactors: BehaviorSubject<PolicyRatingFactorsDTO> = new BehaviorSubject<PolicyRatingFactorsDTO>(null);
  get ratingFactors() : Observable<PolicyRatingFactorsDTO> {
    return this._ratingFactors.asObservable();
  }

  private _raterVersion: BehaviorSubject<any> = new BehaviorSubject<RaterVersion>(null);
  get raterVersion() : Observable<RaterVersion> { return this. _raterVersion.asObservable(); }

  private _experienceRaterDto = new BehaviorSubject<ExperienceRaterDto>(null); 
  get experienceRaterDto() { return this._experienceRaterDto.asObservable(); }

  constructor(
    public summaryData: SummaryData,
    public submissionData: SubmissionData,
    private toastr: ToastrService,
    private raterApi: RaterApiService,
    private vehicleData: VehicleData,
    private limitsData: LimitsData,
    private quoteLimitsData: QuoteLimitsData,
    private coverageData: CoverageData,
    private radiusOfOperationsData: RadiusOfOperationsData,
    private agencyData: AgencyData,
    private applicantData: ApplicantData,
  ) { 
    this.datePipe = new DatePipe('en-US');
    this.agencyData.getAgencies();
  }

  //#region  RATER CODE
  disableCalculatePremium: boolean = false;
  disableDeletePremium: boolean = false;
  uploadFileInput: any;
  btnShowLoadingUploader: any;
  ileSelection: any;
  message: string;
  dotanimation: string = '';
  progress: number;
  datePipe: DatePipe;

  showPremiumToolTipPreload: string = '';
  showRequiredtoRateToolTip: string = 'There are missing required to rate fields';

  onUploadFinished: any;

  getRaterVersionOfEffectiveDate$(effectiveDate: Date) : Observable<RaterVersion> {
    return this.raterApi.getRaterVersionOfEffectiveDate$(effectiveDate);
  }

  public upload(f): Promise<unknown> {
    if (this.submissionData?.isIssued) { return; }
    if (f.length === 0) {
      return;
    }
    // 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'
    const af: Array<string> = ['application/vnd.ms-excel.sheet.binary.macroenabled.12'];
    const fileToUpload = <File>f[0];

    if (!af.find(x => x.toString().toLowerCase() === fileToUpload.type.toLowerCase())) {
      NotifUtils.showError('Invalid Excel Rater File type. Download the correct Rater binary file.', () => {
        return;
      });
      return;
    }

    this.btnShowLoadingUploader.nativeElement.click();
    const formData = new FormData();
    formData.append('file', fileToUpload, fileToUpload.name);
    formData.append('riskDetailId', this.submissionData.riskDetail?.id);
    const id = this.submissionData.riskDetail?.submissionNumber;

    const intervalID = this.dotAnimation();

    const dvloaderbar = document.querySelector('#dvloaderbar') as any;

    this.message = `Upload in progress`;
    this.progress = 0;
    dvloaderbar.style.width = `${this.progress}%`;
    return this.raterApi.upload2(formData, id,
      (event) => { // upload start
        this.progress = 1;
        dvloaderbar.style.width = `${this.progress}%`;
      }, (event) => { // upload in progress
        if (event.lengthComputable) {
          setTimeout(() => {
            this.progress = Math.round((event.loaded / event.total) * 100) - 1;
            this.message = 'Uploading';
            dvloaderbar.style.width = `${this.progress}%`;
            if (this.progress == 99) {
              setTimeout(() => {
                this.message = 'Verifying upload';
              });
            }
          }, 2000);
        }
      }).then((response: any) => {
        const outputData = JSON.parse(response);
        let errorMessage = this.validateRatingFactors(outputData);
        if(response.status == 500) { errorMessage = `Failed uploading rater for ${id}.`; } 

        if (errorMessage) {
          NotifUtils.showError(errorMessage, () => { return; });
          this.onUploadFinished.emit(this.btnShowLoadingUploader.nativeElement.click());
        } else {
          if (this.progress == 99) {
            this.appendRatingFactorsFromUpload(outputData).pipe(
              tap(() => {
                this.progress = 100;
                this.message = `Upload complete`;
                dvloaderbar.style.width = `100%`;
                setTimeout(() => {
                  this.toastr.success('Upload Successful!', 'Success!');
                  this.onUploadFinished.emit(this.btnShowLoadingUploader.nativeElement.click());
                }, 800);
              }),
              switchMap(() => this.preloadRater()),
              switchMap(() => this.loadExperienceRaterDto$())
            ).subscribe();
          }
        }
      })
      .catch((error) => {
        console.error(error);
        var errorMessage= 'Error encountered while uploading the file. ';
        if(!!error) { errorMessage += `Failed uploading rater for ${id}.`; }
        NotifUtils.showError(errorMessage);
        this.onUploadFinished.emit(this.btnShowLoadingUploader.nativeElement.click());
      }).finally(() => {
        clearInterval(intervalID);
      });
  }

  Download(obj) {
    if (this.submissionData?.isIssued) { return; }
    
    this.limitsData.retrieveDropDownValues();
    this.vehicleData.retrieveDropDownValues();
    const riskDetailId = this.submissionData.riskDetail?.id;
    const submissionId = this.submissionData.riskDetail?.submissionNumber;
    const raterVersionId = this.submissionData.riskDetail?.raterVersionId;
    const businessName = this.splice100AndTrimInvalidChar(this.submissionData.riskDetail?.nameAndAddress?.businessName);
    const agencyName = this.splice100AndTrimInvalidChar(this.getAgencyName(this.submissionData.riskDetail?.brokerInfo?.agencyId));
    const localeDate = new Date().toLocaleString('en-US', { timeZone: 'America/New_York' });
    const currentDate = this.datePipe.transform(localeDate, 'MM_dd_yyyy');

    console.log(`Downloading Exp Rater, the current date is ${localeDate}`);

    const fileName = `${businessName}, ${agencyName}, ${currentDate}`;
    
    //const effectiveDate = formatDate(this.summaryData.effectiveDate, 'yyyy-MM-dd', 'en');

    //#region  loading codde
    const dvloaderbar = document.querySelector('#dvloaderbar') as any;

    obj.target.disabled = true;
    this.message = `Requesting download`;
    this.progress = 0;

    this.btnShowLoadingUploader.nativeElement.click();
    dvloaderbar.style.width = `0%`;

    const intervalID = this.dotAnimation();

    //#region Get data sets
    let inputData = raterMappingTemplate; // this.dataMappingTemplate();
    inputData.SubmissionNo = submissionId;

    //#endregion Get data sets
    this.raterApi.download2(riskDetailId, raterVersionId, inputData,
      (event) => {
        this.progress = 0;
        this.message = 'Generating experience rater file';
        dvloaderbar.style.width = `${this.progress}%`;
      },
      (event) => {
        if (event.lengthComputable) {
          this.progress = Math.round(100 * event.loaded / event.total);
          this.message = 'Downloading';
          dvloaderbar.style.width = `${this.progress}%`;
        }
      })
      .then(blobFile => {
        setTimeout(() => {
          this.message = `Download complete`;
          dvloaderbar.style.width = `100%`;
          this.toastr.success('Download Successful!', 'Success!');

          this.downLoadFile((<Blob>blobFile), fileName);
          this.onUploadFinished.emit(this.btnShowLoadingUploader.nativeElement.click());
        }, 2000);
      }).catch(blobFile => {
        const errMsg = 'Error encountered while downloading the file. Please try again.';
        NotifUtils.showError(errMsg, () => { });
        this.toastr.error('Error encountered while downloading the file.', 'Failed!');
        this.onUploadFinished.emit(this.btnShowLoadingUploader.nativeElement.click());
      }).finally(() => {
        obj.target.disabled = false;
        clearInterval(intervalID);
      });
  }

  DownloadPremiumRater(obj) {
    const submissionId = this.submissionData.riskDetail?.submissionNumber;
    const policyName = this.submissionData.riskDetail?.nameAndAddress?.businessName;
    
    //#region  loading codde
    const dvloaderbar = document.querySelector('#dvloaderbar') as any;

    obj.target.disabled = true;
    this.message = `Requesting download`;
    this.progress = 0;

    this.btnShowLoadingUploader.nativeElement.click();
    dvloaderbar.style.width = `0%`;

    const intervalID = this.dotAnimation();

    //#region Get data sets
    let inputData = raterMappingTemplate; // this.dataMappingTemplate();
    inputData.SubmissionNo = submissionId;

    //#endregion Get data sets
    this.raterApi.downloadPremiumRater(inputData.SubmissionNo, inputData,
      (event) => {
        this.progress = 0;
        this.message = 'Preparing premium rater file';
        dvloaderbar.style.width = `${this.progress}%`;
      },
      (event) => {
        if (event.lengthComputable) {
          this.progress = Math.round(100 * event.loaded / event.total);
          this.message = 'Downloading';
          dvloaderbar.style.width = `${this.progress}%`;
        }
      })
      .then(blobFile => {
        setTimeout(() => {
          this.message = `Download complete`;
          dvloaderbar.style.width = `100%`;
          this.toastr.success('Download Successful!', 'Success!');

          const fileName = `${policyName}, Premium Rater`;
          this.downLoadFile((<Blob>blobFile), fileName);
          this.onUploadFinished.emit(this.btnShowLoadingUploader.nativeElement.click());
        }, 2000);
      }).catch(blobFile => {
        const errMsg = 'Error encountered while downloading the file. Please try again.';
        NotifUtils.showError(errMsg, () => { });
        this.toastr.error('Error encountered while downloading the file.', 'Failed!');
        this.onUploadFinished.emit(this.btnShowLoadingUploader.nativeElement.click());
      }).finally(() => {
        obj.target.disabled = false;
        clearInterval(intervalID);
      });
  }

  DownloadExperienceRater(obj) {
    const submissionId = this.submissionData.riskDetail?.submissionNumber;
    const riskId = this.submissionData.riskDetail?.riskId;
    const policyName = this.submissionData.riskDetail?.nameAndAddress?.businessName;
    const raterVersionId = this.submissionData.riskDetail?.raterVersionId;
    
    //#region  loading codde
    const dvloaderbar = document.querySelector('#dvloaderbar') as any;

    obj.target.disabled = true;
    this.message = `Requesting download`;
    this.progress = 0;

    this.btnShowLoadingUploader.nativeElement.click();
    dvloaderbar.style.width = `0%`;

    const intervalID = this.dotAnimation();

    //#region Get data sets
    let inputData = raterMappingTemplate; // this.dataMappingTemplate();
    inputData.SubmissionNo = submissionId;

    //#endregion Get data sets
    this.raterApi.downloadExperienceRater(inputData.SubmissionNo, raterVersionId, riskId, inputData,
      (event) => {
        this.progress = 0;
        this.message = 'Preparing experience rater file';
        dvloaderbar.style.width = `${this.progress}%`;
      },
      (event) => {
        if (event.lengthComputable) {
          this.progress = Math.round(100 * event.loaded / event.total);
          this.message = 'Downloading';
          dvloaderbar.style.width = `${this.progress}%`;
        }
      })
      .then(blobFile => {
        setTimeout(() => {
          this.message = `Download complete`;
          dvloaderbar.style.width = `100%`;
          this.toastr.success('Download Successful!', 'Success!');

          const fileName = `${policyName}, Bound Exp Rater`;
          this.downLoadFile((<Blob>blobFile), fileName);
          this.onUploadFinished.emit(this.btnShowLoadingUploader.nativeElement.click());
        }, 2000);
      }).catch(blobFile => {
        const errMsg = 'Error encountered while downloading the file. Please try again.';
        NotifUtils.showError(errMsg, () => { });
        this.toastr.error('Error encountered while downloading the file.', 'Failed!');
        this.onUploadFinished.emit(this.btnShowLoadingUploader.nativeElement.click());
      }).finally(() => {
        obj.target.disabled = false;
        clearInterval(intervalID);
      });
  }

  dotAnimation() {
    let counter = 1;
    const intervalID = setInterval(() => {
      if (counter > 0 && counter < 5) {
        this.dotanimation = this.dotanimation + '.';
      } else {
        counter = 0;
        this.dotanimation = '.';
      }
      counter += 1;
    }, 700);
    return intervalID;
  }

  downLoadFile(blob: Blob, fileName: string) {
    const downloadLink: HTMLAnchorElement = document.createElement('a') as HTMLAnchorElement;
    const url = window.URL.createObjectURL(blob);
    downloadLink.href = url;
    downloadLink.download = `${fileName}.xlsb`;
    document.body.appendChild(downloadLink);
    downloadLink.click();
    document.body.removeChild(downloadLink);
    URL.revokeObjectURL(url);
  }

  validateRequiredToRateModules(): boolean {
    const excludeBindreq = ['applicantNameAndAddressBinding', 'additionalInterestBinding', 'businessDetailsBinding', 'bind', 'brokerBinding'];
    const riskValidationList = Object.keys(this.submissionData.validationList).filter(x => !excludeBindreq.find(r => r === x));
    const invalidModule = riskValidationList
      .find(e => e !== 'uwAnswersConfirmed' && this.submissionData.validationList[e] == false);
    const lstToValidate: Array<any> = [undefined, null];
    var areModulesValidToRate = (lstToValidate.indexOf(invalidModule) !== -1);
    this._areModulesValidToRate.next(areModulesValidToRate);
    return areModulesValidToRate;
  }

  validateRequiredToRatePremiumModules(): boolean {
    const excludeBindreq = ['applicantNameAndAddressBinding', 'additionalInterestBinding', 'businessDetailsBinding', 'bind', 'brokerBinding'];
    const riskValidationList = Object.keys(this.submissionData.validationList).filter(x => !excludeBindreq.find(r => r === x));
    const invalidModule = riskValidationList
      .find(e => e !== 'uwAnswersConfirmed' && this.submissionData.validationList[e] == false);
    const lstToValidate: Array<any> = [undefined, null];

    var areModulesValidToRate = (lstToValidate.indexOf(invalidModule) !== -1);

    if (this.submissionData != null
      && this.submissionData.riskDetail != null
      && (this.submissionData?.riskDetail?.riskCoverage != null || this.submissionData?.riskDetail?.riskCoverages != null)
    ) {
      var riskCov;
      if (this.submissionData.riskDetail.riskCoverages != null) {
        riskCov = this.submissionData?.riskDetail?.riskCoverages[0];
      } else {
        riskCov = this.submissionData?.riskDetail?.riskCoverage;
      }
      areModulesValidToRate = areModulesValidToRate && (riskCov.liabilityScheduleRatingFactor != null && riskCov.liabilityScheduleRatingFactor != null);
    } else {
      areModulesValidToRate = false;
    }

    this._areModulesValidToRatePremium.next(areModulesValidToRate);

    return areModulesValidToRate;
  }


  preloadRater(fromPolicy?: boolean): Observable<any> {
    // making sure that it cant proceed here
    if (fromPolicy != true && !this.validateRequiredToRatePremiumModules()) {
      return throwError(this.showRequiredtoRateToolTip);
    }
    this._preloadingStatus.next(PreloadingStatus.InProgress);
    this.disableCalculatePremium = true;
    this.showPremiumToolTipPreload = '';
    const riskDetailId = this.submissionData.riskDetail?.id;
    const submissionId = this.submissionData.riskDetail?.submissionNumber;
    const EffectiveDate = this.submissionData.riskDetail?.brokerInfo?.effectiveDate;

    // do not allow pre load if incomplete required parameter.
    // || !EffectiveDate
    if (!riskDetailId || !submissionId) { return; }
    console.log(`Preloaded Init Rater for ${submissionId}.`);
    this.showPremiumToolTipPreload = `Preloading rater...`;
    return this.raterApi.preLoadPremium(submissionId, riskDetailId, fromPolicy)
      .pipe(
        tap((result) => {
          if (result.status === true) {
            this.disableCalculatePremium = false;
            this.showPremiumToolTipPreload = null;
            console.log(`Preloaded Rater for ${submissionId}.`);
            this._preloadingStatus.next(PreloadingStatus.Complete);
          } else {
            // do not allow the premium rating(calculate premium ) to be executed if there is an error thrown.
            if (result.message) {
              console.log(result.message);
              this.showPremiumToolTipPreload = `${result.message}`;
              this.disableCalculatePremium = true;
              this._preloadingStatus.next(PreloadingStatus.Failed);
            }
          }
        }),
        catchError(error => {
          console.error(error);
          console.error(`Error trying to preload the Rating engine.`, 'Failed!');
          this.showPremiumToolTipPreload = `${error}`;
          this.disableCalculatePremium = true;
          this._preloadingStatus.next(PreloadingStatus.Failed);
          return EMPTY;
        })
      );
  }

  UnloadPremium(ispageunload: boolean = false, fromPolicy?: boolean) {
    fromPolicy = fromPolicy ?? false;
    const submissionId = this.submissionData.riskDetail?.submissionNumber;
    const riskDetailId = this.submissionData.riskDetail?.id;
    this.disableDeletePremium = true;

    this.raterApi.unloadPremium(submissionId, riskDetailId, ispageunload, fromPolicy).subscribe(() => {
      console.log(`Unloaded Rater for ${submissionId}.`);
      this.disableDeletePremium = false;
    }, (error) => {
      console.log(error);
      this.disableDeletePremium = false;
    });
  }

  Calculate(optionId: number) {
    if (this.submissionData?.isIssued) { return; }
    Utils.blockUI();
    const inputData = this.createInputDataFromOptionId(optionId);
    this.executeCalculatePremium(inputData);
  }

  private executeCalculatePremium(obj: any) {
    this.raterApi.calculatePremium(obj).subscribe(event => {
      if (event.type === HttpEventType.Response) {
        const result = JSON.parse(event.body);
        if (!result.success) {
          console.log(result);
          this.toastr.error('Error encountered while calculating premium.', 'Failed!');
        } else {
          console.log(result);
          this.appendCalculatedData(result);
          this._ratingData.next(result);
          this.toastr.success('Premium calculated successfully.', 'Success!');
        }
        Utils.unblockUI();
      }
    }, (error) => {
      Utils.unblockUI();
      console.log(error);
      this.toastr.error('Error encountered while calculating premium.', 'Failed!');
    });

    setTimeout(() => {
      this.disableCalculatePremium = false;
    }, 12000);
  }

  calculateForQuote(optionId: number, forIssuance?: boolean) : Promise<any> {
    const coverages = this.submissionData.riskDetail?.riskCoverages;
  
    const promise = new Promise((resolve, reject) => {
      try {
        if (!this.validateRequiredToRateModules()) {
          throw new Error('There are still missing Required to Quote fields.');
        }
        if (coverages?.length < optionId) {
          resolve(`No Option ${optionId}. Premium calculation is skipped.`);
        } else {
          // Save quote options so the factors in it are always updated before calculating.
          this.quoteLimitsData.saveOptions().pipe(
            map(() => this.createInputDataFromOptionId(optionId)),
            switchMap((inputData) => this.raterApi.calculatePremium({ inputData: inputData, forIssuance: forIssuance }))
          ).subscribe(result => {
            const t1 = performance.now();
            var data = JSON.parse(result);
            console.log(data);
            if (!data.success) {
              reject(`Error calculating Option ${optionId} premium. ${data.message}`);
            } else {
              //this.appendCalculatedData(result);
              this._ratingData.next(data);
              this.appendCalculatedData(data);
              this.toastr.success('Premium calculated successfully.', 'Success!');
              resolve(`Successfully calculated Option ${optionId} premium.`);

              this.summaryData.setFromOptions(this.roundToOneDecimalPlace(coverages[0].brokerCommisionAL),this.roundToOneDecimalPlace(coverages[0].brokerCommisionPD));
              this.submissionData.setFromOptions(this.roundToOneDecimalPlace(coverages[0].brokerCommisionAL),this.roundToOneDecimalPlace(coverages[0].brokerCommisionPD));
              
            }
          }, (error) => {
            console.error(error);
            var errorMessage = `Error calculating Option ${optionId} premium.`;
            if(error.status == 400) {
              errorMessage += ` ${error.error}`
            }
            this.toastr.error(errorMessage, 'Error!', {
              timeOut: 60000,
              closeButton: true
            });
            reject(`${errorMessage}`);
          });
        }
      } catch (error) {
        reject(error);
      }
    });
    return promise;
  }

  calculate(params: { covOption: RiskCoverageDTO, effectiveDate: Date, forIssuance?: boolean }): Observable<any> {
    if (!this.validateRequiredToRateModules())
      throw new Error('There are still missing Required to Quote fields.');

    let inputData = null;
    try {
      inputData = this.createInputDataFromRiskCoverage(params.covOption);
      const t0 = performance.now();
    } catch (e) {
      return of(e);
    }

    var calculateParams = {
      inputData: inputData,
      fromPolicy: true,
      effectiveDate: params.effectiveDate,
      forIssuance: params.forIssuance
    };
    return this.raterApi.calculatePremium(calculateParams).pipe(
      tap((data) => {
        this.toastr.success('Premium calculated successfully.', 'Success!');
        const t1 = performance.now();
        console.log(JSON.parse(data));
        this.appendCalculatedData(JSON.parse(data));
        this._ratingData.next(data);
      }),
      catchError((err) => {
        console.log(err);
        var errorMessage = `Error calculating premium`;
        this.toastr.error(errorMessage, 'Error!');
        throw "Error calculating premium";
      })
    );
  }

  /**
   * Fetches the updated rating factors from api and updates the stored data.
   * Subscribe to this.ratingFactors to retrieve the stored data.
   */
  loadRatingFactors$(riskDetailId) : Observable<any> {
    return this.raterApi.loadRatingFactors(riskDetailId).pipe(
      tap((data) => {
        let newData = data as PolicyRatingFactorsDTO;
        this._ratingFactors.next(newData);
      })
    );
  }

  updateRatingFactors(ratingFactors): Observable<any> {
    let riskDetail = this.submissionData.riskDetail;
    return this.raterApi.updateRatingFactors(riskDetail.id, ratingFactors).pipe(
      tap((data) => this._ratingFactors.next(data as PolicyRatingFactorsDTO))
    );
  }

  private createInputDataFromOptionId(optionId: number) {
    this.disableCalculatePremium = true;
    // const effectiveDate = formatDate(this.summaryData.effectiveDate, 'yyyy-MM-dd', 'en'); // should we need to cast it to this format?
    // we need to get this query to get the right coverageId and covOption combination
    // in quote option page we need to use this query to get the option detail base on selected optionNumber.
    const covOption = this.submissionData?.riskDetail?.riskCoverages?.find(x => x.optionNumber === optionId);
    const _effectiveDate = this.submissionData.riskDetail?.brokerInfo?.effectiveDate;

    const riskCoverageId = covOption?.id;
    if (!riskCoverageId || !covOption || !_effectiveDate) {
      this.toastr.error(`Risk Coverage not found for option ${optionId}`, 'Failed!');
      this.disableCalculatePremium = false;
      Utils.unblockUI();
      return;
    }

    return this.createInputDataFromRiskCoverage(covOption);
  }

  private createInputDataFromRiskCoverage(covOption: RiskCoverageDTO) {
    const riskDetailId = this.submissionData.riskDetail?.id;
    const submissionId = this.submissionData.riskDetail?.submissionNumber;
    const isZeroYrsInBusiness: boolean = this.submissionData?.riskDetail?.businessDetails?.yearsInBusiness < 1;

    //#region Get data sets
    const inputData = raterMappingTemplate;
    inputData.SubmissionNo = submissionId;
    inputData.riskDetailId = riskDetailId;
    inputData.RiskCoverageId = covOption.id;
    inputData.OptionId = covOption?.optionNumber;
    inputData.Broker.EffectiveDate = this.submissionData.riskDetail?.brokerInfo?.effectiveDate;

    inputData.Applicant.BusinessDetails.UseClass = LvMainUse;
    inputData.Applicant.BusinessDetails.BusinessType = LvBusinessType;
    inputData.Applicant.BusinessDetails.AccountCategory = LvAccountCategory;
    inputData.Applicant.BusinessDetails.NewVenturePreviousExperience = LvNewVenturePreviousExperience;
    inputData.Applicant.BusinessDetails.MainGVW = this.applicantData.applicantDropdownList.mainGVWList;
    inputData.Applicant.BusinessDetails.SecondaryBusinessGroup = this.applicantData.applicantDropdownList.secondaryBusinessGroupList;

    // inputData.Driver.KOUnder25 = ApplyFilter;
    // inputData.Driver.KOAFOrMovingViolations = ApplyFilter;
    this.vehicleData.retrieveDropDownValues();
    this.vehicleData.fetchVehicleDatasets();
    inputData.Vehicle.VehicleDropdowns.VehicleType = this.vehicleData.vehicleDropdownsList.vehicleTypeList;
    inputData.Vehicle.VehicleDropdowns.BusinessClass = this.vehicleData.vehicleDropdownsList.vehicleBusinessClassList;
    inputData.Vehicle.VehicleDropdowns.UseClass = this.vehicleData.vehicleDropdownsList.useClassList;
    inputData.Vehicle.VehicleDropdowns.Description = this.vehicleData.vehicleDropdownsList.vehicleDescriptionList;
    inputData.Vehicle.VehicleDropdowns.CollDeductible = this.limitsData.CollisionList;
    inputData.Vehicle.VehicleDropdowns.CompDeductible = this.limitsData.ComprehensiveList;

    inputData.RequestedCoverages.AutoLiability.AutoBodilyInjury = this.limitsData.biLimitList;
    inputData.RequestedCoverages.AutoLiability.AutoPropertyDamage = this.limitsData.pdLimitList;
    inputData.RequestedCoverages.AutoLiability.AutoLiabilityDeductible = this.limitsData.liabDeductibleList;
    inputData.RequestedCoverages.AutoLiability.MedicalPaymentsLimit = this.limitsData.medpayList;
    inputData.RequestedCoverages.AutoLiability.PIPLimit = this.limitsData.pipList;
    inputData.RequestedCoverages.AutoLiability.UMSCLLimit = this.limitsData.umBiLimitList;
    inputData.RequestedCoverages.AutoLiability.UMPDLimit = this.limitsData.umPdLimitList;
    inputData.RequestedCoverages.AutoLiability.UIMSCLLimit = this.limitsData.uimBiLimitList;
    inputData.RequestedCoverages.AutoLiability.UIMPDLimit = this.limitsData.uimPdLimitList;
    inputData.RequestedCoverages.AutoLiability.SymbolsRequested = this.limitsData.symbolList;

    inputData.RequestedCoverages.PhysicalDamage.FireTheftSpecPerils = this.limitsData.ComprehensiveList;
    inputData.RequestedCoverages.PhysicalDamage.COMPDeductible = this.limitsData.ComprehensiveList;
    inputData.RequestedCoverages.PhysicalDamage.COLLDeductible = this.limitsData.CollisionList;
    inputData.RequestedCoverages.PhysicalDamage.PdSymbol = this.limitsData.symbolList;

    inputData.RequestedCoverages.OtherCoverages.GLBodilyInjuryLimits = this.limitsData.glList;
    inputData.RequestedCoverages.OtherCoverages.CargoCollLimit = this.limitsData.cargoList;
    inputData.RequestedCoverages.OtherCoverages.RefrigeratedCargoLimits = this.limitsData.refBreakList;

    inputData.RequestedCoverages.OtherCoverages.TrailerInterchangeLimit = this.limitsData.trailerInterchangeList;
    inputData.RequestedCoverages.OtherCoverages.NonOwnedTrailerLimit = this.limitsData.nonOwnedTrailerList;
    inputData.RequestedCoverages.OtherCoverages.PollutionCoverageLimit = this.limitsData.pollutionCovList;
    inputData.RequestedCoverages.OtherCoverages.TrailerType = 
      ((covOption.trailerInterchangeLimitId && covOption.trailerInterchangeLimitId !== LimitsDefaultValues.triId) ||
      (covOption.nonOwnedTrailerLimitId && covOption.nonOwnedTrailerLimitId !== LimitsDefaultValues.noId)) ? this.limitsData.trailerTypeList : null;

    inputData.RatingFactors.LiabilityScheduleRatingFactor = covOption?.liabilityScheduleRatingFactor;
    inputData.RatingFactors.LiabilityExperieceRatingFactor = covOption?.liabilityExperienceRatingFactor;
    inputData.RatingFactors.GLScheduleRatingFactor = covOption?.glScheduleRF;
    inputData.RatingFactors.GLExperienceRatingFactor = covOption?.glExperienceRF;
    inputData.RatingFactors.CargoScheduleRatingFactor = covOption?.cargoScheduleRF;
    inputData.RatingFactors.CargoExperienceRatingFactor = covOption?.cargoExperienceRF;
    inputData.RatingFactors.APDScheduleRatingFactor = covOption?.apdScheduleRF;
    inputData.RatingFactors.APDExperienceRatingFactor = covOption?.apdExperienceRF;

    // endorsement premiums
    const riskForms = this.submissionData?.riskDetail?.riskForms;
    const hiredPhysicalDamage = riskForms?.find(form => form.form.id === 'HiredPhysicalDamage' && form.isSelected === true);
    const trailerInterchange = riskForms?.find(form => form.form.id === 'TrailerInterchangeCoverage' && form.isSelected === true);
    const hiredPhysicalDmgPremium = (hiredPhysicalDamage) ? JSON.parse(hiredPhysicalDamage?.other)?.estimatedAdditionalPremium : 0;
    const trailerIntPremium = (trailerInterchange) ? JSON.parse(trailerInterchange?.other) : trailerInterchange;
    inputData.EndorsementPremiums.HiredPhysicalDamage = Number(hiredPhysicalDmgPremium);
    inputData.EndorsementPremiums.TrailerInterchange = (trailerInterchange) ? (Number(trailerIntPremium?.comprePremium) +
      Number(trailerIntPremium?.lossPremium) +
      Number(trailerIntPremium?.collisionPremium) +
      Number(trailerIntPremium?.firePremium) +
      Number(trailerIntPremium?.fireTheftPremium)) : 0;

    const manuscripts = this.submissionData.riskDetail?.riskManuscripts;
    const alManuscriptPremium = manuscripts?.filter(m => m.isSelected && m.premiumType === 'AL')
      ?.reduce((sum, current) => sum + current.premium, 0);
    const pdManuscriptPremium = manuscripts?.filter(m => m.isSelected && m.premiumType === 'PD')
      ?.reduce((sum, current) => sum + current.premium, 0);

    inputData.EndorsementPremiums.ALManuscriptPremium = alManuscriptPremium;
    inputData.EndorsementPremiums.PDManuscriptPremium = pdManuscriptPremium;

    return inputData;
  }

  public appendCalculatedData(dataSet: any): void {
    if (!dataSet.success) {
      return;
    }

    let data = dataSet.data;
    const riskDetailId = data?.inputValues?.riskDetailId;
    const optionNumber = data?.inputValues?.OptionId;
    const vehicleFactors = data?.result?.Vehicle;
    const driverFactors = data?.result?.Driver;
    const driverHeader = this.submissionData?.riskDetail?.driverHeader;
    const drivers = this.submissionData?.riskDetail?.drivers;
    const vehicles = this.submissionData?.riskDetail?.vehicles;
    const riskCoverage = this.submissionData?.riskDetail?.riskCoverages?.find(x => x.optionNumber == optionNumber && x.riskDetailId === riskDetailId);
    // driver header
    if (optionNumber === '1' || optionNumber === 1 && driverHeader !== null) {
      driverHeader.accDriverFactor = this.parseValueToDecimal(data?.result?.AccountDriverFactor ?? 0);
    }

    drivers?.forEach(drvr => {
      const dataDrvr = driverFactors.find((element) => element.DriverID === drvr.id);
      if (dataDrvr) {
        drvr.ageFactor = this.parseValueToDecimal(+(dataDrvr.AgeFactor ?? 0).toFixed(4));
        drvr.yearPoint = dataDrvr.FiveYearPoints;
        drvr.totalFactor = this.parseValueToDecimal(+(dataDrvr.TotalFactor ?? 0).toFixed(4));
      }
    });

    // premiums
    let totalALPremium = 0;
    let totalPDPremium = 0;
    let totalCargoPremium = 0;
    let vehicleLevelPremium = 0;
    let arsCommissionAmount = 0;

    if (optionNumber == 1) {
      // vehicles
      vehicles?.forEach(veh => {
        const dataVeh = vehicleFactors.find((element) => element.VehicleID === veh.id);
        if (dataVeh) {
          veh.autoLiabilityPremium = Number(this.parseValueToDecimal(dataVeh.ALPremium ?? 0));
          veh.cargoPremium = Number(this.parseValueToDecimal(dataVeh.CargoPremium ?? 0));
          veh.physicalDamagePremium = Number(this.parseValueToDecimal(dataVeh.PDPremium ?? 0));
          veh.totalPremium = Number(this.parseValueToDecimal(dataVeh.PerVehiclePremium ?? 0));
          if(this.submissionData?.riskDetail?.raterVersionId < 4)
          {
            veh.radius = Number(this.parseValueToDecimal(dataVeh.Radius ?? 0));
          }
          veh.radiusTerritory = Number(this.parseValueToDecimal(dataVeh.RadiusTerritory ?? 0));

          // compute premiums for display
          vehicleLevelPremium += Number(this.parseValueToDecimal(dataVeh.PerVehiclePremium ?? 0));
          totalALPremium += Number(this.parseValueToDecimal(dataVeh.ALPremium ?? 0));
          totalPDPremium += Number(this.parseValueToDecimal(dataVeh.PDPremium ?? 0));
          totalCargoPremium += Number(this.parseValueToDecimal(dataVeh.CargoPremium ?? 0));

        } else {
          veh.autoLiabilityPremium = 0;
          veh.cargoPremium = 0;
          veh.physicalDamagePremium = 0;
          veh.totalPremium = 0;
          veh.radius = 0;
          veh.radiusTerritory = 0;
        }
      });
    }

    // endorsement premiums
    const hiredPhysicalDmgPremium = this.parseValueToDecimal(data?.inputValues?.EndorsementPremiums.HiredPhysicalDamage);
    const trailerIntPremium = this.parseValueToDecimal(data?.inputValues?.EndorsementPremiums.TrailerInterchange);
    const pdManuscriptPremium = this.parseValueToDecimal(data?.inputValues?.EndorsementPremiums.PDManuscriptPremium);
    const alManuscriptPremium = this.parseValueToDecimal(data?.inputValues?.EndorsementPremiums.ALManuscriptPremium);
    totalPDPremium = totalPDPremium + Number(hiredPhysicalDmgPremium) + Number(trailerIntPremium) + Number(pdManuscriptPremium);
    totalALPremium = totalALPremium + Number(alManuscriptPremium);

    const generalLiabilityPremium = Number(this.parseValueToDecimal(data?.result?.GeneralLiabilityPremium));
    const trailerInterchangePremiumAL = Number(this.parseValueToDecimal(data?.result?.TrailerInterchangePremium));
    const nonOwnedTrailerPremium = Number(this.parseValueToDecimal(data?.result?.NonOwnedTrailerPremium));
    const averagePerVehicle = (vehicleLevelPremium > 0 && vehicles.length > 0) ? vehicleLevelPremium / vehicles.length : 0;
    const additionalInsuredPremium = Number(this.parseValueToDecimal(data?.inputValues?.AdditionalInterest?.AdditionalInsuredPremium));
    const primaryNonContributoryPremium = Number(this.parseValueToDecimal(data?.inputValues?.AdditionalInterest?.PrimaryNonContributoryPremium));
    const waiverOfSubrogationPremium = Number(this.parseValueToDecimal(data?.inputValues?.AdditionalInterest?.WaiverOfSubrogationPremium));
    const totalPremium = totalALPremium + totalPDPremium + totalCargoPremium + generalLiabilityPremium + trailerInterchangePremiumAL + nonOwnedTrailerPremium +
          additionalInsuredPremium + primaryNonContributoryPremium + waiverOfSubrogationPremium;
    const fees = Number(this.parseValueToDecimal(data?.inputValues?.Vehicle?.RiskManagementFeeAL)) + Number(this.parseValueToDecimal(data?.inputValues?.Vehicle?.RiskManagementFeePD));
    totalALPremium += additionalInsuredPremium + primaryNonContributoryPremium + waiverOfSubrogationPremium;
    
    const arsCommission = riskCoverage.arsCommission;
    arsCommissionAmount = totalALPremium * (arsCommission / 100);

    riskCoverage.riskCoveragePremium = {
      accountDriverFactor: Number(this.parseValueToDecimal(data?.result?.AccountDriverFactor)),
      additionalInsuredPremium: additionalInsuredPremium,
      generalLiabilityPremium: generalLiabilityPremium,
      fees: Number(this.parseValueToDecimal(fees)),
      // premiumTaxesFees: data?.result?.PremiumTaxesFees || 0,
      primaryNonContributoryPremium: primaryNonContributoryPremium,
      riskMgrFeeAL: Number(this.parseValueToDecimal(data?.inputValues?.Vehicle?.RiskManagementFeeAL)),
      riskMgrFeePD: Number(this.parseValueToDecimal(data?.inputValues?.Vehicle?.RiskManagementFeePD)),
      // subTotalDue: data?.result?.SubTotalDue || 0,
      // totalAnnualPremiumWithPremiumTax: data?.result?.TotalAnnualPremiumWithPremiumTax || 0,
      cargoPremium: Number(this.parseValueToDecimal(totalCargoPremium)),
      autoLiabilityPremium: Number(this.parseValueToDecimal(totalALPremium)),
      // totalOtherPolicyLevelPremium: data?.result?.TotalOtherPolicyLevelPremium || 0,
      physicalDamagePremium: Number(this.parseValueToDecimal(totalPDPremium)),
      vehicleLevelPremium: Number(this.parseValueToDecimal(vehicleLevelPremium)),
      avgPerVehicle: Number(this.parseValueToDecimal(averagePerVehicle)),
      premium: Number(this.parseValueToDecimal(totalPremium)),
      waiverOfSubrogationPremium: waiverOfSubrogationPremium,
      hiredPhysicalDamage: Number(hiredPhysicalDmgPremium),
      trailerInterchange: Number(trailerIntPremium),
      arsCommissionAmount: Number(this.parseValueToDecimal(arsCommissionAmount)),
      trailerInterchangeAL: Number(this.parseValueToDecimal(trailerInterchangePremiumAL)),
      nonOwnedTrailerPremium: Number(this.parseValueToDecimal(nonOwnedTrailerPremium)),
    };
    this.summaryData.updateEstimatedPremium();
    this.quoteLimitsData.loadQuoteOptions().subscribe(() => {
      this.summaryData.updateEstimatedPremium();
    });
  }

  private appendRatingFactorsFromUpload(dataSet: any): Observable<any> {
    if (dataSet == null) {
      return;
    }

    const aLScheduleRF = dataSet?.data?.ALScheduleRF ?? 0;
    const aLExperienceRF = dataSet?.data?.ALExperienceRF ?? 0;
    const histCovData = dataSet?.data?.HIC;
    const riskCoverages = this.submissionData.riskDetail?.riskCoverages;
    const riskHistories = this.submissionData.riskDetail?.riskHistory?.riskHistories;

    if (riskCoverages.length > 0 || riskCoverages !== null) {
      const riskCoverageOption1 = this.submissionData.riskDetail?.riskCoverages.find(x => x.optionNumber == 1);
      const liabScheduleRFOption1 = riskCoverageOption1.liabilityScheduleRatingFactor;
      const liabExperienceRFOption1 = riskCoverageOption1.liabilityExperienceRatingFactor;

      riskCoverages.forEach((riskCoverage) => {
        if (riskCoverage.optionNumber == 1) {
          riskCoverage.liabilityScheduleRatingFactor = Number(aLScheduleRF);
          riskCoverage.liabilityExperienceRatingFactor = Number(aLExperienceRF);  
        }
        else {
          const quoteOptionFormInput =  this.quoteLimitsData.options.find(x => x.optionNumber == riskCoverage.optionNumber);
          riskCoverage.liabilityScheduleRatingFactor = (quoteOptionFormInput.liabilityScheduleRatingFactor == liabScheduleRFOption1) ?
            Number(aLScheduleRF) : quoteOptionFormInput.liabilityScheduleRatingFactor;
          riskCoverage.liabilityExperienceRatingFactor = (quoteOptionFormInput.liabilityExperienceRatingFactor == liabExperienceRFOption1) ?
            Number(aLExperienceRF) : quoteOptionFormInput.liabilityExperienceRatingFactor;
        }
      });

      //this.quoteLimitsData.populateQuoteFields();
    }

    if (riskHistories !== undefined && riskHistories.length > 0) {
      for (let year = 0; year <= 4; year++) {
        riskHistories[year].numberOfPowerUnitsAL = Number(this.parseValueToDecimal(histCovData[year]?.NumberOfPowerUnitsAL ?? 0));
        riskHistories[year].numberOfPowerUnitsAPD = Number(this.parseValueToDecimal(histCovData[year]?.NumberOfPowerUnitsAPD ?? 0));
      }

      this.coverageData.populateHistoryFields();
    }

    return this.quoteLimitsData.saveOptionsWithParam(riskCoverages)
      .pipe(switchMap(() => this.quoteLimitsData.loadQuoteOptions()));
  }

  private validateRatingFactors(output: any) {
    let errorLevel = 0;

    const errorMessages = [
      {
        level: 1,
        message: 'Liability Schedule RF not found in the uploaded file.'
      },
      {
        level: 2,
        message: 'Liability Experience RF not found in the uploaded file.'
      },
      {
        level: 3,
        message: 'Liability Schedule RF and Liability Experience RF not found in the uploaded file.'
      }
    ];

    if (output.data?.ALScheduleRF == null) {
      errorLevel += 1;
    }
    if (output.data?.ALExperienceRF == null) {
      errorLevel += 2;
    }

    return errorMessages.find(e => e.level === errorLevel)?.message;
  }

  private getAgencyName(id: string): string {
    if(!id) return '';

    const agencyList = this.agencyData.agencyListData;
    if(agencyList.length == 0) return '';

    var agency = agencyList.find(e => e.id == id);
    if(!agency) return '';

    return agency.entity?.fullName;
  }

  //#endregion  RATER CODE


  //#region function helper

  //Gets the first 100 char and removes the [\/:*?"<>|] chars
  public splice100AndTrimInvalidChar(input: string): string {
    if (!input) return "";

    const invalidChars = /[\\/:\*\?"<>\|]/g;
    const trimmedChars = input.replace(invalidChars, '');
    const validString = trimmedChars.slice(0, 100);
    return validString;
  }

  public parseValueToDecimal(value) {
    return (Number(value).toFixed(2) === 'NaN') ? Number(0).toFixed(2) : Number(value).toFixed(2);
  }

  public roundToOneDecimalPlace(value) {
    return Math.round(Number(value) * 10) / 10;
}
  //#endregion

  public loadRaterVersion$() : Observable<any> {
    let riskDetailId = this.submissionData.riskDetail.id;
    return this.raterApi.getRaterVersion$(riskDetailId).pipe(
      tap((data) => {
        this.submissionData.riskDetail.raterVersionId = data.versionId;
        this._raterVersion.next(data);
      })
    );
  }

  public checkIfRaterVersionWillChange$(effectiveDate: Date): Observable<boolean> {
    return of(true);
  }

  public loadExperienceRaterDto$() : Observable<any> {
    let riskId = this.submissionData.riskDetail.riskId;
    return this.raterApi.getExperienceRaterDto$(riskId).pipe(
      tap(data => this._experienceRaterDto.next(data))
    );
  }
  
  public confirmXpRaterUpdate$() {
    let riskId = this.submissionData.riskDetail.riskId;
    return this.raterApi.confirmXpRaterUpdate$(riskId).pipe(
      tap(() => {
        this.submissionData.riskDetail.updateXPRaterVersion = false;
      })
    );
  }

  public hasExperienceRater() : Observable<boolean> {
    let riskId = this.submissionData.riskDetail.riskId;
    return this.raterApi.allowExpRaterDownload(riskId).pipe(
      map(allowDownload => {
        return allowDownload;
      }),
      catchError(error => {
        return of(false);
      }),
    );
  }
}

export interface RaterVersion {
  versionId: number;
  version: string;
  effectiveDate: Date;
}

export interface ExperienceRaterDto {
  raterVersionId: number;
  dateModified: Date
}
