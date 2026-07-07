import { Component } from '@angular/core';
import { HeaderHome } from '../../components/home/header-home/header-home';
import { ContainerHome } from '../../components/home/container-home/container-home';
import { FooterHome } from '../../components/home/footer-home/footer-home';

@Component({
  selector: 'app-home',
  imports: [HeaderHome, ContainerHome, FooterHome],
  templateUrl: './home.html',
  styleUrl: './home.scss',
})
export class Home {}
