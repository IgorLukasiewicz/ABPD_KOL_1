namespace APBD_KOL_1.Models.DTOs;

public class VisitWhileSentDTO
{
    public int VisitId { get; set; }
    
    public int ClientId { get; set; }
    
    public string MechanicLicenceNumber { get; set; }
    
    public List<ServiceWhileSentDTO>? Services { get; set; }
    
}

public class ServiceWhileSentDTO
{
    public string ServiceName { get; set; }
    public decimal ServicePrice { get; set; }
}