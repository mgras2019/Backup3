using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.CAP.ICEM.ApiClient;
using Microsoft.CAP.ICEM.Business.Interface;
using Microsoft.CAP.ICEM.Entities;
using Microsoft.CAP.PAM.ICEM.Entities;
using Microsoft.Extensions.Logging;

namespace Microsoft.CAP.ICEM.Endpoints
{
    public class Request
    {

        private readonly IPAMRequestService _pAMRequest;
        public Request(IPAMRequestService pAMRequestService)
        {
            this._pAMRequest = pAMRequestService;
        }

        [Function("CreatePamIntakeRequest")]
        public async Task<ApiResponse<PAMResponse>> CreatePamIntakeRequest([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("CreatePamIntakeRequest");
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            ApiResponse<PAMResponse> response = new ApiResponse<PAMResponse>();
            List<string> entries = new List<string>();
            if (!string.IsNullOrEmpty(requestBody))
            {
                try
                {
                    var pamRequest = JsonSerializer.Deserialize<PAMRequest>(requestBody);
                    var validationResults = new List<ValidationResult>();

                    bool isValid = Validator.TryValidateObject(pamRequest, new ValidationContext(pamRequest, null, null), validationResults, true);
                    if (isValid)
                    {
                        entries.Add($"CreatePamIntakeRequest-Request:{requestBody}");
                        logger.LogInformation($"CreatePamIntakeRequest-Request", entries);
                        response = await _pAMRequest.Create(pamRequest);
                        entries.Add($"CreatePamIntakeRequest-Response:{JsonSerializer.Serialize(response)}");
                        logger.LogInformation($"CreatePamIntakeRequest", entries);

                    }
                    else
                    {
                        string validationError = string.Empty;
                        validationResults.ForEach(m => { validationError = $"{validationError},{m.ErrorMessage}"; });
                        response.Error = new ApiError() { Message = validationError };
                    }

                
                }
                catch (System.Exception ex)
                {
                    response.Error = new ApiError() { Message = ex.Message };

                    entries.Add($"CreatePamIntakeRequest-Response:{JsonSerializer.Serialize(response)}");
                    logger.LogInformation($"CreatePamIntakeRequest", entries);
                }
            }
            else
            {
                response.Error = new ApiError() { Message = "Request is empty" };
            }

            return response;

        }
    }
}
