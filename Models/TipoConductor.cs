using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConcesionarioWeb.Models;

[Table("tipo_conductor", Schema = "dbo")]
public class TipoConductor
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required, StringLength(255)]
    [Column("nombre")]
    public string Nombre { get; set; } = string.Empty;
}