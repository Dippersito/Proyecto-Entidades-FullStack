import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PersonaNatural } from '../models/persona-natural.model';

@Injectable({
  providedIn: 'root'
})
export class EntidadService {

  private apiUrl = 'http://localhost:5096/api/Entidades';
  constructor(private http: HttpClient) { }

  //MÉTODOS CRUD
  getListarPersonas(): Observable<PersonaNatural[]> {
    return this.http.get<PersonaNatural[]>(this.apiUrl);
  }

  getPersonaById(id: number): Observable<PersonaNatural> {
    return this.http.get<PersonaNatural>(`${this.apiUrl}/${id}`);
  }

  crearPersona(persona: PersonaNatural): Observable<PersonaNatural> {
    return this.http.post<PersonaNatural>(this.apiUrl, persona);
  }

  actualizarPersona(id: number, persona: PersonaNatural): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}`, persona);
  }

  eliminarEntidad(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}