using APBD_KOL_1.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace APBD_KOL_1.Services;

public interface IVisitService
{
    public Task<IActionResult> getInfoForVisitById(int visitId);
    
    public Task <VisitWhileSentDTO> addNewVisitFromBody(VisitWhileSentDTO visit);

    public Task<bool> checkIfVisitExist(int visitId);
    
    public Task<bool> checkIfClientForNewVisitExist(int clientId);
    
    public Task<bool> checkIfMechanicForNewVisitExist(string mechanicLicenceNumber);
    
    public Task<bool> checkIfServiceForNewVisitExist(string serviceName);
}