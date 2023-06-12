using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace MAUI_Library.Models.OutgoingDto;

public class RegisterRequestDto
{

    public required string FirstName { get; set; } = default!;
    public required string LastName { get; set; } = default!;
    public required string DisplayName { get; set; } = default!;
    public required DateTime DateOfBirth { get; set; }
    public required string Email { get; set; } = default!;
    public required string Password { get; set; } = default!;
}

public static class RegisterRequestvalidator
{
    private static readonly Regex EmailRegex = new Regex(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$");
    private static readonly Regex PasswordRegex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$");
    private static readonly Regex NameRegex = new Regex(@"^[a-zA-Z]+$");

    public static bool ValidateRequest(this RegisterRequestDto request, out Dictionary<string, string> errors)
    {
        errors = new Dictionary<string, string>();

        if (!ValidateEmail(request.Email))
            errors.Add("Email", "Invalid email format.");

        if (!ValidatePassword(request.Password))
            errors.Add("Password", "Password must contain at least 8 characters, including uppercase, lowercase, numeric, and special characters.");

        if (!ValidateName(request.FirstName))
            errors.Add("FirstName", "First name can only contain letters.");

        if (!ValidateName(request.LastName))
            errors.Add("LastName", "Last name can only contain letters.");
        return errors.Count == 0;
    }

    private static bool ValidateEmail(string email)
    {
        return !string.IsNullOrWhiteSpace(email) && EmailRegex.IsMatch(email);
    }

    private static bool ValidatePassword(string password)
    {
        return !string.IsNullOrWhiteSpace(password) && PasswordRegex.IsMatch(password);
    }

    private static bool ValidateName(string name)
    {
        return !string.IsNullOrWhiteSpace(name) && NameRegex.IsMatch(name);
    }
}
