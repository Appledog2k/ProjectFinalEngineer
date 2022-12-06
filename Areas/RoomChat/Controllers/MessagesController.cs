using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProjectFinalEngineer.Areas.RoomChat.Models;
using ProjectFinalEngineer.Models;
using ProjectFinalEngineer.Models.AggregateMessage;
using ProjectFinalEngineer.Models.AggregateRoom;

namespace ProjectFinalEngineer.Areas.RoomChat.Controllers;
[Route("/Messages/[action]")]
[ApiController]
[Area("RoomChat")]
public class MessagesController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    public MessagesController(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<Room>> Get(int id)
    {
        var message = await _context.Messages.FindAsync(id);
        if (message == null)
        {
            return NotFound();
        }
        var messageViewModel = _mapper.Map<Message, MessageViewModel>(message);
        return Ok(messageViewModel);
    }
}