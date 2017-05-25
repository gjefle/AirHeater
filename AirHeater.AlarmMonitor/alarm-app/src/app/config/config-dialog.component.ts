import { Component, EventEmitter, Input, Output, ViewChild } from '@angular/core';
import { TemperatureConfig } from '../models/temperatureConfig';
import { MdDialog, MdDialogRef } from '@angular/material';
import 'rxjs/add/operator/debounceTime';
import 'rxjs/add/operator/distinctUntilChanged';

@Component({
  selector: 'config-dialog',
  templateUrl: './config-dialog.component.html'
})
export class ConfigDialog {
  @Input() temperatureConfig: TemperatureConfig;
  @Output() onChange = new EventEmitter<TemperatureConfig>();
  @ViewChild('highAlarmForm') highlarmForm: any;
  @ViewChild('lowAlarmForm') lowAlarmForm: any;

  highAlarmVal: number;
  lowAlarmVal: number;
  constructor(public dialogRef: MdDialogRef<ConfigDialog>) { }
  ngOnInit() {
    this.highAlarmVal = this.temperatureConfig.highAlarmTemperature;
    this.lowAlarmVal = this.temperatureConfig.lowAlarmTemperature;
    // Set a delay of 500ms before changes to config values are saved.
    this.highlarmForm.valueChanges
      .debounceTime(500)
      .distinctUntilChanged()
      .subscribe(() => {
        if (this.highAlarmVal !== this.temperatureConfig.highAlarmTemperature)
          this.temperatureConfig.highAlarmTemperature = this.highAlarmVal;
          this.configChange();
      });
    this.lowAlarmForm.valueChanges
      .debounceTime(500)
      .distinctUntilChanged()
      .subscribe(() => {
        if (this.lowAlarmVal !== this.temperatureConfig.lowAlarmTemperature) {
          this.temperatureConfig.lowAlarmTemperature = this.lowAlarmVal;
          this.configChange();
        }
      });
  }

  configChange() {
    this.onChange.emit(this.temperatureConfig);
  }
}