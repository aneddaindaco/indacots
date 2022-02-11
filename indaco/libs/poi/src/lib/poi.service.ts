import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PoiEntity } from '..';

@Injectable({
  providedIn: 'root'
})
export class PoiService {

  constructor(private http: HttpClient) { }

  getall(): Observable<PoiEntity[]>{
    return this.http.get<PoiEntity[]>("assets/poi.json");
  }
}
