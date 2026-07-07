import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CalendarNso } from './calendar-nso';

describe('CalendarNso', () => {
  let component: CalendarNso;
  let fixture: ComponentFixture<CalendarNso>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CalendarNso],
    }).compileComponents();

    fixture = TestBed.createComponent(CalendarNso);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
