using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFM.Domain.Models;

public class MyApplicationUser : IdentityUser
{
    // TODO: Add your custom properties here
    public string? FullName { get; set; }
    public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
}