using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetUniversity.Data;
using DotNetUniversity.Models;
using Microsoft.EntityFrameworkCore;

namespace DotNetUniversity.DAL
{
    public class StudentRepository : GenericRepository<Student>
    {
        public StudentRepository(SchoolContext schoolContext) : base(schoolContext)
        {
        }

        public async Task<PaginatedList<Student>> GetPaginatedStudents(int? pageNumber, string searchString,
            string sortOrder)
        {
            const int pageSize = 3;

            var students = Students();
            students = ByName(students, searchString);
            students = OrderBy(students, sortOrder).AsNoTracking();
            return await PaginatedList<Student>.CreateAsync(students, pageNumber ?? 1, pageSize);
        }

        public async Task<Student> GetStudent(int id)
        {
            var students = Students();
            return await students.Include(s => s.Enrollments)
                .ThenInclude(e => e.Course)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        private IQueryable<Student> Students()
        {
            return from s in _schoolContext.Students select s;
        }

        private IQueryable<Student> ByName(IQueryable<Student> queryable, string searchString)
        {
            if (string.IsNullOrEmpty(searchString))
            {
                return queryable;
            }

            return queryable.Where(s => s.LastName.Contains(searchString)
                                        || s.FirstMidName.Contains(searchString));
        }

        private IQueryable<Student> OrderBy(IQueryable<Student> queryable, string sortOrder)
        {
            if (string.IsNullOrEmpty(sortOrder))
            {
                sortOrder = "LastName";
            }

            if (sortOrder.EndsWith("_desc"))
            {
                sortOrder = sortOrder.Substring(0, sortOrder.Length - 5);
                return queryable.OrderByDescending(e => EF.Property<object>(e, sortOrder));
            }

            return queryable.OrderBy(e => EF.Property<object>(e, sortOrder));
        }
    }
}
