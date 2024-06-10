namespace RivTech.CABoxTruck.WebService.Constants
{
    public class ExceptionMessages
    {
        public const string SYSTEMEXCEPTION = "System related error encountered. Please contact your system Administrator.";
        public const string NOIMPLEMENTEDEXCEPTION = "No Implementation code error encountered. Please contact your system Administrator.";
        public const string AUTHENTICATIONEXCEPTION = "Authorization required";
        public const string UNAUTHORIZEDEXCEPTION = "You are not allowed to access the file.";
        public const string ARGUMENTEXCEPTION = "Passed arguments does not meet the required parameter specification";
        public const string DBUPDATEEXCEPTION = "Failed to update record.";
        public const string DBEXCEPTION = "Database related error encountered.";
        public const string NULLREFERENCEEXCEPTION = "A code problem error encountered. Please contact your system Administrator.";
        public const string INDEXOUTOFRANGEEXCEPTION = "An code problem error encountered. Please contact your system Administrator.";
        public const string IOEXCEPTION = "A Problem encountered on processing the file. Please contact your system Administrator.";
        public const string WEBEXCEPTION = "An Error occured on your http calls. Please contact your system Administrator.";
        public const string SQLEXCEPTION = "A Database error encountered. Please contact your system Administrator.";
        public const string STACKOVERFLOWEXCEPTION = "A recursive code error encountered. Please contact your system Administrator.";
        public const string OUTOFMEMORYEXCEPTION = "Out of Memory error encountered. Please contact your system Administrator.";
        public const string INVALIDCASTEXCEPTION = "A code error encountered on casting. Please contact your system Administrator.";
        public const string INVALIDOPERATIONEXCEPTION = "A Library/Plugin error encountered. Please contact your system Administrator.";
        public const string OBJECTDISPOSEDEXCEPTION = "An error occued on a disposed object. Please contact your system Administrator.";
        public const string EXCEPTION = "Something went wrong. Please contact your system Administrator.";

        public const string REGEX_ALPHANUMERIC = @"^[A-Za-z0-9\-]+$";
        public const string REGEX_NAME = @"^[A-Za-z0-9'\s]+$";
        public const string REGEX_EMAIL = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,5}$";
        public const string REGEX_ALPHANUMERIC_MSG = "Only use letters, numbers and dashes.";
        public const string VALUE_EMPTY_MSG = "Value cannot be empty.";
        public const string ERROR_MSG = "An error has occured. Error: ";
        public const string EMAIL_INVALID = "Email used is invalid.";
        public const string DOMAIN_CODE_INVALID = "Domain used is invalid, only use letters, numbers and dashes in between with a length of 3 to 50 characters.";
        public const string NAME_INVALID = "Firstname or Lastname used is invalid. Only use letters, spaces and apostrophes.";
        public const string RECORD_NOT_FOUND = "Record not found.";
        public const string USER_IS_NULL_MSG = "User cannot be null.";
        public const string USER_LOCKED_MSG = "User is currently locked. Please unlock the user";
        public const string LOGIN_IS_NULL_MSG = "User login cannot be null.";

        public const string USER_ID_CODE = "USR";

        public const string SAVE_UPDATE_FAILED_MSG = "Process has failed, please check your data content or try again later.";
        public const string SAVE_UPDATE_SUCCESS_MSG = "Successfully saved data.";

        public const string USER_ID_REQUIRED_MSG = "User id is required.";
        public const string USER_NOT_EXISTS_MSG = "User does not exist.";
        public const string USER_INVALID_LOGIN_MSG = "Username or password is incorrect.";
        public const string USER_PASSWORD_REQUIRED_MSG = "Password is required.";
        public const string USER_PASSWORD_MISMATCH_MSG = "Password and confirm password do not match.";
        public const string USER_USERNAME_EXISTS_MSG = "Username is already in use.";
        public const string USER_EMAIL_EXISTS_MSG = "Email is already in use.";
        public const string USER_CHANGE_USERNAME_MSG = "Username cannot be changed.";
        public const string USER_USERPASSLENGTH_MSG = "Passwords must be at least 8 characters.";
        public const string USER_HASH_LENGTH_MSG = "Invalid length of password hash (64 bytes expected).";

        public const string USERROLE_NO_ROLE = "No role is associated to the user.";

        public const string USERDOMAIN_NO_USER = "Selected domain is not accociated to the user.";

        public const string DOMAIN_ID_REQUIRED_MSG = "Domain id is required.";
        public const string DOMAIN_NAME_REQUIRED_MSG = "Organization name is required.";
        public const string DOMAIN_CODE_REQUIRED_MSG = "Domain code is required.";
        public const string DOMAIN_CODE_LENGTH_MSG = "Domain code must be at least 3 characters.";
        public const string DOMAIN_CODE_EXISTS_MSG = "Domain code is already in use.";
        public const string DOMAIN_NOT_EXISTS_MSG = "Domain name does not exist.";

        public const string GROUP_ID_REQUIRED_MSG = "Group id is required.";
        public const string GROUP_NAME_ALREADY_EXISTS = "Group name already exists.";

        public const string NOT_AUTHORIZED = "You are not authorized.";

        public const string OBJECT_APREADY_EXISTS = "An object with the same id already exists";
        public const string RATE_LIMIT_EXCEED = "The request limit has been reached. Please wait for a minute to proceed.";
    }
}
