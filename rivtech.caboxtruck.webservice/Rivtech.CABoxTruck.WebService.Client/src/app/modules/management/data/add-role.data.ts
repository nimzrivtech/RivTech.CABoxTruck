import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { Injectable, OnInit } from '@angular/core';
import { takeUntil } from 'rxjs/operators';
import { BaseClass } from '../../../shared/base-class';
import { RoleGroup } from '../../../shared/models/management/role-group';
import { RoleGroupService } from '../../../core/services/management/role-group.service';
import { RolePageService } from '../../../core/services/management/role-page.service';
import { RolePage } from '../../../shared/models/management/role-page';


@Injectable()
export class AddRoleData extends BaseClass implements OnInit {

    roleGroups: RoleGroup[];
    selectedRoleGroup: RoleGroup;

    public roleForms: FormGroup;


    selectedRolePages: RolePage[];

    ngOnInit(): void {
    }

    constructor(private fb: FormBuilder, private roleGroupService: RoleGroupService, private rolePageService: RolePageService) {
        super();
    }

    initiateFormFields(): FormGroup {

        this.roleForms = this.fb.group({
            'roleGroup': new FormControl('', Validators.required),
            'roleName': new FormControl('', Validators.required),
            'isActive': new FormControl('')
        });

        return this.roleForms;
    }

    populateForm() {
        this.roleGroupService.getList().pipe(takeUntil(this.stop$)).subscribe(
            res => {
                this.roleGroups = res;
            }
        );
    }
}