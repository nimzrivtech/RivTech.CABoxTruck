export class NotificationDTO implements INotificationDTO {
  userId?: number;
  category?: string;
  title?: string;
  notificationDetails?: string;
  notificationStatus?: string;
  completionPercentage?: number;
  dateCreated?: string;
  isRead?: boolean;
  policyNumber?: string;
  companyName?: string;
  oldRiskDetailId?: string;
  newRiskDetailId?: string;
  riskId?: string;
  notificationId?: string;
  dateUpdated?: string;
  isRendered?: boolean;
  errorMessagesList?: string[];

  constructor(data?: INotificationDTO) {
      if (data) {
          for (const property in data) {
              if (data.hasOwnProperty(property)) {
                (<any>this)[property] = (<any>data)[property];
              }
          }
      }
  }
}

export interface INotificationDTO {
  userId?: number;
  category?: string;
  title?: string;
  notificationDetails?: string;
  notificationStatus?: string;
  completionPercentage?: number;
  dateCreated?: string;
  isRead?: boolean;
  policyNumber?: string;
  companyName?: string;
  oldRiskDetailId?: string;
  newRiskDetailId?: string;
  riskId?: string;
  notificationId?: string;
  dateUpdated?: string;
  isRendered?: boolean;
  errorMessagesList?: string[];
}
