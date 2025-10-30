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

            var telefonosDt = new DataTable();
            telefonosDt.Columns.Add("Numero", typeof(string));
            foreach (var tel in persona.Telefonos)
            {
                telefonosDt.Rows.Add(tel.Numero);
            }

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

            await _dbConnection.ExecuteAsync(sql, param, commandType: CommandType.StoredProcedure);

            var nuevoId = param.Get<int>("@NuevoEntidadID");
            return nuevoId;
        }

        // --- READ ---
        public async Task<IEnumerable<PersonaNatural>> ListarPersonasNaturalesAsync()
        {
            var sql = "sp_ListarPersonasNaturales";

            using (var multipleResults = await _dbConnection.QueryMultipleAsync(sql, commandType: CommandType.StoredProcedure))
            {

                var personas = (await multipleResults.ReadAsync<PersonaNatural>()).ToList();

                var telefonos = (await multipleResults.ReadAsync<TelefonoContacto>()).ToList();

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
        public async Task ActualizarPersonaNaturalAsync(PersonaNatural persona)
        {
            var sql = "sp_ActualizarPersonaNatural";

            var telefonosDt = new DataTable();
            telefonosDt.Columns.Add("Numero", typeof(string));
            foreach (var tel in persona.Telefonos)
            {
                telefonosDt.Rows.Add(tel.Numero);
            }

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

            await _dbConnection.ExecuteAsync(sql, param, commandType: CommandType.StoredProcedure);
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

        // --- READ (Por ID) ---
        public async Task<PersonaNatural?> GetPersonaNaturalByIdAsync(int entidadId)
        {
            var sql = "sp_GetPersonaNaturalById";
            var param = new DynamicParameters();
            param.Add("@EntidadID", entidadId);

            using (var multipleResults = await _dbConnection.QueryMultipleAsync(sql, param, commandType: CommandType.StoredProcedure))
            {
                var persona = await multipleResults.ReadFirstOrDefaultAsync<PersonaNatural>();

                if (persona == null)
                {
                    return null; 
                }

                var telefonos = (await multipleResults.ReadAsync<TelefonoContacto>()).ToList();

                persona.Telefonos = telefonos;

                return persona;
            }
        }
    }
}