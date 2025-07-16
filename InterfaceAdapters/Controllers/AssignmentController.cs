using Application.DTO.Assignment;
using Application.DTO.AssignmentTemp;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InterfaceAdapters.Controllers;

[Route("api/assignments")]
[ApiController]
public class AssignmentController : ControllerBase
{
    private readonly IAssignmentService _assignmentService;
    private readonly IAssignmentTempService _assignmentTempService;

    public AssignmentController(IAssignmentService assignmentService, IAssignmentTempService assignmentTempService)
    {
        _assignmentService = assignmentService;
        _assignmentTempService = assignmentTempService;
    }

    [HttpPost]
    public async Task<ActionResult<CreatedAssignmentDTO>> Create([FromBody] CreateAssignmentDTO createAssignmentDTO)
    {
        var assignmentCreated = await _assignmentService.Create(createAssignmentDTO);
        return assignmentCreated.ToActionResult();
    }

    [HttpPut]
    public async Task<ActionResult<UpdatedAssignmentDTO>> UpdateAssignment([FromBody] UpdateAssignmentDTO updateAssignmentDTO)
    {
        var assignmentUpdated = await _assignmentService.Update(updateAssignmentDTO);
        return assignmentUpdated.ToActionResult();
    }

    [HttpPost("with-device")]
    public async Task<ActionResult<AssignmentTempDTO>> CreateWithDevice([FromBody] CreateAssignmentAndDeviceDTO createAssignmentAndDeviceDTO)
    {
        await _assignmentTempService.StartSagaAsync(createAssignmentAndDeviceDTO);
        return Accepted();
    }
}