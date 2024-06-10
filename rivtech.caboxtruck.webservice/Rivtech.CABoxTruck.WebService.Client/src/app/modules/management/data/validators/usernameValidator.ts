import { Injectable } from "@angular/core";
import { AbstractControl, AsyncValidator, AsyncValidatorFn, ValidationErrors } from "@angular/forms";
import { UserService } from "app/core/services/management/user.service";
import { Observable, of } from "rxjs";
import { debounce, delay, filter, map, pairwise } from "rxjs/operators";

@Injectable({
  providedIn: "root"
})
export class UserNameValidator  {

    constructor(private userService: UserService) { }

    public oldUserName: string = '';

    usernameValidator(): AsyncValidatorFn {
      return (control: AbstractControl): Observable<ValidationErrors | null> => {
        return this.checkIfUsernameExists(control.value,this.oldUserName,control).pipe(
          map((res) => {
            // if res is true, username exists, return true
            return res ? { usernameExists: true } : null;
            // NB: Return null if there is no error
          })
        );
      };
    }

    checkIfUsernameExists(newUserName: string, oldUserName: string, control: AbstractControl = null): Observable<boolean> {
      if (!newUserName?.trim() || newUserName?.trim() === oldUserName?.trim()){
        return of(false);
      }
      const returnBoolean = this.userService.doesUserNameAlreadyExists(newUserName,oldUserName);
      return returnBoolean;
    }

}
