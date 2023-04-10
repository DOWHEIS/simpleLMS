using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Interfaces;

public interface IAssignmentRepository
{
    Task<List<Assignment>> GetAssignments();
    Task<Assignment> GetAssignment(int id);
    Task<int> AddAssignment(Assignment assignment);
    Task<Assignment> UpdateAssignment(Assignment assignment);
    Task<Assignment> DeleteAssignment(int id);
}
public class AssignmentRepository : IAssignmentRepository
{
    private readonly LMSContext _context;
    
    public AssignmentRepository(LMSContext context)
    {
        _context = context;
    }
    
    public async Task<List<Assignment>> GetAssignments()
    {
        return await _context.Assignments.ToListAsync();
    }
    
    public async Task<Assignment> GetAssignment(int id)
    {
        return await _context.Assignments.FindAsync(id);
    }
    
    public async Task<int> AddAssignment(Assignment assignment)
    {
        _context.Assignments.Add(assignment);
        _context.SaveChangesAsync();
        return assignment.Id;
    }
    
    public async Task<Assignment> UpdateAssignment(Assignment assignment)
    {
        _context.Assignments.Update(assignment);
        _context.SaveChangesAsync();
        return assignment;
    }
    
    public async Task<Assignment> DeleteAssignment(int id)
    {
        var assignment = _context.Assignments.FirstOrDefault(a => a.Id == id);
        _context.Assignments.Remove(assignment);
        _context.SaveChangesAsync();
        return assignment;
    }
    
}