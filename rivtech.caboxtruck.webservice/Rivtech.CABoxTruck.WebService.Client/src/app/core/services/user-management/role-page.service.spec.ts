import { TestBed } from '@angular/core/testing';

import { RolePageService } from './role-page.service';

describe('RolePageService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: RolePageService = TestBed.get(RolePageService);
    expect(service).toBeTruthy();
  });
});
