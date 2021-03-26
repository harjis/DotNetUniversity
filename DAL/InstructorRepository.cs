using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetUniversity.Data;
using DotNetUniversity.Models;
using Microsoft.EntityFrameworkCore;

namespace DotNetUniversity.DAL
{
    public class InstructorRepository : GenericRepository<Instructor>
    {
        public InstructorRepository(SchoolContext schoolContext) : base(schoolContext)
        {
        }

        public async Task<IEnumerable<Instructor>> GetIncluded()
        {
            return await _schoolContext.Instructors
                .Include(i => i.OfficeAssignment)
                .Include(i => i.CourseAssignments)
                .ThenInclude(i => i.Course)
                .ThenInclude(i => i.Department)
                .OrderBy(i => i.LastName)
                .ToListAsync();
        }

        public async Task<Instructor> GetByIdIncluded(int id)
        {
            return await _schoolContext.Instructors
                .Include(i => i.OfficeAssignment)
                .Include(i => i.CourseAssignments)
                .ThenInclude(i => i.Course)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}
