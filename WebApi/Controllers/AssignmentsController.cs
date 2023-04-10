using Microsoft.AspNetCore.Mvc;
using WebApi.Interfaces;
using WebApi.Models;
namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AssignmentsController : ControllerBase
{
    private IAssignmentRepository _repository;
    public AssignmentsController(IAssignmentRepository repository)
    {
        _repository = repository;
    }
    
    [HttpGet]
    public async Task<ActionResult<List<Assignment>>> GetAssignments()
    {
        var assignment =  await _repository.GetAssignments();
        
        if(assignment.Count == 0)
        {
            return NotFound();
        }

        return Ok(assignment);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<Assignment>> GetAssignment(int id)
    {
        var assignment = await _repository.GetAssignment(id);
        
        if(assignment.Equals(null))
        {
            return NotFound();
        }

        return Ok(assignment);
    }
    
    [HttpPost]
    public async Task<ActionResult<Assignment>> AddAssignment(Assignment assignment)
    {
        var newAssignment = await _repository.AddAssignment(assignment);
        
        if(newAssignment.Equals(null))
        {
            return NotFound();
        }

        return Ok(newAssignment);
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult<Assignment>> UpdateAssignment(int id, Assignment assignment)
    {
        var updatedAssignment = await _repository.UpdateAssignment(assignment);
        
        if(updatedAssignment.Equals(null))
        {
            return NotFound();
        }

        return Ok(updatedAssignment);
    }
    

}