import { Component } from '@angular/core';
import { Card } from 'ui-controls';
import { assassins } from './assassins';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'my-components';
  cards: Card[] = assassins


  OnCardChange(cards: Card[]){
    console.log(cards)
  }

  log(){
    console.log(this.title + " è stato copiato sulla clipboard")
  }
}
