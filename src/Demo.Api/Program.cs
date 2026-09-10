using Demo.Api.Endpoints;
using Demo.Api.Startup;

var builder = WebApplication.CreateBuilder(args);

// if we had settings, we would test them here at start to make sure they exists and not have the app crash at runtime.
// SettingsTester.TestSettingsExist(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// if we had any
// builder.Services.AddAuthorization(builder.Configuration);

builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddRepositories();
builder.Services.AddServices();

// if we had them
// builder.Services.AddWorkers(builder.Configuration);

// if we needed it
// builder.Services.AddHttpClient();                    

#if DEBUG
builder.Services.AddProblemDetails();
#endif

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

// if we had any
// app.UseAuthentication();
// app.UseAuthorization();

app.UseStatusCodePages();
app.UseExceptionHandler();

app.MapEmployeeEndpoints();
app.MapShiftEndpoints();

app.Run();