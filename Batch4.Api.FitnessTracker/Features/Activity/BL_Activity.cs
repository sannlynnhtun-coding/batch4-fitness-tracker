﻿using Azure;
using Batch4.Api.FitnessTracker.Features.ActivityType;
using Batch4.FitnessTracker.Models.Db;
using Batch4.FitnessTracker.Models.Models;
using Batch4.FitnessTracker.Models.Models.Activity;
using Microsoft.Identity.Client;

namespace Batch4.Api.FitnessTracker.Features.Activity
{
    public class BL_Activity
    {
        private readonly DA_Activity _DA_Activity;

        public BL_Activity(DA_Activity daActivty, DA_ActivityType dA_ActivityType)
        {
            _DA_Activity = daActivty;
        }

        public async Task<ActivityResponseModel> CreateActivityAsync(ActivityRequestModel activity)
        {
            ActivityResponseModel response = await _DA_Activity.CreateActivityAsync(activity);
            await _DA_Activity.SetTotalCalorieBurn(activity);
            return response;
        }

        public  ActivityDetailModel GetActivitiesByUserId(int userId)
        {
            var response= _DA_Activity.GetActivitiesByUserId(userId);
            return response; 
        }

        public async Task<ActivityResponseModel> UpdateActivityAsync(
            int activityId,
            ActivityRequestModel request
        )
        {
            ActivityResponseModel response = new ActivityResponseModel();

            try
            {
                Tbl_Activity tblActivity = await _DA_Activity.UpdateActivityAsync(
                    activityId,
                    request
                );
                if (tblActivity is null)
                {
                    response.MessageResponse.IsSuccess = false;
                    response.MessageResponse.Message = "No data found.";
                    return response;
                }

                response.MessageResponse.IsSuccess = true;
                response.MessageResponse.Message = "Update activity is successful.";

                response.ActivityId = activityId;
                response.UserId = tblActivity.UserId;
                response.ActivityTypeId = tblActivity.ActivityTypeId;
                response.Metric1 = tblActivity.Metric1;
                response.Metric2 = tblActivity.Metric2;
                response.Metric3 = tblActivity.Metric3;
                response.CaloriesBurned = tblActivity.CaloriesBurned;
            }
            catch (Exception ex)
            {
                response.MessageResponse = new MessageResponseModel(false, ex);
            }

            return response;
        }

        public async Task<ActivityResponseModel> DeleteActivity(int activityId)
        {
            ActivityResponseModel response = new ActivityResponseModel();
            try
            {
                var result = await _DA_Activity.DeleteActivityAsync(activityId);
                if (result == 0)
                {
                    response.MessageResponse.IsSuccess = false;
                    response.MessageResponse.Message = "No data found.";
                    return response;
                }
                response.MessageResponse.IsSuccess = true;
                response.MessageResponse.Message = "Deleting activity is successful.";
            }
            catch (Exception ex)
            {
                response.MessageResponse = new MessageResponseModel(false, ex);
            }
            return response;
        }
    }
}
