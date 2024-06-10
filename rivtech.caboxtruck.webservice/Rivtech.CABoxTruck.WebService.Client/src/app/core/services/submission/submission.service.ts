import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Submission } from '../../../shared/models/submission/submission';
import { Observable, throwError } from 'rxjs';
import { ISubmissionCategoryData } from '../../../shared/models/submission/ISubmissionCategoryData';
import { ISubmissionCategoryService } from '../../../shared/models/submission/ISubmissionCategoryService';
import { environment } from '../../../../environments/environment';
import { PathConstants } from '../../../shared/constants/path.constants';
import { map, catchError } from 'rxjs/operators';
import { CommonService } from '../common.service';
import { FilterSubmissionListDTO } from '../../../shared/models/submission/filter-submission-list.dto';
import { ListResultDTO } from '../../../shared/models/submission/risk-summary.dto';


@Injectable({
  providedIn: 'root'
})
export class SubmissionService {

  submission: Submission;

  constructor(public http: HttpClient,
    private commonService: CommonService) { }

  getSubmissionId(): number {
    return this.submission.id;
  }

  get(id: number): Observable<Submission> {
    return this.http.get(`${environment.ApiUrl}/submissions/${id}`)
      .pipe(
        map(data => {
          this.submission = data as Submission;
          return data as Submission;
        }),
        catchError(this.commonService.handleObservableHttpError)
      );
  }

  put(submission: Submission): Observable<any> {
    return this.http.put(`${environment.ApiUrl}/submissions`, submission);
  }

  post(submission: Submission) {
    return this.http.post(`${environment.ApiUrl}/submissions`, submission).pipe(
      map(data => {
        return data;
      })
    );
  }

  public saveCurrentCategory(
    categoryService: ISubmissionCategoryService,
    categoryData: ISubmissionCategoryData,
    currentCategory: string) {

    let categoryProperty = currentCategory;
    switch (categoryProperty) {
      case PathConstants.Submission.Applicant.Index:
        categoryProperty = 'applicant';
        break;
      default:
        break;
    }

    this.submission[categoryProperty] = categoryData.getFormValue();
    this.submission[categoryProperty]['id'] = this.getSubmissionId();

    categoryService.put(this.submission).subscribe(data => {
      // show sweetalert message after save
    });

  }

  getSubmissionListAsync(filterSubmissionList: FilterSubmissionListDTO): Observable<ListResultDTO> {
    return this.http.post(`${environment.ApiUrl}/riskSummary/summary/list`, filterSubmissionList).pipe(
      map(data => data as ListResultDTO)
    ).catch(this.commonService.handleObservableHttpError);
  }

}
