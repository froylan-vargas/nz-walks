using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories;

public class RegionRepository : IRegionRepository
{
    private readonly NZWalksDbContext _dbContext;

    public RegionRepository(NZWalksDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<List<Region>> GetAll()
    {
        return await _dbContext.Regions.ToListAsync();
    }

    public async Task<Region?> GetById(Guid id)
    {
       return await _dbContext.Regions.FirstOrDefaultAsync(x=>x.Id == id); 
    }

    public async Task<Region> Create(Region region)
    {
        await _dbContext.Regions.AddAsync(region);
        await _dbContext.SaveChangesAsync();
        return region;
    }

    public async Task<Region?> Update(Guid id, Region region)
    {
        var existingRegion = await _dbContext.Regions.FindAsync(id);
        if (existingRegion == null)
        {
            return null;
        }
        existingRegion.Name = region.Name;
        existingRegion.Code = region.Code;
        existingRegion.RegionImageUrl = region.RegionImageUrl;
        
        await _dbContext.SaveChangesAsync();
        return existingRegion;
    }

    public async Task<Region?> Delete(Guid id)
    {
        var existingRegion = await _dbContext.Regions.FindAsync(id);
        if (existingRegion == null)
        {
            return null;
        }
        _dbContext.Regions.Remove(existingRegion);
        await _dbContext.SaveChangesAsync();
        return existingRegion;
    }
}