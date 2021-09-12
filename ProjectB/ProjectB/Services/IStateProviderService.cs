using ProjectB.Models.States;
using System.Threading.Tasks;

namespace ProjectB.Services
{
    public interface IStateProviderService
    {
        Task AddChatStateAsync(ChatState state);

        Task DeletechatStateAsync(long chatId);

        Task<ChatState> GetChatStateAsync(long chatId);

        Task UpdateChatStateAsync(ChatState istate);
    }
}
