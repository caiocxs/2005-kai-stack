import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RandomIcon } from './random-icon';

describe('RandomIcon', () => {
  let component: RandomIcon;
  let fixture: ComponentFixture<RandomIcon>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RandomIcon],
    }).compileComponents();

    fixture = TestBed.createComponent(RandomIcon);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
