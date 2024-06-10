import { commonEnvironment } from './environment.common';

export const _settings = {
  appVersion: require('../../package.json').version,
  envName: 'test',
  production: true,
  ApiUrl: `https://app-boxtruck-apitest.azurewebsites.net/api`,
  EnvironmentServiceUrl: `https://rivtechenvironmentalapi-test.azurewebsites.net`,
  IdentityServiceUrl: `https://app-useridentitytest.azurewebsites.net`,
  GenericServiceUrl: `https://app-genericservicestest.azurewebsites.net`,
  ClientManagementUrl: `https://app-agencyservicestest.azurewebsites.net`,
  LoginUrl: `https://app-clientmanagement-ui-test.azurewebsites.net`,
  RaterApiUrl: `https://commautocore-ratertest.rivpartners.com/api`, //this can be change if we apply ssl enabled
  BillingServiceUrl: `https://app-boxtruckbilling-api-test.azurewebsites.net`,
  BillingAppId: 'CABT',
  ApplicationId: 2,
  BoxtruckAzureFunction: {
    BaseUrl: "https://func-boxtrucknightlyjob-test.azurewebsites.net",
    PaymentConfirmationCode: "code=DOSx18B0TIxELLmAFetREkjJbyFn17eXhVbqRs7Ujv9zx3MzybzDCA=="
  },
  ClientAccess: {
    uid: 'boxtruckadmin',
    secretKey: 'Bdyk5Lnd+oUzyUvKk6tKBJVkEuKoZNCVbguubQCCFj4='
  },
};

export const environment = Object.assign({}, commonEnvironment, _settings);
