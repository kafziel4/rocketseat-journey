using Journey.Communication.Requests;
using Journey.Communication.Responses;
using Journey.Infrastructure.Entities;

namespace Journey.Application.Mappers;

public static class ActivityMapperExtensions
{
    public static Activity ToEntity(this RequestRegisterActivityJson request) => new()
    {
        Name = request.Name,
        Date = request.Date
    };

    public static ResponseActivityJson ToActivityJson(this Activity entity) => new()
    {
        Id = entity.Id,
        Name = entity.Name,
        Date = entity.Date,
        Status = (Communication.Enums.ActivityStatus)entity.Status
    };
}