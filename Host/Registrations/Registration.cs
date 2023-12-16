using System.ComponentModel.DataAnnotations;
using Host.Events;

namespace Host.Registrations;

public class Registration
{
    [Key]
    public int Id { get; set; }
    [Required]
    public DateTime CreationDate { get; set; }
    [Required]
    public int ClientId { get; set; }
    [Required]
    public RegistrationStatus Status { get; set; }
    public Event Event { get; set; }
}