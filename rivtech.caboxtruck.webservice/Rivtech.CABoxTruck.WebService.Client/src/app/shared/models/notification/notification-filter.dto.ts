export class NotificationFilterDTO implements INotificationFilterDTO {
  sortField?: string;
  thenSortField?: string;
  ascending?: boolean;
  skip?: number;
  pageSize?: number;
  userId?: number;
  category?: string;
  title?: string;
  notificationDetails?: string;
  notificationStatus?: string;
  completionPercentage?: number;
  dateCreated?: string;
  dateUpdated?: string;
  order?: string;
  isRead?: boolean;
  policyNumber?: string;
  companyName?: string;
  oldRiskDetailId?: string;
  newRiskDetailId?: string;
  riskId?: string;
  notificationId?: string;
  isRendered?: boolean;

  constructor(data?: INotificationFilterDTO) {
      if (data) {
          for (const property in data) {
              if (data.hasOwnProperty(property)) {
                (<any>this)[property] = (<any>data)[property];
              }
          }
      }
  }
}

export interface INotificationFilterDTO {
  sortField?: string;
  thenSortField?: string;
  ascending?: boolean;
  skip?: number;
  pageSize?: number;
  userId?: number;
  category?: string;
  title?: string;
  notificationDetails?: string;
  notificationStatus?: string;
  completionPercentage?: number;
  dateCreated?: string;
  dateUpdated?: string;
  order?: string;
  isRead?: boolean;
  policyNumber?: string;
  companyName?: string;
  oldRiskDetailId?: string;
  newRiskDetailId?: string;
  riskId?: string;
  notificationId?: string;
  isRendered?: boolean;
}
