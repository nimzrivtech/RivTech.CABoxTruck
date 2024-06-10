import { commonEnvironment } from './environment.common';

export const _settings = {
  appVersion: require('../../package.json').version,
  envName: 'test',
  production: true,
  ApiUrl: `https://app-boxtruck-api-prodtest.azurewebsites.net/api`,
  EnvironmentServiceUrl: ``,
  IdentityServiceUrl: `https://appi-boxtruckuseridentity-api-prodtest.azurewebsites.net`,
  GenericServiceUrl: `https://app-boxtruckgeneric-api-prodtest.azurewebsites.net`,
  ClientManagementUrl: `https://app-boxtruckagency-api-prodtest.azurewebsites.net`,
  LoginUrl: ``,
  RaterApiUrl: `https://commautocore-raterprodtest.rivpartners.com/api`,
  BillingServiceUrl: `https://app-boxtruckbilling-api-prodtest.azurewebsites.net`,
  BillingAppId: 'CABT',
  ApplicationId: 2,
  BoxtruckAzureFunction: {
    BaseUrl: 'https://func-boxtruck-nightlyjobprodtest.azurewebsites.net',
    PaymentConfirmationCode: 'code=DOSx18B0TIxELLmAFetREkjJbyFn17eXhVbqRs7Ujv9zx3MzybzDCA=='
  },
  ClientAccess: {
    uid: 'boxtruckadmin',
    secretKey: 'Bdyk5Lnd+oUzyUvKk6tKBJVkEuKoZNCVbguubQCCFj4='
  },
};

export const environment = Object.assign({}, commonEnvironment, _settings);
