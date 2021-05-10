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
        [HttpPost]
        [Route("{id}/left")]
        public IActionResult SetLeft(string id, [FromBody] DiffCheckerRequest requestBody)
        {
            var data = requestBody.Data;

            if (string.IsNullOrEmpty(data)) throw new InvalidInputException("Missing data field");

            diffCheckerService.SetLeft(id, data);
            return Ok($"Left data set correctly for id {id}");
        }

        [HttpPut]
        [HttpPost]
        [Route("{id}/right")]
        public IActionResult SetRight(string id, [FromBody] DiffCheckerRequest requestBody)
        {
            var data = requestBody.Data;

            if (string.IsNullOrEmpty(data)) throw new InvalidInputException("Missing data field");

            diffCheckerService.SetRight(id, data);
            return Ok($"Right data set correctly for id {id}");
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult FindDiff(string id)
        {
            return Ok(diffCheckerService.FindDifference(id));
        }
    }
}