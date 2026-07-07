import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProfileWindow } from './profile-window';

describe('ProfileWindow', () => {
  let component: ProfileWindow;
  let fixture: ComponentFixture<ProfileWindow>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProfileWindow],
    }).compileComponents();

    fixture = TestBed.createComponent(ProfileWindow);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
