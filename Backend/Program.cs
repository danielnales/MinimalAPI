var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddTransient<IPersonStorage, TextFilesPersonStorage>();
builder.Services.AddTransient<IAddressStorage, TextFilesAddressStorage>();

var app = builder.Build();
app.MapControllers();

app.Run("https://localhost:5000");

/*
Questions:


*/
