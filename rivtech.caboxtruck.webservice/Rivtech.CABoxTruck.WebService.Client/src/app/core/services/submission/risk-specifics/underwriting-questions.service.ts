import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../../../environments/environment';
import { IUnderwritingQuestionsDto } from '../../../../shared/models/submission/riskspecifics/underwriting-questions.dto';
import { CommonService } from '../../common.service';

@Injectable({
  providedIn: 'root'
})

export class UnderwritingQuestionsService {
  private baseUrl: string;

  constructor(private http: HttpClient, private commonService: CommonService) {
    this.baseUrl = environment.ApiUrl;
  }

  updateUnderwritingQuestions(underwritingQuestions: IUnderwritingQuestionsDto): Observable<string> {
    return this.http.put(`${this.baseUrl}/riskspecificunderwritingquestion/risk/${underwritingQuestions.riskDetailId}`, underwritingQuestions, { responseType: 'text' })
      .catch(this.commonService.handleObservableHttpError);
  }
}
