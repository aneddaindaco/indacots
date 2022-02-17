import { Component, OnInit, ViewChild } from '@angular/core';
import { Store } from '@ngrx/store';
import { PoiSelectors, PoiActions } from '@indaco/poi';
import { MapMarker, MapInfoWindow } from '@angular/google-maps';

@Component({
  selector: 'indaco-map',
  templateUrl: './map.component.html',
  styleUrls: ['./map.component.css']
})
export class MapComponent  implements OnInit {

  poi$ = this.store.select(PoiSelectors.getSelected)
  @ViewChild(MapInfoWindow) info: MapInfoWindow | undefined;

  constructor(private store: Store) { }

  ngOnInit():void{
   this.metQuel()
  }

  showInfo(marker: MapMarker, poiId: string | number){
    this.store.dispatch(PoiActions.visitPoi({poiId}))
    this.info ?.open(marker)
  }

  metQuel(){
    console.log('pippo')
  }
}
