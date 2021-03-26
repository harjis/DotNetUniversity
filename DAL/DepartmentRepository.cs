using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetUniversity.Data;
using DotNetUniversity.Models;

namespace DotNetUniversity.DAL
{
    public class DepartmentRepository : GenericRepository<Department>
    {
        public DepartmentRepository(SchoolContext schoolContext) : base(schoolContext)
        {
        }

        public async Task<IEnumerable<Department>> GetOrderByName()
        {
            return await Get(orderBy: q => q.OrderBy(d => d.Name));
        }
    }
}
