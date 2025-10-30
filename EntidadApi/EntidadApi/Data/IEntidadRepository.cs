using EntidadApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EntidadApi.Data
{
    public interface IEntidadRepository
    {
        // CREATE
        Task<int> CrearPersonaNaturalAsync(PersonaNatural persona);

        // READ
        Task<IEnumerable<PersonaNatural>> ListarPersonasNaturalesAsync();

        // UPDATE
        Task<bool> ActualizarPersonaNaturalAsync(PersonaNatural persona);

        // DELETE
        Task<bool> EliminarEntidadAsync(int entidadId);
    }
}