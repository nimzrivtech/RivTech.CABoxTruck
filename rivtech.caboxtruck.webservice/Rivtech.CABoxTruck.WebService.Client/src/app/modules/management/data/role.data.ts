import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { Injectable, OnInit, Output, EventEmitter } from '@angular/core';
import { Observable } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { RoleGroup } from '../../../shared/models/management/role-group';
import { RoleGroupService } from '../../../core/services/management/role-group.service';
import { Role } from '../../../shared/models/management/role';
import { RolePageService } from '../../../core/services/management/role-page.service';
import { RolePage } from '../../../shared/models/management/role-page';
import { BaseClass } from '../../../shared/base-class';
import Utils from '../../../shared/utilities/utils';


@Injectable()
export class RoleData extends BaseClass implements OnInit {

    rolePagesObservable: Observable<RolePage[]>;

    roleGroups: RoleGroup[];
    roles: Role[];

    selectedRoleGroup: RoleGroup;

    public roleForms: FormGroup;

    selectedRolePages: RolePage[];

    @Output() roleGroupChanged = new EventEmitter<number>();
    @Output() rolePageLoaded = new EventEmitter<RolePage[]>();


    ngOnInit(): void {
    }

    constructor(private fb: FormBuilder, private roleGroupService: RoleGroupService, private rolePageService: RolePageService) {
        super();
    }

    initiateFormFields(): FormGroup {

        this.roleForms = this.fb.group({
            'roleGroup': new FormControl('', Validators.required),
            'role': new FormControl('', Validators.required),
            'isActive': new FormControl('')
        });

        this.initializeOnValueChanges();

        return this.roleForms;
    }

    initializeOnValueChanges() {
        this.roleForms.get('roleGroup').valueChanges.pipe(takeUntil(this.stop$)).subscribe(value => {
            let selected = parseInt(value);
            if (selected) {
                this.selectRoleGroup(selected);

                this.roles = this.selectedRoleGroup.roles;

                this.roleGroupChanged.emit(selected);
            }
            else {
                this.roles = [];
                this.roleForms.patchValue({ role: null });
            }
        });

        this.roleForms.get('role').valueChanges.pipe(takeUntil(this.stop$)).subscribe(value => {
            let selected = parseInt(value);
            if (selected) {
                Utils.blockUI();
                this.selectRolePage(selected).subscribe(p => {
                    Utils.unblockUI();
                    this.rolePageLoaded.emit(p);
                });
            }
            else {
                this.rolePageLoaded.emit(null);
            }
        });
    }

    populateForm(): Observable<RoleGroup[]> {
        return this.roleGroupService.getList();
    }

    populateFormObservable(): Observable<RoleGroup[]> {
        return this.roleGroupService.getList()
    }

    selectRoleGroup(id: number) {

        this.selectedRoleGroup = this.roleGroups.filter(p => p.roleGroupId == id)[0];
        this.roleForms.patchValue({'role': ''});
    }

    public getRoleName(): string {
        return this.roleForms.get('role').value;
    }

    selectRolePage(roleId: number): Observable<RolePage[]> {
        return this.rolePageService.getByRoleId(roleId);
    }
}