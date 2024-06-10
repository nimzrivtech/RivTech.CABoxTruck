export class Broker {
  public constructor(init?: Partial<Broker>) {
    Object.assign(this, init);
  }

  public brokerId: number;
  public agentId: number;
  public agentEmail: string;
  public brokerCommission: number;

  public Agency: string;
  public AgencyPhone: string;
  public AgencyFax: string;
  public ApplicantLength: string;
  public IncumbentAgency: string;
  public EffectiveDate: string;
  public ProducersName: string;
  public ProducersEmailAddress: string;
  public ProducerPhone: string;
  public SubProducerName: string;

  public WrittenLength: number;
}
