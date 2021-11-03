using Identity.API.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace Identity.API
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers();
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "Identity.API", Version = "v1" });
			});

			services.Configure<JwtSettings>(Configuration.GetSection("JwtSettings"));
			//services.Configure<JwtSettings>(Configuration);
			var jwtSettings = new JwtSettings();
			Configuration.Bind("JwtSettings", jwtSettings);

			services.AddAuthentication(option =>
			{
				option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidIssuer = jwtSettings.Issuer,
					ValidAudience = jwtSettings.Audience,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))

					/***********************************TokenValidationParameters的参数默认值***********************************/
					// RequireSignedTokens = true,
					// SaveSigninToken = false,
					// ValidateActor = false,
					// 将下面两个参数设置为false，可以不验证Issuer和Audience，但是不建议这样做。
					// ValidateAudience = true,
					// ValidateIssuer = true, 
					// ValidateIssuerSigningKey = false,
					// 是否要求Token的Claims中必须包含Expires
					// RequireExpirationTime = true,
					// 允许的服务器时间偏移量
					// ClockSkew = TimeSpan.FromSeconds(300),
					// 是否验证Token有效期，使用当前时间与Token的Claims中的NotBefore和Expires对比
					// ValidateLifetime = true
				};
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Identity.API v1"));
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthentication();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
