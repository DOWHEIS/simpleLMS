using Microsoft.AspNetCore.Mvc;
using WebApi.Interfaces;
using WebApi.Models;
using Microsoft.AspNetCore.Http;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ModulesController : ControllerBase
{
    private IModuleRepository _repository;
    public ModulesController(IModuleRepository repository)
    {
        _repository = repository;
    }
    
    [HttpGet]
    public async Task<ActionResult<List<Module>>> GetModules()
    {
        var module =  await _repository.GetModules();
        
        if(module.Count == 0)
        {
            return NotFound();
        }

        return Ok(module);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<Module>> GetModule(int id)
    {
        var module = await _repository.GetModule(id);
        
        if(module.Equals(null))
        {
            return NotFound();
        }

        return Ok(module);
    }
    
    [HttpPost]
    public async Task<ActionResult<Module>> AddModule(Module module)
    {
        var newModule = await _repository.AddModule(module);
        
        if(newModule.Equals(null))
        {
            return NotFound();
        }

        return Ok(newModule);
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult<Module>> UpdateModule(int id, Module module)
    {
        var updatedModule = await _repository.UpdateModule(module);
        
        if(updatedModule.Equals(null))
        {
            return NotFound();
        }

        return Ok(updatedModule);
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult<Module>> DeleteModule(int id)
    {
        var deletedModule = await _repository.DeleteModule(id);
        
        if(deletedModule.Equals(null))
        {
            return NotFound();
        }

        return Ok(deletedModule);
    }
}