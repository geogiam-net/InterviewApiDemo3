using Demo.Api.Endpoints;
using Demo.Domain.Interfaces;
using Demo.Api.Exceptions;
using Demo.Api.Startup;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInMemoryDbContext();
builder.Services.AddScoped<IEmployeeStorageService>();
builder.Services.AddScoped<IShiftStorageService>();
builder.Services.AddScoped<IEmployeeService>();
builder.Services.AddScoped<IShiftService>();

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ValidationExceptionHandler>();

var app = builder.Build();

// not needed for this demo, but in production you should use https and authorization
// app.UseHttpsRedirection();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

// UseStatusCodePages enables middleware that provides default responses for HTTP status codes
// (like 404, 400, 500) when your API does not return a body
app.UseStatusCodePages();
app.UseExceptionHandler();

app.MapEmployeeEndpoints();
app.MapShiftEndpoints();

// not needed for this demo, but in production you should use https and authorization
// app.UseAuthorization(); 

app.Run();