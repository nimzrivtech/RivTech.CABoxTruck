import { SubmissionInsuredInformation } from './submission-insured-information';
import { SubmissionInsuredContactInformation } from './submission-insured-contact-information';
import { SubmissionBrokerInformation } from './submission-broker-information';
import { SubmissionDetails } from './submission-details';

export class Applicant {
    public id?: number;

    public submissionInsuredInformation: SubmissionInsuredInformation;
    public submissionInsuredContactInformation: SubmissionInsuredContactInformation;
    public submissionBrokerInformation: SubmissionBrokerInformation;
    public submissionDetails: SubmissionDetails

    public constructor(init?: Partial<Applicant>) {
        this.submissionInsuredInformation = new SubmissionInsuredInformation();
        this.submissionInsuredContactInformation = new SubmissionInsuredContactInformation();
        this.submissionBrokerInformation = new SubmissionBrokerInformation();
        this.submissionDetails = new SubmissionDetails();
    }
}