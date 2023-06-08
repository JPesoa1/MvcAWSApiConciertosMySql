using Amazon.S3;
using MvcAWSApiConciertosMySql.Helpers;
using MvcAWSApiConciertosMySql.Models;
using MvcAWSApiConciertosMySql.ServiceApiConcierto;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAWSService<IAmazonS3>();

string miSecreto = await HelperSecretManager.GetSecretAsync();
KeysModel model = JsonConvert.DeserializeObject<KeysModel>(miSecreto);

builder.Services.AddSingleton<KeysModel>(model);
builder.Services.AddTransient<ServiceApiConcierto>();
builder.Services.AddTransient<ServiceStorageS3>();

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
