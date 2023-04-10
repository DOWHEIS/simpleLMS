using Microsoft.AspNetCore.Mvc;
using WebApi.Interfaces;
using WebApi.Models;
using Microsoft.AspNetCore.Http;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CoursesController : ControllerBase
{
    private ICourseRepository _repository;
    public CoursesController(ICourseRepository repository)
    {
        _repository = repository;
    }
    
    [HttpGet]
    public async Task<ActionResult<List<Course>>> GetCourses()
    {
        var course =  await _repository.GetCourses();
        
        if(course.Count == 0)
        {
            return NotFound();
        }

        return Ok(course);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<Course>> GetCourse(int id)
    {
        var course = await _repository.GetCourse(id);
        
        if(course.Equals(null))
        {
            return NotFound();
        }

        return Ok(course);
    }
    
    [HttpPost]
    public async Task<ActionResult<Course>> AddCourse(Course course)
    {
        var newCourse = await _repository.AddCourse(course);
        
        if(newCourse.Equals(null))
        {
            return NotFound();
        }

        return Ok(newCourse);
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult<Course>> UpdateCourse(int id, Course course)
    {
        var updatedCourse = await _repository.UpdateCourse(course);
        
        if(updatedCourse.Equals(null))
        {
            return NotFound();
        }

        return Ok(updatedCourse);
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult<Course>> DeleteCourse(int id)
    {
        var course = await _repository.DeleteCourse(id);
        
        if(course.Equals(null))
        {
            return NotFound();
        }

        return Ok(course);
    }
}