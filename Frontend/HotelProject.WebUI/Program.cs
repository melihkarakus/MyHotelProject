using FluentValidation;
using FluentValidation.AspNetCore;
using HotelProject.DataAccessLayer.Concrete;
using HotelProject.EntityLayer.Concrete;
using HotelProject.WebUI.Dtos.GuestDto;
using HotelProject.WebUI.Models.CustomerIdentityValidator;
using HotelProject.WebUI.ValidationRules.GuestValidationRules;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<Context>();
builder.Services.AddIdentity<AppUser, AppRole>().AddEntityFrameworkStores<Context>().AddErrorDescriber<CustomerIdentityValidator>();
//En buraya tan�mlaman laz�m yoksa �al��maz parola hatalar�n� da tan�mlaman i�in AddErrrorDescriber eklemelisin 
builder.Services.AddAutoMapper(typeof(Program));
// Add services to the container.
//builder.Services.AddFluentValidationAutoValidation();
//builder.Services.AddScoped<IValidator<CreateGuestDto>, CreateGuestValidator>();

builder.Services.AddTransient<IValidator<CreateGuestDto>, CreateGuestValidator>();//Burada GuestDto ve Validator �a��rmazsak i�e yaramaz.   
builder.Services.AddTransient<IValidator<UpdateGuestDto>, UpdateGuestValidator>();//Burada GuestDto ve Validator �a��rmazsak i�e yaramaz.   
builder.Services.AddControllersWithViews().AddFluentValidation();//AddFluentValidation FluentValidator i�in ge�ilmesi gereken yer*/
builder.Services.AddHttpClient();
builder.Services.AddMvc(config =>
{
    var policy = new AuthorizationPolicyBuilder()//policy de�i�keni olu�turuldu
    .RequireAuthenticatedUser()//AuthorizationPolicyBuilder s�n�f�ndan kullan�ld�
    .Build();//�n�a edildi
    config.Filters.Add(new AuthorizeFilter(policy));
});
builder.Services.ConfigureApplicationCookie(options =>// Cookie ayarlar�n� yap�land�rma i�lemi ba�l�yor
{
    options.Cookie.HttpOnly = true;// �erezin sadece HTTP �zerinden eri�ilebilir olmas�n� sa�lar
    options.ExpireTimeSpan = TimeSpan.FromMinutes(10);// �erezin ge�erlilik s�resini 10 dakika olarak ayarlar
    options.LoginPath = "/Login/Index";// Kullan�c�n�n giri� yapmad��� durumda y�nlendirilecek olan sayfa yolunu belirler
});
var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/ErrorPage/Error404", "?code={0}");
app.UseHttpsRedirection();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
