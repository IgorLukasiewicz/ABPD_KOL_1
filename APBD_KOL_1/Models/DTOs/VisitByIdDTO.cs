namespace APBD_KOL_1.Models.DTOs;

public class VisitByIdDTO
{
    public DateTime Date { get; set; }
    public ClientInVisitByIdDTO Client {get; set;}
    public MechanicInVisitByIdDTO Mechanic {get; set;}
    public List<ServiceInVisitByIdDTO> Services {get; set;}
}

public class ClientInVisitByIdDTO
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    
    public List<ServiceInVisitByIdDTO> Services {get; set;}
}

public class MechanicInVisitByIdDTO
{
    public int MechanicId { get; set; }
    public string LicenceNumber { get; set; }
}

public class ServiceInVisitByIdDTO
{
    public string ServiceName { get; set; }
    public decimal ServiceFee { get; set; }
}