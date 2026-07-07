import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PipeBarNso } from './pipe-bar-nso';

describe('PipeBarNso', () => {
  let component: PipeBarNso;
  let fixture: ComponentFixture<PipeBarNso>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PipeBarNso],
    }).compileComponents();

    fixture = TestBed.createComponent(PipeBarNso);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
