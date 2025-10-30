import { Component, OnInit } from '@angular/core'; 
import { CommonModule } from '@angular/common'; 
import { RouterModule } from '@angular/router';
import { EntidadService } from '../../services/entidad';
import { PersonaNatural } from '../../models/persona-natural.model';

@Component({
  selector: 'app-entidad-list',
  standalone: true,

  imports: [CommonModule, RouterModule],

  templateUrl: './entidad-list.html', 
  styleUrl: './entidad-list.css'     
})
export class EntidadList implements OnInit { 

  public personas: PersonaNatural[] = [];
  public error: string | null = null;

  constructor(private entidadService: EntidadService) { }

  ngOnInit(): void {
    this.cargarPersonas();
  }

  cargarPersonas(): void {
    this.error = null; 

    this.entidadService.getListarPersonas().subscribe({

      next: (data) => {
        this.personas = data;
      },

      error: (err) => {
        console.error('Error al cargar personas:', err);
        this.error = 'No se pudo cargar la lista de personas. Asegúrate de que la API (C#) esté corriendo y el puerto sea correcto.';
      }
    });
  }
}