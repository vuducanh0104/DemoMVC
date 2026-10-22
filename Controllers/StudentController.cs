using DemoMVC.Data;
using DemoMVC.Models;
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

    public async Task<IActionResult> Index()
    {
        var students = await _context.Student
            .AsNoTracking()
            .Include(student => student.Department)
            .OrderBy(student => student.StudentCode)
            .ToListAsync();
        return View(students);
    }

    public async Task<IActionResult> Create()
    {
        await PopulateDepartmentsAsync();
        return View(new Student { AdmissionYear = DateTime.Now.Year });
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
            return View(student);
        }

        var prefix = $"{student.AdmissionYear % 100:00}" +
                     $"{student.TrainingSystemCode}{student.MajorCode}";
        var existingCodes = await _context.Student
            .Where(item => item.StudentCode.StartsWith(prefix))
            .Select(item => item.StudentCode)
            .ToListAsync();
        var lastSequence = existingCodes
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
            return View(student);
        }

        student.StudentCode = $"{prefix}{lastSequence + 1:0000}";
        _context.Student.Add(student);
        await _context.SaveChangesAsync();

        TempData["Message"] = $"Đã thêm sinh viên với mã {student.StudentCode}.";
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
        return View(student);
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
            return View(input);
        }

        student.FullName = input.FullName;
        student.Address = input.Address;
        student.Email = input.Email;
        student.DepartmentCode = input.DepartmentCode;
        await _context.SaveChangesAsync();

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
        return student is null ? NotFound() : View(student);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        var student = await _context.Student.FindAsync(id);
        if (student is not null)
        {
            _context.Student.Remove(student);
            await _context.SaveChangesAsync();
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
}
