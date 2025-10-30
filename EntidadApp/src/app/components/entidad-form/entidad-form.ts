import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router'; 

import { PersonaNatural } from '../../models/persona-natural.model';
import { TelefonoContacto } from '../../models/telefono-contacto.model';
import { EntidadService } from '../../services/entidad';


@Component({
  selector: 'app-entidad-form',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule], 

  templateUrl: './entidad-form.html',
  styleUrl: './entidad-form.css'
})
export class EntidadForm implements OnInit {
  public persona: PersonaNatural = {
    entidadID: 0,
    nombres: '',
    apellidoPaterno: '',
    fechaNacimiento: new Date(),
    edad: 0,
    tipoDocumento: 'DNI', 
    numeroDocumento: '',
    fechaCreacion: new Date(),
    telefonos: []
  };

  constructor(
    private entidadService: EntidadService,
    private router: Router
  ) { }

  ngOnInit(): void {

  }

  onSubmit(): void {
    console.log('Datos del formulario:', this.persona);

    this.entidadService.crearPersona(this.persona).subscribe({
      next: (personaCreada) => {
        console.log('¡Persona creada!', personaCreada);

        this.router.navigate(['/']); 
      },
      error: (err) => {
        console.error('Error al crear la persona:', err);
      }
    });
  }

  agregarTelefono(): void {
    this.persona.telefonos.push({
      telefonoID: 0,
      entidadID: this.persona.entidadID,
      numero: '' 
    });
  }

  eliminarTelefono(index: number): void {
    this.persona.telefonos.splice(index, 1);
  }

}