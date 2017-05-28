import { Component, OnInit, Input } from '@angular/core';
import { IntervalObservable } from 'rxjs/observable/IntervalObservable';
import { DataContextService } from '../../app/data/data-context.service';
import { Temperature } from '../models/temperature';
import { TemperatureConfig } from '../models/temperatureConfig';
import * as moment from "moment";
@Component({
  selector: 'app-temperature',
  templateUrl: './temperature.component.html',
  styleUrls: ['./temperature.component.scss'],
  providers: [DataContextService]
})
export class TemperatureComponent implements OnInit {
  @Input() highAlarm: number;
  @Input() lowAlarm: number;

  interval: any;
  temperatures: Temperature[];
  lastUpdate: Date;
  // Highcharts objects
  options;
  chart;
  constructor(private ctx: DataContextService) { }

  ngOnInit() {
    this.lastUpdate = new Date();
    this.lastUpdate.setFullYear(1970);
    this.setOptions();
    //this.interval = IntervalObservable.create(2000).subscribe(this.updateTemperatures);
    this.getAllTemperatures();
  }
  ngOnChanges() {
    this.updatePlotLines();
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
      var m = moment(t.logDate).milliseconds(0).utc(true).valueOf();
      
      data.push([m, t.value]);
    });
    this.chart.series[0].setData(data, true);
  }
  private updatePlotLines(): void {
    if (this.chart) {
      var plotLines = this.getPlotLines();
      plotLines.forEach(line => {
        this.chart.yAxis[0].removePlotBand(line.id);
        this.chart.yAxis[0].addPlotBand(line);
      });
    }
  }
  private getPlotLines(): any[] {
    return [
      {
        id: 'lowerLevel',
        color: 'rgba(0, 163, 212, 0.7)',
        width: 1,
        value: this.lowAlarm,
        dashStyle: 'Dash'
      },
      {
        id: 'higherLevel',
        color: 'rgba(0, 163, 212, 0.7)',
        width: 1,
        value: this.highAlarm,
        dashStyle: 'Dash'
      }
    ];
  }
  private setOptions() {
    this.options = {
      tooltip: {
        valueSuffix: " \u00B0C"
      },
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
        min: 0,
        max: 50,
        plotLines: this.getPlotLines()
  },
      title: { text: "" },
      series: [{
        name: "Logged temperature",
        data: []
      }]
    };
  }

}
