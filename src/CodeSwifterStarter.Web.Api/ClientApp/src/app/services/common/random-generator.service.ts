import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class RandomGeneratorService {
  constructor() {
  }

  newGuid(): string { return uuidv5(); }

  postGuid(guid: string): string { return `{${guid}}`; }
}
