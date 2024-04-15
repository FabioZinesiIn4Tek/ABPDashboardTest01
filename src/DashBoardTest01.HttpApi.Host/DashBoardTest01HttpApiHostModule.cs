using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Microsoft.AspNetCore.Authentication.Twitter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Extensions.DependencyInjection;
using OpenIddict.Validation.AspNetCore;
using OpenIddict.Server.AspNetCore;
using DashBoardTest01.EntityFrameworkCore;
using DashBoardTest01.MultiTenancy;
using Microsoft.OpenApi.Models;
using Volo.Abp;
using Volo.Abp.Account.Web;
using Volo.Abp.Account.Public.Web;
using Volo.Abp.Account.Public.Web.Impersonation;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Autofac;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation.Urls;
using Volo.Abp.VirtualFileSystem;
using Volo.Abp.Account;
using Volo.Abp.Account.Public.Web.ExternalProviders;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Microsoft.AspNetCore.Hosting;
using DashBoardTest01.HealthChecks;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonX;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonX.Bundling;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Identity;
using Volo.Abp.LeptonX.Shared;
using Volo.Abp.OpenIddict;
using Volo.Abp.Swashbuckle;
using Volo.Saas.Host;
using Volo.Abp.Security.Claims;


using DevExpress.AspNetCore;
using DevExpress.DashboardCommon;
using DevExpress.DashboardWeb;
using DevExpress.DataAccess.Json;
using DevExpress.CodeParser;
using DevExpress.DashboardAspNetCore;
using DevExpress.DashboardWeb.Native;
using DevExpress.DashboardCommon.ViewerData;

using Microsoft.Extensions.FileProviders;
using DashBoardTest01.Controllers;



using DashBoardTest01;

using DashBoardTest01.Bundling.Common;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Bundling;
using DevExpress.XtraReports.Summary.Native;
using ASPNETCoreDashboardAngular;
using System.Drawing;
using AspNetCoreDashboard;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Net;




namespace DashBoardTest01;

[DependsOn(
    typeof(DashBoardTest01HttpApiModule),
    typeof(AbpAutofacModule),
    typeof(AbpAspNetCoreMultiTenancyModule),
    typeof(DashBoardTest01ApplicationModule),
    typeof(DashBoardTest01EntityFrameworkCoreModule),
    typeof(AbpAspNetCoreMvcUiLeptonXThemeModule),
    typeof(AbpAccountPublicWebImpersonationModule),
    typeof(AbpAccountPublicWebOpenIddictModule),
    typeof(AbpSwashbuckleModule),
    typeof(AbpAspNetCoreSerilogModule)
    )]
public class DashBoardTest01HttpApiHostModule : AbpModule
{

    IFileProvider? fileProvider;// = context.Services //builder.Environment.ContentRootFileProvider;
    IConfiguration? configuration;// = context.Services.GetConfiguration();
    IHttpContextAccessor httpContextAccessor;

    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();
        var configuration = context.Services.GetConfiguration();

        PreConfigure<OpenIddictBuilder>(builder =>
        {
            builder.AddValidation(options =>
            {
                options.AddAudiences("DashBoardTest01");
                options.UseLocalServer();
                options.UseAspNetCore();
            });
        });

        if (!hostingEnvironment.IsDevelopment())
        {
            PreConfigure<AbpOpenIddictAspNetCoreOptions>(options =>
            {
                options.AddDevelopmentEncryptionAndSigningCertificate = false;
            });

            PreConfigure<OpenIddictServerBuilder>(serverBuilder =>
            {
                serverBuilder.AddProductionEncryptionAndSigningCertificate("openiddict.pfx", "d3da2a36-2877-4d8e-985b-d301aaa7368d");
            });
        }
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        fileProvider = context.Services.GetHostingEnvironment().ContentRootFileProvider; //builder.Environment.ContentRootFileProvider;
        
        var configuration = context.Services.GetConfiguration();
        var hostingEnvironment = context.Services.GetHostingEnvironment();

        if (!configuration.GetValue<bool>("App:DisablePII"))
        {
            Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;
        }

        if (!configuration.GetValue<bool>("AuthServer:RequireHttpsMetadata"))
        {
            Configure<OpenIddictServerAspNetCoreOptions>(options =>
            {
                options.DisableTransportSecurityRequirement = true;
            });
        }

        context.Services.AddAntiforgery(options => {
            // Set Cookie properties using CookieBuilder properties†.  
            options.FormFieldName = "X-CSRF-TOKEN";
            options.HeaderName = "X-CSRF-TOKEN";
            options.SuppressXFrameOptionsHeader = false;
        });

        ConfigureAuthentication(context);
        ConfigureUrls(configuration);
        ConfigureBundles();
        ConfigureConventionalControllers();
        ConfigureImpersonation(context, configuration);
        ConfigureSwagger(context, configuration);
        ConfigureVirtualFileSystem(context);
        ConfigureCors(context, configuration);
        ConfigureExternalProviders(context);
        ConfigureHealthChecks(context);
        ConfigureTheme();
        ConfigureDashboard(context);
    }

    private void ConfigureTheme()
    {
        Configure<LeptonXThemeOptions>(options =>
        {
            options.DefaultStyle = LeptonXStyleNames.System;
        });
    }

    private void ConfigureAuthentication(ServiceConfigurationContext context)
    {
        context.Services.ForwardIdentityAuthenticationForBearer(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
        context.Services.Configure<AbpClaimsPrincipalFactoryOptions>(options =>
        {
            options.IsDynamicClaimsEnabled = true;
        });
    }

    private void ConfigureHealthChecks(ServiceConfigurationContext context)
    {
        context.Services.AddDashBoardTest01HealthChecks();
    }

    private void ConfigureUrls(IConfiguration configuration)
    {
        Configure<AppUrlOptions>(options =>
        {
            options.Applications["MVC"].RootUrl = configuration["App:SelfUrl"];
            options.Applications["Angular"].RootUrl = configuration["App:AngularUrl"];
            options.Applications["Angular"].Urls[AccountUrlNames.PasswordReset] = "account/reset-password";
            options.Applications["Angular"].Urls[AccountUrlNames.EmailConfirmation] = "account/email-confirmation";
            options.RedirectAllowedUrls.AddRange(configuration["App:RedirectAllowedUrls"]?.Split(',') ?? Array.Empty<string>());
        });
    }

    private void ConfigureBundles()
    {
        Configure<AbpBundlingOptions>(options =>
        {
            options.StyleBundles.Configure(
                LeptonXThemeBundles.Styles.Global,
                bundle =>
                {
                    bundle.AddFiles("/global-styles.css");
                }
            );
            options
                .StyleBundles
                .Get(StandardBundles.Styles.Global)
                .AddContributors(typeof(DevExtremeCommonStyleContributor));
        });
    }

    private void ConfigureDashboard(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();
        fileProvider = context.Services.GetHostingEnvironment().ContentRootFileProvider; //builder.Environment.ContentRootFileProvider;
        configuration = context.Services.GetConfiguration();

        context.Services.AddDevExpressControls();
        context.Services.AddControllersWithViews();
        //context.Services.AddSingleton<DashboardConfigurator, MultiTenantDashboardConfigurator>();

        context.Services.AddScoped<DashboardConfigurator>((IServiceProvider serviceProvider) =>
        {

            //V1

            //DashboardConfigurator configurator = new DashboardConfigurator();
            //configurator.SetDashboardStorage(new DashboardFileStorage(fileProvider.GetFileInfo("Data/Dashboards").PhysicalPath));
            //configurator.SetConnectionStringsProvider(new DashboardConnectionStringsProvider(configuration));
            ////configurator.CustomPalette += Default_CustomPalette;


            //configurator.SetConnectionStringsProvider(new DashboardConnectionStringsProvider(configuration));


            //V2
            MultiTenantDashboardConfigurator configurator = new MultiTenantDashboardConfigurator(hostingEnvironment, httpContextAccessor);
            //DashboardConfigurator configurator = new DashboardConfigurator();
            configurator.SetDashboardStorage(new CustomDashboardFileStorage(fileProvider.GetFileInfo("Data/Dashboards").PhysicalPath));
            configurator.SetDataSourceStorage(CreateDataSourceStorage());
            configurator.SetConnectionStringsProvider(new DashboardConnectionStringsProvider(configuration));
            configurator.CustomPalette += Default_CustomPalette;
            configurator.ConfigureDataConnection += Configurator_ConfigureDataConnection;

            return configurator;

        });
    }

    void Default_CustomPalette(object sender, CustomPaletteWebEventArgs e)
    {
        if (e.DashboardId == "SalesByCategory")
        {

            // Create a new custom palette.
            List<Color> customColors = new List<Color>();
            customColors.Add(Color.LightBlue);
            customColors.Add(Color.Aquamarine);
            customColors.Add(Color.SkyBlue);
            customColors.Add(Color.LightCoral);
            customColors.Add(Color.Tomato);
            customColors.Add(Color.IndianRed);
            customColors.Add(Color.Violet);
            customColors.Add(Color.Plum);
            customColors.Add(Color.MediumOrchid);

            // Assign a newly created custom palette to the Web Dashboard.
            e.Palette = new DashboardPalette(customColors);
        }
    }


    void Configurator_ConfigureDataConnection(object sender, ConfigureDataConnectionWebEventArgs e)
    {
        if (e.ConnectionName == "jsonSupport")
        {
            Uri fileUri = new Uri(fileProvider.GetFileInfo("Data/support.json").PhysicalPath, UriKind.RelativeOrAbsolute);
            JsonSourceConnectionParameters jsonParams = new JsonSourceConnectionParameters();
            jsonParams.JsonSource = new UriJsonSource(fileUri);
            e.ConnectionParameters = jsonParams;
        }
        if (e.ConnectionName == "jsonCategories")
        {
            Uri fileUri = new Uri(fileProvider.GetFileInfo("Data/categories.json").PhysicalPath, UriKind.RelativeOrAbsolute);
            JsonSourceConnectionParameters jsonParams = new JsonSourceConnectionParameters();
            jsonParams.JsonSource = new UriJsonSource(fileUri);
            e.ConnectionParameters = jsonParams;
        }
    }


    DataSourceInMemoryStorage CreateDataSourceStorage()
    {
        DataSourceInMemoryStorage dataSourceStorage = new DataSourceInMemoryStorage();

        DashboardJsonDataSource jsonDataSourceSupport = new DashboardJsonDataSource("Support");
        jsonDataSourceSupport.ConnectionName = "jsonSupport";
        jsonDataSourceSupport.RootElement = "Employee";
        dataSourceStorage.RegisterDataSource("jsonDataSourceSupport", jsonDataSourceSupport.SaveToXml());

        DashboardJsonDataSource jsonDataSourceCategories = new DashboardJsonDataSource("Categories");
        jsonDataSourceCategories.ConnectionName = "jsonCategories";
        jsonDataSourceCategories.RootElement = "Products";
        dataSourceStorage.RegisterDataSource("jsonDataSourceCategories", jsonDataSourceCategories.SaveToXml());
        return dataSourceStorage;
    }

    private void ConfigureVirtualFileSystem(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();

        if (hostingEnvironment.IsDevelopment())
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.ReplaceEmbeddedByPhysical<DashBoardTest01DomainSharedModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}DashBoardTest01.Domain.Shared"));
                options.FileSets.ReplaceEmbeddedByPhysical<DashBoardTest01DomainModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}DashBoardTest01.Domain"));
                options.FileSets.ReplaceEmbeddedByPhysical<DashBoardTest01ApplicationContractsModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}DashBoardTest01.Application.Contracts"));
                options.FileSets.ReplaceEmbeddedByPhysical<DashBoardTest01ApplicationModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}DashBoardTest01.Application"));
            });
        }
    }

    private void ConfigureConventionalControllers()
    {
        Configure<AbpAspNetCoreMvcOptions>(options =>
        {
            options.ConventionalControllers.Create(typeof(DashBoardTest01ApplicationModule).Assembly);
        });
    }

    private static void ConfigureSwagger(ServiceConfigurationContext context, IConfiguration configuration)
    {
        context.Services.AddAbpSwaggerGenWithOAuth(
            configuration["AuthServer:Authority"]!,
            new Dictionary<string, string>
            {
                    {"DashBoardTest01", "DashBoardTest01 API"}
            },
            options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "DashBoardTest01 API", Version = "v1" });
                options.DocInclusionPredicate((docName, description) => true);
                options.CustomSchemaIds(type => type.FullName);
            });
    }

    private void ConfigureCors(ServiceConfigurationContext context, IConfiguration configuration)
    {
        context.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder
                    .WithOrigins(
                        configuration["App:CorsOrigins"]?
                            .Split(",", StringSplitOptions.RemoveEmptyEntries)
                            .Select(o => o.Trim().RemovePostFix("/"))
                            .ToArray() ?? Array.Empty<string>()
                    )
                    .WithAbpExposedHeaders()
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });
    }

    private void ConfigureExternalProviders(ServiceConfigurationContext context)
    {
        context.Services.AddAuthentication()
            .AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
            {
                options.ClaimActions.MapJsonKey(AbpClaimTypes.Picture, "picture");
            })
            .WithDynamicOptions<GoogleOptions, GoogleHandler>(
                GoogleDefaults.AuthenticationScheme,
                options =>
                {
                    options.WithProperty(x => x.ClientId);
                    options.WithProperty(x => x.ClientSecret, isSecret: true);
                }
            )
            .AddMicrosoftAccount(MicrosoftAccountDefaults.AuthenticationScheme, options =>
            {
                //Personal Microsoft accounts as an example.
                options.AuthorizationEndpoint = "https://login.microsoftonline.com/consumers/oauth2/v2.0/authorize";
                options.TokenEndpoint = "https://login.microsoftonline.com/consumers/oauth2/v2.0/token";

                options.ClaimActions.MapCustomJson("picture", _ => "https://graph.microsoft.com/v1.0/me/photo/$value");
                options.SaveTokens = true;
            })
            .WithDynamicOptions<MicrosoftAccountOptions, MicrosoftAccountHandler>(
                MicrosoftAccountDefaults.AuthenticationScheme,
                options =>
                {
                    options.WithProperty(x => x.ClientId);
                    options.WithProperty(x => x.ClientSecret, isSecret: true);
                }
            )
            .AddTwitter(TwitterDefaults.AuthenticationScheme, options =>
            {
                options.ClaimActions.MapJsonKey(AbpClaimTypes.Picture, "profile_image_url_https");
                options.RetrieveUserDetails = true;
            })
            .WithDynamicOptions<TwitterOptions, TwitterHandler>(
                TwitterDefaults.AuthenticationScheme,
                options =>
                {
                    options.WithProperty(x => x.ConsumerKey);
                    options.WithProperty(x => x.ConsumerSecret, isSecret: true);
                }
            );
    }

    private void ConfigureImpersonation(ServiceConfigurationContext context, IConfiguration configuration)
    {
        context.Services.Configure<AbpAccountOptions>(options =>
        {
            options.TenantAdminUserName = "admin";
            options.ImpersonationTenantPermission = SaasHostPermissions.Tenants.Impersonation;
            options.ImpersonationUserPermission = IdentityPermissions.Users.Impersonation;
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseAbpRequestLocalization();

        if (!env.IsDevelopment())
        {
            app.UseErrorPage();
        }

        app.UseAbpSecurityHeaders();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseCors();
        app.UseAuthentication();
        app.UseAbpOpenIddictValidation();



        // Registers the DevExpress middleware.            
        app.UseDevExpressControls();
        app.UseEndpoints(endpoints => {
            endpoints.MapDashboardRoute("api/dashboard", "DefaultDashboard");
            /*endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");*/
        });


        if (MultiTenancyConsts.IsEnabled)
        {
            app.UseMultiTenancy();
        }

        app.UseUnitOfWork();
        app.UseDynamicClaims();
        app.UseAuthorization();

        app.UseSwagger();
        app.UseAbpSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "DashBoardTest01 API");

            var configuration = context.ServiceProvider.GetRequiredService<IConfiguration>();
            options.OAuthClientId(configuration["AuthServer:SwaggerClientId"]);
        });
        app.UseAuditing();
        app.UseAbpSerilogEnrichers();
        app.UseConfiguredEndpoints();


    }
}
