import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../_services/auth.service';
import { TokenStorageService } from '../_services/token-storage.service';
import { ValidationService }  from '../_services/validation.service';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  formSignin: FormGroup = new FormGroup({
    username: new FormControl(''),
    password: new FormControl(''),
  });
  isLoggedIn = false;
  isLoginFailed = false;
  errorMessage = '';
  roles: string[] = [];

  constructor(private authService: AuthService,
    private tokenStorage: TokenStorageService,
    private formBuilder: FormBuilder,
    private validationService: ValidationService,
    private router: Router) { }

  ngOnInit(): void {
    if (this.tokenStorage.getToken()) {
      this.isLoggedIn = true;
      this.roles = this.tokenStorage.getUser().roles;
      this.router.navigate(['/edit-profile']);
    }
    this.formSignin = this.formBuilder.group(
    {
      username: [ '',[Validators.required ]],
      password: ['', [Validators.required]]
    });
  }


  onSubmit(): void {
    if (this.formSignin.invalid) {
      return;
    }
    else {
      this.authService.login(this.formSignin.value).subscribe(
        data => {
          this.tokenStorage.saveToken(data.token);
          this.tokenStorage.saveUser(data);
          this.isLoginFailed = false;
          this.isLoggedIn = true;
          this.roles = this.tokenStorage.getUser().roles;
          this.router.navigate(['/edit-profile']);
        },
        err => {
          this.errorMessage = err.error.errorMessage;
          this.isLoginFailed = true;
        }
      );
    }
  }

  reloadPage(): void {
    window.location.reload();
  }

}
