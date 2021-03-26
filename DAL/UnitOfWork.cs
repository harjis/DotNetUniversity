using System;
using System.Threading.Tasks;
using DotNetUniversity.Data;

namespace DotNetUniversity.DAL
{
    public class UnitOfWork : IDisposable
    {
        private SchoolContext _schoolContext;
        private GenericRepository<CourseRepository> _courseRepository;

        public GenericRepository<CourseRepository> CourseRepository
        {
            get
            {
                if (_courseRepository == null)
                {
                    _courseRepository = new GenericRepository<CourseRepository>(_schoolContext);
                }

                return _courseRepository;
            }
        }

        public async Task Save()
        {
            await _schoolContext.SaveChangesAsync();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
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
