using MAUI_Library.Models.Incoming;

namespace MAUI_Library.API.Interfaces;

public interface IAdminEndpoint
{
    Task<IEnumerable<BasicEventModel>> GetAllEvents(TimeSpan timeSpan);
    Task<IEnumerable<RoleModel>> GetAllRoles();
    Task<IEnumerable<RoleModel>> GetAllRequiredRoles();
    Task<(bool, string)> SubmitRequestAsync(string roleId);
}
