using System.ComponentModel.DataAnnotations;

namespace TunaAPI.Models;

public class Song
{
    public int Id { get; set; }
    [Required]
    public string Title { get; set; }
    public int ArtistId { get; set; }
    public string Album { get; set; }
    public int Length { get; set; }
    
}
