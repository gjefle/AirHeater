﻿import { Component } from '@angular/core';
import { DataContextService } from './data/data-context.service';
import { AlarmView } from './models/AlarmView';
import { IntervalObservable } from 'rxjs/observable/IntervalObservable';
import { MdDialog, MdDialogRef } from '@angular/material';
import { TemperatureConfig } from './models/temperatureConfig';
import { ConfigDialog } from './config/config-dialog.component';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  providers: [DataContextService]
})

export class AppComponent {
  title = 'Alarm Monitor';
  alarms: AlarmView[];
  temperatureConfig: TemperatureConfig;
  interval: any;
  constructor(private ctx: DataContextService, private dialog: MdDialog) { }
  ngOnInit() {
    this.getTodaysAlarms();
    this.interval = IntervalObservable.create(600).subscribe(this.getTodaysAlarms);
    this.getTemperatureConfig();
  }
  ngOnDestroy() {
    this.interval.unsubscribe(this.getAlarms);
  }
  selectedOption;
  openDialog() {
    let dialogRef = this.dialog.open(ConfigDialog);
    let instance = dialogRef.componentInstance;
    instance.temperatureConfig = this.temperatureConfig;
    instance.onChange.subscribe(this.onConfigChange);
    dialogRef.afterClosed().subscribe(result => {
      this.selectedOption = result;
    });
  }

  onConfigChange = (config: TemperatureConfig) => {
    this.ctx.updateTemperatureConfig(config);
  }

  get sortedAlarms() {
    if (!this.alarms || this.alarms.length < 1) return [];
    return this.alarms;
    //return this.alarms.sort((a: AlarmView, b: AlarmView) => {
    //  if (a.active > b.active) {
    //    return -1;
    //  } else if (a.active < b.active) {
    //    return 1;
    //  } else {
    //    return 0;
    //  }
    //});
  }

  //get sortedAlarms2() {
  //  if (!this.alarms || this.alarms.length < 1) return [];
  //  return this.alarms.sort((a, b) => {
  //    if (a.active === b.active)
  //      return (a.acknowledged > b.acknowledged) ? 1 : 0;
  //    else if (a.active)
  //      return -1;
  //    else return 1;
  //  });
  //}
  get enabledAlarms() {
    if (!this.alarms || this.alarms.length < 1) return [];
    return this.alarms.filter((ta) => {
      return ta.active || !ta.acknowledged;
    });

  }
  alarmStyle(alarm: AlarmView) {
    return {
      "font-weight": alarm.active || !alarm.acknowledged
        ? 600
        : 300
    }
  }
  acknowledgeAlarm(alarm: AlarmView) {
    this.ctx.acknowledgeAlarm(alarm);
  }
  private getTemperatureConfig() {
    this.ctx.getTemperatureConfig().then((config: TemperatureConfig) => {
      this.temperatureConfig = config;
    });
  }
  private getAlarms = () => {
    this.ctx.getAlarms().then(alarms => {
      this.alarms = alarms;
    });
  }
  private getTodaysAlarms = () => {
    this.ctx.todaysAlarms().then(alarms => {
      this.alarms = alarms;
    });
  }


}
