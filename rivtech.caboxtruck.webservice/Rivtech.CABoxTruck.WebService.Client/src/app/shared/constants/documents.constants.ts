export const DocumentsConstants = {
    acceptedFileTypes: [
        '.pdf',
        '.doc',
        '.docx',
        '.msg',
        '.jpg',
        '.jpeg',
        '.bmp',
        '.png',
        '.xls',
        '.xlsx',
        '.txt',
        '.zip',
    ],
    maxFileSize: 'Maximum upload file size: 22MB',
    maxFileSizeExceeded: 'The file being uploaded exceeds the maximum allowable size of 22MB.',
    invalidFileTypeErrorMessage: `The document {0} could not be uploaded. The file type is invalid.
        Note: Only the following file types are valid and can be attached – <list of valid file types e.g. {1}>`,
    customInvalidFileTypeErrorMsg: `The document {0} could not be uploaded. The file type is invalid.
        Note: Only {1} file type is valid and can be attached.`
};

export const DocumentFileCategory = {
    Underwriting: 1,
    Quoting: 2,
    Billing: 3,
    BindRequirement: 4,
    QuoteCondition: 5,
}