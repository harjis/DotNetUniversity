using System.Linq;
using System.Threading.Tasks;
using DotNetUniversity.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DotNetUniversity.Data;
using DotNetUniversity.Models;

namespace DotNetUniversity.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly SchoolContext _context;
        private readonly UnitOfWork _unitOfWork;

        public DepartmentsController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Departments
        public async Task<IActionResult> Index()
        {
            var departments = await _unitOfWork.DepartmentRepository.Get(includedProperties: "Administrator");
            return View(departments);
        }

        // GET: Departments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _unitOfWork.DepartmentRepository.GetByIdAdminIncluded((int) id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // GET: Departments/Create
        public IActionResult Create()
        {
            ViewData["InstructorId"] = new SelectList(_context.Instructors, "Id", "FirstMidName");
            return View();
        }

        // POST: Departments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DepartmentId,Name,Budget,StartDate,InstructorId,RowVersion")]
            Department department)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.DepartmentRepository.Add(department);
                await _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }

            ViewData["InstructorId"] =
                new SelectList(_context.Instructors, "Id", "FirstMidName", department.InstructorId);
            return View(department);
        }

        // GET: Departments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _unitOfWork.DepartmentRepository.GetById(id);
            if (department == null)
            {
                return NotFound();
            }

            ViewData["InstructorId"] =
                new SelectList(_context.Instructors, "Id", "FirstMidName", department.InstructorId);
            return View(department);
        }

        // POST: Departments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, byte[] rowVersion)
        {
            if (id == null)
            {
                return NotFound();
            }

            var departmentToUpdate = await _context.Departments.Include(i => i.Administrator)
                .FirstOrDefaultAsync(m => m.DepartmentId == id);

            if (departmentToUpdate == null)
            {
                var deletedDepartment = new Department();
                await TryUpdateModelAsync(deletedDepartment);
                ModelState.AddModelError(string.Empty,
                    "Unable to save changes. The department was deleted by another user.");
                ViewData["InstructorId"] =
                    new SelectList(_context.Instructors, "Id", "FullName", deletedDepartment.InstructorId);
                return View(deletedDepartment);
            }

            _context.Entry(departmentToUpdate).Property("RowVersion").OriginalValue = rowVersion;

            if (await TryUpdateModelAsync<Department>(
                departmentToUpdate,
                "",
                s => s.Name, s => s.StartDate, s => s.Budget, s => s.InstructorId))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var exceptionEntry = ex.Entries.Single();
                    var clientValues = (Department) exceptionEntry.Entity;
                    var databaseEntry = exceptionEntry.GetDatabaseValues();
                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError(string.Empty,
                            "Unable to save changes. The department was deleted by another user.");
                    }
                    else
                    {
                        var databaseValues = (Department) databaseEntry.ToObject();

                        if (databaseValues.Name != clientValues.Name)
                        {
                            ModelState.AddModelError("Name", $"Current value: {databaseValues.Name}");
                        }

                        if (databaseValues.Budget != clientValues.Budget)
                        {
                            ModelState.AddModelError("Budget", $"Current value: {databaseValues.Budget:c}");
                        }

                        if (databaseValues.StartDate != clientValues.StartDate)
                        {
                            ModelState.AddModelError("StartDate", $"Current value: {databaseValues.StartDate:d}");
                        }

                        if (databaseValues.InstructorId != clientValues.InstructorId)
                        {
                            var databaseInstructor =
                                await _context.Instructors.FirstOrDefaultAsync(i =>
                                    i.Id == databaseValues.InstructorId);
                            ModelState.AddModelError("InstructorId", $"Current value: {databaseInstructor?.FullName}");
                        }

                        ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                                                               + "was modified by another user after you got the original value. The "
                                                               + "edit operation was canceled and the current values in the database "
                                                               + "have been displayed. If you still want to edit this record, click "
                                                               + "the Save button again. Otherwise click the Back to List hyperlink.");
                        departmentToUpdate.RowVersion = (byte[]) databaseValues.RowVersion;
                        ModelState.Remove("RowVersion");
                    }
                }
            }

            ViewData["InstructorId"] =
                new SelectList(_context.Instructors, "Id", "FullName", departmentToUpdate.InstructorId);
            return View(departmentToUpdate);
        }

        // GET: Departments/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? concurrencyError)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _unitOfWork.DepartmentRepository.GetByIdAdminIncluded((int) id);
            if (department == null)
            {
                if (concurrencyError.GetValueOrDefault())
                {
                    return RedirectToAction(nameof(Index));
                }

                return NotFound();
            }

            if (concurrencyError.GetValueOrDefault())
            {
                ViewData["ConcurrencyErrorMessage"] = "The record you attempted to delete "
                                                      + "was modified by another user after you got the original values. "
                                                      + "The delete operation was canceled and the current values in the "
                                                      + "database have been displayed. If you still want to delete this "
                                                      + "record, click the Delete button again. Otherwise "
                                                      + "click the Back to List hyperlink.";
            }

            return View(department);
        }

        // POST: Departments/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Department department)
        {
            try
            {
                if (!await DepartmentExists(department.DepartmentId)) return RedirectToAction(nameof(Index));

                _unitOfWork.DepartmentRepository.Delete(department);
                await _unitOfWork.Save();

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction(nameof(Delete), new {concurrencyError = true, id = department.DepartmentId});
            }
        }

        private async Task<bool> DepartmentExists(int id)
        {
            return await _unitOfWork.DepartmentRepository.Exists(id);
        }
    }
}
