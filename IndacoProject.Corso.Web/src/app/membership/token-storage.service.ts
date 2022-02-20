import { Injectable } from '@angular/core';
import { UserService, EmailApiService } from '../api/indaco-api';

const TOKEN_KEY = 'auth-token';
const USER_KEY = 'auth-user';

@Injectable({
  providedIn: 'root'
})
export class TokenStorageService {
  constructor(private user: EmailApiService) { }

  signOut(): void {
    window.sessionStorage.clear();
  }

  public saveToken(token: string): void {
    this.user.configuration.credentials = { Bearer: () => `Bearer ${window.sessionStorage.getItem(TOKEN_KEY)}` }
    window.sessionStorage.removeItem(TOKEN_KEY);
    window.sessionStorage.setItem(TOKEN_KEY, token);
  }

  public getToken(): string | null {
    this.user.configuration.credentials = { Bearer: () => `Bearer ${window.sessionStorage.getItem(TOKEN_KEY)}` }
    return window.sessionStorage.getItem(TOKEN_KEY);
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
}
