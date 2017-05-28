import { Component } from '@angular/core';
import { DataContextService } from './data/data-context.service';
import { TemperatureConfig } from './models/temperatureConfig';
import { AlarmView } from './models/AlarmView';
import { AlarmType} from './models/AlarmType';
import { ConfigDialog } from './config/config-dialog.component';
import { ShelveDialog } from './shelve-dialog/shelve-dialog.component';
import { MdDialog, MdDialogRef } from '@angular/material';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})

export class AppComponent {

  constructor(private ctx: DataContextService, private dialog: MdDialog) { }
  
  ngOnInit() {

  }
  ngOnDestroy() {

  }
  get alarmTypes(): AlarmType[] {
    return this.ctx.alarmTypes;
  }
  get shelvedAlarmTypes() {
    return this.alarmTypes.filter((alarmType) => {
      return alarmType.shelved;
    });
  }
  get enabledAlarms(): AlarmView[] {
    var alarms = this.ctx.enabledAlarms;
    return alarms ? alarms : [];
  }
  get temperatureConfig(): TemperatureConfig {
    return this.ctx.temperatureConfig;
  }
  openConfigDialog() {
    let dialogRef = this.dialog.open(ConfigDialog);
    let instance = dialogRef.componentInstance;
    instance.temperatureConfig = this.temperatureConfig;
    instance.onChange.subscribe(this.onConfigChange);
    dialogRef.afterClosed();
  }
  openShelveDialog() {
    let dialogRef = this.dialog.open(ShelveDialog);
    let instance = dialogRef.componentInstance;
    instance.alarmTypes = this.alarmTypes;
    instance.onChange.subscribe(this.onAlarmTypeChange);
    dialogRef.afterClosed();
  }
  onAlarmTypeChange = (alarmType: AlarmType) => {
    this.ctx.saveAlarmType(alarmType);
  }
  onConfigChange = (config: TemperatureConfig) => {
    this.ctx.updateTemperatureConfig(config);
  }


}
