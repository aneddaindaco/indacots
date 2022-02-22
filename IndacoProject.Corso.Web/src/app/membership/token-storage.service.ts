import { HttpClient, HttpContext } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { UserService, EmailApiService, Configuration } from '../api/indaco-api';

const TOKEN_KEY = 'auth-token';
const REFRESH_KEY = 'refresh-token';
const USER_KEY = 'auth-user';

@Injectable({
  providedIn: 'root'
})
export class TokenStorageService {
  private timer: ReturnType<typeof setTimeout>  | undefined

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
      this.user.configuration.credentials = { Bearer: () => `Bearer ${this.accessToken}` }
    } else {
      window.sessionStorage.removeItem(TOKEN_KEY)
      delete this.user.configuration.credentials["Bearer"]
    }
  }

  constructor(private http: HttpClient, private user: UserService) {
    this.user.configuration.credentials = { Bearer: () => `Bearer ${this.accessToken}` }
    if (this.refreshToken) {
      this.callRefreshToken().subscribe({
        next: (data) => {
          this.refreshToken = data.refreshToken
          this.accessToken = data.accessToken
          this.timer = setInterval(this.onRefresh, 5000)
        },
        error: () => {
          this.refreshToken = null
        }
      })
    }
  }

  private onRefresh() {
    this.callRefreshToken().subscribe({
      next: (data) => {
        this.refreshToken = data.refreshToken
        this.accessToken = data.accessToken
        this.timer = setInterval(this.onRefresh, 5000)
      },
      error: () => {
        this.refreshToken = null
      }
    })
  }

  public logout(): void {
    window.sessionStorage.clear();
    if (this.timer) {
      clearTimeout(this.timer);
    }
  }

  public get loggedIn() {
    return !!this.accessToken
  }

  public login(accessToken: string, refreshToken: string) {
    this.accessToken = accessToken;
    this.refreshToken = refreshToken
    this.timer = setTimeout(this.onRefresh, 5000);
  }

  public saveUser(user: any): void {
    window.sessionStorage.removeItem(USER_KEY);
    window.sessionStorage.setItem(USER_KEY, JSON.stringify(user));
  }

  public getUser(): any {
    const user = window.sessionStorage.getItem(USER_KEY);
    if (user) {
      return JSON.parse(user);
    }

    return {};
  }

  private callRefreshToken(): Observable<any> {
    const cfg = new Configuration({
      basePath: "",
      credentials: { Bearer: () => `this.refreshToken` }
    });
    const srv = new UserService(this.http, "", cfg);
    return srv.apiUserRefreshPost()
  }
}
