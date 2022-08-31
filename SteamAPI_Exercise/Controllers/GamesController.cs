using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SteamAPI.Controllers.CustomResponses;
using SteamAPI.DTO;
using SteamAPI.Interfaces;
using SteamAPI.Models;
using SteamAPI.Repositories;
using System.Net.Mime;

namespace SteamAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {

        private readonly IBaseRepository<Games> _repository;

        public GamesController(IBaseRepository<Games> repository)
        {
            _repository = repository;
        }

        private Games UpdateGamesModel(Games newData, Games entity)
        {
            newData.AppId = entity.AppId;
            newData.Name = entity.Name;
            newData.Developer = entity.Developer;
            newData.Platforms = entity.Platforms;
            newData.Categories = entity.Categories;
            newData.Genres = entity.Genres;
            return newData;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int page, int maxResults)
        {
            var games = await _repository.Get(page, maxResults);
            return Ok(games);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Games), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var game = await _repository.GetByKey(id);
            if (game == null)
            {
                return NotFound("Id Inexistente");
            }
            return Ok(game);
        }

        [HttpPut("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Games), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Games), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status415UnsupportedMediaType)]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] Games entity)
        {
            var databaseGames = await _repository.GetByKey(id);

            // UPSERT
            if (databaseGames == null)
            {
                var inserted = await _repository.Insert(entity);
                return Created(string.Empty, inserted);
            }

            databaseGames = UpdateGamesModel(databaseGames, entity);

            var updated = await _repository.Update(id, databaseGames);

            return Ok(updated);
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Games), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(string), StatusCodes.Status415UnsupportedMediaType)]
        public async Task<IActionResult> AddGames([FromBody] Games entity)
        {
            var databaseGames = await _repository.GetByKey(entity.Id);

            if (databaseGames != null)
            {
                return Conflict($"Não é possível adicionar um jogo com um id existente. O id #{entity.Id} já existe!");
            }

            var inserted = await _repository.Insert(entity);
            return Created(string.Empty, inserted);
        }

        [HttpDelete("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(DeleteOkCustomResponse),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status415UnsupportedMediaType)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var databaseGames = await _repository.GetByKey(id);

            if (databaseGames == null)
            {
                return NoContent();
            }

            var deletedGame = await _repository.Delete(id);
            return Ok(new DeleteOkCustomResponse
            {
                Code = StatusCodes.Status200OK.ToString(),
                Id = id,
                Message = $"Id #{id} deletado com sucesso!"
            });
        }

        [HttpPatch("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Games), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status415UnsupportedMediaType)]
        public async Task<IActionResult> Patch([FromRoute] int id, [FromBody] JsonPatchDocument<Games> patchEntity)
        {
            var databaseGames = await _repository.GetByKey(id);

            if (databaseGames == null)
            {
                return NotFound("Id inexistente");
            }

            patchEntity.ApplyTo(databaseGames, ModelState);

            var updatedGame = await _repository.Update(id, databaseGames);

            return Ok(updatedGame);
        }

        // Segunda forma de implementar o patch
        //[HttpPatch("{id2}")]
        //[Consumes(MediaTypeNames.Application.Json)]
        //[ProducesResponseType(typeof(Games), StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        //[ProducesResponseType(typeof(string), StatusCodes.Status415UnsupportedMediaType)]
        //public async Task<IActionResult> Patch2([FromRoute] int id2, [FromBody] GamesPatch patchEntity)
        //{
        //    var databaseGames = await _repository.GetByKey(id2);

        //    if (databaseGames == null)
        //    {
        //        return NotFound("Id inexistente");
        //    }

        //    databaseGames.Platforms = patchEntity.Platforms;
        //    databaseGames.Categories = patchEntity.Categories;
        //    databaseGames.Genres = patchEntity.Genres;

        //    var updatedGame = await _repository.Update(id2, databaseGames);

        //    return Ok(updatedGame);
        //}

    }
}
