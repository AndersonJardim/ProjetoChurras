using Microsoft.AspNetCore.Mvc;
using ProjetoChurras.Models;
using ProjetoChurras.Repository;

namespace ProjetoChurras.Controllers
{
    [ApiController]
    [Route("api/churras")]
    public class ChurrasController : ControllerBase
    {
        private readonly ChurrasRepository repository;

        public ChurrasController(ChurrasRepository repository)
        {
            this.repository = repository;
        }

        [HttpPost]
        public async Task<ActionResult<ChurrasModel>> CreateChurras([FromBody] ChurrasModel churras)
        {
            try
            {
                var findAll = await repository.FindAll();
                if (findAll.Any(x => x.DateBbq == churras.DateBbq))
                {
                    return NotFound("Já existe churras para essa data.");
                }

                var response = await repository.Create(churras);
                return Ok(response.Resource);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.Message ?? e.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<IList<ChurrasModel>>> GetChurras()
        {
            try
            {
                var response = await repository.FindAll();
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.Message ?? e.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult<ChurrasModel>> PutChurras([FromBody] ChurrasModel churras)
        {
            try
            {
                var response = await repository.Update(churras);
                return Ok($"Update realizado. Status: {response}");
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.Message ?? e.Message);
            }
        }
    }
}