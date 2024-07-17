using Journey.Communication.Requests;
using Journey.Communication.Responses;
using Journey.Infrastructure.Entities;

namespace Journey.Application.Mappers;

public static class TripMapperExtensions
{
    public static Trip ToEntity(this RequestRegisterTripJson request) => new()
    {
        Name = request.Name,
        StartDate = request.StartDate,
        EndDate = request.EndDate
    };

    public static ResponseShortTripJson ToShortTripJson(this Trip entity) => new()
    {
        Id = entity.Id,
        Name = entity.Name,
        StartDate = entity.StartDate,
        EndDate = entity.EndDate
    };

    public static ResponseTripJson ToTripJson(this Trip entity) => new()
    {
        Id = entity.Id,
        Name = entity.Name,
        StartDate = entity.StartDate,
        EndDate = entity.EndDate,
        Activities = entity.Activities.Select(a => a.ToActivityJson()).ToList()
    };
}