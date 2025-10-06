using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ConcesionarioWeb.Data;
using ConcesionarioWeb.Models;

namespace ConcesionarioWeb.Controllers;

public class TipoVehiculosController : Controller
{
    private readonly ConcesionarioContext _context;
    public TipoVehiculosController(ConcesionarioContext context) => _context = context;

    public async Task<IActionResult> Index(string? q)
    {
        var query = _context.TipoVehiculos.AsQueryable();
        if (!string.IsNullOrWhiteSpace(q))
            query = query.Where(t => t.Nombre.Contains(q));
        var list = await query.OrderBy(t => t.Nombre).ToListAsync();
        ViewData["q"] = q;
        return View(list);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var tipoVehiculo = await _context.TipoVehiculos.FirstOrDefaultAsync(m => m.Id == id);
        if (tipoVehiculo == null) return NotFound();
        return View(tipoVehiculo);
    }

    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Nombre")] TipoVehiculo tipoVehiculo)
    {
        if (ModelState.IsValid)
        {
            _context.Add(tipoVehiculo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(tipoVehiculo);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var tipoVehiculo = await _context.TipoVehiculos.FindAsync(id);
        if (tipoVehiculo == null) return NotFound();
        return View(tipoVehiculo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre")] TipoVehiculo tipoVehiculo)
    {
        if (id != tipoVehiculo.Id) return NotFound();
        if (!ModelState.IsValid) return View(tipoVehiculo);

        try
        {
            _context.Update(tipoVehiculo);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.TipoVehiculos.Any(e => e.Id == id)) return NotFound();
            else throw;
        }
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var tipoVehiculo = await _context.TipoVehiculos.FirstOrDefaultAsync(m => m.Id == id);
        if (tipoVehiculo == null) return NotFound();
        return View(tipoVehiculo);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var tipoVehiculo = await _context.TipoVehiculos.FindAsync(id);
        if (tipoVehiculo != null)
        {
            _context.TipoVehiculos.Remove(tipoVehiculo);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}
