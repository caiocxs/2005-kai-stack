import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RouteBarNso } from './route-bar-nso';

describe('RouteBarNso', () => {
  let component: RouteBarNso;
  let fixture: ComponentFixture<RouteBarNso>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RouteBarNso],
    }).compileComponents();

    fixture = TestBed.createComponent(RouteBarNso);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
