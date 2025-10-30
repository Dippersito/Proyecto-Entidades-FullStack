import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common'; 
import { FormsModule } from '@angular/forms';
import { Router, RouterModule, ActivatedRoute } from '@angular/router';
import { PersonaNatural } from '../../models/persona-natural.model';
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
    apellidoMaterno: '', 
    fechaNacimiento: new Date(),
    edad: 0,
    tipoDocumento: 'DNI',
    numeroDocumento: '',
    sexo: undefined,
    fechaCreacion: new Date(),
    telefonos: []
  };

  public esModoEdicion: boolean = false;
  private entidadIdParaEditar: number = 0;

  constructor(
    private entidadService: EntidadService,
    private router: Router,
    private route: ActivatedRoute
  ) { }

  ngOnInit(): void {
    const idString = this.route.snapshot.paramMap.get('id');

    if (idString) {
      this.esModoEdicion = true;
      this.entidadIdParaEditar = +idString;

      this.entidadService.getPersonaById(this.entidadIdParaEditar).subscribe({
        next: (data) => {
          this.persona = data; 
        },
        error: (err) => {
          console.error('Error al cargar persona para editar:', err);
          alert('No se pudo cargar la persona. Volviendo a la lista.');
          this.router.navigate(['/']);
        }
      });
    }
  }

  onSubmit(): void {
    if (this.esModoEdicion) {
      this.entidadService.actualizarPersona(this.entidadIdParaEditar, this.persona).subscribe({
        next: () => {
          alert('¡Persona actualizada con éxito!');
          this.router.navigate(['/']);
        },
        error: (err) => {
          console.error('Error al actualizar:', err);
          alert('Hubo un error al actualizar la persona.');
        }
      });
    } else {
      this.entidadService.crearPersona(this.persona).subscribe({
        next: (personaCreada) => {
          console.log('¡Persona creada!', personaCreada);
          alert('¡Persona creada con éxito!');
          this.router.navigate(['/']);
        },
        error: (err) => {
          console.error('Error al crear la persona:', err);
          alert('Hubo un error al crear la persona.');
        }
      });
    }
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
