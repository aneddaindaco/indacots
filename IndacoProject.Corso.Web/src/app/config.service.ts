import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

interface Iconfig {
  [k: string]: string
}

@Injectable({
  providedIn: 'root'
})
export class ConfigService implements Iconfig {
  [k: string]: string;

  constructor() {
    const browserWindow: any = window || {};
    const browserWindowEnv = Object.assign({}, environment, browserWindow['__env']);

    for (const key in browserWindowEnv) {
      if (browserWindowEnv.hasOwnProperty(key)) {
        this[key] = browserWindowEnv[key];
      }
    }
  }
}
