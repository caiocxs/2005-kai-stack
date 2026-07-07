import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WindowNso } from './window-nso';

describe('WindowNso', () => {
  let component: WindowNso;
  let fixture: ComponentFixture<WindowNso>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [WindowNso],
    }).compileComponents();

    fixture = TestBed.createComponent(WindowNso);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
