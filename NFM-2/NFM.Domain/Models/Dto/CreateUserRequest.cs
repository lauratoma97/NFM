
using System.ComponentModel.DataAnnotations;

namespace NFM.Domain.Models.Dto;

public class CreateUserRequest
{
    [Required]
    [DataType(DataType.EmailAddress)]
    public string Username { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}