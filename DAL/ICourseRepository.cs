using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DotNetUniversity.Models;

namespace DotNetUniversity.DAL
{
    public interface ICourseRepository : IDisposable
    {
        Task<IEnumerable<Course>> All();
        Task<Course> Find(int id);
        Task<Course> ById(int id);
        Task Add(Course course);
        Task Delete(int id);
        Task Update(Course course);
        Task Save();
        Task<bool> Exists(int id);
    }
}
