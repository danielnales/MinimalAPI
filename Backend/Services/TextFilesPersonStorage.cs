using System.Text.Json;

public interface IPersonStorage
{
  Task Create(Person person);
  Task Delete(Guid personId);
  Task<Person?> Find(Guid personId);
  Task<List<Person>> FindMany(Guid[] personIds);
  Task Update(Guid personId, Person person);
}

public class TextFilesPersonStorage : IPersonStorage
{
    public async Task Create(Person person) => 
        await File.WriteAllTextAsync($"Database/Addresses/{person.Id}.json", JsonSerializer.Serialize(person));
    
    public async Task Delete(Guid personId) =>
        await Task.Run(() => File.Delete($"Database/Addresses/{personId}.json"));

    public async Task<Person?> Find(Guid personId)
    {
        var path = $"Database/Addresses/{personId}.json";
        if (File.Exists(path))
            return JsonSerializer.Deserialize<Person>(await File.ReadAllTextAsync(path));
        return null;
    }

    public async Task<List<Person>> FindMany(Guid[] personIds)
    {
        var people = new List<Person>();
        foreach (Guid personId in personIds)
        {
            var path = $"Database/Addresses/{personId}.json";
            if (File.Exists(path))
            {
                var person = JsonSerializer.Deserialize<Person>(await File.ReadAllTextAsync(path));
                if (person is not null)
                    people.Add(person);
            }
        }

        return people;
    }

    public async Task Update(Guid personId, Person person)
        => await File.WriteAllTextAsync($"Database/Addresses/{person.Id}.json", JsonSerializer.Serialize(person with { Id = personId }));
}