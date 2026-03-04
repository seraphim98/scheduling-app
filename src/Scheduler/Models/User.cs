using System;
using Microsoft.EntityFrameworkCore;

namespace Scheduler.Models;

[Index(nameof(NormalisedUserName), IsUnique = true)]
public class User(
    Guid id,
    string username,
    string email,
    string password,
    string firstName,
    string lastName,
    string salt)
{
    public Guid Id { get; set; } = id;
    public string Username { get; set; } = username;

    public string NormalisedUserName { get; set; } = username.ToUpperInvariant();

    public string Email { get; set; } = email;

    public string FirstName { get; set; } = firstName;
    public string LastName { get; set; } = lastName;

    public string Password { get; set; } = password;

    public string Salt { get; set; } = salt;
}
