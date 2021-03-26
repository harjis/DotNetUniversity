using System.Threading.Tasks;
using DotNetUniversity.Data;
using DotNetUniversity.Models;
using Microsoft.EntityFrameworkCore;

namespace DotNetUniversity.DAL
{
    public class CourseRepository : GenericRepository<Course>
    {
        public CourseRepository(SchoolContext schoolContext) : base(schoolContext)
        {
        }

        public async Task<bool> Exists(int id)
        {
            return await _schoolContext.Courses.AnyAsync(c => c.CourseId == id);
        }
    }
}
