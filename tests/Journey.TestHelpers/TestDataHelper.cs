using Journey.Infrastructure.Entities;

namespace Journey.TestHelpers;

public static class TestDataHelper
{
    public static List<Trip> Trips { get; } =
    [
        new Trip
        {
            Id = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
            Name = "Viagem a Paris",
            StartDate = new DateTime(2024, 7, 1, 0, 0, 0, DateTimeKind.Utc),
            EndDate = new DateTime(2024, 7, 10, 23, 59, 59, DateTimeKind.Utc),
            Activities =
            [
                new Activity
                {
                    Id = Guid.Parse("2b34c4e8-5678-4d3a-a2be-2c9e9f1e34c7"),
                    Name = "Passeio no Louvre",
                    Date = new DateTime(2024, 7, 4, 14, 0, 0, DateTimeKind.Utc),
                    Status = Infrastructure.Enums.ActivityStatus.Pending
                },
                new Activity
                {
                    Id = Guid.Parse("9b04ef5c-7bfc-4a71-931e-74dcb9cb9a69"),
                    Name = "Cruzamento no Rio Sena",
                    Date = new DateTime(2024, 7, 6, 12, 0, 0, DateTimeKind.Utc),
                    Status = Infrastructure.Enums.ActivityStatus.Pending
                },
                new Activity
                {
                    Id = Guid.Parse("e1a1df1e-1cbb-4c2b-989e-501a333d33e2"),
                    Name = "Visita à Torre Eiffel",
                    Date = new DateTime(2024, 7, 2, 10, 0, 0, DateTimeKind.Utc),
                    Status = Infrastructure.Enums.ActivityStatus.Pending
                },
                new Activity
                {
                    Id = Guid.Parse("f53cb0c4-5e9f-4f30-9a76-0348c328dbf8"),
                    Name = "Visita a Montmartre",
                    Date = new DateTime(2024, 7, 8, 11, 0, 0, DateTimeKind.Utc),
                }
            ]
        },
        new Trip
        {
            Id = Guid.Parse("7b9c61ae-3ef8-48b1-94a8-896e5fb90895"),
            Name = "Viagem ao Japão",
            StartDate = new DateTime(2024, 8, 15, 0, 0, 0, DateTimeKind.Utc),
            EndDate = new DateTime(2024, 8, 25, 23, 59, 59, DateTimeKind.Utc),
            Activities =
            [
                new Activity
                {
                    Id = Guid.Parse("a34d33ef-8941-4f72-912c-58a67e3b74df"),
                    Name = "Visita a Tokyo Tower",
                    Date = new DateTime(2024, 8, 16, 9, 0, 0, DateTimeKind.Utc),
                    Status = Infrastructure.Enums.ActivityStatus.Pending
                },
                new Activity
                {
                    Id = Guid.Parse("cbac3b2a-7fb6-4b91-9332-b9cf5b70f1d3"),
                    Name = "Excursão a Hiroshima",
                    Date = new DateTime(2024, 8, 22, 8, 0, 0, DateTimeKind.Utc),
                    Status = Infrastructure.Enums.ActivityStatus.Pending
                },
                new Activity
                {
                    Id = Guid.Parse("e91bdc6f-9e92-4f7b-97df-8d3c02c9c8f1"),
                    Name = "Visita a Kyoto",
                    Date = new DateTime(2024, 8, 20, 10, 0, 0, DateTimeKind.Utc),
                    Status = Infrastructure.Enums.ActivityStatus.Pending
                },
                new Activity
                {
                    Id = Guid.Parse("f1c8a6de-3e7b-4980-8a2d-24b944b75567"),
                    Name = "Passeio em Shibuya",
                    Date = new DateTime(2024, 8, 18, 15, 0, 0, DateTimeKind.Utc),
                },
                new Activity
                {
                    Id = Guid.Parse("f89b34a1-0a14-4f52-a9f1-3f0c4e3347d7"),
                    Name = "Visita ao Monte Fuji",
                    Date = new DateTime(2024, 8, 24, 13, 0, 0, DateTimeKind.Utc),
                }
            ]
        },
        new Trip
        {
            Id = Guid.Parse("a7bb7c50-0f04-4af6-9635-48c6b539e2e7"),
            Name = "Viagem à Austrália",
            StartDate = new DateTime(2024, 11, 1, 0, 0, 0, DateTimeKind.Utc),
            EndDate = new DateTime(2024, 11, 11, 23, 59, 59, DateTimeKind.Utc),
            Activities =
            [
                new Activity
                {
                    Id = Guid.Parse("bfe7e9e1-334b-4c1f-9f4d-d2b8d69f2a33"),
                    Name = "Passeio na Grande Barreira de Coral",
                    Date = new DateTime(2024, 11, 4, 12, 0, 0, DateTimeKind.Utc),
                    Status = Infrastructure.Enums.ActivityStatus.Pending
                },
                new Activity
                {
                    Id = Guid.Parse("d5c3e9fb-8c8b-4f91-926b-9f27b5aaf9f1"),
                    Name = "Visita à Opera House",
                    Date = new DateTime(2024, 11, 2, 9, 0, 0, DateTimeKind.Utc),
                    Status = Infrastructure.Enums.ActivityStatus.Pending
                },
                new Activity
                {
                    Id = Guid.Parse("e4b8a1f6-7f84-4b6a-b1a9-8c9e2d1c4f8e"),
                    Name = "Visita ao Parque Nacional de Kakadu",
                    Date = new DateTime(2024, 11, 6, 8, 0, 0, DateTimeKind.Utc),
                    Status = Infrastructure.Enums.ActivityStatus.Pending
                }
            ]
        }
    ];
}
