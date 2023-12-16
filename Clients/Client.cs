using System.ComponentModel.DataAnnotations;

namespace Clients;

public class Client
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Surname { get; set; }
    [EmailAddress]
    [Required]
    public string EmailAddress { get; set; }
}