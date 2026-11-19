using DemoMVC.Data;
using DemoMVC.Models;
using DemoMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DemoMVC.Controllers;

public class StudentController : Controller
{
    private readonly ApplicationDbcontext _context;

    public StudentController(ApplicationDbcontext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(
        string? searchString,
        int pageNumber = 1,
        int pageSize = PaginationOptions.DefaultPageSize)
    {
        var normalizedSearch = searchString?.Trim() ?? string.Empty;
        var query = _context.Student.AsNoTracking();

        if (normalizedSearch.Length > 0)
        {
            var searchPattern = SearchPattern.CreateContainsPattern(normalizedSearch);
            query = query.Where(student =>
                student.StudentCode.Contains(normalizedSearch) ||
                EF.Functions.Like(student.StudentCode, searchPattern, SearchPattern.EscapeCharacter) ||
                student.FullName.Contains(normalizedSearch) ||
                EF.Functions.Like(student.FullName, searchPattern, SearchPattern.EscapeCharacter) ||
                student.Email.Contains(normalizedSearch) ||
                EF.Functions.Like(student.Email, searchPattern, SearchPattern.EscapeCharacter) ||
                (student.Address != null && student.Address.Contains(normalizedSearch)) ||
                (student.Address != null &&
                 EF.Functions.Like(student.Address, searchPattern, SearchPattern.EscapeCharacter)) ||
                student.AdmissionYear.ToString().Contains(normalizedSearch) ||
                EF.Functions.Like(
                    student.AdmissionYear.ToString(),
                    searchPattern,
                    SearchPattern.EscapeCharacter) ||
                student.TrainingSystemCode.Contains(normalizedSearch) ||
                EF.Functions.Like(student.TrainingSystemCode, searchPattern, SearchPattern.EscapeCharacter) ||
                student.MajorCode.Contains(normalizedSearch) ||
                EF.Functions.Like(student.MajorCode, searchPattern, SearchPattern.EscapeCharacter) ||
                student.DepartmentCode.Contains(normalizedSearch) ||
                EF.Functions.Like(student.DepartmentCode, searchPattern, SearchPattern.EscapeCharacter) ||
                (student.Department != null &&
                 (student.Department.Name.Contains(normalizedSearch) ||
                  EF.Functions.Like(
                      student.Department.Name,
                      searchPattern,
                      SearchPattern.EscapeCharacter))));
        }

        var students = await PaginatedList<Student>.CreateAsync(
            query.Include(student => student.Department)
                .OrderBy(student => student.StudentCode),
            pageNumber,
            pageSize,
            HttpContext.RequestAborted);

        return View(new PagedListViewModel<Student>
        {
            Items = students.Items,
            Pagination = new PaginationViewModel
            {
                SearchString = normalizedSearch,
                PageNumber = students.PageNumber,
                PageSize = students.PageSize,
                TotalCount = students.TotalCount,
                TotalPages = students.TotalPages
            }
        });
    }

    public async Task<IActionResult> Create()
    {
        await PopulateDepartmentsAsync();
        var student = new Student { AdmissionYear = DateTime.Now.Year };
        return IsAjaxRequest()
            ? PartialView("_CreateModal", student)
            : View(student);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("FullName,Address,Email,AdmissionYear,TrainingSystemCode,MajorCode,DepartmentCode")]
        Student student)
    {
        ModelState.Remove(nameof(student.StudentCode));

        if (!await _context.Departments.AnyAsync(
                department => department.DepartmentCode == student.DepartmentCode))
        {
            ModelState.AddModelError(
                nameof(student.DepartmentCode),
                "Vui lòng chọn một khoa hợp lệ.");
        }

        if (!ModelState.IsValid)
        {
            await PopulateDepartmentsAsync();
            return IsAjaxRequest()
                ? PartialView("_CreateModal", student)
                : View(student);
        }

        var prefix = $"{student.AdmissionYear % 100:00}" +
                     $"{student.TrainingSystemCode}{student.MajorCode}";
        var existingCodes = await _context.Student
            .Where(item => item.StudentCode.StartsWith(prefix))
            .Select(item => item.StudentCode)
            .ToListAsync();
        var lastSequence = existingCodes
            .Where(code => code.Length == prefix.Length + 4)
            .Select(code => int.TryParse(code.AsSpan(prefix.Length), out var sequence)
                ? sequence
                : 0)
            .DefaultIfEmpty(0)
            .Max();

        if (lastSequence >= 9999)
        {
            ModelState.AddModelError(
                string.Empty,
                $"Mã sinh viên với tiền tố {prefix} đã vượt quá 4 chữ số thứ tự.");
            await PopulateDepartmentsAsync();
            return IsAjaxRequest()
                ? PartialView("_CreateModal", student)
                : View(student);
        }

        student.StudentCode = $"{prefix}{lastSequence + 1:0000}";
        _context.Student.Add(student);
        await _context.SaveChangesAsync();

        TempData["Message"] = $"Đã thêm sinh viên với mã {student.StudentCode}.";
        if (IsAjaxRequest())
        {
            return Json(new { success = true });
        }

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(string? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var student = await _context.Student.FindAsync(id);
        if (student is null)
        {
            return NotFound();
        }

        await PopulateDepartmentsAsync();
        return IsAjaxRequest()
            ? PartialView("_EditModal", student)
            : View(student);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        string id,
        [Bind("StudentCode,FullName,Address,Email,AdmissionYear,TrainingSystemCode,MajorCode,DepartmentCode")]
        Student input)
    {
        if (id != input.StudentCode)
        {
            return NotFound();
        }

        var student = await _context.Student.FindAsync(id);
        if (student is null)
        {
            return NotFound();
        }

        if (!await _context.Departments.AnyAsync(
                department => department.DepartmentCode == input.DepartmentCode))
        {
            ModelState.AddModelError(
                nameof(input.DepartmentCode),
                "Vui lòng chọn một khoa hợp lệ.");
        }

        if (!ModelState.IsValid)
        {
            await PopulateDepartmentsAsync();
            return IsAjaxRequest()
                ? PartialView("_EditModal", input)
                : View(input);
        }

        student.FullName = input.FullName;
        student.Address = input.Address;
        student.Email = input.Email;
        student.DepartmentCode = input.DepartmentCode;
        await _context.SaveChangesAsync();

        TempData["Message"] = "Đã cập nhật thông tin sinh viên.";
        if (IsAjaxRequest())
        {
            return Json(new { success = true });
        }

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(string? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var student = await _context.Student
            .AsNoTracking()
            .Include(item => item.Department)
            .FirstOrDefaultAsync(item => item.StudentCode == id);
        if (student is null)
        {
            return NotFound();
        }

        return IsAjaxRequest()
            ? PartialView("_DeleteModal", student)
            : View(student);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        var student = await _context.Student.FindAsync(id);
        if (student is null)
        {
            return NotFound();
        }

        _context.Student.Remove(student);
        await _context.SaveChangesAsync();
        TempData["Message"] = "Đã xóa sinh viên.";
        if (IsAjaxRequest())
        {
            return Json(new { success = true });
        }

        return RedirectToAction(nameof(Index));
    }

    private async Task PopulateDepartmentsAsync()
    {
        ViewData["DepartmentCode"] = new SelectList(
            await _context.Departments
                .AsNoTracking()
                .OrderBy(department => department.Name)
                .ToListAsync(),
            nameof(Department.DepartmentCode),
            nameof(Department.Name));
    }

    private bool IsAjaxRequest() =>
        string.Equals(Request.Headers["X-Requested-With"], "XMLHttpRequest", StringComparison.Ordinal);
}
