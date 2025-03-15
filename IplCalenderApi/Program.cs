using IplCalenderApi.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
//builder.Services.AddDbContext<IplDbContext>(options =>
//    options.UseSqlite(builder.Configuration.GetConnectionString("IplScheduleDatabase"))
//);

var app = builder.Build();

app.MapControllers();
app.Run();
