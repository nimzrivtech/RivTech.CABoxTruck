export const LoginLabelsConstants = {
    username: 'Username',
    password: 'Password',

    loginAuditType: 'LOGIN',

    method: {
      getAuth: 'getAuthFromServer',
      onLogin: 'onLoginSuccessProcess',
      processAuth: 'processTwoAuthFactor'
    },

    errorMessage: {
      exist: 'exist',
      incorrect: 'incorrect',
      userInactive: 'inactive',
      locked: 'locked',
      agencyInactive: 'Agency and/or subagency is inactive',
      incorrectDescription: 'Username {0} failed to login because the username/password is not valid',
      existDescription: 'Username {0} tried to use but the user is not existing',
      userInactiveDescription: 'The Account of username {0} is In-active',
      agencyInactiveDescription: 'The Agency/SubAgency of username {0} is In-active',
      incorrectPassword: 'Incorrect password.',
      requiredCredentials: 'Username and password is required.'
    }
};

export const PasswordFormConstants = {
    password: 'Password',
    newPassword: 'New Password',
    confirmPassword: 'Confirm New Password',
    confirmRetypePassword: 'Confirm Password',
    changePassword: 'Change Password',
    enterPassword: 'Please enter your new password',
    resetPassword: 'Reset Password',
    retypePassword: 'Retype Password',
    changeSuccessMessage: 'Password changed successfully',
    cancel: 'Cancel',
    newUserChangePassword: 'Password successfully changed, please login using your new password',
    resetPasswordRequestSuccess: 'Password Reset Email has been successfully received by {0}.',
    resetPasswordConfirmation: 'Are you sure you want to reset the password?',
    resetPasswordError: 'Error in sending Password Reset Email. Please check if {0} is correct. If issue persists, please contact the system administrator.',

    auditLog: {
      forgotPwRequestLog: {
        auditType: 'FORGOTPWD',
        description: 'Username {0} request a forgot password',
      },
      forgotPwSuccessLog: {
        auditType: 'FORGOTPWD',
        description: 'Username {0} successfully changed his/her forgot password',
      },
      changePwSuccessLog: {
        auditType: 'CHANGEDPWD',
        description: 'Username {0} successfully changed his/her password',
      },
      resetPwSuccessLog: {
        auditType: 'RESETPWD',
        description: 'Username {0} successfully reset his/her password',
      },
      methodName: {
        requestForgot: 'sendForgotPasswordEmail',
        successForgot: 'resetPassword',
        change: 'changePassword',
        reset: 'userResetPassword'
      },
      action: {
        requestForgot: 'request-forgot',
        successForgot: 'success-forgot',
        successChange: 'success-change',
        successReset: 'success-reset',
      }
    },
};

export const ReferenceCodeConstants = {
  title: 'Verification Code',
  description1: 'A security code was sent to you via email. If you don`t see the email, check your spam folder.',
  description2: 'Please enter the code  below then click Continue to complete your login.',
  continue: 'Continue',
  resendCode: 'Resend Code',
  referenceNumber: 'Reference Number:',
  countdown: 60,
  tick: 1000,
  CodeSuccessSent: 'Verification Code Sent',
  CodeisEmpty: 'Vertification Code is empty',
  invalidCode: 'Security Code is invalid',
  verificationError: 'Verification Error encountered',
};