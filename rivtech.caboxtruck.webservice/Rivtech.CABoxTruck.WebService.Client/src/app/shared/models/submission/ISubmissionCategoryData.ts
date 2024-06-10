import { Applicant } from './applicant/applicant';

export interface ISubmissionCategoryData {
    initiateFormFields(): void;
    populateForm(): void;
    getFormValue(): Applicant;
    formsAreDirty(): boolean;
    // markFormsAsPristine(): void;
}