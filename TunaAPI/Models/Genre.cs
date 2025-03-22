using System.ComponentModel.DataAnnotations;

namespace TunaAPI.Models;

public class Genre
{
    public int Id { get; set; }
    [Required]
    public string Description { get; set; }
    
}
