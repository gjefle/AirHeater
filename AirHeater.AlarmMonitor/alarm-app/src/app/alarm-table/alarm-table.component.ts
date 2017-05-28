import { Component, OnInit, Input } from '@angular/core';
import { AlarmView } from '../models/AlarmView';
import { AlarmType } from '../models/AlarmType';
import { DataContextService } from '../data/data-context.service';

@Component({
  selector: 'alarm-table',
  templateUrl: './alarm-table.component.html',
  styleUrls: ['./alarm-table.component.scss']
})
export class AlarmTableComponent implements OnInit {
  @Input() alarms: AlarmView[];
  constructor(private ctx: DataContextService) { }

  ngOnInit() {
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
  shelveAlarm(alarmType: AlarmType) {
    alarmType.shelved = true;
    this.ctx.saveAlarmType(alarmType);
  }
}
