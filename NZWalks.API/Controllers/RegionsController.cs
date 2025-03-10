using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
using System.Text.Json;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;
        private readonly ILogger<RegionsController> logger;

        public RegionsController(NZWalksDbContext dbContext,
            IRegionRepository regionRepository,
            IMapper mapper,
            ILogger<RegionsController> logger)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
            this.logger = logger;
        }
        [HttpGet]
        //[Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                throw new Exception("Custom exception");


                var regionsDomain = await regionRepository.GetAllAsync();

                //var regionsDto = new List<RegionDto>();
                //foreach (var regionDomain in regionsDomain)
                //{
                //    regionsDto.Add(new RegionDto()
                //    {
                //        Id = regionDomain.Id,
                //        Name = regionDomain.Name,
                //        Code = regionDomain.Code,
                //        RegionImageUrl = regionDomain.RegionImageUrl,
                //    });

                //}
                logger.LogInformation($"Finished GetAllRegions request with data: {JsonSerializer.Serialize(regionsDomain)}");

                return Ok(mapper.Map<List<RegionDto>>(regionsDomain));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message );
                throw;
            }

        }
        [HttpGet("{id:Guid}")]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            //   var region = dbContext.Regions.Find(id);

            var regionDomain = await regionRepository.GetByIdAsync(id);

            if (regionDomain == null)
            {
                return BadRequest();
            }

            return Ok(mapper.Map<RegionDto>(regionDomain));
        }
        [HttpPost]
        [ValidateModel]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            {
                var regionDomainModel = mapper.Map<Region>(addRegionRequestDto);

                regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);

                var regionDto = mapper.Map<RegionDto>(regionDomainModel);

                return CreatedAtAction(nameof(GetById), new { id = regionDomainModel.Id }, regionDomainModel);
            }   

        }
        [HttpPut("{id:Guid}")]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            {
                var regionDomainModel = mapper.Map<Region>(updateRegionRequestDto);

                regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);

                if (regionDomainModel == null)
                {
                    return NotFound();
                }

                return Ok(mapper.Map<RegionDto>(regionDomainModel));
            }


        }
        [HttpDelete("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async  Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var regionDomainModel = await regionRepository.DeleteAsync(id);

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<RegionDto>(regionDomainModel));
        }
    }
}
