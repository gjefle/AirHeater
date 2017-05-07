import { Component, OnInit } from '@angular/core';
import { IntervalObservable } from 'rxjs/observable/IntervalObservable';
import { DataContextService } from '../../app/data/data-context.service';
import { Temperature } from '../models/temperature';
import * as moment from "moment";
@Component({
  selector: 'app-temperature',
  templateUrl: './temperature.component.html',
  styleUrls: ['./temperature.component.scss'],
  providers: [DataContextService]
})
export class TemperatureComponent implements OnInit {
  interval: any;
  temperatures: Temperature[];
  lastUpdate: Date;
  // Highcharts objects
  options;
  chart;
  constructor(private ctx: DataContextService) { }

  ngOnInit() {
    this.setOptions();
    this.interval = IntervalObservable.create(2000).subscribe(this.updateTemperatures);
    this.getAllTemperatures();
  }
  ngOnChanges() {
    this.getAllTemperatures();
  }
  ngOnDestroy() {
    this.interval.unsubscribe(this.updateTemperatures);
  }
  saveInstance(chartInstance) {
    this.chart = chartInstance;
  }
  private updateTemperatures = () => {
    this.ctx.updateTemperatures(this.lastUpdate).then(temperatures => {
      if (temperatures && temperatures.length > 0) {
        this.updateChartData(temperatures);
        this.lastUpdate = new Date();
      }
    });
  }
  private getAllTemperatures() {
    this.ctx.getTemperatures().then(temperatures => {
      this.temperatures = temperatures;
      this.setChartData(temperatures);
      this.lastUpdate = new Date();
    });
  }
  private updateChartData(temperatures: Temperature[]) {
    temperatures.forEach(t => {
      var m = moment(t.logDate).utc(true).valueOf();
      this.chart.series[0].addPoint([m, t.value]);
    });

  }
  private setChartData = (temperatures: Temperature[]) => {
    this.chart.yAxis[0].update({
      title: {
        text: "Temperature \u00B0C"
      }
    });
    var data = [];
    temperatures.forEach(t => {
      var m = moment(t.logDate).utc(true).valueOf();
      data.push([m, t.value]);
    });
    this.chart.series[0].setData(data, true);
  }
  private setOptions() {
    this.options = {
      xAxis: {
        type: 'datetime',
        title: {
          text: 'Date'
        }
      },
      yAxis: {
        title: {
          text: "Temperature \u00B0C"
        },
        min: 0
      },
      title: { text: "" },
      series: [{
        name: "Logged temperature",
        data: []
      }]
    };
  }

}
