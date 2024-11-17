using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RegionsController : ControllerBase
{
    private readonly NZWalksDbContext _dbContext;

    public RegionsController(NZWalksDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var regions = await _dbContext.Regions.ToListAsync();
        var regionsDto = new List<RegionDto>();
        foreach (var region in regions)
        {
            regionsDto.Add(new RegionDto()
            {
                Id = region.Id,
                Name = region.Name,
                Code = region.Code,
                RegionImageUrl = region.RegionImageUrl,
            });
        }
        return Ok(regionsDto);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var region = await _dbContext.Regions.FindAsync(id);
        if (region == null)
        {
            return NotFound();
        }

        var regionDto = new RegionDto
        {
            Id = region.Id,
            Name = region.Name,
            Code = region.Code,
            RegionImageUrl = region.RegionImageUrl,
        };
        return Ok(regionDto);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AddRegionRequestDto region)
    {
        var regionDomain = new Region
        {
            Name = region.Name,
            Code = region.Code,
            RegionImageUrl = region.RegionImageUrl,
        };
        
        await _dbContext.Regions.AddAsync(regionDomain);
        await _dbContext.SaveChangesAsync();
        
        var regionDto = new RegionDto
        {
            Id = regionDomain.Id,
            Name = regionDomain.Name,
            Code = regionDomain.Code,
            RegionImageUrl = regionDomain.RegionImageUrl,
        };

        return CreatedAtAction(
            nameof(GetById),
            new { id = regionDto.Id }, regionDto
            );
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateRegionRequestDto region)
    {
        var regionDomain = await _dbContext.Regions.FindAsync(id);
        if (regionDomain == null)
        {
            return NotFound();
        }
        regionDomain.Name = region.Name;
        regionDomain.Code = region.Code;
        regionDomain.RegionImageUrl = region.RegionImageUrl;
        
        await _dbContext.SaveChangesAsync();

        var regionDto = new RegionDto
        {
            Id = regionDomain.Id,
            Name = regionDomain.Name,
            Code = regionDomain.Code,
            RegionImageUrl = regionDomain.RegionImageUrl,
        };
        
        return Ok(regionDto);
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var region = await _dbContext.Regions.FindAsync(id);
        if (region == null)
        {
            return NotFound();
        }
        _dbContext.Regions.Remove(region);
        await _dbContext.SaveChangesAsync();
        return Ok();
    }
}