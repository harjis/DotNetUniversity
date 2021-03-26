using System;
using System.Linq;
using System.Threading.Tasks;
using DotNetUniversity.DAL;
using DotNetUniversity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DotNetUniversity.Models;

namespace DotNetUniversity.Controllers
{
    public class CoursesController : Controller
    {
        private readonly SchoolContext _context;
        private readonly UnitOfWork _unitOfWork;

        public CoursesController(SchoolContext schoolContext, UnitOfWork unitOfWork)
        {
            _context = schoolContext;
            _unitOfWork = unitOfWork;
        }

        // GET: Courses
        public async Task<IActionResult> Index()
        {
            var courses = _unitOfWork.CourseRepository.Get();
            return View(courses);
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = _unitOfWork.CourseRepository.GetById(id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // GET: Courses/Create
        public IActionResult Create()
        {
            PopulateDepartmentsDropDownList();
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseId,Title,Credits,DepartmentId")]
            Course course)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.CourseRepository.Add(course);
                await _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }

            PopulateDepartmentsDropDownList(course.DepartmentId);

            return View(course);
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = _unitOfWork.CourseRepository.GetById(id);
            if (course == null)
            {
                return NotFound();
            }

            PopulateDepartmentsDropDownList(course.DepartmentId);

            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CourseId,Title,Credits,DepartmentId")]
            Course course)
        {
            if (id != course.CourseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.CourseRepository.Update(course);
                    await _unitOfWork.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await CourseExists(course.CourseId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            PopulateDepartmentsDropDownList(course.DepartmentId);

            return View(course);
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = _unitOfWork.CourseRepository.GetById(id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _unitOfWork.CourseRepository.Delete(id);
            await _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> CourseExists(int id)
        {
            return await _unitOfWork.CourseRepository.Exists(id);
        }

        private void PopulateDepartmentsDropDownList(object selectedDepartment = null)
        {
            var departmentsQuery = from department in _context.Departments
                orderby department.Name
                select department;

            ViewBag.DepartmentId =
                new SelectList(departmentsQuery.AsNoTracking(), "DepartmentId", "Name", selectedDepartment);
        }
    }
}
