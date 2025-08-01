using Microsoft.AspNetCore.Mvc;

namespace YiraHealthCampManagerAPI.Models.Common
{
    public class Response<T> where T : class
    {
        public bool status { get; set; }
        public string message { get; set; }
        public T data { get; set; }
    }

    public static class ErrorResponse
    {
        public static BadRequestObjectResult CustomErrorResponse(ActionContext actionContext)
        {
            BadRequestObjectResult errorResp = new BadRequestObjectResult(actionContext.ModelState
                .Where(modelError => modelError.Value.Errors.Count > 0)
                .Select(modelError => new Response<string>
                {
                    status = false,
                    message = modelError.Value.Errors.FirstOrDefault()?.ErrorMessage
                }).FirstOrDefault());

            return errorResp;
        }
    }
}
