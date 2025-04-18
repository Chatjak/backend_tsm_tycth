using Microsoft.EntityFrameworkCore;
using BackendTSM.Models; // อย่าลืมเปลี่ยน namespace ให้ตรงกับ DbContext ของคุณ

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSwaggerGen(); // ✅ แก้ปัญหา ISwaggerProvider
builder.Services.AddEndpointsApiExplorer(); // ✅ ใช้ร่วมกับ Swagger UI

// ➕ Register DbContext
builder.Services.AddDbContext<TsmDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// ➕ Register Controllers
builder.Services.AddControllers();

// ➕ OpenAPI (Swagger)
builder.Services.AddOpenApi();

var app = builder.Build();

// ➕ Swagger UI เฉพาะตอน dev
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();       // สำหรับ dotnet openapi tools
    app.UseSwagger();       // Swagger JSON
    app.UseSwaggerUI();     // Swagger UI หน้าเว็บ
}

// ➕ Map routing
app.UseHttpsRedirection();

app.MapControllers();       // สำคัญ! เพื่อให้ controller เช่น UsersController ทำงานได้

app.Run();