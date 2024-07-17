using Journey.Application.UseCases.Activities.Complete;
using Journey.Application.UseCases.Activities.Delete;
using Journey.Application.UseCases.Activities.Register;
using Journey.Application.UseCases.Trips.Delete;
using Journey.Application.UseCases.Trips.GetAll;
using Journey.Application.UseCases.Trips.GetById;
using Journey.Application.UseCases.Trips.Register;
using Journey.Communication.Requests;
using Journey.Communication.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Journey.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TripsController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseShortTripJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorsJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(
        [FromBody] RequestRegisterTripJson request,
        [FromServices] RegisterTripUseCase registerTripUseCase)
    {
        var response = await registerTripUseCase.ExecuteAsync(request);

        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ResponseTripsJson), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromServices] GetAllTripsUseCase getAllTripsUseCase)
    {
        var response = await getAllTripsUseCase.ExecuteAsync();

        return Ok(response);
    }

    [HttpGet]
    [Route("{id}")]
    [ProducesResponseType(typeof(ResponseTripJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorsJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        [FromRoute] Guid id,
        [FromServices] GetTripByIdUseCase getTripByIdUseCase)
    {
        var response = await getTripByIdUseCase.ExecuteAsync(id);

        return Ok(response);
    }

    [HttpDelete]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorsJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        [FromRoute] Guid id,
        [FromServices] DeleteTripByIdUseCase deleteTripByIdUseCase)
    {
        await deleteTripByIdUseCase.ExecuteAsync(id);

        return NoContent();
    }

    [HttpPost]
    [Route("{tripId}/activities")]
    [ProducesResponseType(typeof(ResponseActivityJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorsJson), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseErrorsJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RegisterActivity(
        [FromRoute] Guid tripId,
        [FromBody] RequestRegisterActivityJson request,
        [FromServices] RegisterActivityForTripUseCase registerActivityForTripUseCase)
    {
        var response = await registerActivityForTripUseCase.ExecuteAsync(tripId, request);

        return Created(string.Empty, response);
    }

    [HttpPut]
    [Route("{tripId}/activities/{activityId}/complete")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorsJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CompleteActivity(
        [FromRoute] Guid tripId,
        [FromRoute] Guid activityId,
        [FromServices] CompleteActivityForTripUseCase completeActivityForTripUseCase)
    {
        await completeActivityForTripUseCase.ExecuteAsync(tripId, activityId);

        return NoContent();
    }

    [HttpDelete]
    [Route("{tripId}/activities/{activityId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorsJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteActivity(
        [FromRoute] Guid tripId,
        [FromRoute] Guid activityId,
        [FromServices] DeleteActivityForTripUseCase deleteActivityForTripUseCase)
    {
        await deleteActivityForTripUseCase.ExecuteAsync(tripId, activityId);

        return NoContent();
    }
}
