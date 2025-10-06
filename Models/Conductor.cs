using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConcesionarioWeb.Models;

[Table("conductor", Schema = "dbo")]
public class Conductor
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required, StringLength(255)]
    [Column("nombre")]
    [Display(Name = "Nombre")]
    public string Nombre { get; set; } = string.Empty;

    [Required, StringLength(255)]
    [Column("tipo_licencia")]
    [Display(Name = "Tipo de Licencia")]
    public string TipoLicencia { get; set; } = string.Empty;

    [Required]
    [Column("id_tipo_conductor")]
    [Display(Name = "Tipo de Conductor")]
    public int IdTipoConductor { get; set; }

    // Relación con TipoConductor - NO requerida para validación
    [ForeignKey("IdTipoConductor")]
    public virtual TipoConductor? TipoConductor { get; set; }
}