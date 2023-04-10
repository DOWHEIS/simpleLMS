using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Interfaces;

public interface ICourseRepository
{
    Task<List<Course>> GetCourses();
    Task<Course> GetCourse(int id);
    Task<int> AddCourse(Course course);
    Task<Course> UpdateCourse(Course course);
    Task<Course> DeleteCourse(int id);
}
public class CourseRepository : ICourseRepository
{
    private readonly LMSContext _context;
    
    public CourseRepository(LMSContext context)
    {
        _context = context;
    }
    
    public async Task<List<Course>> GetCourses()
    {
        return await _context.Courses.Include(c => c.Modules).ToListAsync();
    }
    
    public async Task<Course> GetCourse(int id)
    {
        return await _context.Courses.FindAsync(id);
    }
    
    public async Task<int> AddCourse(Course course)
    {
        _context.Courses.Add(course);
        _context.SaveChangesAsync();
        return course.Id;
    }
    
    public async Task<Course> UpdateCourse(Course course)
    {
        _context.Courses.Update(course);
        _context.SaveChangesAsync();
        return course;
    }
    
    public async Task<Course> DeleteCourse(int id)
    {
        var course = _context.Courses.FirstOrDefault(c => c.Id == id);
        _context.Courses.Remove(course);
        _context.SaveChangesAsync();
        return course;
    }
    
  
}