using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DotNetUniversity.Data;
using DotNetUniversity.Models;
using Microsoft.EntityFrameworkCore;

namespace DotNetUniversity.DAL
{
    public class CourseRepository : ICourseRepository, IDisposable
    {
        private SchoolContext _schoolContext;

        public CourseRepository(SchoolContext schoolContext)
        {
            _schoolContext = schoolContext;
        }

        public async Task<IEnumerable<Course>> All()
        {
            return await _schoolContext.Courses
                .Include(c => c.Department)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Course> Find(int id)
        {
            return await _schoolContext.Courses.FindAsync(id);
        }

        public async Task<Course> ById(int id)
        {
            return await _schoolContext.Courses
                .Include(c => c.Department)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.CourseId == id);
        }

        public async Task Add(Course course)
        {
            await _schoolContext.Courses.AddAsync(course);
        }

        public async Task Delete(int id)
        {
            var course = await _schoolContext.Courses.FindAsync(id);
            _schoolContext.Courses.Remove(course);
        }

        public async Task Update(Course course)
        {
            _schoolContext.Entry(course).State = EntityState.Modified;
        }

        public async Task Save()
        {
            await _schoolContext.SaveChangesAsync();
        }

        public async Task<bool> Exists(int id)
        {
            return await _schoolContext.Courses.AnyAsync(c => c.CourseId == id);
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

            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
