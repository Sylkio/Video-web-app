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

        [HttpGet("sas-token/{containerName}/{blobName}")]
        public IActionResult GenerateSasToken(string containerName, string blobName)
        {
            var token = _azureService.GenerateSasToken(containerName, blobName);
            return Ok(new { token });
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

        [HttpPost("Video-Upload")]
        public async Task<IActionResult> UploadVideo(IFormFile file)
        {
            if (file.Length > 0 && IsSupportedFileType(file.FileName))
            {
                // validate file
                if (file.Length > 200 * 1024 * 1024)
                {
                    return BadRequest("File bigger");
                }

                //
                var fileUrl = await _azureService.UploadFileToStorage(file);
                return Ok(new { fileUrl });

            }
            return Ok();    

        }
        private bool IsSupportedFileType(string fileName)
        {
            var supportedTypes = new[] { ".mp4", ".mov", ".hevc", ".webm" };
            return supportedTypes.Any(t => fileName.EndsWith(t, StringComparison.OrdinalIgnoreCase));
        }
    }
}