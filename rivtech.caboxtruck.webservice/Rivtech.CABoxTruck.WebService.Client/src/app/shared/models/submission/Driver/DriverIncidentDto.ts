export class DriverIncidentDto {
  id?: string = null;
  driverId?: string = null;
  incidentType?: string | undefined = null;
  incidentDate?: Date | undefined = null;
  convictionDate?: Date | undefined = null;

  constructor(init?: Partial<DriverIncidentDto>) {
    // Object.assign(this, init);
    if (init) {
      for (const property in init) {
        if (init.hasOwnProperty(property)) {
          (<any>this)[property] = (<any>init)[property];
        }
      }
    }
  }
}
