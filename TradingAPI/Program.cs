using System.Text;
using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using TradingAPI.Business.Abstract.Auth;
using TradingAPI.Business.ServiceAbstract.Auth;
using TradingAPI.Business.ServiceAbstract.Services;
using TradingAPI.Business.ServiceImplementation;
using TradingAPI.Business.ServiceImplementation.Auth;
using TradingAPI.Business.Shared;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
//builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);

// FirestoreAccessor should be registered first
builder.Services.AddSingleton<FirestoreAccessor>();

// Add FirestoreInitService as hosted service
builder.Services.AddHostedService<FirestoreInitService>();

// Add Services
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUserService, UserServices>();

// Configure AWS Secrets Manager
builder.Services.AddSingleton<IAmazonSecretsManager>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    //Console.WriteLine($"AWS Region: {config["AWS:SecretsManager:Region"]}");
    return new AmazonSecretsManagerClient(RegionEndpoint.GetBySystemName(config["AWS:SecretsManager:Region"]));
});


builder.Services.AddSingleton(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    //Console.WriteLine($"AWS Secret Name: {config["AWS:SecretsManager:SecretName"]}");
    return new GetSecretValueRequest
    {
        SecretId = config["AWS:SecretsManager:SecretName"],
        VersionStage = "AWSCURRENT"
    };
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Secrets:JWTKey"]!)),
        ValidateIssuer = true,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Secrets:Issuer"]
    };
});


var app = builder.Build();

app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();
app.Run();
