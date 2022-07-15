import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators, FormBuilder, AbstractControl } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../_services/auth.service';
import { TokenStorageService } from '../_services/token-storage.service';
import { UserService } from '../_services/user.service';
import { ValidationService } from '../_services/validation.service';

@Component({
  selector: 'app-edit',
  templateUrl: './edit.component.html',
  styleUrls: ['./edit.component.css']
})
export class EditComponent implements OnInit {
  formEdit: FormGroup = new FormGroup({
    firstname: new FormControl(''),
    lastname: new FormControl(''),
    username: new FormControl(''),
    password: new FormControl(''),
    userGuid: new FormControl('')
  });

  submitted = false;
  isEditFailed = false;
  errorMessage = '';
  isEditPass = false;
  Message = '';
  isSuccessful = false;

  constructor(
    private formBuilder: FormBuilder,
    private validationService: ValidationService,
    private user: UserService,
    private tokenStorage: TokenStorageService,
    private router: Router,
    private autheService: AuthService) { }


  ngOnInit(): void {
    if (!this.tokenStorage.getToken()) {
      this.router.navigate(['/app-login']);
    }
    this.user.getUserInfo().subscribe(data => {
      this.formEdit = this.formBuilder.group(
        {
          firstname: [data.firstName, Validators.required],
          lastname: [data.lastName, Validators.required],
          username: [
            data.username,
            [
              Validators.required,
              Validators.minLength(6),
              Validators.maxLength(12),
              Validators.pattern('^[A-Za-z0-9_-]{0,12}$')
              //Validators.usernameNotAvailable
            ]
          ],
          password: [
            '',
            [
              Validators.minLength(6),
              Validators.maxLength(40)
            ]
          ],
          userGuid: [data.userGuid],
        }
      );
    }, err => {
      this.errorMessage = err.error.ErrorMessage;
      this.isEditFailed = true;
    }
    )

  }


  get f(): { [key: string]: AbstractControl } {
    return this.formEdit.controls;
  }
  onSubmit(): void {
    this.submitted = true;
    if (this.formEdit.invalid) {
      return;
    }
    else {
      this.user.updateuserInfo(this.formEdit.value).subscribe(
        data => {
          this.Message = data.message;
          this.isSuccessful = true;
          this.isEditFailed = false;
          /*this.router.navigate(['/edit-profile']);*/
        }, error => {
        this.isEditFailed = true;
        this.errorMessage = error.error.errorMessage;
      })
    }
  }

  onReset(): void {
    this.submitted = false;
    this.user.getUserInfo().subscribe(data => {
      this.formEdit = this.formBuilder.group(
        {
          firstname: [data.firstName, Validators.required],
          lastname: [data.lastName, Validators.required],
          username: [
            data.username,
            [
              Validators.required,
              Validators.minLength(6),
              Validators.maxLength(12),
              Validators.pattern('^[A-Za-z0-9_-]{0,12}$')
              //Validators.usernameNotAvailable
            ]
          ],
          password: [
            '',
            [
              Validators.minLength(6),
              Validators.maxLength(40)
            ]
          ],
          userGuid: [data.userGuid]
        }
      );
    }, err => {
      this.errorMessage = err.error.ErrorMessage;
    }
    );
  }
}
