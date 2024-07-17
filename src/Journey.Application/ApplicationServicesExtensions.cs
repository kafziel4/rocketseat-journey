using FluentValidation;
using Journey.Application.UseCases.Activities.Complete;
using Journey.Application.UseCases.Activities.Delete;
using Journey.Application.UseCases.Activities.Register;
using Journey.Application.UseCases.Trips.Delete;
using Journey.Application.UseCases.Trips.GetAll;
using Journey.Application.UseCases.Trips.GetById;
using Journey.Application.UseCases.Trips.Register;
using Microsoft.Extensions.DependencyInjection;

namespace Journey.Application;

public static class ApplicationServicesExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<RegisterTripUseCase>();
        services.AddScoped<GetAllTripsUseCase>();
        services.AddScoped<GetTripByIdUseCase>();
        services.AddScoped<DeleteTripByIdUseCase>();

        services.AddScoped<RegisterActivityForTripUseCase>();
        services.AddScoped<CompleteActivityForTripUseCase>();
        services.AddScoped<DeleteActivityForTripUseCase>();

        services.AddValidatorsFromAssemblyContaining(typeof(ApplicationServicesExtensions));

        return services;
    }
}