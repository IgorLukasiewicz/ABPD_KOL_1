using APBD_KOL_1.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace APBD_KOL_1.Services;

public class VisitService : IVisitService
{
    private readonly string _connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=APBD_KOL_1;Integrated Security=True;";


    public async Task<IActionResult> getInfoForVisitById(int visitId)
    {
        var toBeReturned = new Dictionary<int, VisitByIdDTO>();

        string query =
            @"SELECT V.visit_id, V.date, C.first_name, C.last_name, C.date_of_birth, M.mechanic_id, M.licence_number, S.name, S.base_fee
                         FROM Visit V
                         JOIN Client C ON C.client_id = V.client_id
                         JOIN Mechanic M on M.mechanic_id = V.mechanic_id
                         JOIN VISIT_SERVICE VSE ON VSE.visit_id = V.visit_id
                         JOIN SERVICE S ON S.service_id = VSE.service_id
                         WHERE V.visit_id = @VisitId";
        
        
        await using var conn = new SqlConnection(_connectionString);
        await using var cmd = new SqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@patientId", visitId);
        
        await conn.OpenAsync();
        using SqlDataReader reader = await cmd.ExecuteReaderAsync();



        while (await reader.ReadAsync())
        {
            int visistId = reader.GetInt32(reader.GetOrdinal("visit_id"));

            if (!toBeReturned.ContainsKey(visistId))
            {
                toBeReturned[visistId] = new VisitByIdDTO
                {
                    Date = reader.GetDateTime(reader.GetOrdinal("date")),
                    Client = new ClientInVisitByIdDTO()
                    {
                        FirstName = reader.GetString(reader.GetOrdinal("first_name")),
                        LastName = reader.GetString(reader.GetOrdinal("last_name")),
                        DateOfBirth = reader.GetDateTime(reader.GetOrdinal("date_of_birth")),
                    },

                    Mechanic = new MechanicInVisitByIdDTO()
                    {
                        MechanicId = reader.GetInt32(reader.GetOrdinal("mechanic_id")),
                        LicenceNumber = reader.GetString(reader.GetOrdinal("licence_number")),
                    },

                    Services = new List<ServiceInVisitByIdDTO>()

                };
                
                var newService = new ServiceInVisitByIdDTO
                {
                    ServiceName = reader.GetString(reader.GetOrdinal("name")),
                    ServiceFee = reader.GetDecimal(reader.GetOrdinal("base_fee")),
                };
                
                var current = toBeReturned[visistId];
                if (!current.Services.Any(a =>
                        a.ServiceName == newService.ServiceName
                        && a.ServiceFee == newService.ServiceFee))
                {
                    current.Services.Add(newService);
                }
                
            }
            
        }
        return new OkObjectResult(toBeReturned.Values.ToList());
    }


    public async Task<VisitWhileSentDTO> addNewVisitFromBody(VisitWhileSentDTO visit)
    {
        DateTime newDate = DateTime.Now;
        
        string query = @"
        INSERT INTO Visit (visit_id, client_id, mechanic_id,date)
        VALUES (@VisitId, @ClientId,SELECT(MECHANIC_ID FROM MECHANIC WHERE LICENCE_NUMBER = @MechanicLicenceNumber), newDate)";
        
        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand(query, conn);
        
        cmd.Parameters.AddWithValue("@VisitId", visit.VisitId);
        cmd.Parameters.AddWithValue("@ClientId", visit.ClientId);
        cmd.Parameters.AddWithValue("@Telephone", visit.MechanicLicenceNumber);
        
        
        await conn.OpenAsync();
        return visit;
    }
    
    public async Task<bool> checkIfVisitExist(int visitId)
    {
        string query = @"SELECT COUNT(1) FROM Visit WHERE visit_id = @visitId";

        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@visitId", visitId);

        await conn.OpenAsync();
        var result = (int)await cmd.ExecuteScalarAsync(); 
        
        return result > 0;
    }
    
    public async Task<bool> checkIfClientForNewVisitExist(int clientId)
    {
        string query = @"SELECT COUNT(1) FROM Client WHERE client_id = @clientId";

        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@clientId", clientId);

        await conn.OpenAsync();
        var result = (int)await cmd.ExecuteScalarAsync(); 
        
        return result > 0;
    }
    
    public async Task<bool> checkIfMechanicForNewVisitExist(string mechanicLicenceNumber)
    {
        string query = @"SELECT COUNT(1) FROM Mechanic WHERE licence_number = @mechanicLicenceNumber";

        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@mechanicLicenceNumber", mechanicLicenceNumber);

        await conn.OpenAsync();
        var result = (int)await cmd.ExecuteScalarAsync(); 
        
        return result > 0;
    }
    public async Task<bool> checkIfServiceForNewVisitExist(string serviceName)
    {
        string query = @"SELECT COUNT(1) FROM Service WHERE name = @serviceName";

        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@serviceName", serviceName);

        await conn.OpenAsync();
        var result = (int)await cmd.ExecuteScalarAsync(); 
        
        return result > 0;
    }
}