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
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.EnablePersistAuthorization();
    options.DefaultModelsExpandDepth(-1);
    options.DefaultModelExpandDepth(-1);
    options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
    options.EnableFilter();
}
);

app.UseHttpsRedirection();

app.MapControllers();

app.UseAuthentication();

app.UseAuthorization();

app.Run();