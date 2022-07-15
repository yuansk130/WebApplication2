import { Injectable } from '@angular/core';
import { AbstractControl, FormGroup, ValidatorFn } from '@angular/forms';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Observable } from "rxjs";


@Injectable({
  providedIn: 'root'
})
export class ValidationService {
  baseUrl = environment.apiUrl;

  constructor(
    private http: HttpClient
    ) { }

  match(password: string, confirmPassword: string): ValidatorFn {
    return (controls: AbstractControl) => {
      const control = controls.get(password);
      const checkControl = controls.get(confirmPassword);
      if (checkControl?.errors && !checkControl.errors['matching']) {
        return null;
      }
      if (control?.value !== checkControl?.value) {
        controls.get(confirmPassword)?.setErrors({ matching: true });
        return { matching: true };
      } else {
        return null;
      }
    };
}

  CheckPassNotSequ(password: string)  {
    var list = password.split('');
    for (let i in list) {
      if (list[+i + 2]  && +list[i] + 1 == +list[+i + 1]
        && +list[i] + 2 == +list[+i + 2])
        return false;
      if (list[+i + 2] != null && list[i].charCodeAt(0) + 1 == list[+i + 1].charCodeAt(0)
        && list[i].charCodeAt(0) + 2 == list[+i + 2].charCodeAt(0))
        return false;
    }
    return true;
  }


  //checkUserExist(username:string) {
  //  return this.http.get(this.baseUrl + '/Account/validateForm');
  //}

  //validateForm(formgroup: FormGroup) {
  //  return this.http.get(this.baseUrl + '/Account/validateForm');
  //}

  //Validateform(formgroup: FormGroup): ValidatorFn{
  //  return (controls: AbstractControl) => {
  //     const control = controls.get('username');

  //    this.validateForm(formgroup).subscribe(
  //    (response) => {
  //      if (response == null) {
  //        return null;
  //      } else {
  //        controls.get('username')?.setErrors({ userExit: true })
  //      }
  //      return null;
  //    }, (error) => {
  //      return null;
  //    })
  //     return null;
  //  };



    //var username = formGroup.controls['username'].value;

    //this.UserExist(username).subscribe(
    //  (response) => {
    //    if (response == null) {
    //      return null;
    //    } else {
    //      formGroup.controls['username'].setErrors({ userExit: true })
    //    }
    //    return null;
    //  }, (error) => {
    //    return null;
    //  })
  
}


