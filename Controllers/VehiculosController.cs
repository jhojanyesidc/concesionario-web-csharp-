using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ConcesionarioWeb.Data;
using ConcesionarioWeb.Models;
using Microsoft.Extensions.Logging;

namespace ConcesionarioWeb.Controllers;

public class VehiculosController : Controller
{
    private readonly ConcesionarioContext _context;
    private readonly ILogger<VehiculosController> _logger;
    
    public VehiculosController(ConcesionarioContext context, ILogger<VehiculosController> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IActionResult> Index(string? q)
    {
        var query = _context.Vehiculos.Include(v => v.TipoVehiculo).AsQueryable();
        if (!string.IsNullOrWhiteSpace(q))
            query = query.Where(v => v.Marca.Contains(q) || v.Modelo.Contains(q) || v.Matricula.Contains(q));
        var list = await query.OrderBy(v => v.Marca).ThenBy(v => v.Modelo).ToListAsync();
        ViewData["q"] = q;
        return View(list);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var vehiculo = await _context.Vehiculos
            .Include(v => v.TipoVehiculo)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (vehiculo == null) return NotFound();
        return View(vehiculo);
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.IdTipoVehiculo = new SelectList(await _context.TipoVehiculos.ToListAsync(), "Id", "Nombre");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Marca,Modelo,Matricula,Año,IdTipoVehiculo")] Vehiculo vehiculo)
    {
        if (ModelState.IsValid)
        {
            _context.Add(vehiculo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
        // Recargar el dropdown si hay errores de validación
        ViewBag.IdTipoVehiculo = new SelectList(await _context.TipoVehiculos.ToListAsync(), "Id", "Nombre", vehiculo.IdTipoVehiculo);
        return View(vehiculo);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) 
        {
            _logger.LogWarning("Edit llamado con id null");
            return NotFound();
        }
        
        var vehiculo = await _context.Vehiculos.FindAsync(id);
        if (vehiculo == null) 
        {
            _logger.LogWarning($"Vehículo con id {id} no encontrado");
            return NotFound();
        }
        
        ViewBag.IdTipoVehiculo = new SelectList(await _context.TipoVehiculos.ToListAsync(), "Id", "Nombre", vehiculo.IdTipoVehiculo);
        _logger.LogInformation($"Editando vehículo: Id={vehiculo.Id}, Marca={vehiculo.Marca}, Modelo={vehiculo.Modelo}");
        return View(vehiculo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Marca,Modelo,Matricula,Año,IdTipoVehiculo")] Vehiculo vehiculo)
    {
        _logger.LogInformation($"POST Edit llamado: id={id}, vehiculo.Id={vehiculo.Id}");
        
        if (id != vehiculo.Id) 
        {
            _logger.LogWarning($"ID mismatch: parámetro id={id}, vehiculo.Id={vehiculo.Id}");
            return NotFound();
        }
        
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("ModelState inválido:");
            foreach (var error in ModelState)
            {
                _logger.LogWarning($"Campo: {error.Key}, Errores: {string.Join(", ", error.Value.Errors.Select(e => e.ErrorMessage))}");
            }
            
            ViewBag.IdTipoVehiculo = new SelectList(await _context.TipoVehiculos.ToListAsync(), "Id", "Nombre", vehiculo.IdTipoVehiculo);
            return View(vehiculo);
        }

        try
        {
            _logger.LogInformation($"Intentando actualizar vehículo: {vehiculo.Id}");
            
            // Obtener el vehículo existente de la base de datos
            var vehiculoExistente = await _context.Vehiculos.FindAsync(id);
            if (vehiculoExistente == null)
            {
                _logger.LogWarning($"Vehículo existente con id {id} no encontrado");
                return NotFound();
            }

            // Actualizar las propiedades
            vehiculoExistente.Marca = vehiculo.Marca;
            vehiculoExistente.Modelo = vehiculo.Modelo;
            vehiculoExistente.Matricula = vehiculo.Matricula;
            vehiculoExistente.Año = vehiculo.Año;
            vehiculoExistente.IdTipoVehiculo = vehiculo.IdTipoVehiculo;

            _context.Update(vehiculoExistente);
            var cambiosGuardados = await _context.SaveChangesAsync();
            
            _logger.LogInformation($"Vehículo actualizado exitosamente. Cambios guardados: {cambiosGuardados}");
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogError(ex, $"Error de concurrencia al actualizar vehículo {id}");
            if (!_context.Vehiculos.Any(e => e.Id == id)) 
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
            _logger.LogError(ex, $"Error inesperado al actualizar vehículo {id}");
            
            ViewBag.IdTipoVehiculo = new SelectList(await _context.TipoVehiculos.ToListAsync(), "Id", "Nombre", vehiculo.IdTipoVehiculo);
            ModelState.AddModelError("", "Ocurrió un error al guardar los cambios. Por favor, inténtelo de nuevo.");
            return View(vehiculo);
        }
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var vehiculo = await _context.Vehiculos
            .Include(v => v.TipoVehiculo)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (vehiculo == null) return NotFound();
        return View(vehiculo);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var vehiculo = await _context.Vehiculos.FindAsync(id);
        if (vehiculo != null)
        {
            _context.Vehiculos.Remove(vehiculo);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}