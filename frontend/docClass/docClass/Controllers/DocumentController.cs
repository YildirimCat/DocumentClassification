using docClass.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace docClass.Controllers
{
    public class DocumentController : Controller
    {
    
        private readonly IHttpClientFactory _clientFactory;

        public DocumentController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [HttpPost]
        public async Task<IActionResult> UploadDocument(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var client = _clientFactory.CreateClient();
            var content = new MultipartFormDataContent();
            var fileContent = new StreamContent(file.OpenReadStream());
            content.Add(fileContent, "file", file.FileName);

            var response = await client.PostAsync("http://localhost:5000/classify", content);
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode);
            }

            var result = await response.Content.ReadAsStringAsync();
            ViewBag.Result = result;

            return View();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
