using System.ComponentModel.DataAnnotations;

namespace Host.Registrations;

public class Registration
{
    [Key]
    public Guid Id { get; set; }
    [Required]
    public DateTime CreationDate { get; set; }
    [Required]
    public Guid ClientId { get; set; }
    [Required]
    public RegistrationStatus Status { get; set; }
}