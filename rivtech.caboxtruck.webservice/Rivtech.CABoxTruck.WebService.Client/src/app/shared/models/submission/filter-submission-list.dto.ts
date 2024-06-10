export class FilterSubmissionListDTO implements IFilterSubmissionListDTO {
  pageNumber?: number | undefined;
  pageSize?: number | undefined;
  sortField?: string | undefined;
  order?: string | undefined;
  submissionNumber?: string | undefined;
  insuredName?: string | undefined;
  insuredDBA?: string | undefined;
  broker?: string | undefined;
  underwriter?: string | undefined;
  au_rep?: string | undefined;
  au_psr?: string | undefined;
  owner?: string | undefined;
  inceptionDateFrom?: Date | undefined;
  inceptionDateTo?: Date | undefined;
  status?: string | undefined;
  state?: string | undefined;
  searchText?: string | undefined;
  isRenewal?: boolean;
  isDashboard?: boolean;
}

export interface IFilterSubmissionListDTO {
  pageNumber?: number | undefined;
  pageSize?: number | undefined;
  sortField?: string | undefined;
  order?: string | undefined;
  submissionNumber?: string | undefined;
  insuredName?: string | undefined;
  insuredDBA?: string | undefined;
  broker?: string | undefined;
  underwriter?: string | undefined;
  au_rep?: string | undefined;
  au_psr?: string | undefined;
  owner?: string | undefined;
  inceptionDateFrom?: Date | undefined;
  inceptionDateTo?: Date | undefined;
  status?: string | undefined;
  state?: string | undefined;
  searchText?: string | undefined;
  isRenewal?: boolean;
  isDashboard?: boolean;
}
