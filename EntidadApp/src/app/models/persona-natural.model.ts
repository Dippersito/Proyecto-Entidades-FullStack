import { TelefonoContacto } from './telefono-contacto.model';

export interface PersonaNatural {
  entidadID: number;
  nombres: string;
  apellidoPaterno: string;
  apellidoMaterno?: string;
  fechaNacimiento: Date;
  edad: number;
  tipoDocumento: string;
  numeroDocumento: string;
  sexo?: string;
  fechaCreacion: Date;

  telefonos: TelefonoContacto[]; 
}