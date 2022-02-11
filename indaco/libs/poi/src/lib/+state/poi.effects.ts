import { Injectable } from '@angular/core';
import { createEffect, Actions, ofType } from '@ngrx/effects';
import { fetch } from '@nrwl/angular';

import * as PoiActions from './poi.actions';
import * as PoiFeature from './poi.reducer';
import { map } from 'rxjs/operators';
import { PoiService } from '../poi.service';

@Injectable()
export class PoiEffects {
  init$ = createEffect(() =>
    this.actions$.pipe(
      ofType(PoiActions.init),
      fetch({
        run: (action) => {
          // Your custom service 'load' logic goes here. For now just return a success action...
          return this.poiservice.getall().pipe(map(pois=> PoiActions.loadPoiSuccess({poi: pois})))
        },
        onError: (action, error) => {
          console.error('Error', error);
          return PoiActions.loadPoiFailure({ error });
        },
      })
    )
  );

  constructor(private readonly actions$: Actions, private poiservice: PoiService) {}
}
