import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import 'rxjs/add/operator/toPromise';
import 'rxjs/add/operator/map';
import { AlarmView } from '../models/AlarmView';
import { IntervalObservable } from 'rxjs/observable/IntervalObservable';
import { Observable } from 'rxjs/observable';
import { Temperature } from '../models/temperature';
import { TemperatureConfig } from '../models/temperatureConfig';
import { Headers, RequestOptions } from '@angular/http';

@Injectable()
export class DataContextService {
  enabledAlarms: AlarmView[];
  interval: any;
  temperatureConfig: TemperatureConfig;
  constructor(private http: Http) {
    this.setEnabledAlarms();
    this.interval = IntervalObservable.create(600).subscribe(this.updateEnabledAlarms);
    this.setTemperatureConfig();
  }
  getTemperatureConfig(): Promise<TemperatureConfig> {
    const url = "/api/config/";
    return this.http.get(url)
      .toPromise()
      .then(res => res.json() as TemperatureConfig)
      .catch(this.handleError);

  }

  private headers = new Headers({ 'Content-Type': 'application/json' });
  updateTemperatureConfig(config: TemperatureConfig) {
    const url = `api/config/${config.temperatureConfigId}`;
    return this.http
      .put(url, JSON.stringify(config), { headers: this.headers })
      .toPromise()
      .then(() => config)
      .catch(this.handleError);
  }
  updateEnabledAlarms = () => {
    this.getEnabledAlarms().then(alarms => {
      this.enabledAlarms = alarms;
    });
  }
  getAlarms(): Promise<AlarmView[]> {
    const url = '/api/alarm/all';
    return this.http.get(url)
      .toPromise()
      // Call map on the response observable to get the parsed people object
      .then(res => res.json() as AlarmView[])
      .catch(this.handleError);
  }
  getEnabledAlarms = (): Promise<AlarmView[]> => {
    const url = '/api/alarm/enabledAlarms';
    return this.http.get(url)
      .toPromise()
      .then(res => res.json() as AlarmView[])
      .catch(this.handleError);
  }
  acknowledgeAlarm(alarm: AlarmView) {
    const url = '/api/alarm/AcknowledgeAlarm/' + alarm.alarmId;
    return this.http.get(url)
      .toPromise()
      .then(res => res.json() as AlarmView[])
      .catch(this.handleError);
  }
  getTemperatures(): Promise<Temperature[]> {
    const url = '/api/temperature';
    return this.http.get(url)
      .toPromise()
      .then(res => res.json() as Temperature[])
      .catch(this.handleError);
  }
  updateTemperatures(lastUpdate: Date): Promise<Temperature[]> {
    var datevalue = lastUpdate.toUTCString();
    const url = '/api/temperature/updateTemperature/' + datevalue;

    return this.http.get(url)
      .toPromise()
      .then(res => res.json() as Temperature[])
      .catch(this.handleError);
  }

  private handleError(error: any): Promise<any> {
    console.error('An error occurred', error);
    return Promise.reject(error.message || error);
  }
  private setEnabledAlarms() {
    this.getEnabledAlarms().then(alarms => {
      this.enabledAlarms = alarms;
    });
  }
  private setTemperatureConfig() {
    this.getTemperatureConfig().then(config => {
      this.temperatureConfig = <TemperatureConfig>config;
    });
  }

}