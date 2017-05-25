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
  constructor(public dialogRef: MdDialogRef<ConfigDialog>) {

  }
  ngOnInit() {
    // Set a delay of 500ms before changes to config values are saved.
    this.highlarmForm.valueChanges
      .debounceTime(500)
      .distinctUntilChanged()
      .subscribe(data => this.configChange());
    this.lowAlarmForm.valueChanges
      .debounceTime(500)
      .distinctUntilChanged()
      .subscribe(data => this.configChange());
  }

  configChange() {
    this.onChange.emit(this.temperatureConfig);
  }
}