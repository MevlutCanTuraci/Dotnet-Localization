#region Imports
using App.Api.Infrastructer.Extensions;
using App.Api.Infrastructer.Helper.Localize;
#endregion


#if DEBUG    
    LocalizeFileHelper.Start();
#endif


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Added this line
builder.Services.ConfigureLocalize();

var app = builder.Build();

//Added this line
app.UseRequestLocalization();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
