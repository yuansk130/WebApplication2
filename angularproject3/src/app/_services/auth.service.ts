import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';


const baseUrl = environment.apiUrl;

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http: HttpClient) { }


  Register(Form1: FormGroup): Observable<any>{
    return this.http.post(baseUrl + '/Account/register', Form1);
  }

  login(Form1: FormGroup): Observable<any> {
    return this.http.post(baseUrl + '/Account/signin', Form1);
  }

}
