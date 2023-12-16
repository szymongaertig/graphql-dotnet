using System.ComponentModel.DataAnnotations;
using Host.Registrations;

namespace Host.Events;

public class Event
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string EventName { get; set; }
    public ICollection<Registration> Registrations { get; set; }
}