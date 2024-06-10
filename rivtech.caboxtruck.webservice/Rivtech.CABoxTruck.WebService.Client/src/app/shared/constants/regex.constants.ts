export const RegexConstants = {
  phoneValidation: /^\d{10}$/ ,
  zipValidation: /[^0-9]*/g,
  hasANumber: /\d/,
  hasALetterAndASpaceAndAHyphen: /^[a-zA-Z\s\-]*$/,
  policyNumber: /^[a-zA-Z0-9-_]*$/,
  payer: /^[a-zA-Z' ]*$/,
  hasBothLettersAndNumbersAndUnderScore: /[A-Za-z0-9\-\_]+/
};
