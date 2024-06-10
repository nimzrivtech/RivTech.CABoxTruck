import { NameAndAddressDTO } from '../submission/applicant/applicant-name-address';
import { BusinessDetailsDTO } from '../submission/applicant/businessDetailsDto';
import { FilingsInformationDto } from '../submission/applicant/FilingsInformationDto';
import { BrokerInfoDto } from '../submission/brokerinfo/brokerInfoDto';
import { VehicleDto } from '../submission/Vehicle/VehicleDto';
import { DriverDto, DriverHeaderDto } from '../submission/Driver/DriverDto';
import { RiskCoverageDTO } from '../submission/limits/riskCoverageDto';
import { IDotInfoDto } from '../submission/riskspecifics/dot-info.dto';
import { IDriverHiringCriteriaDto } from '../submission/riskspecifics/driver-hiring-criteria.dto';
import { IDriverInfoDto } from '../submission/riskspecifics/driver-info.dto';
import { GeneralLiabilityCargoDto } from '../submission/riskspecifics/general-liability-cargoDto';
import { MaintenanceSafetyDto } from '../submission/riskspecifics/maintenance-safety/maintenance-safetyDto';
import { IRadiusOfOperationsDto } from '../submission/riskspecifics/radius-of-operations.dto';
import { IUnderwritingQuestionsDto } from '../submission/riskspecifics/underwriting-questions.dto';
import { RiskHistoryListDTO } from '../submission/coverages/riskHistoryDto';
import { ClaimsHistoryDto } from '../submission/claimsHistory/claimsHistoryDto';
import { NoteDTO } from '../submission/note/NoteDto';
import { FormDTO } from '../submission/forms/FormDto';
import { BindingDto } from '../submission/binding/BindingDto';
import { BindingRequirementsDto } from '../submission/binding/BindingRequirementsDto';
import { BlanketCoverageDto } from '../submission/applicant/BlanketCoverageDto';
import { QuoteConditionsDto } from '../submission/binding/QuoteConditionsDto';
import { RiskAdditionalInterestsDTO } from '../submission/RiskAdditionalInterestDto';
import { ManuscriptDTO } from '../submission/manuscript/ManuscriptDto';
import { FeeDetailsDto } from '../submission/applicant/FeeDetailsDto';
import { FileUploadDocumentDto } from '../submission/binding/FileUploadDocumentDto';
import { RiskPolicyContactDTO } from '../submission/RiskPolicyContactDto';
import { DestinationDto } from '../submission/riskspecifics/destinationDto';
import { RiskRelatedDTO } from '../submission/RiskRelatedDto';
import { FieldVersionDTO } from '../submission/fieldVersionDto';

export class RiskDTO {
  public id?: string | undefined = null;
  public submissionNumber?: string | undefined = null;

  public riskId?: string | undefined = null;
  public quoteNumber?: string | undefined = null;
  public policyNumber?: string | undefined = null;
  public policySuffix?: string | undefined = null;
  public firstIssueDate?: Date | undefined = null;

  public effectiveDate?: Date | undefined = null;
  public expirationDate?: Date | undefined = null;

  public pendingCancellationDate?: Date | undefined = null;
  public pendingCancellationNoticeDate?: Date | undefined = null;
  public cancellationDate?: Date | undefined = null;
  public dateReceived?: Date | undefined = null;
  public quoteNeededBy?: Date | undefined = null;

  public term?: string | undefined = null;
  public programID?: number | undefined = null;

  public string?: string | undefined = null;
  public agentCommission?: number | undefined = null;
  public paymentPlanID?: number | undefined = null;
  public productTypeID?: number | undefined = null;
  public underWriterID?: number | undefined = null;

  public status?: string | undefined = null;
  public subStatus?: string | undefined = null;
  public assignedToId?: number | undefined = null;

  public createdBy?: number | undefined = null;
  public createdDate?: Date | undefined = null;
  public IsActive?: boolean | undefined;
  public endorsementEffectiveDate?: Date | undefined;
  public prevEndorsementEffectiveDate?: Date | undefined;

  public insuredName?: string | undefined = null;
  public broker?: string | undefined = null;
  public riskPolicyContacts?: RiskPolicyContactDTO[] | undefined = null;

  public brokerInfo?: BrokerInfoDto | undefined = null;
  public radiusOfOperations?: IRadiusOfOperationsDto | undefined = null;
  public driverInfo?: IDriverInfoDto | undefined = null;
  public driverHiringCriteria?: IDriverHiringCriteriaDto | undefined = null;
  public dotInfo?: IDotInfoDto | undefined = null;
  public underwritingQuestions?: IUnderwritingQuestionsDto | undefined = null;
  public maintenanceSafety?: MaintenanceSafetyDto | undefined = null;
  public generalLiabilityCargo?: GeneralLiabilityCargoDto | undefined = null;

  public filingsInformation?: FilingsInformationDto | undefined = null;
  public nameAndAddress?: NameAndAddressDTO | undefined = null;
  public businessDetails?: BusinessDetailsDTO | undefined = null;

  public vehicles?: VehicleDto[] | undefined = null;
  public drivers?: DriverDto[] | undefined = null;
  public driverHeader?: DriverHeaderDto | undefined = null;
  public riskCoverage?: RiskCoverageDTO | undefined = null;
  public riskCoverages?: RiskCoverageDTO[] | undefined = null;
  public riskHistory?: RiskHistoryListDTO | undefined = null;
  public claimsHistory?: ClaimsHistoryDto[] | undefined = null;
  public riskNotes?: NoteDTO[] | undefined = null;
  public riskForms?: FormDTO[] | undefined = null;

  public binding?: BindingDto | undefined = null;
  public bindingRequirements?: BindingRequirementsDto[] | undefined = null;
  public blanketCoverage?: BlanketCoverageDto | undefined = null;
  public additionalInterest?: RiskAdditionalInterestsDTO[] | undefined = null;

  public quoteConditions?: QuoteConditionsDto[] | undefined = null;
  public riskManuscripts?: ManuscriptDTO[] | undefined = null;
  public feeDetails?: FeeDetailsDto[] | undefined = null;
  public riskDocuments?: FileUploadDocumentDto[] | undefined = null;

  public isPolicy?: boolean | undefined;
  public hasInitialDriverExclusion?: boolean | undefined;
  public riskSpecificDestinations?: DestinationDto[] | undefined = null;
  public raterVersionId?: number;
  public isRaterVersionLocked?: boolean | undefined;
  public updateXPRaterVersion?: boolean | undefined;

  public isRenewal: boolean;
  public isFromRenewal: boolean;
  public renewalDate?: Date | undefined;
  public requestedQuoteDate?: Date | undefined;
  public previousPolicyNumber: string;

  public riskRelated?: RiskRelatedDTO[] | undefined = null;

  public CountPowerUnit?: number;
  public CountTrailer?: number;
  public CountOthers?: number;
  public fieldVersions?: FieldVersionDTO[] | undefined = null;
  constructor(init?: Partial<RiskDTO>) {
    Object.assign(this, init);
  }
}
