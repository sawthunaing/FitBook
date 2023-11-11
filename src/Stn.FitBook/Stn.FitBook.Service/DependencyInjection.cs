using Stn.FitBook.Domain.IServices;
using Stn.FitBook.Domain.IRepositories;
using Microsoft.Extensions.DependencyInjection;
using Stn.FitBook.Repo.Ef.Repositories.UnitOfWork;
using Stn.FitBook.Repo.Ef.Repositories;
using Stn.FitBook.Service.Services;

namespace Stn.FitBook.Service
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddConfig(this IServiceCollection services)
        {
            #region Repository

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddTransient(typeof(IQueryRepository<>), typeof(QueryRepository<>));
            services.AddTransient(typeof(ICommandRepository<>), typeof(CommandRepository<>));
            services.AddTransient<ICustomRepository, CustomerRepository>();

            #endregion

            #region Service
            services.AddTransient<IUserService, UserServices>();
            services.AddTransient<IPackageService, PackagesServices>();
            services.AddTransient<IScheduleClassService, ClassScheduleServices>();
            services.AddTransient<ISchedulerForRefundService, SchedulerForRefundServices>();
            services.AddTransient<IJwtService, JwtServices>();
            services.AddTransient<IValidator, ValidatorServices>();
            services.AddTransient<ICachesService, CachesServices>();
            #endregion

            return services;
        }

    }
}
