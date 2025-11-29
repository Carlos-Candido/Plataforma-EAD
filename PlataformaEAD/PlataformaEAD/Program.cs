using PlataformaEAD.Data;
using PlataformaEAD.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configurar DbContext com SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Configurar Identity
builder.Services.AddIdentity<Usuario, IdentityRole>(options =>
{
    // Configurações de senha
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;

    // Configurações de usuário
    options.User.RequireUniqueEmail = true;

    // Configurações de login
    options.SignIn.RequireConfirmedEmail = false;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// Configurar cookie de autenticação
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Conta/Login";
    options.LogoutPath = "/Conta/Logout";
    options.AccessDeniedPath = "/Conta/AcessoNegado";
    
    // Cookie expira em 2 horas se "Lembrar-me" NÃO estiver marcado
    options.ExpireTimeSpan = TimeSpan.FromHours(2);
    
    // Cookie persiste por 30 dias se "Lembrar-me" estiver marcado
    options.Cookie.MaxAge = TimeSpan.FromDays(30);
    
    // Sliding expiration: renova o cookie se o usuário estiver ativo
    options.SlidingExpiration = true;
    
    // Cookie persiste mesmo após fechar o navegador (quando "Lembrar-me" está marcado)
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Rota padrão MVC
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
