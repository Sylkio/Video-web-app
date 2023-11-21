using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VideoWebAppApi.Data;
using VideoWebAppApi.Interface;
using VideoWebAppApi.Models;
using VideoWebAppApi.Models.Dtos;
namespace VideoWebAppApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VideosController : ControllerBase
    {
        private readonly IAzureService? _azureService;
        private readonly AppDbContext? _context;

        public VideosController(IAzureService azureService, AppDbContext context)
        {
            _azureService = azureService;
            _context = context;
        }

        [HttpGet("sas-token")]
        public IActionResult GenerateSasToken()
        {
            var videos = _context?.Videos.ToList();
            return Ok(videos);
        }
        [HttpPost]
        public async Task<IActionResult> saveVideoMetaData([FromBody] VideoMetaDto videoMetadata)
        {
            var video = new Video
            {
                VideoName = videoMetadata.Name,
                VideoDescription = videoMetadata.Description,
                Url = videoMetadata.Url
            };
            _context.Videos.Add(video);
            await _context.SaveChangesAsync();

            return Ok(video);
        }
        [HttpGet]
        public async Task<IActionResult> ListVideos()
        {
            var videos = await _context.Videos.ToListAsync();
            return Ok(videos);
        }
    }
}