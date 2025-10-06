using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConcesionarioWeb.Models;

[Table("vehiculo", Schema = "dbo")]
public class Vehiculo
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required, StringLength(255)]
    [Column("marca")]
    [Display(Name = "Marca")]
    public string Marca { get; set; } = string.Empty;

    [Required, StringLength(255)]
    [Column("modelo")]
    [Display(Name = "Modelo")]
    public string Modelo { get; set; } = string.Empty;

    [Required, StringLength(255)]
    [Column("matricula")]
    [Display(Name = "Matrícula")]
    public string Matricula { get; set; } = string.Empty;

    [Required]
    [Column("año")]
    [Display(Name = "Año")]
    public int Año { get; set; }

    [Required]
    [Column("id_tipo_vehiculo")]
    [Display(Name = "Tipo de Vehículo")]
    public int IdTipoVehiculo { get; set; }

    // Relación con TipoVehiculo - NO requerida para validación
    [ForeignKey("IdTipoVehiculo")]
    public virtual TipoVehiculo? TipoVehiculo { get; set; }
}