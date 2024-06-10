import { commonEnvironment } from './environment.common';

export const _settings = {
  appVersion: require('../../package.json').version,
  envName: 'production',
  production: true,
  ApiUrl: `https://app-boxtruck-api-prod.azurewebsites.net/api`,
  IdentityServiceUrl: `https://identity.rivtechglobal.com`,
  GenericServiceUrl: `https://genericapi.rivtechglobal.com`,
  ClientManagementUrl: `https://agencyapi.rivtechglobal.com`,
  LoginUrl: `https://app-clientmanagement-ui-test.azurewebsites.net`,
  RaterApiUrl: `https://commautocore-rater.rivpartners.com/api`,
  BillingServiceUrl: `https://app-boxtruckbilling-api-prod.azurewebsites.net`,
  BillingAppId: 'CABT',
  ApplicationId: 2,
  BoxtruckAzureFunction: {
    BaseUrl: 'https://func-boxtruck-nightlyjobprod.azurewebsites.net',
    PaymentConfirmationCode: 'code=CTfw0G/0WliJlD324g6LJ4oWC2BPbDkcNxHhsU4V/LDWs4p8byePrA=='
  },
  ClientAccess: {
    uid: 'boxtruckadmin',
    secretKey: 'Bdyk5Lnd+oUzyUvKk6tKBJVkEuKoZNCVbguubQCCFj4='
  },
};

export const environment = Object.assign({}, commonEnvironment, _settings);
