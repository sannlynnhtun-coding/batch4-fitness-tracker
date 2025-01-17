﻿using Batch4.FitnessTracker.Models.Models.Activity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Batch4.Api.FitnessTracker.Features.Activity;

[Route("api/[controller]")]
[ApiController]
public class ActivityController : ControllerBase
{
    private readonly BL_Activity _bl_Activity;

    public ActivityController(BL_Activity bl_Activity)
    {
        _bl_Activity = bl_Activity;
    }

    [HttpPost]
    public async Task<IActionResult> CreateActivityAsync(ActivityRequestModel request)
    {
        ActivityResponseModel response = await _bl_Activity.CreateActivityAsync(request);

        if (!response.MessageResponse.IsSuccess)
            return BadRequest(response.MessageResponse.Message);

        return Ok(response);
    }

    [HttpGet("{userId}")]
    public IActionResult GetActivityByUserId(int userId)
    {
        try
        {
            var response = _bl_Activity.GetActivitiesByUserId(userId);
            if (response is null || response.messageResponse!.IsError)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        catch(Exception ex)
        {
            return StatusCode(500, ex.ToString());
        }
    }

    [HttpPut("{activityId}")]
    public async Task<IActionResult> UpdateActivityAsync(
        int activityId,
        ActivityRequestModel request
    )
    {
        ActivityResponseModel response = await _bl_Activity.UpdateActivityAsync(
            activityId,
            request
        );

        if (!response.MessageResponse.IsSuccess)
        {
            if (response.MessageResponse.Message == "No data found.")
                return NotFound(response.MessageResponse.Message);

            return BadRequest(response.MessageResponse.Message);
        }

        return Ok(response);
    }

    [HttpDelete("{activityId}")]
    public async Task<IActionResult> DeleteActivityAsync(int activityId)
    {
        ActivityResponseModel response = await _bl_Activity.DeleteActivity(activityId);
        if (!response.MessageResponse.IsSuccess)
        {
            if (response.MessageResponse.Message == "No data found.")
                return NotFound(response.MessageResponse.Message);

            return BadRequest(response.MessageResponse.Message);
        }

        return Ok(response);
    }
}
