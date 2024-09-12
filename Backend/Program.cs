var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
app.Urls.Add("https://localhost:5000");

app.MapPost("/person/{id}"), (int id, Person who) => {
  if (People.ContainsKey(id)) 
  {
    return Results.Conflict("Person already exists");
  }

  People[id] = who;
  Friends[id] = [];
  return Results.Ok();
});

app.MapPost("/person/{personOne}/befriend/{personTwo}", (int personOne, int personTwo) => {
  if (!People.ContainsKey(personOne))
    return Results.NotFound($"Person with ID {personOne} not found.");

  if (!People.ContainsKey(personTwo))
    return Results.NotFound($"Person with ID {personTwo} not found.");

  if (Friends[personOne].Contains(personTwo))
    return Results.Conflict($"Person {personOne} is already friends with {personTwo}.");

  // Establish friendship
  Friends[personOne].Add(personTwo);
  Friends[personTwo].Add(personOne);

  return Results.Ok("Friendship established successfully.");
});

app.MapGet("/person/{id}", (int id) => {
  if (!People.ContainsKey(id))
    return Results.NotFound($"Person with ID {id} not found.");
  return People[id];
}); 

app.MapGet("/person/friends/{id}", (int id) => {
  if (!Friends.ContainsKey(id))
    return Results.NotFound($"Person with ID {id} not found.");
  return Friends[id];
});

app.MapPut("/person/edit/{id}", (int id, Person who) => {
  if (!People.ContainsKey(id)) 
    return Results.NotFound($"Person with ID {id} not found.");
  
  People[id] = who;
  return Results.Ok("Person successfully updated.");
});

app.MapDelete("/person/delete/{id}", (int id) => {
  if (!People.ContainsKey(id))
    return Results.NotFound($"Person with ID {id} not found.");

  People.Remove(id);
  
  foreach (var Friendship in Friends) 
    Relationships.Value.Remove(id);

  return Results.Ok("Person successfully removed.");
});

app.Run();

record Person(string Name, int Age);
var People = new Dictionary<int, Person>();
var Friends = new Dictionary<int, List<int>()>;ecord Person(string Name, int Age);
