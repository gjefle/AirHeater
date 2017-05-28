import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ShelveDialog} from './shelve-dialog.component';

describe('ShelveDialogComponent', () => {
  let component: ShelveDialog;
  let fixture: ComponentFixture<ShelveDialog>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ShelveDialog]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ShelveDialog);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
