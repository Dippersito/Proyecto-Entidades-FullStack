import { TestBed } from '@angular/core/testing';

import { Entidad } from './entidad';

describe('Entidad', () => {
  let service: Entidad;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(Entidad);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
