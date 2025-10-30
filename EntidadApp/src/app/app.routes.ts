import { Routes } from '@angular/router';
import { EntidadList } from './components/entidad-list/entidad-list';
import { EntidadForm } from './components/entidad-form/entidad-form';

export const routes: Routes = [
    {
        path: '',
        component: EntidadList 
    },
    {
        path: 'personas/nuevo',
        component: EntidadForm 
    },
    {
        path: 'personas/editar/:id', 
        component: EntidadForm 
    }
];