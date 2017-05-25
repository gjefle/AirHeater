import { BrowserModule } from '@angular/platform-browser';
import {RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { ChartModule } from 'angular2-highcharts';
import { HighchartsStatic } from 'angular2-highcharts/dist/HighchartsService';
import { AppComponent } from './app.component';
import { TemperatureComponent } from './temperature/temperature.component';
import { ConfigDialog } from './config/config-dialog.component';
import { MdDialogModule } from '@angular/material';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MdInputModule } from '@angular/material';
import { OverviewComponent } from './overview/overview.component';
import { AlarmDetailsComponent } from './alarm-details/alarm-details.component';
import { DataContextService } from './data/data-context.service';
import { AlarmTableComponent } from './alarm-table/alarm-table.component';
declare var require: any;
export function highchartsFactory() {
  return require('highcharts');
}

@NgModule({
  declarations: [
    AppComponent,
    TemperatureComponent,
    ConfigDialog,
    OverviewComponent,
    AlarmDetailsComponent,
    AlarmTableComponent
  ],
  entryComponents: [ConfigDialog],
  imports: [
    BrowserModule,
    RouterModule.forRoot([
      {
        path: "",
        redirectTo: "/alarm-details",
        pathMatch: "full"
      },
      {
        path: "overview",
        component: OverviewComponent
      },
      {
        path: "alarm-details",
        component: AlarmDetailsComponent
      }
    ]),
    FormsModule,
    HttpModule,
    ChartModule,
    MdDialogModule,
    BrowserAnimationsModule,
    MdInputModule
  ],
  providers: [
    {
      provide: HighchartsStatic,
      useFactory: highchartsFactory
    },
    DataContextService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
