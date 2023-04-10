using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Interfaces;

public interface IModuleRepository
{
    Task<List<Module>> GetModules();
    Task<Module> GetModule(int id);
    Task<int> AddModule(Module module);
    Task<Module> UpdateModule(Module module);
    Task<Module> DeleteModule(int id);
}
public class ModuleRepository : IModuleRepository
{
    private readonly LMSContext _context;
    
    public ModuleRepository(LMSContext context)
    {
        _context = context;
    }
    
    public async Task<List<Module>> GetModules()
    {
        return await _context.Modules.Include(c => c.Assignments).ToListAsync();
    }
    
    public async Task<Module> GetModule(int id)
    {
        return await _context.Modules.FindAsync(id);
    }
    
    public async Task<int> AddModule(Module module)
    {
        _context.Modules.Add(module);
        _context.SaveChangesAsync();
        return module.Id;
    }
    
    public async Task<Module> UpdateModule(Module module)
    {
        _context.Modules.Update(module);
        _context.SaveChangesAsync();
        return module;
    }
    
    public async Task<Module> DeleteModule(int id)
    {
        var module = _context.Modules.FirstOrDefault(m => m.Id == id);
        _context.Modules.Remove(module);
        _context.SaveChangesAsync();
        return module;
    }
    

}