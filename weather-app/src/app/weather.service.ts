import { HttpClient, HttpParams } from '@angular/common/http';
import { NULL_EXPR } from '@angular/compiler/src/output/output_ast';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Weather } from './weather';

@Injectable({
  providedIn: 'root'
})
export class WeatherService {


  constructor(private http: HttpClient) { }

  getWeather(city: string): Observable<Weather>{
    const options = new HttpParams()
              .set("units", "metric")
              .set("q", city)
              .set("lang", "it")
              .set("appid", environment.apiKey);
    return this.http.get<Weather>(`${environment.apiUrl}weather`, {params: options} );    
  }
}
