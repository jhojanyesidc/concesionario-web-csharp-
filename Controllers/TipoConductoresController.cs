using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ConcesionarioWeb.Data;
using ConcesionarioWeb.Models;

namespace ConcesionarioWeb.Controllers;

public class TipoConductoresController : Controller
{
    private readonly ConcesionarioContext _context;
    public TipoConductoresController(ConcesionarioContext context) => _context = context;

    public async Task<IActionResult> Index(string? q)
    {
        var query = _context.TipoConductores.AsQueryable();
        if (!string.IsNullOrWhiteSpace(q))
            query = query.Where(t => t.Nombre.Contains(q));
        var list = await query.OrderBy(t => t.Nombre).ToListAsync();
        ViewData["q"] = q;
        return View(list);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var tipoConductor = await _context.TipoConductores.FirstOrDefaultAsync(m => m.Id == id);
        if (tipoConductor == null) return NotFound();
        return View(tipoConductor);
    }

    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Nombre")] TipoConductor tipoConductor)
    {
        if (ModelState.IsValid)
        {
            _context.Add(tipoConductor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(tipoConductor);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var tipoConductor = await _context.TipoConductores.FindAsync(id);
        if (tipoConductor == null) return NotFound();
        return View(tipoConductor);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre")] TipoConductor tipoConductor)
    {
        if (id != tipoConductor.Id) return NotFound();
        if (!ModelState.IsValid) return View(tipoConductor);

        try
        {
            _context.Update(tipoConductor);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.TipoConductores.Any(e => e.Id == id)) return NotFound();
            else throw;
        }
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var tipoConductor = await _context.TipoConductores.FirstOrDefaultAsync(m => m.Id == id);
        if (tipoConductor == null) return NotFound();
        return View(tipoConductor);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var tipoConductor = await _context.TipoConductores.FindAsync(id);
        if (tipoConductor != null)
        {
            _context.TipoConductores.Remove(tipoConductor);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}