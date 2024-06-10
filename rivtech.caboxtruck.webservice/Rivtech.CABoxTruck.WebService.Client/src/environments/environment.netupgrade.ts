import { commonEnvironment } from './environment.common';

export const _settings = {
  appVersion: require('../../package.json').version,
  envName: 'devtest',
  production: true,
  ApiUrl: `https://app-boxtruck-api-netupgrade.azurewebsites.net/api`,
  EnvironmentServiceUrl: `https://rivtechenvironmentalapi-test.azurewebsites.net`,
  IdentityServiceUrl: `https://app-useridentitytest.azurewebsites.net`,
  GenericServiceUrl: `https://app-genericservices-test.azurewebsites.net`,
  ClientManagementUrl: `https://app-agencyservices-test.azurewebsites.net`,
  LoginUrl: `https://app-clientmanagement-ui-test.azurewebsites.net`,
  RaterApiUrl: `https://commautocore-ratertest.rivpartners.com:3000/api`, //this can be change if we apply ssl enabled
  BillingServiceUrl: `https://app-boxtruckbilling-api-netupgrade.azurewebsites.net`,
  BillingAppId: 'CABT',
  ApplicationId: 2,
  BoxtruckAzureFunction: {
    BaseUrl: "https://func-boxtrucknightlyjob-netupgrade.azurewebsites.net",
    PaymentConfirmationCode: "code=ex6p4dYNal-sh2jr_CYA5KH_F95xg95T7iJdk2W0lu_SAzFu8JvIjQ=="
  },
  ClientAccess: {
    uid: 'boxtruckadmin',
    secretKey: '/6So4KYMmHWmUxK3ODFK/3wTHyEVpXFDMaL4VoWLs9Q='
  }
};

export const environment = Object.assign({}, commonEnvironment, _settings);
