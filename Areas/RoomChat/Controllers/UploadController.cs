using System.Text.RegularExpressions;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ProjectFinalEngineer.Areas.RoomChat.Hubs;
using ProjectFinalEngineer.Areas.RoomChat.Models;
using ProjectFinalEngineer.Areas.RoomChat.Services;
using ProjectFinalEngineer.Models;
using ProjectFinalEngineer.Models.AggregateMessage;
using ProjectFinalEngineer.Models.AggregateRoom;

namespace ProjectFinalEngineer.Areas.RoomChat.Controllers;
[Authorize]
[Route("api/[controller]")]
[ApiController]
public class UploadController : ControllerBase
{
    private readonly int FileSizeLimit;
    private readonly string[] AllowedExtensions;
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly IWebHostEnvironment _environment;
    private readonly IHubContext<ChatHub> _hubContext;
    private readonly IFileValidator _fileValidator;

    public UploadController(AppDbContext context,
        IMapper mapper,
        IWebHostEnvironment environment,
        IHubContext<ChatHub> hubContext,
        IConfiguration configruation,
        IFileValidator fileValidator)
    {
        _context = context;
        _mapper = mapper;
        _environment = environment;
        _hubContext = hubContext;
        _fileValidator = fileValidator;

        FileSizeLimit = configruation.GetSection("FileUpload").GetValue<int>("FileSizeLimit");
        AllowedExtensions = configruation.GetSection("FileUpload").GetValue<string>("AllowedExtensions").Split(",");
    }

    [HttpPost]
    //[ValidateAntiForgeryToken]
    public async Task<IActionResult> Upload([FromForm] UploadViewModel uploadViewModel)
    {
        if (ModelState.IsValid)
        {
            if (!_fileValidator.IsValid(uploadViewModel.File))
                return BadRequest("Validation failed!");

            var fileName = DateTime.Now.ToString("yyyymmddMMss") + "_" + Path.GetFileName(uploadViewModel.File.FileName);
            var folderPath = Path.Combine(_environment.WebRootPath, "uploads");
            var filePath = Path.Combine(folderPath, fileName);
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await uploadViewModel.File.CopyToAsync(fileStream);
            }

            var user = _context.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            var room = _context.Rooms.Where(r => r.Id == uploadViewModel.RoomId).FirstOrDefault();
            if (user == null || room == null)
                return NotFound();

            string htmlImage = string.Format(
                "<a href=\"/uploads/{0}\" target=\"_blank\">" +
                "<img src=\"/uploads/{0}\" class=\"post-image\">" +
                "</a>", fileName);

            var message = new Message()
            {
                Content = Regex.Replace(htmlImage, @"(?i)<(?!img|a|/a|/img).*?>", string.Empty),
                Timestamp = DateTime.Now,
                FromUser = user,
                ToRoom = room
            };

            await _context.Messages.AddAsync(message);
            await _context.SaveChangesAsync();

            // Send image-message to group
            var messageViewModel = _mapper.Map<Message, MessageViewModel>(message);
            await _hubContext.Clients.Group(room.Name).SendAsync("newMessage", messageViewModel);

            return Ok();
        }

        return BadRequest();
    }
}