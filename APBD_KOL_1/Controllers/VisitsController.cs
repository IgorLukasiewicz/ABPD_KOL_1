using APBD_KOL_1.Models.DTOs;
using APBD_KOL_1.Services;
using Microsoft.AspNetCore.Mvc;

namespace APBD_KOL_1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VisitsController : ControllerBase
{
    private readonly IVisitService _service;
    
    
    public VisitsController(IVisitService service)
    {
        _service = service;
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> getInfoForVisitById(int id)
    {
        return await _service.getInfoForVisitById(id);
    }
    
    [HttpPost]
    public async Task<IActionResult> AddClient([FromBody] VisitWhileSentDTO visit)
    {
        var newClient = await _service.addNewVisitFromBody(visit);

        return Created(visit.VisitId.ToString(), visit);

    }
    
}