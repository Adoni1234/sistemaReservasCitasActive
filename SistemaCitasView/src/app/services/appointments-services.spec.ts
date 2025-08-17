import { TestBed } from '@angular/core/testing';

import { AppointmentsServices } from './appointments-services';

describe('AppointmentsServices', () => {
  let service: AppointmentsServices;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AppointmentsServices);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
