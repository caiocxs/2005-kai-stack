import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ButtonBarNso } from './button-bar-nso';

describe('ButtonBarNso', () => {
  let component: ButtonBarNso;
  let fixture: ComponentFixture<ButtonBarNso>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ButtonBarNso],
    }).compileComponents();

    fixture = TestBed.createComponent(ButtonBarNso);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
