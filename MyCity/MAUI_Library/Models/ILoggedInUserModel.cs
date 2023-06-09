
namespace MAUI_Library.Models;

public interface ILoggedInUserModel
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    void ResetUserModel();
}
