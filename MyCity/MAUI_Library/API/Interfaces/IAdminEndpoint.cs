using MAUI_Library.Models.Incoming;
using MAUI_Library.Models.OutgoingDto;

namespace MAUI_Library.API.Interfaces;

public interface IAdminEndpoint
{
    Task<IEnumerable<BasicEventModel>> GetAllEvents(TimeSpan timeSpan);
    Task<IEnumerable<BasicEventModel>> GetAllEmergencies(TimeSpan timeSpan);
    Task<IEnumerable<RoleModel>> GetAllRoles();
    Task<IEnumerable<RoleModel>> GetAllRequiredRoles();
    Task<(bool, string)> SubmitRequestAsync(string roleId);
    Task<bool> RespondToRoleRequest(RoleRequestRespondDto response);
    Task<(bool, IEnumerable<RoleRequestModel>)> GetAllRoleRequests();
    Task<bool> RespondToEmergencyEvent(string eventId);
}
