using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ConcesionarioWeb.Data;
using ConcesionarioWeb.Models;
using Microsoft.Extensions.Logging;

namespace ConcesionarioWeb.Controllers;

public class ConductoresController : Controller
{
    private readonly ConcesionarioContext _context;
    private readonly ILogger<ConductoresController> _logger;
    
    public ConductoresController(ConcesionarioContext context, ILogger<ConductoresController> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IActionResult> Index(string? q)
    {
        var query = _context.Conductores.Include(c => c.TipoConductor).AsQueryable();
        if (!string.IsNullOrWhiteSpace(q))
            query = query.Where(c => c.Nombre.Contains(q) || c.TipoLicencia.Contains(q));
        var list = await query.OrderBy(c => c.Nombre).ToListAsync();
        ViewData["q"] = q;
        return View(list);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var conductor = await _context.Conductores
            .Include(c => c.TipoConductor)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (conductor == null) return NotFound();
        return View(conductor);
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.IdTipoConductor = new SelectList(await _context.TipoConductores.ToListAsync(), "Id", "Nombre");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Nombre,TipoLicencia,IdTipoConductor")] Conductor conductor)
    {
        if (ModelState.IsValid)
        {
            _context.Add(conductor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
        // Recargar el dropdown si hay errores de validación
        ViewBag.IdTipoConductor = new SelectList(await _context.TipoConductores.ToListAsync(), "Id", "Nombre", conductor.IdTipoConductor);
        return View(conductor);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) 
        {
            _logger.LogWarning("Edit llamado con id null");
            return NotFound();
        }
        
        var conductor = await _context.Conductores.FindAsync(id);
        if (conductor == null) 
        {
            _logger.LogWarning($"Conductor con id {id} no encontrado");
            return NotFound();
        }
        
        ViewBag.IdTipoConductor = new SelectList(await _context.TipoConductores.ToListAsync(), "Id", "Nombre", conductor.IdTipoConductor);
        _logger.LogInformation($"Editando conductor: Id={conductor.Id}, Nombre={conductor.Nombre}, TipoLicencia={conductor.TipoLicencia}, IdTipoConductor={conductor.IdTipoConductor}");
        return View(conductor);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,TipoLicencia,IdTipoConductor")] Conductor conductor)
    {
        _logger.LogInformation($"POST Edit llamado: id={id}, conductor.Id={conductor.Id}, Nombre={conductor.Nombre}, TipoLicencia={conductor.TipoLicencia}, IdTipoConductor={conductor.IdTipoConductor}");
        
        if (id != conductor.Id) 
        {
            _logger.LogWarning($"ID mismatch: parámetro id={id}, conductor.Id={conductor.Id}");
            return NotFound();
        }
        
        // Verificar ModelState
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("ModelState inválido:");
            foreach (var error in ModelState)
            {
                _logger.LogWarning($"Campo: {error.Key}, Errores: {string.Join(", ", error.Value.Errors.Select(e => e.ErrorMessage))}");
            }
            
            // Recargar el dropdown si hay errores
            ViewBag.IdTipoConductor = new SelectList(await _context.TipoConductores.ToListAsync(), "Id", "Nombre", conductor.IdTipoConductor);
            return View(conductor);
        }

        try
        {
            _logger.LogInformation($"Intentando actualizar conductor: {conductor.Id}");
            
            // Obtener el conductor existente de la base de datos
            var conductorExistente = await _context.Conductores.FindAsync(id);
            if (conductorExistente == null)
            {
                _logger.LogWarning($"Conductor existente con id {id} no encontrado");
                return NotFound();
            }

            // Actualizar las propiedades
            conductorExistente.Nombre = conductor.Nombre;
            conductorExistente.TipoLicencia = conductor.TipoLicencia;
            conductorExistente.IdTipoConductor = conductor.IdTipoConductor;

            _context.Update(conductorExistente);
            var cambiosGuardados = await _context.SaveChangesAsync();
            
            _logger.LogInformation($"Conductor actualizado exitosamente. Cambios guardados: {cambiosGuardados}");
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogError(ex, $"Error de concurrencia al actualizar conductor {id}");
            if (!_context.Conductores.Any(e => e.Id == id)) 
            {
                return NotFound();
            }
            else 
            {
                throw;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error inesperado al actualizar conductor {id}");
            
            // Recargar el dropdown y mostrar el error
            ViewBag.IdTipoConductor = new SelectList(await _context.TipoConductores.ToListAsync(), "Id", "Nombre", conductor.IdTipoConductor);
            ModelState.AddModelError("", "Ocurrió un error al guardar los cambios. Por favor, inténtelo de nuevo.");
            return View(conductor);
        }
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var conductor = await _context.Conductores
            .Include(c => c.TipoConductor)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (conductor == null) return NotFound();
        return View(conductor);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var conductor = await _context.Conductores.FindAsync(id);
        if (conductor != null)
        {
            _context.Conductores.Remove(conductor);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}