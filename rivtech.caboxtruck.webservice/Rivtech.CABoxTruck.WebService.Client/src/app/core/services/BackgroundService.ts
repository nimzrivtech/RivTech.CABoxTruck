import { HttpBackend, HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'environments/environment';
import { Observable } from 'rxjs';
import { CommonService } from '@app/core/services/common.service';
import { NotificationFilterDTO } from '@app/shared/models/notification/notification-filter.dto';

@Injectable({
  providedIn: 'root'
})
export class BackgroundService {
  private localServiceUrl: string = environment.ApiUrl.replace('/api', '');

  constructor(private http: HttpClient, private httpbackend: HttpBackend, private commonService: CommonService) { }

  triggerBackgroundProcess(notificationFilter: NotificationFilterDTO): Observable<NotificationFilterDTO> {
    return this.http.post<NotificationFilterDTO>(`${this.localServiceUrl}/api/BackgroundService/TriggerBackgroundProcess`, notificationFilter)
      .catch(this.commonService.handleObservableHttpError);
  }
}
