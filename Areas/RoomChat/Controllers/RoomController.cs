using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectFinalEngineer.Areas.RoomChat.Models;
using ProjectFinalEngineer.Models;
using ProjectFinalEngineer.Models.AggregateMessage;
using ProjectFinalEngineer.Models.AggregateRoom;

namespace ProjectFinalEngineer.Areas.RoomChat.Controllers;
[Route("api/[controller]")]
[ApiController]

public class RoomsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public RoomsController(AppDbContext context,
      IMapper mapper)
    {
        _context = context;
        _mapper = mapper;

    }


    [HttpGet]
    public async Task<ActionResult<IEnumerable<RoomViewModel>>> Get()
    {

        var rooms = await _context.Rooms.ToListAsync();

        var roomsViewModel = _mapper.Map<IEnumerable<Room>, IEnumerable<RoomViewModel>>(rooms);

        return Ok(roomsViewModel);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Room>> Get(int id)
    {
        var room = await _context.Rooms.FindAsync(id);
        if (room == null)
            return NotFound();

        var roomViewModel = _mapper.Map<Room, RoomViewModel>(room);
        return Ok(roomViewModel);
    }
}



