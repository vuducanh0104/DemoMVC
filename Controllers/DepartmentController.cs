using DemoMVC.Data;
using DemoMVC.Models;
using DemoMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DemoMVC.Controllers;

public class DepartmentController : Controller
{
    private readonly ApplicationDbcontext _context;

    public DepartmentController(ApplicationDbcontext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(
        string? searchString,
        int pageNumber = 1,
        int pageSize = PaginationOptions.DefaultPageSize)
    {
        var normalizedSearch = searchString?.Trim() ?? string.Empty;
        var query = _context.Departments.AsNoTracking();

        if (normalizedSearch.Length > 0)
        {
            var searchPattern = SearchPattern.CreateContainsPattern(normalizedSearch);
            query = query.Where(department =>
                department.DepartmentCode.Contains(normalizedSearch) ||
                EF.Functions.Like(
                    department.DepartmentCode,
                    searchPattern,
                    SearchPattern.EscapeCharacter) ||
                department.Name.Contains(normalizedSearch) ||
                EF.Functions.Like(
                    department.Name,
                    searchPattern,
                    SearchPattern.EscapeCharacter));
        }

        var departments = await PaginatedList<Department>.CreateAsync(
            query.OrderBy(department => department.Name)
                .ThenBy(department => department.DepartmentCode),
            pageNumber,
            pageSize,
            HttpContext.RequestAborted);

        return View(new PagedListViewModel<Department>
        {
            Items = departments.Items,
            Pagination = new PaginationViewModel
            {
                SearchString = normalizedSearch,
                PageNumber = departments.PageNumber,
                PageSize = departments.PageSize,
                TotalCount = departments.TotalCount,
                TotalPages = departments.TotalPages
            }
        });
    }

    public IActionResult Create()
    {
        return IsAjaxRequest()
            ? PartialView("_CreateModal", new Department())
            : View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("DepartmentCode,Name")] Department department)
    {
        if (await _context.Departments.AnyAsync(
                item => item.DepartmentCode == department.DepartmentCode))
        {
            ModelState.AddModelError(
                nameof(department.DepartmentCode),
                "Mã khoa này đã tồn tại.");
        }

        if (!ModelState.IsValid)
        {
            return IsAjaxRequest()
                ? PartialView("_CreateModal", department)
                : View(department);
        }

        _context.Departments.Add(department);
        await _context.SaveChangesAsync();
        TempData["Message"] = "Đã thêm khoa.";
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

        var department = await _context.Departments.FindAsync(id);
        if (department is null)
        {
            return NotFound();
        }

        return IsAjaxRequest()
            ? PartialView("_EditModal", department)
            : View(department);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        string id,
        [Bind("DepartmentCode,Name")] Department input)
    {
        if (id != input.DepartmentCode)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return IsAjaxRequest()
                ? PartialView("_EditModal", input)
                : View(input);
        }

        var department = await _context.Departments.FindAsync(id);
        if (department is null)
        {
            return NotFound();
        }

        department.Name = input.Name;
        await _context.SaveChangesAsync();
        TempData["Message"] = "Đã cập nhật thông tin khoa.";
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

        var department = await _context.Departments
            .AsNoTracking()
            .FirstOrDefaultAsync(item => item.DepartmentCode == id);
        if (department is null)
        {
            return NotFound();
        }

        ViewData["HasStudents"] = await _context.Student
            .AnyAsync(student => student.DepartmentCode == id);
        return IsAjaxRequest()
            ? PartialView("_DeleteModal", department)
            : View(department);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        var department = await _context.Departments.FindAsync(id);
        if (department is null)
        {
            return NotFound();
        }

        if (await _context.Student.AnyAsync(student => student.DepartmentCode == id))
        {
            if (IsAjaxRequest())
            {
                return Conflict(new { success = false, message = "Không thể xóa khoa đang có sinh viên." });
            }

            TempData["Error"] = "Không thể xóa khoa đang có sinh viên.";
            return RedirectToAction(nameof(Index));
        }

        _context.Departments.Remove(department);
        await _context.SaveChangesAsync();
        TempData["Message"] = "Đã xóa khoa.";
        if (IsAjaxRequest())
        {
            return Json(new { success = true });
        }

        return RedirectToAction(nameof(Index));
    }

    private bool IsAjaxRequest() =>
        string.Equals(Request.Headers["X-Requested-With"], "XMLHttpRequest", StringComparison.Ordinal);
}
