import { TestBed } from '@angular/core/testing';

import { RaterApiService } from './rater-api.service';

describe('RaterApiService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: RaterApiService = TestBed.get(RaterApiService);
    expect(service).toBeTruthy();
  });
});
