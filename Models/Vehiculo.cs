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
    [Display(Name = "Matr�cula")]
    public string Matricula { get; set; } = string.Empty;

    [Required]
    [Column("a�o")]
    [Display(Name = "A�o")]
    public int A�o { get; set; }

    [Required]
    [Column("id_tipo_vehiculo")]
    [Display(Name = "Tipo de Veh�culo")]
    public int IdTipoVehiculo { get; set; }

    // Relaci�n con TipoVehiculo - NO requerida para validaci�n
    [ForeignKey("IdTipoVehiculo")]
    public virtual TipoVehiculo? TipoVehiculo { get; set; }
}