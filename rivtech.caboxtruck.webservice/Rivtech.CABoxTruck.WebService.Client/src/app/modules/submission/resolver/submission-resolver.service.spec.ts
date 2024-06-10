import { TestBed } from '@angular/core/testing';

import { SubmissionResolverService } from './submission-resolver.service';

describe('SubmissionResolverService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: SubmissionResolverService = TestBed.get(SubmissionResolverService);
    expect(service).toBeTruthy();
  });
});
