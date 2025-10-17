using PimApi.Models;

namespace PimApi.Services
{
    public interface IChatAssignmentService
    {
        Task<int?> AssignChatToTechnician(int chatId);
        Task<List<Usuario>> GetAvailableTechnicians();
    }
}
