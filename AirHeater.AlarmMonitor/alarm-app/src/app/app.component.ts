import { Component } from '@angular/core';
import { DataContextService } from './data/data-context.service';
import { TemperatureConfig } from './models/temperatureConfig';
import { AlarmView } from './models/AlarmView';
import { ConfigDialog } from './config/config-dialog.component';
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
  get enabledAlarms(): AlarmView[] {
    var alarms = this.ctx.enabledAlarms;
    return alarms ? alarms : [];
  }
  get temperatureConfig(): TemperatureConfig {
    return this.ctx.temperatureConfig;
  }
  openDialog() {
    let dialogRef = this.dialog.open(ConfigDialog);
    let instance = dialogRef.componentInstance;
    instance.temperatureConfig = this.temperatureConfig;
    instance.onChange.subscribe(this.onConfigChange);
    dialogRef.afterClosed();
  }
  onConfigChange = (config: TemperatureConfig) => {
    this.ctx.updateTemperatureConfig(config);
  }


}
