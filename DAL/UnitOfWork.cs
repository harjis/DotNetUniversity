using System;
using System.Threading.Tasks;
using DotNetUniversity.Data;

namespace DotNetUniversity.DAL
{
    public sealed class UnitOfWork : IDisposable
    {
        private readonly SchoolContext _schoolContext;
        public readonly CourseRepository CourseRepository;
        public readonly DepartmentRepository DepartmentRepository;

        public UnitOfWork(SchoolContext schoolContext, CourseRepository courseRepository,
            DepartmentRepository departmentRepository)
        {
            _schoolContext = schoolContext;
            CourseRepository = courseRepository;
            DepartmentRepository = departmentRepository;
        }

        public async Task Save()
        {
            await _schoolContext.SaveChangesAsync();
        }

        private bool disposed = false;

        private void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _schoolContext.Dispose();
                }
            }

            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
