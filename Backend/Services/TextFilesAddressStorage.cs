using System.Text.Json;

public interface IAddressStorage
{
    Task AddAddressToPerson(Guid personId, string newAddress);
    Task<string[]> GetAddressesFromPerson(Guid personId);
    Task RemoveAddressFromPerson(Guid personId, string address);
}

public class TextFilesAddressStorage : IAddressStorage
{
    private readonly IConfiguration _configuration;
    private readonly string _addressStoragePath;

    public TextFilesAddressStorage(IConfiguration configuration)
    {
        _configuration = configuration;
        _addressStoragePath = configuration["FilePaths:AddressStoragePath"];
    }

    public async Task AddAddressToPerson(Guid personId, string newAddress)
    {
        var deserializedData = JsonSerializer.Deserialize<Dictionary<Guid, List<string>>>(await File.ReadAllTextAsync(_addressStoragePath)) ?? ([]);
        if (deserializedData[personId] is null)
            deserializedData[personId] = [newAddress];
        else
            deserializedData[personId].Add(newAddress);
        await File.WriteAllTextAsync(_addressStoragePath, JsonSerializer.Serialize(deserializedData));
    }

    public async Task<string[]> GetAddressesFromPerson(Guid personId)
    {
        var deserializedData = JsonSerializer.Deserialize<Dictionary<Guid, List<string>>>(await File.ReadAllTextAsync(_addressStoragePath)) ?? ([]);
        return [.. deserializedData[personId]];
    }

    public async Task RemoveAddressFromPerson(Guid personId, string address)
    {
        var deserializedData = JsonSerializer.Deserialize<Dictionary<Guid, List<string>>>(await File.ReadAllTextAsync(_addressStoragePath)) ?? ([]);
        deserializedData[personId].Remove(address);
    }

}