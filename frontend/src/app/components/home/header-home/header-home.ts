import { Component } from '@angular/core';
import { ProfileWindow } from '../../general/profile-window/profile-window';
import { NavigationLink } from '../../general/navigation-link/navigation-link';

@Component({
  selector: 'app-header-home',
  imports: [ProfileWindow, NavigationLink],
  templateUrl: './header-home.html',
  styleUrl: './header-home.scss',
})
export class HeaderHome {}
