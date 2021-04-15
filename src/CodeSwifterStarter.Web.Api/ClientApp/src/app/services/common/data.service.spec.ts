/* tslint:disable:no-unused-variable */

import { TestBed, inject } from '@angular/core/testing';
import { DataService } from './data.service';
import { HttpClientTestingModule } from '@angular/common/http/testing';

describe('Service: Data',
  () => {
    beforeEach(() => {
      TestBed.configureTestingModule({
        providers: [
          DataService
        ],
        imports: [
          HttpClientTestingModule
        ]
      });
    });

    it('should create',
      inject([DataService],
        (service: DataService) => {
          expect(service).toBeTruthy();
        }));
  });
