import { TestBed } from '@angular/core/testing';

import { ClaimsHistoryService } from './claims-history.service';

describe('ClaimsHistoryService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ClaimsHistoryService = TestBed.get(ClaimsHistoryService);
    expect(service).toBeTruthy();
  });
});
