using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging; 
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
        private readonly IAzureService _azureService;
        private readonly AppDbContext _context;
        private readonly ILogger<VideosController> _logger;

        public VideosController(IAzureService azureService, AppDbContext context, ILogger<VideosController> logger)
        {
            _azureService = azureService;
            _context = context;
            _logger = logger;
        }

        [HttpGet("sas-token/{containerName}/{blobName}")]
        public IActionResult GenerateSasToken(string containerName, string blobName)
        {
            var token = _azureService.GenerateSasToken(containerName, blobName);
            return Ok(new { token });
        }

        [HttpPost]
        public async Task<IActionResult> SaveVideoMetaData([FromBody] VideoMetaDto videoMetadata)
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
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            try
            {
                var fileUrl = await _azureService.UploadFileToStorage(file);
                _logger.LogInformation($"File uploaded. URL: {fileUrl}");

                if (string.IsNullOrEmpty(fileUrl))
                {
                    return BadRequest("File upload failed, URL is null or empty.");
                }

                return Ok(new { Success = true, FileUrl = fileUrl });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error during file upload: {ex.Message}");
                return StatusCode(500, "Internal server error during file upload.");
            }
        }

        private bool IsSupportedFileType(string fileName)
        {
            var supportedTypes = new[] { ".mp4", ".mov", ".hevc", ".webm" };
            return supportedTypes.Any(t => fileName.EndsWith(t, StringComparison.OrdinalIgnoreCase));
        }
    }
}
