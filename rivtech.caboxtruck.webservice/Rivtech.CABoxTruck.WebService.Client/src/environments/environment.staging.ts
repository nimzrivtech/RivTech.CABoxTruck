import { commonEnvironment } from './environment.common';

export const _settings = {
  appVersion: require('../../package.json').version,
  envName: 'staging',
  production: true,
  ApiUrl: `https://app-boxtruck-apistaging.azurewebsites.net/api`,
  IdentityServiceUrl: `https://app-useridentitystg.azurewebsites.net`,
  GenericServiceUrl: `https://app-genericservicesstg.azurewebsites.net`,
  ClientManagementUrl: `https://app-agencyservicesstg.azurewebsites.net`,
  LoginUrl: `https://app-clientmanagement-ui-test.azurewebsites.net`,
  RaterApiUrl: `https://commautocore-raterstaging.rivpartners.com/api`,
  BillingServiceUrl: `https://app-boxtruckbilling-api-stg.azurewebsites.net`,
  BillingAppId: 'CABT',
  ApplicationId: 2,
  BoxtruckAzureFunction: {
    BaseUrl: "https://function-boxtruckstaging.azurewebsites.net",
    PaymentConfirmationCode: "code=H2VUSnkFDlRPopDp8bfnbwaZB29OR/n5aEPaYkr0giaqClnzAiLOPQ=="
  },
  ClientAccess: {
    uid: 'boxtruckadmin',
    secretKey: 'Bdyk5Lnd+oUzyUvKk6tKBJVkEuKoZNCVbguubQCCFj4='
  },
};

export const environment = Object.assign({}, commonEnvironment, _settings);
