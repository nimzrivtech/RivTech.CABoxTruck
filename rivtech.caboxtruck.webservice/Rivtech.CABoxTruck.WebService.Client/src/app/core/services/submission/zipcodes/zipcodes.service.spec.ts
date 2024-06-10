import { TestBed } from '@angular/core/testing';

import { ZipcodesService } from './zipcodes.service';

describe('ZipcodesService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ZipcodesService = TestBed.get(ZipcodesService);
    expect(service).toBeTruthy();
  });
});
