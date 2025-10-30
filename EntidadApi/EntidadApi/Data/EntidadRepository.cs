using Dapper;              
using EntidadApi.Models;   
using System.Data;           
using System.Collections.Generic;
using System.Linq;   
using System.Threading.Tasks;

namespace EntidadApi.Data
{
    public class EntidadRepository : IEntidadRepository
    {
        private readonly IDbConnection _dbConnection;

        public EntidadRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        // --- CREATE ---
        public async Task<int> CrearPersonaNaturalAsync(PersonaNatural persona)
        {
            var sql = "sp_CrearPersonaNatural";

            // 1. Mapear la lista de teléfonos a un DataTable
            var telefonosDt = new DataTable();
            telefonosDt.Columns.Add("Numero", typeof(string));
            foreach (var tel in persona.Telefonos)
            {
                telefonosDt.Rows.Add(tel.Numero);
            }

            // 2. Configurar los parámetros
            var param = new DynamicParameters();
            param.Add("@Nombres", persona.Nombres);
            param.Add("@ApellidoPaterno", persona.ApellidoPaterno);
            param.Add("@ApellidoMaterno", persona.ApellidoMaterno);
            param.Add("@FechaNacimiento", persona.FechaNacimiento);
            param.Add("@TipoDocumento", persona.TipoDocumento);
            param.Add("@NumeroDocumento", persona.NumeroDocumento);
            param.Add("@Sexo", persona.Sexo);
            param.Add("@Telefonos", telefonosDt.AsTableValuedParameter("dbo.TipoListaTelefono"));
            param.Add("@NuevoEntidadID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            // 3. Ejecutar
            await _dbConnection.ExecuteAsync(sql, param, commandType: CommandType.StoredProcedure);

            // 4. Obtener el ID de salida
            var nuevoId = param.Get<int>("@NuevoEntidadID");
            return nuevoId;
        }

        // --- READ ---
        public async Task<IEnumerable<PersonaNatural>> ListarPersonasNaturalesAsync()
        {
            var sql = "sp_ListarPersonasNaturales";

            using (var multipleResults = await _dbConnection.QueryMultipleAsync(sql, commandType: CommandType.StoredProcedure))
            {
                // 1. Leer Personas
                var personas = (await multipleResults.ReadAsync<PersonaNatural>()).ToList();

                // 2. Leer Telefonos
                var telefonos = (await multipleResults.ReadAsync<TelefonoContacto>()).ToList();

                // 3. Unir los datos en C#
                foreach (var persona in personas)
                {
                    persona.Telefonos = telefonos
                                          .Where(t => t.EntidadID == persona.EntidadID)
                                          .ToList();
                }

                return personas;
            }
        }

        // --- UPDATE ---
        public async Task<bool> ActualizarPersonaNaturalAsync(PersonaNatural persona)
        {
            var sql = "sp_ActualizarPersonaNatural";

            // 1. Mapear la lista de teléfonos a un DataTable
            var telefonosDt = new DataTable();
            telefonosDt.Columns.Add("Numero", typeof(string));
            foreach (var tel in persona.Telefonos)
            {
                telefonosDt.Rows.Add(tel.Numero);
            }

            // 2. Configurar los parámetros
            var param = new DynamicParameters();
            param.Add("@EntidadID", persona.EntidadID);
            param.Add("@Nombres", persona.Nombres);
            param.Add("@ApellidoPaterno", persona.ApellidoPaterno);
            param.Add("@ApellidoMaterno", persona.ApellidoMaterno);
            param.Add("@FechaNacimiento", persona.FechaNacimiento);
            param.Add("@TipoDocumento", persona.TipoDocumento);
            param.Add("@NumeroDocumento", persona.NumeroDocumento);
            param.Add("@Sexo", persona.Sexo);
            param.Add("@Telefonos", telefonosDt.AsTableValuedParameter("dbo.TipoListaTelefono"));

            // 3. Ejecutar
            var filasAfectadas = await _dbConnection.ExecuteAsync(sql, param, commandType: CommandType.StoredProcedure);

            // Devolver true si se actualizó algo
            return filasAfectadas > 0;
        }

        // --- DELETE ---
        public async Task<bool> EliminarEntidadAsync(int entidadId)
        {
            var sql = "sp_EliminarEntidad";
            var param = new DynamicParameters();
            param.Add("@EntidadID", entidadId);

            var filasAfectadas = await _dbConnection.ExecuteAsync(sql, param, commandType: CommandType.StoredProcedure);
            return filasAfectadas > 0;
        }
    }
}