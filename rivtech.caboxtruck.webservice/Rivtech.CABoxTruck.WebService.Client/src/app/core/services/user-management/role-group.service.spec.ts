import { TestBed } from '@angular/core/testing';

import { RoleGroupService } from './role-group.service';

describe('RoleGroupService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: RoleGroupService = TestBed.get(RoleGroupService);
    expect(service).toBeTruthy();
  });
});
