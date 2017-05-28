import { Component, EventEmitter, Input, Output } from '@angular/core';
import { AlarmType } from '../models/alarmType';
import { MdDialog, MdDialogRef } from '@angular/material';
@Component({
  selector: 'app-shelve-dialog',
  templateUrl: './shelve-dialog.component.html',
  styleUrls: ['./shelve-dialog.component.scss']
})
export class ShelveDialog {
  @Input() alarmTypes: AlarmType[];
  @Output() onChange = new EventEmitter<AlarmType>();
  constructor() { }
  unshelveAlarmType(alarmType: AlarmType) {
    alarmType.shelved = false;
    this.onChange.emit(alarmType);
  }
  shelveAlarmType(alarmType: AlarmType) {
    alarmType.shelved = true;
    this.onChange.emit(alarmType);
  }
}
