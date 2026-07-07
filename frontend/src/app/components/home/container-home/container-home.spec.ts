import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ContainerHome } from './container-home';

describe('ContainerHome', () => {
  let component: ContainerHome;
  let fixture: ComponentFixture<ContainerHome>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ContainerHome],
    }).compileComponents();

    fixture = TestBed.createComponent(ContainerHome);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
