using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;

namespace PlatformService.Controllers;

[ApiController]
[Route("api/platforms")]
public class PlatformsController : ControllerBase
{
    private readonly IPlatformRepository _repository;
    private readonly IMapper _mapper;
    private readonly ICommandDataClient _commandDataClient;

    public PlatformsController(IPlatformRepository repository,
        IMapper mapper,
        ICommandDataClient commandDataClient)
    {
        _repository = repository;
        _mapper = mapper;
        _commandDataClient = commandDataClient;
    }

    [HttpGet]
    public ActionResult<IEnumerable<PlatformReadDto>> GetAll()
    {
        var platforms = _repository.GetAll();
        var dtos = _mapper.Map<IEnumerable<PlatformReadDto>>(platforms);

        return Ok(dtos);
    }
    
    [HttpGet("{id:int}", Name = "GetById")]
    public ActionResult<PlatformReadDto> GetById(int id)
    {
        var platform = _repository.GetById(id);
        if (platform == null)
            return NotFound();
        var dto = _mapper.Map<PlatformReadDto>(platform);
        return Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<PlatformReadDto>> Create([FromBody] PlatformCreateDto platformDto)
    {
        var platform = _mapper.Map<Platform>(platformDto);
        
        _repository.Create(platform);
        _repository.SaveChanges();

        var readDto = _mapper.Map<PlatformReadDto>(platform);
        try
        {
            await _commandDataClient.SendPlatformToCommand(readDto);
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error while sending platform to command service! {ex.Message}");
        }
        return CreatedAtRoute(nameof(GetById), new { readDto.Id }, readDto);
    }
}