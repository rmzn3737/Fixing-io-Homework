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

//Autofac, Ninject, CastleWindsor,StructureMap, LightInject, DryInject --> Bunlar .Net te IoC yokken bile bunun yaptýðý iþi yapan alt yapý sunuyor idi.
//AOP yapacaðýz. Autofac bu imkaný sunuyor.
builder.Services.AddControllers();
//builder.Services.AddSingleton<ICourseService,CourseManager>();//Biri CourseManagerda ICourseService isterse ona CourseManager ver demiþ oluyoruz. Ýçinde data tutmuyorsak singleton kullanýyoruz. 11. video 1. saat 37. dakikadan yeniden dinleyelim. Bunlar Javadaki in configurasyonu.
//builder.Services.AddSingleton<ICourseDal, EfCourseDal>();//Biri CourseManagerda ICourseService isterse ona CourseManager ver demiþ oluyoruz.

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

builder.Services.AddDependencyResolvers(new ICoreModule[]//Yazdýðýmýz extensions burada bu þekilde çaðýrýyoruz.
{
    new CoreModule()//Daha sonra baþka modüllerde eklersek buraya virgül koyup yanýna ekleyebileceðiz.
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
