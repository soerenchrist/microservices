using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;

namespace PlatformService.Controllers;

[ApiController]
[Route("api/platforms")]
public class PlatformsController : ControllerBase
{
    private readonly IPlatformRepository _repository;
    private readonly IMapper _mapper;

    public PlatformsController(IPlatformRepository repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
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
    public ActionResult<PlatformReadDto> Create([FromBody] PlatformCreateDto platformDto)
    {
        var platform = _mapper.Map<Platform>(platformDto);
        
        _repository.Create(platform);
        _repository.SaveChanges();

        var readDto = _mapper.Map<PlatformReadDto>(platform);
        return CreatedAtRoute(nameof(GetById), new { readDto.Id }, readDto);
    }
}