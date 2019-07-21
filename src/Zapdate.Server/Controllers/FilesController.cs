using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zapdate.Core;
using Zapdate.Server.Core.Errors;
using Zapdate.Server.Extensions;
using Zapdate.Server.Infrastructure.Data;
using Zapdate.Server.Infrastructure.Interfaces;
using Zapdate.Server.Models.Errors;

namespace Zapdate.Server.Controllers
{
    [ApiController]
    [Route("api/v1/projects/{projectId}/[controller]")]
    public class FilesController : Controller
    {
        private readonly IServerFilesManager _serverFilesManager;
        private readonly AppDbContext _context;

        public FilesController(IServerFilesManager serverFilesManager, AppDbContext context)
        {
            _serverFilesManager = serverFilesManager;
            _context = context;
        }

        // POST api/v1/projects/{id}/files
        [HttpPost, Authorize]
        public async Task<ActionResult> Upload()
        {
            var boundary = GetBoundary(Request.ContentType);
            var reader = new MultipartReader(boundary, Request.Body, 80 * 1024);

            MultipartSection section;
            while ((section = await reader.ReadNextSectionAsync()) != null)
            {
                var contentDispo = section.GetContentDispositionHeader();

                if (contentDispo.IsFileDisposition())
                {
                    var fileSection = section.AsFileSection();
                    var file = await _serverFilesManager.AddFile(fileSection.FileStream);

                    try
                    {
                        _context.Add(file);
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception)
                    {
                        // the file couldn't be added because it already exists. everything is fine
                        if (await _context.StoredFiles.AnyAsync(x => x.FileHash == file.FileHash))
                            continue;

                        throw;
                    }
                }
            }

            return Ok();
        }

        // POST api/v1/projects/{id}/files/check
        [HttpPost("check"), Authorize]
        public async Task<ActionResult<Dictionary<string, bool>>> CheckFiles(List<string> files)
        {
            var hashes = new HashSet<Hash>();
            foreach (var file in files)
                if (Hash.TryParse(file, out var hash) && hash!.Value.IsSha256Size)
                    hashes.Add(hash!.Value);
                else
                    return new FieldValidationError(file, "The hash could not be parsed or it's not a SHA256 hash.").ToActionResult();

            var result = new Dictionary<string, bool>();
            foreach (var hash in hashes)
            {
                var exists = await _context.StoredFiles.AnyAsync(x => x.FileHash == hash.ToString());
                result.Add(hash.ToString(), exists);
            }

            return result;
        }

        // GET api/v1/projects/{id}/files/{hash}
        [HttpGet("{fileHash}"), AllowAnonymous]
        public async Task<ActionResult> Download(string fileHash)
        {
            if (fileHash.Length != 64 || !Hash.TryParse(fileHash, out var hash))
                return new UrlParameterValidationError("Please provide a valid SHA256 hash").ToActionResult();

            var isFileStored = await _context.StoredFiles.AnyAsync(x => x.FileHash == hash.ToString());
            if (!isFileStored)
                return new ResourceNotFoundError($"The file {fileHash} was not found.", ErrorCode.FileNotFound).ToActionResult();

            var fileStream = _serverFilesManager.ReadFile(hash!.Value);
            return File(fileStream, "application/octet-stream");
        }

        private static string GetBoundary(string contentType)
        {
            if (contentType == null)
                throw new ArgumentNullException(nameof(contentType));

            var elements = contentType.Split(' ');
            var element = elements.First(entry => entry.StartsWith("boundary="));
            var boundary = element.Substring("boundary=".Length);

            return HeaderUtilities.RemoveQuotes(new StringSegment(boundary)).ToString();
        }
    }
}
