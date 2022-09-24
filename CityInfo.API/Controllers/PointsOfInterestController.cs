using AutoMapper;
using CityInfo.API.Entities;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [Route("api/cities/{cityId}/pointsofinterest")]
    [Authorize(Policy = "CityPolicy")]
    [ApiController]
    public class PointsOfInterestController : ControllerBase
    {
        private readonly ILogger<PointsOfInterestController> _logger;
        private readonly IMailService _mailService;
        private readonly CitiesDataStore? _citiesDataStore;
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;

        public PointsOfInterestController(ILogger<PointsOfInterestController> logger, IMailService localMailService, CitiesDataStore citiesDataStore, ICityInfoRepository cityInfoRepository, IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mailService = localMailService ?? throw new ArgumentNullException(nameof(localMailService));
            _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
            _mapper = mapper;
        }

        #region GET

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PointOfInterestDto>>> GetPointsOfInterest(int cityId)
        {
            //check if the user can request the city
            var userCityName = this.User.Claims.FirstOrDefault(c => c.Type == "city")?.Value;

            if(! await _cityInfoRepository.CityNameMatchesCityId(cityId, userCityName))
            {
                return Forbid();
            }

            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                _logger.Log(LogLevel.Error, $"City with id {cityId} was not found");
                return NotFound();
            }
            var pointofinterest = await _cityInfoRepository.GetPointsOfInterestAsync(cityId);
            return Ok(_mapper.Map<IEnumerable<PointOfInterestDto>>(pointofinterest));
        }

        [HttpGet("{pointsofinterestId}", Name = "GetPointOfInterest")]
        public async Task<ActionResult<PointOfInterestDto>> GetPointOfInterest(int cityId, int pointsofinterestId)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            }
            var pointOfInterest = await _cityInfoRepository.GetPointOfInterestAsync(cityId, pointsofinterestId);
            if (pointOfInterest == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<PointOfInterestDto>(pointOfInterest));
        }
        #endregion

        #region POST

        [HttpPost]
        public async Task<ActionResult<PointOfInterestDto>> CreatePointOfInterest(int cityId, PointOfInterestForCreationDto pointOfInterest)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            }
            PointOfInterest pointOfInterestEntity = _mapper.Map<PointOfInterest>(pointOfInterest);

            await _cityInfoRepository.AddPointOfInterestToCityAsync(cityId, pointOfInterestEntity);
            await _cityInfoRepository.SaveChangesAsync();

            PointOfInterestDto pointOfInterestDto = _mapper.Map<PointOfInterestDto>(pointOfInterestEntity);
            return CreatedAtRoute("GetPointOfInterest", new { cityId = cityId, pointsofinterestId = pointOfInterestDto.Id }, pointOfInterestDto);
        }
        #endregion

        #region UPDATE

        [HttpPut("{pointOfInterestId}")]
        public async Task<ActionResult> UpdatePointOfInterest(int cityId, int pointOfInterestId, PointOfInterestForUpdateDto pointInterestForUpdateDto)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            }
            var pointOfInterest = await _cityInfoRepository.GetPointOfInterestAsync(cityId, pointOfInterestId);
            if (pointOfInterest == null)
            {
                return NotFound();
            }

            _mapper.Map(pointInterestForUpdateDto, pointOfInterest);

            return NoContent();
        }

        [HttpPatch("{pointOfInterestId}")]
        public async Task<ActionResult> PatchPointOfInterest(int cityId, int pointOfInterestId, JsonPatchDocument<PointOfInterestForUpdateDto> jsonPatchDocument)
        {

            if (await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            }

            var pointOfInterestEntity = await _cityInfoRepository.GetPointOfInterestAsync(cityId, pointOfInterestId);
            if (pointOfInterestEntity == null)
            {
                return NotFound();
            }

            var pointOfInterestToPatch = _mapper.Map<PointOfInterestForUpdateDto>(pointOfInterestEntity);
            jsonPatchDocument.ApplyTo(pointOfInterestToPatch, ModelState);

            if (!TryValidateModel(pointOfInterestToPatch))
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            _mapper.Map(pointOfInterestToPatch, pointOfInterestEntity);
            await _cityInfoRepository.SaveChangesAsync();
            return NoContent();
        }
        #endregion

        #region DELETE

        [HttpDelete("{pointsofinterestId}")]
        public async Task<ActionResult> DetetePointOfInterest(int cityId, int pointsofinterestId)
        {
            if (await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            }

            var pointOfInterest = await _cityInfoRepository.GetPointOfInterestAsync(cityId, pointsofinterestId);
            if (pointOfInterest == null)
            {
                return NotFound();
            }

            _cityInfoRepository.DeletePointOfInterest(pointOfInterest);
            _mailService.Send(subject: "Entity removed", message: $"User has remover point of interest {pointOfInterest.Name} with id: {pointOfInterest.Id}");
            return NoContent();
        }

        #endregion
    }
}
