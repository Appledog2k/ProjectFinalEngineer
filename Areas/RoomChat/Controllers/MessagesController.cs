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
    [HttpGet("Room/{roomName}")]
    public IActionResult GetMessages(string roomName)
    {
        var room = _context.Rooms.FirstOrDefault(r => r.Name == roomName);
        if (room == null)
            return BadRequest();

        var messages = _context.Messages.Where(m => m.ToRoomId == room.Id)
            .Include(m => m.FromUser)
            .Include(m => m.ToRoom)
            .OrderByDescending(m => m.Timestamp)
            .Take(20)
            .AsEnumerable()
            .Reverse()
            .ToList();

        var messagesViewModel = _mapper.Map<IEnumerable<Message>, IEnumerable<MessageViewModel>>(messages);

        return Ok(messagesViewModel);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var message = await _context.Messages
            .Include(u => u.FromUser)
            .Where(m => m.Id == id && m.FromUser.UserName == User.Identity.Name)
            .FirstOrDefaultAsync();

        if (message == null)
            return NotFound();

        _context.Messages.Remove(message);
        await _context.SaveChangesAsync();

        return NoContent();
    }

}