import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import { CommonService } from '../../services/common.service';
import { Question } from '../../../shared/models/dynamic/questionDto';

@Injectable({
  providedIn: 'root'
})

export class QuestionService {
  constructor(
      private http: HttpClient,
      private commonService: CommonService) {
  }

  get() {
    return this.http.get(`${environment.ApiUrl}/Question`)
      .map(data => { return data as Question[]; })
      .catch(this.commonService.handleObservableHttpError);
  }

  getQuestionByCategorySection(categoryId, sectionId) {
    return this.http.get(`${environment.ApiUrl}/Question/${categoryId}/${sectionId}`)
      .map(data => { return data as Question[]; })
      .catch(this.commonService.handleObservableHttpError);
  }
}
