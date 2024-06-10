import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from '@angular/router';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { FilingTypeDto } from '../../../shared/models/submission/applicant/FilingTypeDto';
import { ApplicantFilingsInformationService } from '../services/applicant-filings-information.service';

@Injectable({ providedIn: 'root' })
export class FilingTypesResolver implements Resolve<FilingTypeDto> {

  constructor(private service: ApplicantFilingsInformationService) { }

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<any> | Promise<any> | any {
    return this.service.getFilingTypes().pipe(
      catchError(error => {
        console.error(`Filing Types resolver failed: ${error}`);
        return of([]);
      })
    );
  }
}
