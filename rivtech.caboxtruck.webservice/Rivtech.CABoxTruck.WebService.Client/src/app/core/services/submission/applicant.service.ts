import { Injectable } from '@angular/core';
import { ISubmissionCategoryService } from '../../../shared/models/submission/ISubmissionCategoryService';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { environment } from "../../../../environments/environment";
import { Submission } from '../../../shared/models/submission/submission';

@Injectable({
  providedIn: 'root'
})
export class ApplicantService implements ISubmissionCategoryService {

  constructor(private http: HttpClient) { }

  get<T>({ }: {}): import("rxjs").Observable<T> {
    throw new Error("Method not implemented.");
  }

  post<T>({ }: {}): import("rxjs").Observable<T> {
    throw new Error("Method not implemented.");
  }

  put(submission: Submission): Observable<any> {
    return this.http.put(`${environment.ApiUrl}/submissions/applicant`, submission.applicant);
  }

}
