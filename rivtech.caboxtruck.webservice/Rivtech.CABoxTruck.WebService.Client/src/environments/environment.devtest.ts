import { commonEnvironment } from './environment.common';

export const _settings = {
  appVersion: require('../../package.json').version,
  envName: 'devtest',
  production: true,
  ApiUrl: `https://app-boxtruck-apidev.azurewebsites.net/api`,
  EnvironmentServiceUrl: `https://rivtechenvironmentalapi-test.azurewebsites.net`,
  IdentityServiceUrl: `https://app-useridentitydev.azurewebsites.net`,
  GenericServiceUrl: `https://app-genericservicesdev.azurewebsites.net`,
  ClientManagementUrl: `https://app-agencyservicesdev.azurewebsites.net`,
  LoginUrl: `https://app-clientmanagement-ui-test.azurewebsites.net`,
  RaterApiUrl: `https://commautocore-raterdev.rivpartners.com/api`, //this can be change if we apply ssl enabled
  BillingServiceUrl: `https://app-boxtruckbilling-api-dev.azurewebsites.net`,
  BillingAppId: 'CABT',
  ApplicationId: 2,
  BoxtruckAzureFunction: {
    BaseUrl: "https://func-boxtrucknightlyjob-dev.azurewebsites.net",
    PaymentConfirmationCode: "code=7X1mnImbqY9nIVflB1RCEZkPQaSRhxsbDEp7pJhLFkD6AzFuQfoAyw%3D%3D"
  },
  ClientAccess: {
    uid: 'boxtruckadmin',
    secretKey: '/6So4KYMmHWmUxK3ODFK/3wTHyEVpXFDMaL4VoWLs9Q='
  },
};

export const environment = Object.assign({}, commonEnvironment, _settings);
