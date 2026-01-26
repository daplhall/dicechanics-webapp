using Microsoft.AspNetCore.Mvc;
using Python.Runtime;
using PythonSerivce.Service;
using PythonSerivce.Service.Interpreter;
[assembly: ApiController]

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options => {
        options.AddPolicy("CorsPolicy",
            policyBuilder => {
                    policyBuilder.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200");
            });
});
builder.Services.AddSingleton<IPyConsole, PyConsole>();
builder.Services.AddOpenApi();
builder.Services.AddControllers(); // This adds then you still need to map them

var app = builder.Build();

app.UseCors("CorsPolicy");
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers(); // Important adds them to an end point

PythonEngine.Initialize();
PythonEngine.BeginAllowThreads();

app.Run();