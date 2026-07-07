import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ClockNso } from './clock-nso';

describe('ClockNso', () => {
  let component: ClockNso;
  let fixture: ComponentFixture<ClockNso>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ClockNso],
    }).compileComponents();

    fixture = TestBed.createComponent(ClockNso);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
