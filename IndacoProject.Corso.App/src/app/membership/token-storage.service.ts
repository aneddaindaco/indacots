import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Configuration, UserService } from '../api/indaco-api';

const TOKEN_KEY = 'auth-token';
const REFRESH_KEY = 'refresh-token';

@Injectable({
  providedIn: 'root'
})
export class TokenStorageService {

  constructor(private http: HttpClient, private user: UserService) {
    this.user.configuration.credentials = { Bearer: () => `Bearer ${this.accessToken}` }    
    if(this.refreshToken){
      this.callRefreshToken().subscribe({
        next: (data) => {         
        this.refreshToken = data.refreshToken;
        this.accessToken =  data.accessToken;
      }, error: () => {this.refreshToken = null} 
      })
    }
   }

  private get refreshToken() {
    return window.localStorage.getItem(REFRESH_KEY) as string
  }

  private set refreshToken(value: string | null) {
    if (value) {
      window.localStorage.setItem(REFRESH_KEY, value)
    } else {
      window.localStorage.removeItem(REFRESH_KEY)
    }
  }

  get accessToken() {
    return window.sessionStorage.getItem(TOKEN_KEY) as string
  }

  private set accessToken(value: string | null) {
    if (value) {
      window.sessionStorage.setItem(TOKEN_KEY, value as string)
    } else {
      window.sessionStorage.removeItem(TOKEN_KEY)
    }
  }

  private callRefreshToken(): Observable<any> {
    const cfg = new Configuration({
      basePath: "http://localhost:5000",
      credentials: { Bearer: () => `${this.refreshToken}` }
    });
    const srv = new UserService(this.http, "http://localhost:5000", cfg);
    return srv.apiUserRefreshPost()
  }

}
