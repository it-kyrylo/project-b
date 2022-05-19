namespace ProjectB.Services;

public class CosmosDbService<T> : ICosmosDbService<T> where T:UserInformation
{
    private ICosmosDbRepository<T> _cosmosDbRepository;
    public CosmosDbService(ICosmosDbRepository<T> cosmosDbRepository)
    {
        _cosmosDbRepository = cosmosDbRepository;
    }
    public async Task AddAsync(T userInformation)
    {
        await _cosmosDbRepository.AddAsync(userInformation, userInformation.Id);
    }
    public async Task<T> GetAsync(string chatId)
    {
        return await _cosmosDbRepository.GetAsync(chatId);
    }
    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _cosmosDbRepository.GetAllAsync();
    }
    public async Task UpdateAsync(string chatId, T userInformation)
    {
        await _cosmosDbRepository.UpdateAsync(chatId, userInformation);
    }
    public async Task DeleteAsync(string chatId)
    {
        await _cosmosDbRepository.DeleteAsync(chatId);
    }
    public async Task AddToHistoryAsync(T userInformation, string searchToAdd)
    {
        var searchId = userInformation.Id;
        var oldSearchesToEdit = await GetAsync(searchId);

        if (oldSearchesToEdit != null)
        {
            if (oldSearchesToEdit.SearchHistory == null)
            {
                oldSearchesToEdit.SearchHistory = new List<string>();
            }
            oldSearchesToEdit.SearchHistory.Add(searchToAdd);
            await UpdateAsync(searchId, oldSearchesToEdit);
            return;
        }
        await _cosmosDbRepository.AddAsync(userInformation, userInformation.Id);
    }
    public async Task AddHotelIdAsync(T userInformation)
    {
        var userId = userInformation.Id;
        var userInformationToEdit = await GetAsync(userId);

        if (userInformationToEdit != null)
        {
            string hotelIdToAdd = userInformation.HotelId;
            string newHotelId = hotelIdToAdd;
            userInformationToEdit.HotelId = newHotelId;
            await UpdateAsync(userId, userInformationToEdit);
            return;
        }
        await _cosmosDbRepository.AddAsync(userInformation, userInformation.Id);
    }
    public async Task AddCheckInDateAsync(T userInformation)
    {
        var userId = userInformation.Id;
        var userInformationToEdit = await GetAsync(userId);

        if (userInformationToEdit != null)
        {
            string CheckInDateToAdd = userInformation.CheckInDate;
            string newCheckInDate = CheckInDateToAdd;
            userInformationToEdit.CheckInDate = newCheckInDate;
            await UpdateAsync(userId, userInformationToEdit);
            return;
        }
        await _cosmosDbRepository.AddAsync(userInformation, userInformation.Id);
    }
    public async Task AddCheckOutDateAsync(T userInformation)
    {
        var userId = userInformation.Id;
        var userInformationToEdit = await GetAsync(userId);

        if (userInformationToEdit != null)
        {
            string CheckOutDateToAdd = userInformation.CheckOutDate;
            string newCheckOutDate = CheckOutDateToAdd;
            userInformationToEdit.CheckOutDate = newCheckOutDate;
            await UpdateAsync(userId, userInformationToEdit);
            return;
        }
        await _cosmosDbRepository.AddAsync(userInformation, userInformation.Id);
    }
    public async Task<IEnumerable<string>> GetHistoryAsync(string chatId)
    {
        if (await CheckIfExists(chatId))
        {
            var chat = await GetAsync(chatId);
            List<string> searchHistory = chat.SearchHistory;
            return searchHistory;
        }
        return null;
    }
    public async Task<string> GetHotelIdByChatIdAsync(string chatId)
    {
        if (await CheckIfExists(chatId))
        {
            var chatInfo = await GetAsync(chatId);
            var hotelId = chatInfo.HotelId;
            return hotelId;
        }
        return null;
    }
    public async Task<string> GetCheckInDateByChatIdAsync(string chatId)
    {
        if (await CheckIfExists(chatId))
        {
            var chatInfo = await GetAsync(chatId);
            var checkInDate = chatInfo.CheckInDate;
            return checkInDate;
        }
        return null;
    }
    public async Task<string> GetCheckOutDateByChatIdAsync(string chatId)
    {
        if (await CheckIfExists(chatId))
        {
            var chatInfo = await GetAsync(chatId);
            var checkOutDate = chatInfo.CheckOutDate;
            return checkOutDate;
        }
        return null;
    }

    public async Task<bool> CheckIfExists(string chatId)
    {
        var userToCheck = await GetAsync(chatId);
        if (userToCheck == null)
        {
            return false;
        }
        return true;
    }
}
