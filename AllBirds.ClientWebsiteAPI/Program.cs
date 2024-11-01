using AllBirds.Application.Contracts;
using AllBirds.Application.Mapper;
using AllBirds.Application.Services.AccountServices;
using AllBirds.Application.Services.CategoryProductServices;
using AllBirds.Application.Services.CategoryServices;
using AllBirds.Application.Services.ColorServices;
using AllBirds.Application.Services.CouponServices;
using AllBirds.Application.Services.OrderDetailServices;
using AllBirds.Application.Services.OrderMasterServices;
using AllBirds.Application.Services.OrderStateServices;
using AllBirds.Application.Services.ProductColorImageServices;
using AllBirds.Application.Services.ProductColorServices;
using AllBirds.Application.Services.ProductDetailService;
using AllBirds.Application.Services.ProductServices;
using AllBirds.Application.Services.ProductSpecificationServices;
using AllBirds.Application.Services.SizeServices;
using AllBirds.Application.Services.SpecificationServices;
using AllBirds.Context;
using AllBirds.Infrastructure;
using AllBirds.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

//namespace AllBirds.ClientWebsiteAPI
//{
//    public class Program
//    {
//        public static void Main(string[] args)
//        {

//        }
//    }
//}
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<AllBirdsContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
// Account
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IAccountRoleRepository, AccountRoleRepository>();
// Category
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
// CategoryProduct
builder.Services.AddScoped<ICategoryProductService, CategoryProductService>();
builder.Services.AddScoped<ICategoryProductRepository, CategoryProductRepository>();
// ClientFavorite

// Color
builder.Services.AddScoped<IColorService, ColorService>();
builder.Services.AddScoped<IColorRepository, ColorRepository>();
// Coupon
builder.Services.AddScoped<ICouponService, CouponService>();
builder.Services.AddScoped<ICouponRepository, CouponRepository>();
// OrderDetail
builder.Services.AddScoped<IOrderDetailService, OrderDetailService>();
builder.Services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
// OrderMaster
builder.Services.AddScoped<IOrderMasterService, OrderMasterService>();
builder.Services.AddScoped<IOrderMasterRepository, OrderMasterRepository>();
// OrderState
builder.Services.AddScoped<IOrderStateService, OrderStateService>();
builder.Services.AddScoped<IOrderStateRepository, OrderStateRepository>();
// Product
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
// ProductColor
builder.Services.AddScoped<IProductColorService, ProductColorService>();
builder.Services.AddScoped<IProductColorRepository, ProductColorRepository>();
// ProductColorImage
builder.Services.AddScoped<IProductColorImageRepository, ProductColorImageRepository>();
builder.Services.AddScoped<IProductColotImageService, ProductColorImageService>();
// ProductColorSize

// ProductDetail
builder.Services.AddScoped<IProductDetailsService, ProductDetailsService>();
builder.Services.AddScoped<IProductDetailsRepository, ProductDetailsRepository>();
// ProductReview

// ProductSpecification
builder.Services.AddScoped<IProductSpecificationService, ProductSpecificationService>();
builder.Services.AddScoped<IProductSpecificationRepository, ProductSpecificationRepository>();
// Size
builder.Services.AddScoped<ISizeService, SizeService>();
builder.Services.AddScoped<ISizeRepository, SizeRepository>();
// Specification
builder.Services.AddScoped<ISpecificationService, SpecificationService>();
builder.Services.AddScoped<ISpecificationRepository, SpecificationRepository>();

builder.Services.AddIdentity<CustomUser, IdentityRole<int>>(options =>
{
    options.SignIn.RequireConfirmedEmail = true;
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.User.RequireUniqueEmail = true;
})
    .AddRoles<IdentityRole<int>>()
    .AddEntityFrameworkStores<AllBirdsContext>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(sa =>
{
    sa.SwaggerDoc("V1", new OpenApiInfo
    {
        Title = "Client API",
        Version = "v1"
    });
    sa.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please Insert JWT with Bearer Into Field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    sa.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new string[] { }
        }
    });
});
builder.Services.AddAuthentication(op =>
{
    op.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    op.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    op.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(op =>
{
    op.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["jwt:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["jwt:Audience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["jwt:key"] ?? "AllBirdsDefaultJWTKey"))
    };
});
builder.Services.AddCors(op =>
{
    op.AddPolicy("Derfault", policy =>
    {
        //policy.WithOrigins("http://localhost:4200", "http://anydomain:domainport", "null")
        //.WithHeaders("Key")
        //.WithMethods("Post", "Get");
        policy.AllowAnyHeader()
        .AllowAnyOrigin()
        .AllowAnyMethod();
    });
});
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
app.UseCors("Default");

app.MapControllers();

app.Run();
