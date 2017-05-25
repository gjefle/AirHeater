import { Component, OnInit, Input } from '@angular/core';
import { DataContextService } from '../data/data-context.service';
import { AlarmView } from '../models/AlarmView';
import { TemperatureConfig } from '../models/temperatureConfig';


@Component({
  selector: 'overview',
  templateUrl: './overview.component.html',
  styleUrls: ['./overview.component.scss']
})
export class OverviewComponent implements OnInit {
  title = 'Temperature Overview';

  constructor(private ctx: DataContextService) {}

  ngOnInit() {

  }

  get temperatureConfig(): TemperatureConfig {
    return this.ctx.temperatureConfig;
  }
  get alarms(): AlarmView[] {
    return this.ctx.enabledAlarms;
  }

  get sortedAlarms() {
    if (!this.alarms || this.alarms.length < 1) return [];
    return this.alarms;
  }

  get enabledAlarms() {
    if (!this.alarms || this.alarms.length < 1) return [];
    return this.alarms.filter((ta) => {
      return ta.active || !ta.acknowledged;
    });

  }
}
