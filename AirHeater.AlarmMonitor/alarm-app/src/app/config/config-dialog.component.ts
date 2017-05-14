import { Component } from '@angular/core';

import { MdDialog, MdDialogRef } from '@angular/material';

@Component({
  selector: 'config-dialog',
  template: `      
<form class="example-form">
  <md-input-container class="example-full-width">
    <input mdInput placeholder="Low Level">
  </md-input-container>&nbsp;-&nbsp;
  <md-input-container class="example-full-width">
    <input mdInput placeholder="High Level">
  </md-input-container>   

        </form>   
<div class="layout" style="justify-content: flex-end">
<button type="button" (click)="dialogRef.close()">Close</button>
</div>
        
    `,
})
export class ConfigDialog {
  constructor(public dialogRef: MdDialogRef<ConfigDialog>) { }
}