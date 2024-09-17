using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

[Route("api/person")]
public class PersonController : Controller
{
    readonly IPersonStorage personStorage;
    readonly IAddressStorage addressStorage;

    public PersonController(IPersonStorage personStorage, IAddressStorage addressStorage)
    {
        this.personStorage = personStorage;
        this.addressStorage = addressStorage;
    }

    [HttpPost()]
    public async Task<IActionResult> CreatePerson([FromBody] Person person) 
    {
        await personStorage.Create(person with { Id = Guid.NewGuid() });
        return Ok();
    }

    [HttpGet()]
    public async Task<IActionResult> GetPerson([FromQuery] Guid personId)
    {
        var person = await personStorage.Find(personId);
        return Ok(person);
    }

    [HttpGet("batch")]
    public async Task<IActionResult> GetPeople([FromQuery] Guid[] personIds) {
        var people = await personStorage.FindMany(personIds);
        return Ok(people);
    }

    [HttpPut()]
    public async Task<IActionResult> UpdatePerson([FromQuery] Guid personId, [FromBody] Person person)
    {
        await personStorage.Update(personId, person);
        return Ok();
    }

    [HttpPost("address")]
    public async Task<IActionResult> CreateAddress([FromBody] Guid personId, [FromBody] string newAddress)
    {
        await addressStorage.AddAddressToPerson(personId, newAddress);
        return Ok();
    }

    [HttpGet("address")]
    public async Task<IActionResult> GetAddresses([FromQuery] Guid personId)
    {
        var addresses = await addressStorage.GetAddressesFromPerson(personId);
        return Ok(addresses);
    }

    [HttpDelete("addresss")]
    public async Task<IActionResult> RemoveAddress([FromQuery] Guid personId, [FromQuery] string address)
    {
        await addressStorage.RemoveAddressFromPerson(personId, address);
        return Ok();
    }
}

