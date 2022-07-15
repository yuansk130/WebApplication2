import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { TokenStorageService } from './token-storage.service';



const baseUrl = environment.apiUrl;

@Injectable({
  providedIn: 'root'
})

export class UserService {
  auth_token = this.tokenStorage.getToken();
  auth_user = this.tokenStorage.getUsername();
  httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${this.auth_token}`
    })
  };
  constructor(private http: HttpClient, private tokenStorage: TokenStorageService) { }


  getUserInfo(): Observable<any> {

    return this.http.get(baseUrl + '/User/getUserInfo?username=' + this.auth_user, this.httpOptions);
  }

  updateuserInfo(Form1: FormGroup): Observable<any> {

    return this.http.post(baseUrl + '/Account/updateUser', Form1, this.httpOptions);
  }
}
