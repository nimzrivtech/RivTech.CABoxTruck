using Autofac;
using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Submission.DataSets;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Submission.RiskSpecifics;
using RivTech.CABoxTruck.WebService.DataProvider.Repositories;
using RivTech.CABoxTruck.WebService.DataProvider.Repositories.Submission.DataSets;
using RivTech.CABoxTruck.WebService.DataProvider.Repositories.Submission.RiskSpecifics;
using RivTech.CABoxTruck.WebService.DTO.DTOMapping;
using RivTech.CABoxTruck.WebService.Errors;
using RivTech.CABoxTruck.WebService.Extensions;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.IServices.Submission.Applicant;
using RivTech.CABoxTruck.WebService.Service.IServices.Submission.DataSets;
using RivTech.CABoxTruck.WebService.Service.IServices.Submission.RiskSpecifics;
using RivTech.CABoxTruck.WebService.Service.Services;
using RivTech.CABoxTruck.WebService.Service.Services.Submission.Applicant;
using RivTech.CABoxTruck.WebService.Service.Services.Submission.DataSets;
using RivTech.CABoxTruck.WebService.Service.Services.Submission.RiskSpecifics;
using RivTech.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using RivTech.CABoxTruck.WebService.Data.Common;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Submission;
using RivTech.CABoxTruck.WebService.DataProvider.Repositories.Submission;
using RivTech.CABoxTruck.WebService.Service.IServices.DocumentGeneration;
using RivTech.CABoxTruck.WebService.Service.IServices.Risk;
using RivTech.CABoxTruck.WebService.Service.IServices.Submission;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.PolicyPacket;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.QuoteReport;
using RivTech.CABoxTruck.WebService.Service.Services.Risk;
using RivTech.CABoxTruck.WebService.Service.Services.Submission;
using Microsoft.Extensions.Logging;
using RivTech.CABoxTruck.WebService.DataProvider.Repositories.Billing;
using RivTech.CABoxTruck.WebService.Service.Services.Billing;
using RivTech.CABoxTruck.WebService.Service.IServices.Billing;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Billing;
using RivTech.CABoxTruck.WebService.DTO.Rater;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.InvoiceReport;
using RivTech.CABoxTruck.WebService.DataProvider.Repositories.Policy.DataSets;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Policy.DataSets;
using RivTech.CABoxTruck.WebService.Service.Services.Endorsement.Proration;
using RivTech.CABoxTruck.WebService.Service.Services.Policy;
using RivTech.CABoxTruck.WebService.Service.IServices.Policy;
using RivTech.CABoxTruck.WebService.DataProvider.Repositories.Policy;
using RivTech.CABoxTruck.WebService.DataProvider.Repositories.Dashboard;
using RivTech.CABoxTruck.WebService.Service.IServices.Dashboard;
using RivTech.CABoxTruck.WebService.Service.Services.Dashboard;
using RivTech.CABoxTruck.WebService.Service.IServices.User;
using System.Data;
using System.Data.SqlClient;
using RivTech.CABoxTruck.WebService.DALDapper.IDataAccess;
using RivTech.CABoxTruck.WebService.DALDapper.DataAccess;
using RivTech.CABoxTruck.WebService.Common;
using RivTech.CABoxTruck.WebService.ExternalData.Context;
using RivTech.CABoxTruck.WebService.Service.Services.AzureFunctions;
using RivTech.CABoxTruck.WebService.Service.Services.Emails;
using RivTech.CABoxTruck.WebService.Service.IServices.Emails;
using RivTech.CABoxTruck.WebService.Service.Services.Rater.Mapping;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement;
using RivTech.CABoxTruck.WebService.Service.Services.Endorsement;
using RivTech.CABoxTruck.WebService.Service.Services.Rater;
using RivTech.CABoxTruck.WebService.Service.Services.Rater.ListParsers;
using RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItemModifiers;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItems;
using RivTech.CABoxTruck.WebService.Service.Services.Premiums;
using RivTech.CABoxTruck.WebService.DataProvider.Repositories.Vehicles;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Vehicles;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Premiums;
using RivTech.CABoxTruck.WebService.DataProvider.Repositories.Premiums;
using RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeComparers;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeComparer;
using RivTech.CABoxTruck.WebService.Service.Services.Billing.Reports;
using RivTech.CABoxTruck.WebService.StartUp.DependencyInjection;
using RivTech.CABoxTruck.WebService.Service.Services.Endorsement.DataFix;
using RivTech.CABoxTruck.WebService.StartUp.AutofacModules;
using RivTech.CABoxTruck.WebService.BackgroundServices;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Text.Json.Serialization;
using Dapper;
using Quartz;
using System.Collections.Specialized;
using Quartz.Impl;
using System.Threading.Tasks;
using RivTech.CABoxTruck.WebService.BackgroundServices.Schedulers;
using RivTech.CABoxTruck.WebService.Service.Services.BackgroundServices.ISchedulers;
using RivTech.CABoxTruck.WebService.Service.Services.BackgroundServices.JobFactories;
using RivTech.CABoxTruck.WebService.BackgroundServices.Jobs;
using RivTech.CABoxTruck.WebService.Service.Services.Report;
using RivTech.CABoxTruck.WebService.Service.IServices.Report;

namespace RivTech.CABoxTruck.WebService.Web
{
    public class Startup
    {
        private const string POLICY = "CABoxTruckPolicy";

        private IScheduler _quartzScheduler;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _quartzScheduler = ConfigureQuartz();
        }

        public IConfiguration Configuration { get; }

        private readonly DateTime _versionDate = DateTime.Now;
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(o => o.AddPolicy("Development", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            services.AddControllers(options =>
            {
                options.Filters.Add(new AuthorizeFilter(new AuthorizationPolicyBuilder()
                             .RequireAuthenticatedUser()
                             .Build()));
            }).AddFluentValidation()
            .AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve)
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            services.AddSwaggerGen(cc =>
            {
                cc.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "CABoxTruck",
                    Version = $"{_versionDate:MMddyyyy}" + "-1",
                    Description = "An API to perform Pre-Defined data operations."
                });


                cc.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });


                cc.AddSecurityRequirement(new OpenApiSecurityRequirement()
                  {
                    {
                      new OpenApiSecurityScheme
                      {
                        Reference = new OpenApiReference
                          {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                          },
                          Scheme = "oauth2",
                          Name = "Bearer",
                          In = ParameterLocation.Header,

                        },
                        new List<string>()
                      }
                    });
            });

            services.AddMemoryCache();

            services.AddAuthorization(options =>
            {
                options.AddPolicy(POLICY, policy =>
                {
                    policy.RequireAuthenticatedUser();
                    //policy.RequireClaim("applications", "CABoxTruck");
                });
            });

            SetAutoMapper(services); //Set AutoMapper
            SetUpValidators(services);
            RegisterDatabase(services);
            JWTSecurity.RegisterJWTAuthentication(services);

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState
                    .Where(e => e.Value.Errors.Count > 0)
                    .SelectMany(x => x.Value.Errors)
                    .Select(x => x.ErrorMessage)
                    .ToArray();

                    var errorResponse = new ValidationErrorResponse { Errors = errors };
                    return new BadRequestObjectResult(errorResponse.ToString());
                };
            });
            //services.AddTransient<EndorsementProcessJob>();
            services.AddSingleton(provider => _quartzScheduler);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddHttpClient("rater", client =>
            {
                client.BaseAddress = new Uri(Configuration["Rater:RaterApiUrl"]);
                client.DefaultRequestHeaders.Add("XApiKey", Configuration["Rater:ApiKey"]);
                client.Timeout = TimeSpan.FromMinutes(10);
            });

            services.Configure<AzureFunctionAppSettings>(Configuration.GetSection(AzureFunctionAppSettings.APP_SETTINGS_SECTION_KEY));
            services.AddHttpClient<IAzureFunctionApiService, AzureFunctionApiService>();

            //services.AddHostedService<EndorsementProcessBackgroundService>();
        }

        private void OnShutdown()
        {
            if (!_quartzScheduler.IsShutdown) _quartzScheduler.Shutdown();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory, AppDbContext appDbContext, AppDbExternalContext appDbExternalContext, ILogger<Startup> logger, IHostApplicationLifetime applicationLifetime)
        {
            SqlMapper.Settings.CommandTimeout = 600;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            Services.HttpContextAccessor = app.ApplicationServices.GetRequiredService<IHttpContextAccessor>();


            _quartzScheduler.JobFactory = new BoxtruckJobFactory(app.ApplicationServices);
            app.UseErrorLogging();
            app.UseStatusCodePagesWithReExecute("/errors/{0}");
            loggerFactory.AddLog4Net();
            app.UseRateLimit();

            app.UseCors("Development");
            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers()
                    .RequireAuthorization(POLICY);
            });

            bool.TryParse(Configuration["Swagger:Enabled"].ToString(), out bool swaggerEnabled);
            logger.LogDebug($"Swagger enabled: {swaggerEnabled}");
            if (swaggerEnabled)
            {
                app.UseSwagger(o =>
                {
                    o.RouteTemplate = "Documents/{documentName}/docs.json";
                });

                app.UseSwaggerUI(c =>
                {
                    c.DefaultModelsExpandDepth(-1);
                    c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
                    c.RoutePrefix = "Documents";
                    c.SwaggerEndpoint("/Documents/v1/docs.json", "Generic Services");

                });
            }

            PremiumRaterVehicleFactorsDTO.VehicleIdMappings = new Dictionary<Guid, Dictionary<int, Guid>>();

            appDbContext.Database.Migrate();
            appDbExternalContext.Database.Migrate();

            applicationLifetime.ApplicationStopping.Register(OnShutdown);
        }

        private void SetAutoMapper(IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapperProfile());
                mc.AddProfile(new ReportMapperProfile());
                mc.AddProfile(new NotificationMapperProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
        }

        #region Dependency Injection
        public void ConfigureContainer(ContainerBuilder builder)
        {
            #region Service            
            builder.RegisterType<EntityService>().As<IEntityService>().InstancePerLifetimeScope();
            builder.RegisterType<Log4NetService>().As<ILog4NetService>().InstancePerLifetimeScope();
            builder.RegisterType<AdditionalTermService>().As<IAdditionalTermService>().InstancePerLifetimeScope();
            builder.RegisterType<BusinessTypeService>().As<IBusinessTypeService>().InstancePerLifetimeScope();
            builder.RegisterType<PaymentPlanService>().As<IPaymentPlanService>().InstancePerLifetimeScope();
            builder.RegisterType<RateTypeService>().As<IRateTypeService>().InstancePerLifetimeScope();
            builder.RegisterType<RelatedEntityTypeService>().As<IRelatedEntityTypeService>().InstancePerLifetimeScope();
            builder.RegisterType<RiskStatusTypeService>().As<IRiskStatusTypeService>().InstancePerLifetimeScope();
            builder.RegisterType<RiskTypeService>().As<IRiskTypeService>().InstancePerLifetimeScope();
            builder.RegisterType<EntityContactService>().As<IEntityContactService>().InstancePerLifetimeScope();
            builder.RegisterType<EntitySubsidiaryService>().As<IEntitySubsidiaryService>().InstancePerLifetimeScope();
            builder.RegisterType<ApplicantAdditionalInterestService>().As<IApplicantAdditionalInterestService>().InstancePerLifetimeScope();
            builder.RegisterType<QuestionService>().As<IQuestionService>().InstancePerLifetimeScope();
            builder.RegisterType<RiskResponseService>().As<IRiskResponseService>().InstancePerLifetimeScope();
            builder.RegisterType<RiskService>().As<IRiskService>().InstancePerLifetimeScope();
            builder.RegisterType<RiskSummaryService>().As<IRiskSummaryService>().InstancePerLifetimeScope();
            builder.RegisterType<RiskSequenceService>().As<IRiskSequenceService>().InstancePerLifetimeScope();
            builder.RegisterType<CoverageLimitService>().As<ICoverageLimitService>().InstancePerLifetimeScope();
            builder.RegisterType<CoverageHistoryService>().As<ICoverageHistoryService>().InstancePerLifetimeScope();
            builder.RegisterType<ClaimsHistoryService>().As<IClaimsHistoryService>().InstancePerLifetimeScope();
            builder.RegisterType<RiskStatusHistoryService>().As<IRiskStatusHistoryService>().InstancePerLifetimeScope();

            builder.RegisterType<NameAndAddressService>().As<INameAndAddressService>().InstancePerLifetimeScope();
            builder.RegisterType<BusinessDetailsService>().As<IBusinessDetailsService>().InstancePerLifetimeScope();
            builder.RegisterType<DriverService>().As<IDriverService>().InstancePerLifetimeScope();
            builder.RegisterType<UserInfoService>().As<IUserInfoService>().InstancePerLifetimeScope();
            builder.RegisterType<VehicleService>().As<IVehicleService>().InstancePerLifetimeScope();
            builder.RegisterType<RiskSpecificDestinationService>().As<IRiskSpecificDestinationService>().InstancePerLifetimeScope();
            builder.RegisterType<RiskSpecificCommoditiesHauledService>().As<IRiskSpecificCommoditiesHauledService>().InstancePerLifetimeScope();
            builder.RegisterType<RiskSpecificDriverInfoService>().As<IRiskSpecificDriverInfoService>().InstancePerLifetimeScope();
            builder.RegisterType<RiskSpecificDotInfoService>().As<IRiskSpecificDotInfoService>().InstancePerLifetimeScope();
            builder.RegisterType<RiskSpecificRadiusOfOperationService>().As<IRiskSpecificRadiusOfOperationService>().InstancePerLifetimeScope();
            builder.RegisterType<RiskSpecificMaintenanceSafetyService>().As<IRiskSpecificMaintenanceSafetyService>().InstancePerLifetimeScope();
            builder.RegisterType<RiskSpecificSafetyDeviceService>().As<IRiskSpecificSafetyDeviceService>().InstancePerLifetimeScope();
            builder.RegisterType<RiskSpecificDriverHiringCriteriaService>().As<IRiskSpecificDriverHiringCriteriaService>().InstancePerLifetimeScope();
            builder.RegisterType<RiskSpecificGeneralLiabilityCargoService>().As<IRiskSpecificGeneralLiabilityCargoService>().InstancePerLifetimeScope();
            builder.RegisterType<RiskSpecificUnderwritingQuestionService>().As<IRiskSpecificUnderwritingQuestionService>().InstancePerLifetimeScope();

            builder.RegisterType<RiskFactory>().As<IRiskFactory>().InstancePerLifetimeScope();

            builder.RegisterType<TransactionService>().As<ITransactionService>().InstancePerLifetimeScope();

            builder.RegisterType<BrokerInfoService>().As<IBrokerInfoService>().InstancePerLifetimeScope();

            builder.RegisterType<AgencyService>().As<IAgencyService>().InstancePerLifetimeScope();
            builder.RegisterType<SubAgencyService>().As<ISubAgencyService>().InstancePerLifetimeScope();
            builder.RegisterType<AgentService>().As<IAgentService>().InstancePerLifetimeScope();
            builder.RegisterType<SubAgentService>().As<ISubAgentService>().InstancePerLifetimeScope();
            builder.RegisterType<ReasonMoveService>().As<IReasonMoveService>().InstancePerLifetimeScope();

            builder.RegisterType<FilingsInformationService>().As<IFilingsInformationService>().InstancePerLifetimeScope();
            builder.RegisterType<RiskFilingService>().As<IRiskFilingService>().InstancePerLifetimeScope();

            builder.RegisterType<RiskSpecificDriverInfoPageService>().As<IRiskSpecificDriverInfoPageService>().InstancePerLifetimeScope();
            builder.RegisterType<DriverInfoOptionsService>().As<IDriverInfoOptionsService>().InstancePerLifetimeScope();
            builder.RegisterType<DotInfoOptionsService>().As<IDotInfoOptionsService>().InstancePerLifetimeScope();

            builder.RegisterType<QuoteOptionsService>().As<IQuoteOptionsService>().InstancePerLifetimeScope();

            builder.RegisterType<RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.ReportService>().As<RivTech.CABoxTruck.WebService.Service.IServices.DocumentGeneration.IReportService>().InstancePerLifetimeScope();
            builder.RegisterType<QuoteReportService>().As<IQuoteReportService>().InstancePerLifetimeScope();
            builder.RegisterType<PacketReportService>().As<IPacketReportService>().InstancePerLifetimeScope();
            builder.RegisterType<ContentService>().As<IContentService>().InstancePerLifetimeScope();

            builder.RegisterType<LvDatasetService>().As<ILvDatasetService>().InstancePerLifetimeScope();
            builder.RegisterType<LimitsService>().As<ILimitsService>().InstancePerLifetimeScope();
            builder.RegisterType<RiskNotesService>().As<IRiskNotesService>().InstancePerLifetimeScope();
            builder.RegisterType<FormsService>().As<IFormsService>().InstancePerLifetimeScope();
            builder.RegisterType<AzureStorageService>().As<IStorageService>().InstancePerLifetimeScope();

            builder.RegisterType<BindingService>().As<IBindingService>().InstancePerLifetimeScope();
            builder.RegisterType<FileUploadDocumentService>().As<IFileUploadDocumentService>().InstancePerLifetimeScope();

            builder.RegisterType<PolicyContactsService>().As<IPolicyContactsService>().InstancePerLifetimeScope();

            builder.RegisterType<RiskPremiumFeesFactory>().As<IRiskPremiumFeesFactory>().InstancePerLifetimeScope();
            builder.RegisterType<TaxDetailsFactory>().As<ITaxDetailsFactory>().InstancePerLifetimeScope();
            builder.RegisterType<TaxService>().As<ITaxService>().InstancePerLifetimeScope();

            builder.RegisterType<SLNumberService>().As<ISLNumberService>().InstancePerLifetimeScope();

            builder.RegisterType<BannerService>().As<IBannerService>().InstancePerLifetimeScope();

            builder.RegisterType<BillingService>().As<IBillingService>().InstancePerLifetimeScope();
            builder.RegisterType<ApplogService>().As<IApplogService>().InstancePerLifetimeScope();
            builder.RegisterType<RiskManuscriptsService>().As<IRiskManuscriptsService>().InstancePerLifetimeScope();
            builder.RegisterType<BillingService>().As<IBillingService>().InstancePerLifetimeScope();
            builder.RegisterType<PolicyHistoryService>().As<IPolicyHistoryService>().InstancePerLifetimeScope();
            builder.RegisterType<InvoiceReportService>().As<IInvoiceReportService>().InstancePerLifetimeScope();
            builder.RegisterType<EndorsementPendingChangeService>().As<IEndorsementPendingChangeService>().InstancePerLifetimeScope();
            builder.RegisterType<EndorsementService>().As<IEndorsementService>().InstancePerLifetimeScope();
            builder.RegisterType<CancellationService>().As<ICancellationService>().InstancePerLifetimeScope();
            builder.RegisterType<ReinstatementService>().As<IReinstatementService>().InstancePerLifetimeScope();
            builder.RegisterType<AmountFactory>().As<IAmountFactory>().InstancePerLifetimeScope();

            builder.RegisterType<DashboardService>().As<IDashboardService>().InstancePerLifetimeScope();
            builder.RegisterType<RetailerService>().As<IRetailerService>().InstancePerLifetimeScope();
            builder.RegisterType<RetailerAgentService>().As<IRetailerAgentService>().InstancePerLifetimeScope();
            builder.RegisterType<BlacklistedDriverService>().As<IBlacklistedDriverService>().InstancePerLifetimeScope();
            builder.RegisterType<RiskPolicyContactService>().As<IRiskPolicyContactService>().InstancePerLifetimeScope();
            builder.RegisterType<RivTech.CABoxTruck.WebService.Service.Services.Report.ReportService>().As<RivTech.CABoxTruck.WebService.Service.IServices.Report.IReportService>().InstancePerLifetimeScope();
            builder.RegisterType<NotificationService>().As<INotificationService>().InstancePerLifetimeScope();

            builder.RegisterType<AccountsReceivableService>().As<IAccountsReceivableService>().InstancePerLifetimeScope();
            builder.RegisterType<EmailQueueService>().As<IEmailQueueService>().InstancePerLifetimeScope();
            builder.RegisterType<EmailQueueArchiveService>().As<IEmailQueueArchiveService>().InstancePerLifetimeScope();
            builder.RegisterType<FTPClientServerSevice>().As<IFTPClientServerSevice>().InstancePerLifetimeScope();
            builder.RegisterType<InvoiceEmailService>().As<IInvoiceEmailService>().InstancePerLifetimeScope();
            builder.RegisterType<PaymentPostingService>().As<IPaymentPostingService>().InstancePerLifetimeScope();
            builder.RegisterType<BrokerCommissionService>().As<IBrokerCommissionService>().InstancePerLifetimeScope();
            builder.RegisterType<EndorsementEmailService>().As<IEndorsementEmailService>().InstancePerLifetimeScope();
            builder.RegisterType<PreBindPaymentService>().As<IPreBindPaymentService>().InstancePerLifetimeScope();
            builder.RegisterType<NOPCEmailService>().As<INOPCEmailService>().InstancePerLifetimeScope();
            builder.RegisterType<EndorsementChangesService>().As<IEndorsementChangesService>().InstancePerLifetimeScope();
            builder.RegisterType<RiskVehiclePremiumService>().As<IRiskVehiclePremiumService>().InstancePerLifetimeScope();
            builder.RegisterType<PremiumService>().As<IPremiumService>().InstancePerLifetimeScope();
            builder.RegisterType<PaymentReceiptService>().As<IPaymentReceiptService>().InstancePerLifetimeScope();
            builder.RegisterType<PolicyStatementReportService>().As<IPolicyStatementReportService>().InstancePerLifetimeScope();
            builder.RegisterType<CovenirReportService>().As<ICovenirReportService>().InstancePerLifetimeScope();
            builder.RegisterType<SubmissionQuoteReportService>().As<ISubmissionQuoteReportService>().InstancePerLifetimeScope();
            builder.RegisterType<FilingsReportService>().As<IFilingsReportService>().InstancePerLifetimeScope();

            builder.RegisterType<RaterInputMapperResolver>().As<IRaterInputMapperResolver>().InstancePerLifetimeScope();
            builder.RegisterType<PolicyInputMapper>().As<IRaterInputMapper>().InstancePerLifetimeScope();
            builder.RegisterType<BrokerInputMapper>().As<IRaterInputMapper>().InstancePerLifetimeScope();
            builder.RegisterType<HistoricalCoverageInputMapper>().As<IRaterInputMapper>().InstancePerLifetimeScope();
            builder.RegisterType<ClaimsHistoryInputMapper>().As<IRaterInputMapper>().InstancePerLifetimeScope();
            builder.RegisterType<RiskSpecificsInputMapper>().As<IRaterInputMapper>().InstancePerLifetimeScope();
            builder.RegisterType<RequestedCoveragesInputMapper>().As<IRaterInputMapper>().InstancePerLifetimeScope();


            builder.RegisterType<EndorsementProcessor>().As<IEndorsementProcessor>().InstancePerLifetimeScope();
            builder.RegisterType<EndorsementDataCloner>().As<IEndorsementDataCloner>().InstancePerLifetimeScope();
            builder.RegisterType<EndorsementDataCleaner>().As<IEndorsementDataCleaner>().InstancePerLifetimeScope();
            builder.RegisterType<EndorsementDataPremiumRater>().As<IEndorsementDataPremiumRater>().InstancePerLifetimeScope();
            builder.RegisterType<EndorsementDataPremiumProrator>().As<IEndorsementDataPremiumProrator>().InstancePerLifetimeScope();
            builder.RegisterType<RiskPremiumTransactionService>().As<IRiskPremiumTransactionService>().InstancePerLifetimeScope();
            builder.RegisterType<RiskPremiumTransactionFactory>().As<IRiskPremiumTransactionFactory>().InstancePerLifetimeScope();
            builder.RegisterType<EndorsementChangeItemModifierResolver>().As<IEndorsementChangeItemModifierResolver>().InstancePerLifetimeScope();
            builder.RegisterType<ListValueTextParser>().As<IListValueTextParser>().InstancePerLifetimeScope();
            builder.RegisterType<EndorsementChangesService>().As<IEndorsementChangesService>().InstancePerLifetimeScope();
            builder.RegisterType<EndorsementChangeDeserializer>().As<IEndorsementChangeDeserializer>().InstancePerLifetimeScope();
            builder.RegisterType<ChangeComparerContext>().As<IChangeComparerContext>().InstancePerLifetimeScope();

            builder.RegisterType<OOSDataFixService>().As<IOOSDataFixService>().InstancePerLifetimeScope();

            builder.RegisterType<SASURLGeneratorService>().As<ISASURLGeneratorService>().InstancePerLifetimeScope();

            //builder.RegisterType<EndorsementProcessScheduler>().As<IEndorsementProcessScheduler>().InstancePerLifetimeScope();

            RegisterListParsers(builder);
            RegisterEndorsementChangeItemModifers(builder);
            RegisterEndorsementChangeComparers(builder);

            #endregion

            #region Modules 

            builder.RegisterModule(new DriverServicesModule());
            builder.RegisterModule(new ServicesModule());
            builder.RegisterModule(new RepositoryModule());
            builder.RegisterModule(new RaterServicesModule());
            builder.RegisterModule(new EndorsementServicesModule());

            #endregion

            #region DataProvider
            builder.RegisterType<AddressRepository>().As<IAddressRepository>().InstancePerLifetimeScope();
            builder.RegisterType<RiskDetailRepository>().As<IRiskDetailRepository>().InstancePerLifetimeScope();
            builder.RegisterType<RiskRepository>().As<IRiskRepository>().InstancePerLifetimeScope();
            builder.RegisterType<AdditionalTermRepository>().As<IAdditionalTermRepository>().InstancePerLifetimeScope();
            builder.RegisterType<BusinessTypeRepository>().As<IBusinessTypeRepository>().InstancePerLifetimeScope();
            builder.RegisterType<PaymentPlanRepository>().As<IPaymentPlanRepository>().InstancePerLifetimeScope();
            builder.RegisterType<RateTypeRepository>().As<IRateTypeRepository>().InstancePerLifetimeScope();
            builder.RegisterType<RelatedEntityTypeRepository>().As<IRelatedEntityTypeRepository>().InstancePerLifetimeScope();
            builder.RegisterType<RiskStatusTypeRepository>().As<IRiskStatusTypeRepository>().InstancePerLifetimeScope();
            builder.RegisterType<RiskTypeRepository>().As<IRiskTypeRepository>().InstancePerLifetimeScope();
            builder.RegisterType<EntityRepository>().As<IEntityRepository>().InstancePerLifetimeScope();
            builder.RegisterType<EntityContactRepository>().As<IEntityContactRepository>().InstancePerLifetimeScope();
            builder.RegisterType<EntitySubsidiaryRepository>().As<IEntitySubsidiaryRepository>().InstancePerLifetimeScope();
            builder.RegisterType<EntityAddressRepository>().As<IEntityAddressRepository>().InstancePerLifetimeScope();
            builder.RegisterType<RiskAdditionalInterestRepository>().As<IRiskAdditionalInterestRepository>().InstancePerLifetimeScope();

            builder.RegisterType<AppLogRepository>().As<IAppLogRepository>().InstancePerLifetimeScope();
            builder.RegisterType<AuditLogRepository>().As<IAuditLogRepository>().InstancePerLifetimeScope();
            builder.RegisterType<QuestionRepository>().As<IQuestionRepository>().InstancePerLifetimeScope();
            builder.RegisterType<RiskResponseRepository>().As<IRiskResponseRepository>().InstancePerLifetimeScope();
            builder.RegisterType<RiskCoverageRepository>().As<IRiskCoverageRepository>().InstancePerLifetimeScope();
            builder.RegisterType<RiskHistoryRepository>().As<IRiskHistoryRepository>().InstancePerLifetimeScope();
            builder.RegisterType<ClaimsHistoryRepository>().As<IClaimsHistoryRepository>().InstancePerLifetimeScope();
            builder.RegisterType<RiskStatusHistoryRepository>().As<IRiskStatusHistoryRepository>().InstancePerLifetimeScope();

            builder.RegisterType<DriverRepository>().As<IDriverRepository>().InstancePerLifetimeScope();
            builder.RegisterType<DriverIncidentRepository>().As<IDriverIncidentsRepository>().InstancePerLifetimeScope();
            builder.RegisterType<DriverHeaderRepository>().As<IDriverHeaderRepository>().InstancePerLifetimeScope();

            builder.RegisterType<VehicleRepository>().As<IVehicleRepository>().InstancePerLifetimeScope();
            builder.RegisterType<RiskSpecificDestinationRepository>().As<IRiskSpecificDestinationRepository>().InstancePerLifetimeScope();
            builder.RegisterType<RiskSpecificCommoditiesHauledRepository>().As<IRiskSpecificCommoditiesHauledRepository>().InstancePerLifetimeScope();

            builder.RegisterType<BrokerInfoRepository>().As<IBrokerInfoRepository>().InstancePerLifetimeScope();
            builder.RegisterType<RiskSpecificDriverInfoRepository>().As<IRiskSpecificDriverInfoRepository>().InstancePerLifetimeScope();
            builder.RegisterType<RiskSpecificDotInfoRepository>().As<IRiskSpecificDotInfoRepository>().InstancePerLifetimeScope();
            builder.RegisterType<RiskSpecificRadiusOfOperationRepository>().As<IRiskSpecificRadiusOfOperationRepository>().InstancePerLifetimeScope();
            builder.RegisterType<RiskSpecificSafetyDeviceRepository>().As<IRiskSpecificSafetyDeviceRepository>().InstancePerLifetimeScope();
            builder.RegisterType<RiskSpecificUnderwritingQuestionRepository>().As<IRiskSpecificUnderwritingQuestionRepository>().InstancePerLifetimeScope();

            builder.RegisterType<AgencyRepository>().As<IAgencyRepository>().InstancePerLifetimeScope();
            builder.RegisterType<AgentRepository>().As<IAgentRepository>().InstancePerLifetimeScope();
            builder.RegisterType<SubAgencyRepository>().As<ISubAgencyRepository>().InstancePerLifetimeScope();
            builder.RegisterType<SubAgentRepository>().As<ISubAgentRepository>().InstancePerLifetimeScope();
            builder.RegisterType<ReasonMoveRepository>().As<IReasonMoveRepository>().InstancePerLifetimeScope();

            builder.RegisterType<RiskSpecificDriverHiringCriteriaRepository>().As<IRiskSpecificDriverHiringCriteriaRepository>().InstancePerLifetimeScope();
            builder.RegisterType<RiskFilingRepository>().As<IRiskFilingRepository>().InstancePerLifetimeScope();

            builder.RegisterType<MvrPullingFrequencyOptionRepository>().As<IMvrPullingFrequencyOptionRepository>().InstancePerLifetimeScope();
            builder.RegisterType<BackgroundCheckOptionRepository>().As<IBackgroundCheckOptionRepository>().InstancePerLifetimeScope();
            builder.RegisterType<DriverTrainingOptionRepository>().As<IDriverTrainingOptionRepository>().InstancePerLifetimeScope();
            builder.RegisterType<ChameleonIssuesOptionRepository>().As<IChameleonIssuesOptionRepository>().InstancePerLifetimeScope();
            builder.RegisterType<SaferRatingOptionRepository>().As<ISaferRatingOptionRepository>().InstancePerLifetimeScope();
            builder.RegisterType<TrafficLightRatingOptionRepository>().As<ITrafficLightRatingOptionRepository>().InstancePerLifetimeScope();

            builder.RegisterType<CacheService>().As<ICacheService>().InstancePerLifetimeScope();
            builder.RegisterType<KeyValueRepository>().As<IKeyValueRepository>().InstancePerLifetimeScope();

            builder.RegisterType<LvDatasetRepository>().As<ILvDatasetRepository>().InstancePerLifetimeScope();
            builder.RegisterType<LvLimitsRepository>().As<ILvLimitsRepository>().InstancePerLifetimeScope();
            builder.RegisterType<RiskNotesRepository>().As<IRiskNotesRepository>().InstancePerLifetimeScope();
            builder.RegisterType<FormRepository>().As<IFormRepository>().InstancePerLifetimeScope();
            builder.RegisterType<RiskFormRepository>().As<IRiskFormRepository>().InstancePerLifetimeScope();

            builder.RegisterType<BindingRepository>().As<IBindingRepository>().InstancePerLifetimeScope();
            builder.RegisterType<BindingRequirementsRepository>().As<IBindingRequirementsRepository>().InstancePerLifetimeScope();
            builder.RegisterType<QuoteConditionsRepository>().As<IQuoteConditionsRepository>().InstancePerLifetimeScope();

            builder.RegisterType<FileUploadDocumentRepository>().As<IFileUploadDocumentRepository>().InstancePerLifetimeScope();
            builder.RegisterType<PolicyContactsRepository>().As<IPolicyContactsRepository>().InstancePerLifetimeScope();

            builder.RegisterType<StateTaxRepository>().As<IStateTaxRepository>().InstancePerLifetimeScope();
            builder.RegisterType<TaxDetailsRepository>().As<ITaxDetailsRepository>().InstancePerLifetimeScope();
            builder.RegisterType<FeeRepository>().As<IFeeRepository>().InstancePerLifetimeScope();
            builder.RegisterType<SLNumberRepository>().As<ISLNumberRepository>().InstancePerLifetimeScope();
            builder.RegisterType<BannerRepository>().As<IBannerRepository>().InstancePerLifetimeScope();
            builder.RegisterType<ErrorLogRepository>().As<IErrorLogRepository>().InstancePerLifetimeScope();
            builder.RegisterType<RiskManuscriptsRepository>().As<IRiskManuscriptsRepository>().InstancePerLifetimeScope();
            builder.RegisterType<MainUseRepository>().As<IMainUseRepository>().InstancePerLifetimeScope();
            builder.RegisterType<PolicyHistoryRepository>().As<IPolicyHistoryRepository>().InstancePerLifetimeScope();
            builder.RegisterType<FileCategoryRepository>().As<IFileCategoryRepository>().InstancePerLifetimeScope();
            builder.RegisterType<EndorsementPendingChangeRepository>().As<IEndorsementPendingChangeRepository>().InstancePerLifetimeScope();
            builder.RegisterType<CancellationReasonsRepository>().As<ICancellationReasonsRepository>().InstancePerLifetimeScope();
            builder.RegisterType<PolicyContactTypesRepository>().As<IPolicyContactTypesRepository>().InstancePerLifetimeScope();

            builder.RegisterType<DashboardRepository>().As<IDashboardRepository>().InstancePerLifetimeScope();
            builder.RegisterType<SequenceRepository>().As<ISequenceRepository>().InstancePerLifetimeScope();
            builder.RegisterType<RetailerRepository>().As<IRetailerRepository>().InstancePerLifetimeScope();
            builder.RegisterType<RetailerAgentRepository>().As<IRetailerAgentRepository>().InstancePerLifetimeScope();
            builder.RegisterType<RetailerAgencyRepository>().As<IRetailerAgencyRepository>().InstancePerLifetimeScope();
            builder.RegisterType<BlacklistedDriverRepository>().As<IBlacklistedDriverRepository>().InstancePerLifetimeScope();
            builder.RegisterType<RiskPolicyContactRepository>().As<IRiskPolicyContactRepository>().InstancePerLifetimeScope();
            builder.RegisterType<ReportDataAccess>().As<IReportDataAccess>().InstancePerLifetimeScope();
            builder.RegisterType<ReportGenerator>().As<IReportGenerator>().InstancePerLifetimeScope();
            builder.RegisterType<AccountsReceivableRepository>().As<IAccountsReceivableRepository>().InstancePerLifetimeScope();
            builder.RegisterType<EmailQueueRepository>().As<IEmailQueueRepository>().InstancePerLifetimeScope();
            builder.RegisterType<EmailQueueArchiveRepository>().As<IEmailQueueArchiveRepository>().InstancePerLifetimeScope();
            builder.RegisterType<CancellationBreakdownRepository>().As<ICancellationBreakdownRepository>().InstancePerLifetimeScope();
            builder.RegisterType<ReinstatementBreakdownRepository>().As<IReinstatementBreakdownRepository>().InstancePerLifetimeScope();
            builder.RegisterType<VehiclePremiumRepository>().As<IVehiclePremiumRepository>().InstancePerLifetimeScope();
            builder.RegisterType<FTPDocumentRepository>().As<IFTPDocumentRepository>().InstancePerLifetimeScope();
            builder.RegisterType<FTPDocumentTemporaryRepository>().As<IFTPDocumentTemporaryRepository>().InstancePerLifetimeScope();
            builder.RegisterType<FTPDocumentFilesCountRepository>().As<IFTPDocumentFilesCountRepository>().InstancePerLifetimeScope();
            builder.RegisterType<PaymentPostingRepository>().As<IPaymentPostingRepository>().InstancePerLifetimeScope();
            builder.RegisterType<BrokerCommissionRepository>().As<IBrokerCommissionRepository>().InstancePerLifetimeScope();
            builder.RegisterType<PreBindPaymentRepository>().As<IPreBindPaymentRepository>().InstancePerLifetimeScope();
            builder.RegisterType<EndorsementDataRepository>().As<IEndorsementDataRepository>().InstancePerLifetimeScope();
            builder.RegisterType<EndorsementChangesRepository>().As<IEndorsementChangesRepository>().InstancePerLifetimeScope();
            builder.RegisterType<VehiclePremiumRatingFactorRepository>().As<IVehiclePremiumRatingFactorRepository>().InstancePerLifetimeScope();
            builder.RegisterType<RiskPolicyPremiumFactory>().As<IRiskPolicyPremiumFactory>().InstancePerLifetimeScope();
            builder.RegisterType<RiskVehiclePremiumFactory>().As<IRiskVehiclePremiumFactory>().InstancePerLifetimeScope();
            builder.RegisterType<RiskVehiclePremiumRepository>().As<IRiskVehiclePremiumRepository>().InstancePerLifetimeScope();
            builder.RegisterType<RiskPolicyPremiumRepository>().As<IRiskPolicyPremiumRepository>().InstancePerLifetimeScope();
            builder.RegisterType<RiskVehiclePremiumTransactionRepository>().As<IRiskVehiclePremiumTransactionRepository>().InstancePerLifetimeScope();
            builder.RegisterType<RiskPolicyPremiumTransactionRepository>().As<IRiskPolicyPremiumTransactionRepository>().InstancePerLifetimeScope();
            builder.RegisterType<SubmissionQuoteReportRepository>().As<ISubmissionQuoteReportRepository>().InstancePerLifetimeScope();
            builder.RegisterType<FilingsReportRepository>().As<IFilingsReportRepository>().InstancePerLifetimeScope();
            builder.RegisterType<SubmissionPolicyDataAccess>().As<ISubmissionPolicyDataAccess>().InstancePerLifetimeScope();
            builder.RegisterType<NotificationDataAccess>().As<INotificationDataAccess>().InstancePerLifetimeScope();

            builder.RegisterType<GenericsRepository>().As<IGenericsRepository>().InstancePerLifetimeScope();
            builder.RegisterType<RiskCoveragePremiumAvgRepository>().As<IRiskCoveragePremiumAvgRepository>().InstancePerLifetimeScope();

            #endregion
        }



        private void SetUpValidators(IServiceCollection services)
        {
        }

        public void RegisterListParsers(ContainerBuilder builder)
        {
            builder.RegisterType<BusinessTypeParser>().As<IListParser>().InstancePerLifetimeScope();
            builder.RegisterType<AccountCategoryParser>().As<IListParser>().InstancePerLifetimeScope();
            builder.RegisterType<MainUseParser>().As<IListParser>().InstancePerLifetimeScope();
            builder.RegisterType<NewVentureParser>().As<IListParser>().InstancePerLifetimeScope();
            builder.RegisterType<SymbolsParser>().As<IListParser>().InstancePerLifetimeScope();
            builder.RegisterType<VehicleDescriptionParser>().As<IListParser>().InstancePerLifetimeScope();
            builder.RegisterType<MainGVWParser>().As<IListParser>().InstancePerLifetimeScope();
            builder.RegisterType<MainGVWInLbs>().As<IListParser>().InstancePerLifetimeScope();
            builder.RegisterType<RatingTypeParser>().As<IListParser>().InstancePerLifetimeScope();
            builder.RegisterType<LimitsParser>().As<IListParser>().InstancePerLifetimeScope();
            builder.RegisterType<SecondaryBusinessGroupParser>().As<IListParser>().InstancePerLifetimeScope();
        }

        public void RegisterEndorsementChangeItemModifers(ContainerBuilder builder)
        {
            //builder.RegisterType<AddVehicleChangeModifier>().As<IEndorsementChangeItemModifier<AddVehicleChangeItem>>().InstancePerLifetimeScope();
            builder.RegisterType<AdditionalInterestModifier>().As<IEndorsementChangeItemModifier>().InstancePerLifetimeScope();
            builder.RegisterType<BindRequirementsModifier>().As<IEndorsementChangeItemModifier>().InstancePerLifetimeScope();
            builder.RegisterType<BusinessDetailsModifier>().As<IEndorsementChangeItemModifier>().InstancePerLifetimeScope();
            builder.RegisterType<BusinessNameModifier>().As<IEndorsementChangeItemModifier>().InstancePerLifetimeScope();
            builder.RegisterType<CargoChangeModifier>().As<IEndorsementChangeItemModifier>().InstancePerLifetimeScope();
            builder.RegisterType<CarrierInfoModifier>().As<IEndorsementChangeItemModifier>().InstancePerLifetimeScope();
            builder.RegisterType<CoverageLimitsModifier>().As<IEndorsementChangeItemModifier>().InstancePerLifetimeScope();
            builder.RegisterType<FormDriverRequirementsEndorsementModifier>().As<IEndorsementChangeItemModifier>().InstancePerLifetimeScope();
            builder.RegisterType<FormExclusionRadiusEndorsementModifier>().As<IEndorsementChangeItemModifier>().InstancePerLifetimeScope();
            builder.RegisterType<FormGeneralLiabilityEndorsementModifier>().As<IEndorsementChangeItemModifier>().InstancePerLifetimeScope();
            builder.RegisterType<FormHiredPhysicalDamageEndorsementModifier>().As<IEndorsementChangeItemModifier>().InstancePerLifetimeScope();
            builder.RegisterType<FormLossPayeeModifier>().As<IEndorsementChangeItemModifier>().InstancePerLifetimeScope();
            builder.RegisterType<FormMCS90Modifier>().As<IEndorsementChangeItemModifier>().InstancePerLifetimeScope();
            builder.RegisterType<FormSelectionModifier>().As<IEndorsementChangeItemModifier>().InstancePerLifetimeScope();
            builder.RegisterType<FormTerritoryExclusionEndorsementModifier>().As<IEndorsementChangeItemModifier>().InstancePerLifetimeScope();
            builder.RegisterType<FormTrailerInterchangeCoverageModifier>().As<IEndorsementChangeItemModifier>().InstancePerLifetimeScope();
            builder.RegisterType<GeneralLiabilityModifier>().As<IEndorsementChangeItemModifier>().InstancePerLifetimeScope();
            builder.RegisterType<PolicyContactsModifier>().As<IEndorsementChangeItemModifier>().InstancePerLifetimeScope();

            builder.RegisterType<VehicleRemoveChangeModifier>().As<IEndorsementChangeItemModifier>().InstancePerLifetimeScope();
            builder.RegisterType<VehicleAddChangeModifier>().As<IEndorsementChangeItemModifier>().InstancePerLifetimeScope();
            builder.RegisterType<VehicleReinstateChangeModifier>().As<IEndorsementChangeItemModifier>().InstancePerLifetimeScope();
            builder.RegisterType<VehicleUpdateChangeModifier>().As<IEndorsementChangeItemModifier>().InstancePerLifetimeScope();

            builder.RegisterType<DriverAddChangeModifier>().As<IEndorsementChangeItemModifier>().InstancePerLifetimeScope();
            builder.RegisterType<DriverUpdateChangeModifier>().As<IEndorsementChangeItemModifier>().InstancePerLifetimeScope();
            builder.RegisterType<DriverRemoveChangeModifier>().As<IEndorsementChangeItemModifier>().InstancePerLifetimeScope();
            builder.RegisterType<DriverReinstateChangeModifier>().As<IEndorsementChangeItemModifier>().InstancePerLifetimeScope();

            builder.RegisterType<CommoditiesHauledModifier>().As<IEndorsementChangeItemModifier>().InstancePerLifetimeScope();
            builder.RegisterType<AddressesModifier>().As<IEndorsementChangeItemModifier>().InstancePerLifetimeScope();
            builder.RegisterType<ManuscriptModifier>().As<IEndorsementChangeItemModifier>().InstancePerLifetimeScope();
            builder.RegisterType<RatingFactorsChangeModifier>().As<IEndorsementChangeItemModifier>().InstancePerLifetimeScope();
            builder.RegisterType<CoveredAutoDesignationSymbolModifier>().As<IEndorsementChangeItemModifier>().InstancePerLifetimeScope();
        }

        public void RegisterEndorsementChangeComparers(ContainerBuilder builder)
        {
            builder.RegisterType<AdditionalInterestComparer>().As<IChangeComparer>().InstancePerLifetimeScope();
            builder.RegisterType<BindRequirementsComparer>().As<IChangeComparer>().InstancePerLifetimeScope();
            builder.RegisterType<BusinessDetailsComparer>().As<IChangeComparer>().InstancePerLifetimeScope();
            builder.RegisterType<BusinessNameComparer>().As<IChangeComparer>().InstancePerLifetimeScope();
            builder.RegisterType<CargoChangeComparer>().As<IChangeComparer>().InstancePerLifetimeScope();
            builder.RegisterType<CarrierInfoComparer>().As<IChangeComparer>().InstancePerLifetimeScope();
            builder.RegisterType<CoverageLimitsComparer>().As<IChangeComparer>().InstancePerLifetimeScope();
            //builder.RegisterType<FormDriverRequirementsEndorsementComparer>().As<IChangeComparer>().InstancePerLifetimeScope();
            //builder.RegisterType<FormExclusionRadiusEndorsementComparer>().As<IChangeComparer>().InstancePerLifetimeScope();
            //builder.RegisterType<FormGeneralLiabilityEndorsementComparer>().As<IChangeComparer>().InstancePerLifetimeScope();
            //builder.RegisterType<FormLossPayeeComparer>().As<IChangeComparer>().InstancePerLifetimeScope();
            //builder.RegisterType<FormMCS90Comparer>().As<IChangeComparer>().InstancePerLifetimeScope();
            builder.RegisterType<FormSelectionComparer>().As<IChangeComparer>().InstancePerLifetimeScope();
            //builder.RegisterType<FormTerritoryExclusionEndorsementComparer>().As<IChangeComparer>().InstancePerLifetimeScope();
            builder.RegisterType<FormHiredPhysicalDamageEndorsementComparer>().As<IChangeComparer>().InstancePerLifetimeScope();
            builder.RegisterType<FormTrailerInterchangeCoverageComparer>().As<IChangeComparer>().InstancePerLifetimeScope();
            builder.RegisterType<GeneralLiabilityComparer>().As<IChangeComparer>().InstancePerLifetimeScope();
            builder.RegisterType<PolicyContactsComparer>().As<IChangeComparer>().InstancePerLifetimeScope();
            builder.RegisterType<VehicleChangeComparer>().As<IChangeComparer>().InstancePerLifetimeScope();
            builder.RegisterType<DriverChangeComparer>().As<IChangeComparer>().InstancePerLifetimeScope();
            builder.RegisterType<CommoditiesHauledComparer>().As<IChangeComparer>().InstancePerLifetimeScope();
            builder.RegisterType<AddressesComparer>().As<IChangeComparer>().InstancePerLifetimeScope();
            builder.RegisterType<ManuscriptComparer>().As<IChangeComparer>().InstancePerLifetimeScope();
            builder.RegisterType<RatingFactorsComparer>().As<IChangeComparer>().InstancePerLifetimeScope();
            builder.RegisterType<CoveredAutoDesignationSymbolComparer>().As<IChangeComparer>().InstancePerLifetimeScope();
        }


        #endregion


        private void RegisterDatabase(IServiceCollection services)
        {
            var databaseSection = Configuration.GetSection("ConnectionStrings");
            var connectionString = databaseSection.GetValue<string>("ConCABoxTruck");
            var connectionStringExternal = databaseSection.GetValue<string>("ConCABoxTruckExternal");
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    connectionString,
                    sqlServerOptionsAction: sqlOptions =>
                    {
                        // Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency
                        sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                    }
            ).ConfigureWarnings(warnings =>
                warnings.Ignore(CoreEventId.NavigationBaseIncludeIgnored))
            );
            services.AddDbContext<AppDbExternalContext>(options =>
                options.UseSqlServer(
                    connectionStringExternal,
                    sqlServerOptionsAction: sqlOptions =>
                    {
                        // Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency
                        sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                    }
                )
            );
            services.AddDbContextFactory<AppDbExternalContext>((s, opt) =>
            {
                opt.UseSqlServer(Configuration.GetConnectionString("ConCABoxTruckExternal"));
            });
            services.AddHealthChecks()
                .AddCheck("self", () => HealthCheckResult.Healthy())
                .AddSqlServer(
                    connectionString,
                    name: "CABoxTruck-check",
                    tags: new[] { "CABoxTruckDB" });

            // Set up Dapper DB connection
            services.AddTransient<IDbConnection>(db => new SqlConnection(
                    connectionString));
        }

        public IScheduler ConfigureQuartz()
        {
            NameValueCollection props = new NameValueCollection
            {
                { "quartz.serializer.type", "binary" },
            };

            StdSchedulerFactory factory = new StdSchedulerFactory( props );
            var scheduler = factory.GetScheduler().Result;
            scheduler.Start().Wait();

            return scheduler;
        }

        
    }
}
