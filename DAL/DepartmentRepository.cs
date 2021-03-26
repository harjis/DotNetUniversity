using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetUniversity.Data;
using DotNetUniversity.Models;
using Microsoft.EntityFrameworkCore;

namespace DotNetUniversity.DAL
{
    public class DepartmentRepository : GenericRepository<Department>
    {
        public DepartmentRepository(SchoolContext schoolContext) : base(schoolContext)
        {
        }

        public async Task<bool> Exists(int id)
        {
            return await _schoolContext.Departments.AnyAsync(d => d.DepartmentId == id);
        }

        public async Task<IEnumerable<Department>> GetOrderByName()
        {
            return await Get(orderBy: q => q.OrderBy(d => d.Name));
        }

        public async Task<Department> GetByIdAdminIncluded(int id)
        {
            return await _schoolContext.Departments
                .Include(d => d.Administrator)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.DepartmentId == id);
        }
    }
}
