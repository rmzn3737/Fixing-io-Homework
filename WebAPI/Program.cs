using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using Business.Abstract;
using Business.Concrete;
using Business.DependencyResolvers.Autofac;
using Core.DependencyResolvers;
using Core.Extensions;
using Core.Utilities.IoC;
using Core.Utilities.Security.Encryption;
using Core.Utilities.Security.JWT;
using DataAccsess.Abstract;
using DataAccsess.Concrete.EntityFramework;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;


var builder = WebApplication.CreateBuilder(args); 

builder.Host
    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(builder =>
    {
        builder.RegisterModule(new AutofacBusinessModule());
        
    });

// Add services to the container.

//Autofac, Ninject, CastleWindsor,StructureMap, LightInject, DryInject --> Bunlar .Net te IoC yokken bile bunun yapt��� i�i yapan alt yap� sunuyor idi.
//AOP yapaca��z. Autofac bu imkan� sunuyor.
builder.Services.AddControllers();
//builder.Services.AddSingleton<ICourseService,CourseManager>();//Biri CourseManagerda ICourseService isterse ona CourseManager ver demi� oluyoruz. ��inde data tutmuyorsak singleton kullan�yoruz. 11. video 1. saat 37. dakikadan yeniden dinleyelim. Bunlar Javadaki in configurasyonu.
//builder.Services.AddSingleton<ICourseDal, EfCourseDal>();//Biri CourseManagerda ICourseService isterse ona CourseManager ver demi� oluyoruz.

var tokenOptions = builder.Configuration.GetSection("TokenOptions").Get<TokenOptions>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidIssuer = tokenOptions.Issuer,
        ValidAudience = tokenOptions.Audience,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(tokenOptions.SecurityKey)
    };
});
//ServiceTool.Create(builder.Services);

builder.Services.AddDependencyResolvers(new ICoreModule[]//Yazd���m�z extensions burada bu �ekilde �a��r�yoruz.
{
    new CoreModule()//Daha sonra ba�ka mod�llerde eklersek buraya virg�l koyup yan�na ekleyebilece�iz.
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
