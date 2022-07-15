import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ValidationService } from '../_services/validation.service';
import { AuthService } from '../_services/auth.service';
import { Router } from '@angular/router';
import { TokenStorageService } from '../_services/token-storage.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  form: FormGroup = new FormGroup({
    firstname: new FormControl(''),
    lastname: new FormControl(''),
    username: new FormControl(''),
    password: new FormControl(''),
    confirmPassword: new FormControl(''),
    acceptTerms: new FormControl(false),
  });

  submitted = false;
  isSuccessful = false;
  isSignUpFailed = false;
  errorMessage = '';

  constructor(
    private formBuilder: FormBuilder,
    private validationService: ValidationService,
    private autheService: AuthService,
    private router: Router,
    private tokenStorage: TokenStorageService

  ) { }



  ngOnInit(): void {
    if (this.tokenStorage.getToken()) {
      this.router.navigate(['/edit-profile']);
    }
    this.form = this.formBuilder.group(
      {
        firstname: ['', Validators.required],
        lastname: ['', Validators.required],
        username: [
          '',
          [
            Validators.required,
            Validators.minLength(6),
            Validators.maxLength(12),
            Validators.pattern('^[A-Za-z0-9_-]{0,12}$')
/*            this.validationService.CheckPassNotSequ();*/
          ]
        ],
        password: [
          '',
          [
            Validators.required,
            Validators.minLength(6)
          ]
        ],
        confirmPassword: ['', Validators.required],
        acceptTerms: [false, Validators.requiredTrue]
      },
      {
        validators: [this.validationService.match('password', 'confirmPassword')]
      }
    );
  }

  get f(): { [key: string]: AbstractControl } {
    return this.form.controls;
  }
  onSubmit(): void {
    this.submitted = true;
    if (this.form.invalid) {
      return;
    }
    else if(!this.validationService.CheckPassNotSequ(this.form.get('password')?.value))
    {
      this.form.get('password')?.setErrors({ pattern: true })
      return;
    }
    else {
      this.autheService.Register(this.form.value).subscribe(
        data => {
          console.log(data);
          this.isSuccessful = true;
          this.isSignUpFailed = false;
          this.router.navigate(['/app-login']);
      }, error => {
          this.errorMessage = error.error.errorMessage;
          this.isSignUpFailed = true;
      })
    }
  }


  onReset(): void {
    this.submitted = false;
    this.form.reset();
  }


}
