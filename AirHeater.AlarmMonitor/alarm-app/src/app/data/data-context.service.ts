import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import 'rxjs/add/operator/toPromise';
import 'rxjs/add/operator/map';
import { TemperatureAlarmView } from  '../models/temperatureAlarmView';
import { IntervalObservable } from 'rxjs/observable/IntervalObservable';
import { Observable } from 'rxjs/observable';
import { Temperature } from '../models/temperature';

@Injectable()
export class DataContextService {
  constructor(private http: Http) {

  }

  getTemperatureAlarms(): Promise<TemperatureAlarmView[]> {
    const url = '/api/alarm/temperatureAlarms';
    return this.http.get(url)
      .toPromise()
      // Call map on the response observable to get the parsed people object
      .then(res => res.json() as TemperatureAlarmView[])
      .catch(this.handleError);
    
  }
  acknowledgeAlarm(alarm: TemperatureAlarmView) {
      const url = '/api/alarm/AcknowledgeAlarm/' + alarm.temperatureAlarmId;
      return this.http.get(url)
          .toPromise()
          // Call map on the response observable to get the parsed people object
          .then(res => res.json() as TemperatureAlarmView[])
          .catch(this.handleError);
  }
  getTemperatures(): Promise<Temperature[]> {
    const url = '/api/temperature';
    return this.http.get(url)
      .toPromise()
      // Call map on the response observable to get the parsed people object
      .then(res => res.json() as Temperature[])
      .catch(this.handleError);
  }
  updateTemperatures(lastUpdate: Date): Promise<Temperature[]> {
    var datevalue = lastUpdate.toUTCString();
    const url = '/api/temperature/updateTemperature/' + datevalue;
    
    return this.http.get(url)
      .toPromise()
      // Call map on the response observable to get the parsed people object
      .then(res => res.json() as Temperature[])
      .catch(this.handleError);
  }
 
  private handleError(error: any): Promise<any> {
    console.error('An error occurred', error); // for demo purposes only
    return Promise.reject(error.message || error);
  }

}