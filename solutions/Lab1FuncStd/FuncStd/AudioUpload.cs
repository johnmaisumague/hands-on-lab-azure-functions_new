using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace FuncStd;

public class AudioUpload
{
    private readonly ILogger<AudioUpload> _logger;

    public AudioUpload(ILogger<AudioUpload> logger)
    {
        _logger = logger;
    }

    [Function(nameof(AudioUpload))]
    public AudioUploadOutput Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req)
    {
        _logger.LogInformation("Processing a new audio file upload request");

        // Guard: make sure a file was actually sent
        if (req.Form.Files.Count == 0)
        {
            return new AudioUploadOutput
            {
                Blob = System.Array.Empty<byte>(),
                HttpResponse = new BadRequestObjectResult("No audio file was uploaded.")
            };
        }

        // Get the first file in the form
        byte[] audioFileData;
        var file = req.Form.Files[0];

        using (var memStream = new MemoryStream())
        {
            file.OpenReadStream().CopyTo(memStream);
            audioFileData = memStream.ToArray();
        }

        // Store the file as a blob and return a success response
        return new AudioUploadOutput
        {
            Blob = audioFileData,
            HttpResponse = new OkObjectResult("Uploaded!")
        };
    }
}

public class AudioUploadOutput
{
    [BlobOutput("%STORAGE_ACCOUNT_CONTAINER%/{rand-guid}.wav", Connection = "AudioUploadStorage")]
    public byte[] Blob { get; set; } = System.Array.Empty<byte>();

    [HttpResult]
    public required IActionResult HttpResponse { get; set; }
}