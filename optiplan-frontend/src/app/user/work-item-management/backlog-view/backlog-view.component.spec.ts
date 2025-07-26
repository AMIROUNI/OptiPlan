import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BacklogViewComponent } from './backlog-view.component';

describe('BacklogViewComponent', () => {
  let component: BacklogViewComponent;
  let fixture: ComponentFixture<BacklogViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BacklogViewComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BacklogViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
