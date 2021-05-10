using DiffChecker.Errors;
using DiffChecker.Model;
using DiffChecker.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DiffChecker.Controllers
{
    [ApiController]
    [Route("v1/diff")]
    public class DiffCheckerController : ControllerBase
    {
        private IDiffCheckerService diffCheckerService;

        public DiffCheckerController(IDiffCheckerService diffCheckerService)
        {
            this.diffCheckerService = diffCheckerService;
        }

        [HttpPut]
        [Route("{id}/left")]
        public ActionResult SetLeft(string id, [FromBody] DiffCheckerRequest requestBody)
        {
            var data = requestBody.Data;

            if (string.IsNullOrEmpty(data)) throw new InvalidInputException("Missing data field");

            diffCheckerService.SetLeft(id, data);
            return Ok($"Left data set correctly for id {id}");
        }

        [HttpPut]
        [Route("{id}/right")]
        public ActionResult SetRight(string id, [FromBody] DiffCheckerRequest requestBody)
        {
            var data = requestBody.Data;

            if (string.IsNullOrEmpty(data)) throw new InvalidInputException("Missing data field");

            diffCheckerService.SetRight(id, data);
            return Ok($"Right data set correctly for id {id}");
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult<ServiceResponse> FindDiff(string id)
        {
            // try
            // {
            return Ok(diffCheckerService.FindDifference(id));
            // }
            // catch (Exception e)
            // {
            //    return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            // }
        }
    }
}