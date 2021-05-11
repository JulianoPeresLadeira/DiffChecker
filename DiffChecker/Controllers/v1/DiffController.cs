using DiffChecker.Errors;
using DiffChecker.Model;
using DiffChecker.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DiffChecker.Controllers.v1
{
    [ApiController]
    [Route("v1/[controller]")]
    public class DiffController : ControllerBase
    {
        private IDiffCheckerService diffCheckerService;

        public DiffController(IDiffCheckerService diffCheckerService)
        {
            this.diffCheckerService = diffCheckerService;
        }

        [HttpPut]
        [HttpPost]
        [Route("{id}/left")]
        public ActionResult<DiffData> SetLeft(string id, [FromBody] DiffCheckerRequest requestBody)
        {
            var data = requestBody.Data;

            if (string.IsNullOrEmpty(data)) throw new InvalidInputException("Missing data field");

            return Ok(diffCheckerService.SetLeft(id, data));
        }

        [HttpPut]
        [HttpPost]
        [Route("{id}/right")]
        public ActionResult<DiffData> SetRight(string id, [FromBody] DiffCheckerRequest requestBody)
        {
            var data = requestBody.Data;

            if (string.IsNullOrEmpty(data)) throw new InvalidInputException("Missing data field");

            return Ok(diffCheckerService.SetRight(id, data));
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult<DiffResponse> FindDiff(string id)
        {
            return Ok(diffCheckerService.FindDifference(id));
        }
    }
}