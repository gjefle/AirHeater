import { BrowserModule } from '@angular/platform-browser';
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
declare var require: any;
export function highchartsFactory() {
  return require('highcharts');
}
@NgModule({
  declarations: [
    AppComponent,
    TemperatureComponent,
    ConfigDialog
  ],
  entryComponents: [ConfigDialog],
  imports: [
    BrowserModule,
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
    }],
  bootstrap: [AppComponent]
})
export class AppModule { }
