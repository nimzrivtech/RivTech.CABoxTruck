import { HttpBackend, HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'environments/environment';
import { Observable } from 'rxjs';
import { CommonService } from '@app/core/services/common.service';
import { NotificationFilterDTO } from '@app/shared/models/notification/notification-filter.dto';
import { NotificationDTO } from '@app/shared/models/notification/notification.dto';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  private localServiceUrl: string = environment.ApiUrl.replace('/api', '');

  constructor(private http: HttpClient, private httpbackend: HttpBackend, private commonService: CommonService) { }

  getNotificationsByUserId(notificationFilter: NotificationFilterDTO): Observable<NotificationDTO> {
    return this.http.post<NotificationDTO>(`${this.localServiceUrl}/api/Notification/GetNotificationsByUserId`, notificationFilter)
      .catch(this.commonService.handleObservableHttpError);
  }

  UpdateNotificationsIsReadFlag(notificationFilter: NotificationFilterDTO): Observable<NotificationFilterDTO> {
    return this.http.post<NotificationFilterDTO>(`${this.localServiceUrl}/api/Notification/UpdateNotificationsIsReadFlag`, notificationFilter)
      .catch(this.commonService.handleObservableHttpError);
  }

  GetNotificationByFilter(notificationFilter: NotificationFilterDTO): Observable<NotificationDTO> {
    return this.http.post<NotificationDTO>(`${this.localServiceUrl}/api/Notification/GetNotificationByFilter`, notificationFilter)
      .catch(this.commonService.handleObservableHttpError);
  }

  UpdateNotificationIsRendered(notificationFilter: NotificationFilterDTO): Observable<NotificationDTO> {
    return this.http.post<NotificationDTO>(`${this.localServiceUrl}/api/Notification/UpdateNotificationIsRendered`, notificationFilter)
      .catch(this.commonService.handleObservableHttpError);
  }
}
