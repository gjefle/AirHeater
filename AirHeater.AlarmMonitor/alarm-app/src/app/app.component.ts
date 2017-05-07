import { Component } from '@angular/core';
import { DataContextService } from './data/data-context.service';
import { TemperatureAlarmView } from './models/temperatureAlarmView';
import { IntervalObservable } from 'rxjs/observable/IntervalObservable';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  providers: [DataContextService]
})
export class AppComponent {
  title = 'Alarm Monitor';
  temperatureAlarms: TemperatureAlarmView[];
  interval: any;
  constructor(private ctx: DataContextService) { }
  ngOnInit() {
    this.getTemperatureAlarms();
    this.interval = IntervalObservable.create(600).subscribe(this.getTemperatureAlarms);
  }
  ngOnDestroy() {
    this.interval.unsubscribe(this.getTemperatureAlarms);
  }
  get sortedTemperatureAlarms() {
    if (!this.temperatureAlarms || this.temperatureAlarms.length < 1) return [];
    return this.temperatureAlarms.sort((a: TemperatureAlarmView, b: TemperatureAlarmView) => {
      if (a.active > b.active) {
        return -1;
      } else if (a.active < b.active) {
        return 1;
      } else {
        return 0;
      }
    });
  }
  get enabledTemperatureAlarms() {
    if (!this.temperatureAlarms || this.temperatureAlarms.length < 1) return [];
    return this.temperatureAlarms.filter((ta) => {
      return ta.active || !ta.acknowledged;
    });

  }
  alarmStyle(alarm: TemperatureAlarmView) {
    return {
      "font-weight": alarm.active || !alarm.acknowledged
        ? 600
        : 300
  }
  }
  acknowledgeAlarm(alarm: TemperatureAlarmView) {
    this.ctx.acknowledgeAlarm(alarm).then(alarms => {
      this.temperatureAlarms = alarms;
    });
  }
  private getTemperatureAlarms = () => {
    this.ctx.getTemperatureAlarms().then(alarms => {
      this.temperatureAlarms = alarms;
    });
  }
  


}
