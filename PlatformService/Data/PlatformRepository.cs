using PlatformService.Models;

namespace PlatformService.Data;

public class PlatformRepository : IPlatformRepository
{
    private readonly AppDbContext _context;

    public PlatformRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public bool SaveChanges()
    {
        return _context.SaveChanges() >= 0;
    }

    public IEnumerable<Platform> GetAll()
    {
        return _context.Platforms.ToList();
    }

    public Platform? GetById(int id)
    {
        return _context.Platforms.FirstOrDefault(x => x.Id == id);
    }

    public void Create(Platform platform)
    {
        ArgumentNullException.ThrowIfNull(platform);

        _context.Platforms.Add(platform);
    }
}