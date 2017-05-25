import { Component, EventEmitter, Input, Output, ViewChild } from '@angular/core';
import { TemperatureConfig } from '../models/temperatureConfig';
import { MdDialog, MdDialogRef } from '@angular/material';
import 'rxjs/add/operator/debounceTime';

@Component({
  selector: 'config-dialog',
  templateUrl: './config-dialog.component.html'
})
export class ConfigDialog {
  @Input() temperatureConfig: TemperatureConfig;
  @Output() onChange = new EventEmitter<TemperatureConfig>();
  @ViewChild('highAlarmForm') highlarmForm: any;
  @ViewChild('lowAlarmForm') lowAlarmForm: any;
  constructor(public dialogRef: MdDialogRef<ConfigDialog>) {

  }
  ngOnInit() {

  }

  configChange() {
    this.onChange.emit(this.temperatureConfig);
  }
}