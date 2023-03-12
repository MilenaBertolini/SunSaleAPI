using Data.Context;
using Microsoft.EntityFrameworkCore;
using APISunSale.Startup;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("connection"))
);

new Start(builder).Builder();

var app = builder.Build();

app.UseCors(options => options.AllowAnyOrigin());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => options.EnablePersistAuthorization());
}

app.UseHttpsRedirection();

app.MapControllers();

app.UseAuthentication();

app.UseAuthorization();

app.Run();