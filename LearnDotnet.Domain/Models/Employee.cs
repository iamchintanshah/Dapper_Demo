using System.ComponentModel.DataAnnotations;

namespace LearnDapper.Domain.Model;

public class Employee
{
    public int Id { get; set; } = 0;
    [Required]
    public string? Name { get; set; }
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = default!;
    public string Phone { get; set; } = string.Empty;
    public string Designation { get; set; } = "Software Developer";
    [Range(18, 35)]
    public int Age { get; set; }
}
