using Microsoft.AspNetCore.Mvc;

namespace PogodynkaAPI.Controllers
{
    [Route("api/pogodynka")]
    [ApiController]
    [Authorize]
    public class PogodynkaController : ControllerBase
    {
        private readonly IPogodynkaService pogodynkaService;

        public PogodynkaController(IPogodynkaService pogodynkaService)
        {
            this.pogodynkaService = pogodynkaService;
        }

        [HttpGet]
        public ActionResult<List<PogodynkaDto>> GetAll()
        {
            var data = pogodynkaService.GetAll();
            return Ok(data);
        }

        [HttpPost]
        public ActionResult CreatePost([FromBody]CreatePogodynkaDto dto)
        {
            var data = pogodynkaService.Create(dto);
            return Ok(data);
        }

        [HttpDelete("{id}")]
        public ActionResult DeletePost([FromRoute]int id)
        {
            pogodynkaService.Delete(id);
            return NoContent();
        }
    }
}
