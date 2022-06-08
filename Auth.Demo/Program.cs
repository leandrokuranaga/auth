using Auth.Demo;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.WsFederation;
using System.Text;
using Auth.Demo.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var key = "This is my test key";

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
}
)
.AddWsFederation(options =>
{
    // MetadataAddress represents the Active Directory instance used to authenticate users.
    options.MetadataAddress = builder.Configuration["MetaDataAdress"];

    // Wtrealm is the app's identifier in the Active Directory instance.
    // For ADFS, use the relying party's identifier, its WS-Federation Passive protocol URL:
    options.Wtrealm = builder.Configuration["TrustedServices"];

    // For AAD, use the Application ID URI from the app registration's Overview blade:
    //options.Wtrealm = ;
}); 

//builder.Services.AddAuthentication()
//                  .AddWsFederation(options =>
//                  {
//                      // MetadataAddress represents the Active Directory instance used to authenticate users.
//                      options.MetadataAddress = "https://<ADFS FQDN or AAD tenant>/FederationMetadata/2007-06/FederationMetadata.xml";

//                      // Wtrealm is the app's identifier in the Active Directory instance.
//                      // For ADFS, use the relying party's identifier, its WS-Federation Passive protocol URL:
//                      options.Wtrealm = "https://localhost:44307/";

//                      // For AAD, use the Application ID URI from the app registration's Overview blade:
//                      options.Wtrealm = "api://bbd35166-7c13-49f3-8041-9551f2847b69";
//                  });

builder.Services.AddSingleton<IJwtAuthenticationManager>(new JwtAuthenticationManager(key));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
