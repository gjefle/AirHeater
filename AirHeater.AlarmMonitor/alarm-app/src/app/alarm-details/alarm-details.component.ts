import { Component, OnInit } from '@angular/core';
import { AlarmView } from '../models/AlarmView';
import { DataContextService } from '../data/data-context.service';

@Component({
  selector: 'alarm-details',
  templateUrl: './alarm-details.component.html',
  styleUrls: ['./alarm-details.component.scss']
})
export class AlarmDetailsComponent implements OnInit {
  title = "Alarm details";
  alarms: AlarmView[];
  constructor(private ctx: DataContextService) { }

  ngOnInit() {
    this.ctx.getAlarms().then(alarms => {
      this.alarms = alarms;
    });
  }
}
