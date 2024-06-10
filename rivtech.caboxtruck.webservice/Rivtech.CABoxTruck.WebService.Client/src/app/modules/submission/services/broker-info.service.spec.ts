import { TestBed } from '@angular/core/testing';

import { BrokerInfoService } from './broker-info.service';

describe('BrokerInfoService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: BrokerInfoService = TestBed.get(BrokerInfoService);
    expect(service).toBeTruthy();
  });
});
