import { commonEnvironment } from "./environment.common";

// The file contents for the current environment will overwrite these during build.
// The build system defaults to the dev environment which uses `environment.ts`, but if you do
// `ng build --env=prod` then `environment.prod.ts` will be used instead.
// The list of which env maps to which file can be found in `.angular-cli.json`.

export const _settings = {
  appVersion: require('../../package.json').version,
  envName: 'local',
  production: false,
  ApiUrl: `http://${window.location.hostname}:62636/api`,
  EnvironmentServiceUrl: `https://rivtechenvironmentalapi-test.azurewebsites.net`,
  IdentityServiceUrl: `https://app-useridentity-test.azurewebsites.net`,
  GenericServiceUrl: `https://app-genericservices-test.azurewebsites.net`,
  ClientManagementUrl: `https://app-agencyservices-test.azurewebsites.net`,
  RaterApiUrl: `https://localhost:44334/api`, //this can be change if we apply ssl enabled,
  
  BillingServiceUrl: `https://localhost:44337`,
  BillingAppId: 'CABT',
  ApplicationId: 2,
  BoxtruckAzureFunction: {
    BaseUrl: "http://localhost:7071",
    PaymentConfirmationCode: ""
  },
  ClientAccess: {
    uid: 'boxtruckuser',
    secretKey: '61EoE9+AhcwXUWO5I8B8wQ=='
  },
};

export const environment = Object.assign({}, commonEnvironment, _settings);
