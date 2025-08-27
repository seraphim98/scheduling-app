using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Scheduler;
using Scheduler.Database;
using Scheduler.Models;

var builder = WebApplication.CreateBuilder(args);
var connectionString =
    "Host=localhost;Port=5432;Database=Scheduler;Username=postgres;Password=admin";
builder.Services.AddControllers();
var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MappingProfile());
});

IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddMvc();
builder.Services.AddDbContext<DatabaseContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IRepository<Person>, Repository<Person>>();
builder.Services.AddScoped<IRepository<PersonEvent>, Repository<PersonEvent>>();
builder.Services.AddScoped<IRepository<Event>, Repository<Event>>();
builder.Services.AddScoped<IRepository<Holiday>, Repository<Holiday>>();
builder.Services.AddScoped<IRepository<Recipe>, Repository<Recipe>>();
builder.Services.AddCors(o =>
    o.AddPolicy(
        "MyPolicy",
        builder =>
        {
            builder.WithOrigins("http://localhost:5173").AllowAnyMethod().AllowAnyHeader();
        }
    )
);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("MyPolicy");
app.UseAuthorization();

app.MapControllers();

app.Run();
