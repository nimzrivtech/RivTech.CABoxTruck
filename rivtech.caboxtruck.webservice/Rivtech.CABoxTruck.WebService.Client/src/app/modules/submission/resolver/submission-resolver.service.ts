import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, Router, RouterStateSnapshot } from '@angular/router';
import { RiskDTO } from '../../../shared/models/risk/riskDto';
import { Observable } from 'rxjs';
import { StorageService } from '../../../core/services/storage.service';
import NotifUtils from '../../../shared/utilities/notif-utils';
import { Guid } from 'guid-typescript';
import { take } from 'rxjs/operators';
import { Location } from '@angular/common';
import { SubmissionData } from '../data/submission.data';

@Injectable({
  providedIn: 'root',
})
export class SubmissionResolverService implements Resolve<any> {
  constructor(private storageService: StorageService,
    private submissionData: SubmissionData,
    private router: Router,
  ) { }

  // THIS RESOLVER ENSURE THE DATA PASSING TO SUBMISSION PAGE IS LOADED TO AVOID INCORRECT DATA POPULATION
  // RESOLVE RUN FIRST BEFORE LOADING THE SUBMISSION PAGE
  // RESOLVER USE TO VALIDATE INPUT PARAMETER IF IT IS A VALID QUERY OR NOT
  // PREVENT HACK FROM DIRECT URL INPUT
  // FOR FUTURE ENHANCEMENT AND SECURITY, WE CAN ALSO USE THIS INTERCEPTOR TO CREATE A MASKID FOR RISKDETAILID SO THAT THEY CANNOT SEE THE REAL VALUE.
  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): any | Observable<any> | Promise<any> {
    const tempriskHeaderId = route.params['id'];

    const resolveData: any = {
      isNew: false,
      isValid: false,
      entityId: '',
      risk: new RiskDTO(),
      landingUrl: state.url
    };

    const observable: Observable<any> = Observable.create(observer => {
      this.submissionData.reset();
      if (tempriskHeaderId === 'new') {
        resolveData.isNew = true;
        const newRisk: RiskDTO = new RiskDTO();
        resolveData.risk = newRisk;
        resolveData.isValid = true;

        observer.next(resolveData);
        observer.complete();
      } else if (Guid.isGuid(tempriskHeaderId)) {
        // validate the database if exist and get the updated riskList

        this.submissionData.getRiskByIdAsync(tempriskHeaderId).pipe(take(1)).subscribe(data => {
          resolveData.risk = data;
          resolveData.isValid = !(data == null);

          observer.next(resolveData);
          observer.complete();
        }, (error) => {
          NotifUtils.showError('There was an error trying to create a new record. Please try again.');
          resolveData.isValid = false;
          observer.next(resolveData);
          observer.complete();
        });
      } else {
        // handles if you have input invalid parameter
        NotifUtils.showError('You are trying to access an invalid link. Please access a record from Submission list page.', () => {
        });
        this.router.navigate(['/submissions/']);
      }

      // if (tempriskHeaderId === 'new') {
      //   resolveData.isNew = true;

      //   const newRisk: RiskDTO = new RiskDTO();
      //   newRisk.id = Guid.create().toString();
      //   newRisk.submissionNumber = `UTO-CA-${Math.floor(100000 + Math.random() * 900000)}`;
      //   newRisk.quoteNumber = newRisk.submissionNumber;

      //   resolveData.risk = newRisk;
      //   resolveData.isValid = true;

      //   observer.next(resolveData);
      //   observer.complete();
      // } else if (Guid.isGuid(tempriskHeaderId)) {
      //   // validate the database if exist and get the updated riskList
      //   this.submissionData.getRiskByIdAsync(tempriskHeaderId).pipe(take(1)).subscribe(data => {
      //     resolveData.risk = data;
      //     resolveData.isValid = !(data == null);

      //     observer.next(resolveData);
      //     observer.complete();
      //   }, (error) => {
      //     NotifUtils.showError('There was an error trying to create a new record. Please try again.');
      //     resolveData.isValid = false;
      //     observer.next(resolveData);
      //     observer.complete();
      //   });
      // } else {
      //   //handles if you have input invalid parameter
      //   NotifUtils.showError('You are trying to access an invalid link. Please access a record from Submission list page.', () => {
      //   });
      //   this.router.navigate(['/submissions/']);
      // }
    });

    return observable;
  }

  isGuid(value) {
    const regex = /[a-f0-9]{8}(?:-[a-f0-9]{4}){3}-[a-f0-9]{12}/i;
    const match = regex.exec(value);
    return match != null;
  }
}
