using FluentValidation;
using FluentValidation.AspNetCore;
using HotelProject.DataAccessLayer.Concrete;
using HotelProject.EntityLayer.Concrete;
using HotelProject.WebUI.Dtos.GuestDto;
using HotelProject.WebUI.Models.CustomerIdentityValidator;
using HotelProject.WebUI.ValidationRules.GuestValidationRules;

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
var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
