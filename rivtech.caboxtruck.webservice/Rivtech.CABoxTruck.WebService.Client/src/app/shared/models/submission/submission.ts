import { Applicant } from './applicant/applicant';


export class Submission {
    
    public id: number;
    
    public applicant: Applicant;

    constructor(init?: Partial<Submission>) {
        this.applicant = new Applicant();      
        
        Object.assign(this, init);
    }
}
