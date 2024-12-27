using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RegionsController : ControllerBase
{
    private readonly NZWalksDbContext _dbContext;
    private readonly IRegionRepository _regionRepository;
    private readonly IMapper _mapper;

    public RegionsController(NZWalksDbContext dbContext, 
        IRegionRepository regionRepository,
        IMapper mapper)
    {
        _dbContext = dbContext;
        _regionRepository = regionRepository;
        _mapper = mapper;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var regions = await _regionRepository.GetAll();
        var regionsDto = _mapper.Map<IEnumerable<RegionDto>>(regions);
        return Ok(regionsDto);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var region = await _regionRepository.GetById(id);
        if (region == null)
        {
            return NotFound();
        }
        
        var regionDto = _mapper.Map<RegionDto>(region);
        return Ok(regionDto);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AddRegionRequestDto region)
    {
        var regionDomain = _mapper.Map<Region>(region);
        regionDomain = await _regionRepository.Create(regionDomain);
        var regionDto = _mapper.Map<RegionDto>(regionDomain);

        return CreatedAtAction(
            nameof(GetById),
            new { id = regionDto.Id }, regionDto
            );
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateRegionRequestDto region)
    {
        var regionDomain = _mapper.Map<Region>(region);
        regionDomain = await _regionRepository.Update(id,regionDomain);
        
        if (regionDomain == null)
        {
            return NotFound();
        }
        
        var regionDto = _mapper.Map<RegionDto>(regionDomain);
        return Ok(regionDto);
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var region = await _regionRepository.Delete(id);
        if (region == null)
        {
            return NotFound();
        }
        return Ok();
    }
}