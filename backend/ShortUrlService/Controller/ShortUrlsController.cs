using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShortUrlService.AsyncDataServices;
using ShortUrlService.Data.Repository;
using ShortUrlService.Model;

namespace ShortUrlService.Controller
{
    [ApiController]
    [Route("/api/shorturl")]
    public class ShortUrlsController : ControllerBase
    {
        private readonly IShortUrlRepository _shortUrlRepository;

        public ShortUrlsController(IShortUrlRepository shortUrlRepository)
        {
            _shortUrlRepository = shortUrlRepository;
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public ActionResult GetAllShortUrls()
        {
            return Ok(_shortUrlRepository.GetAllShortUrls());
        }

        [HttpGet("{code}")]
        public ActionResult VisitShortUrl(string code)
        {
            ShortUrl shortUrl = _shortUrlRepository.GetShortUrlWithCode(code);
            if (shortUrl == null)
            {
                return NotFound(new { Message = "No ShortUrl mapped to this code"} );
            }

            return new RedirectResult(shortUrl.DestinationUrl);
        }

        [HttpPost]
        public async Task<ActionResult> CreateNewShortUrl(string url, string alias="")
        {
            string baseUrl = GetBaseUrlWithRequest(HttpContext.Request);
            try
            {
                if (Uri.IsWellFormedUriString(url, UriKind.Absolute))
                {
                    string aliasCode = "";
                    if (!string.IsNullOrEmpty(alias)){
                        if (alias.Length <= 12)
                        {
                            var shortUrlWithAlias = _shortUrlRepository.GetShortUrlWithCode(alias);
                            aliasCode = shortUrlWithAlias == null ? await _shortUrlRepository.AddShortUrl(url, alias):shortUrlWithAlias.Code;
                            _shortUrlRepository.SaveChanges();
                        }
                        else{
                            return BadRequest(new { Message = "Make sure the alias is at most 12 characters." });
                        }
                    }

                    ShortUrl shortUrl = _shortUrlRepository.GetShortUrlWithDestination(url);
                    
                    if (shortUrl != null)
                    {
                        return string.IsNullOrEmpty(aliasCode) ? 
                            Ok(new { ShortenedUrl = baseUrl + shortUrl.Code }):
                            Ok(new { ShortenedUrl = baseUrl + shortUrl.Code, ShortenedUrlWithAlias = baseUrl + aliasCode });
                    }

                    string code = await _shortUrlRepository.AddShortUrl(url);

                    _shortUrlRepository.SaveChanges();
                    return string.IsNullOrEmpty(aliasCode) ? 
                            Ok(new { ShortenedUrl = baseUrl + code }):
                            Ok(new { ShortenedUrl = baseUrl + code, ShortenedUrlWithAlias = baseUrl + aliasCode });
                }
                else
                {
                    return BadRequest(new { ErrorMessage = "Invalid url format"});
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("--> Error while adding shorturl: " + ex.Message);
                return StatusCode(500, new { Message = "Something went wrong"} );
            }
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{code}")]
        public ActionResult DeleteShortUrl(string code)
        {
            ShortUrl shortUrl = _shortUrlRepository.GetShortUrlWithCode(code);
            if (shortUrl == null)
            {
                return NotFound(new { Message = "No ShortUrl mapped to this code"} );
            }

            try{
                _shortUrlRepository.DeleteShortUrl(code);
                _shortUrlRepository.SaveChanges();

                return Ok(new 
                { 
                    Message = $"ShortUrl (code: {code}) was deleted successfully!",
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Error while deleting shorturl (code: {code}) : {ex.Message}");
                return StatusCode(500, new { Message = "Something went wrong"} );
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{code}")]
        public ActionResult UpdateShortUrlDestiantion(string code, [FromQuery]string newDestinationUrl)
        {
            ShortUrl shortUrl = _shortUrlRepository.GetShortUrlWithCode(code);
            if (shortUrl == null)
            {
                return NotFound(new { Message = "No ShortUrl mapped to this code"} );
            }

            try{
                string oldDestinationUrl = shortUrl.DestinationUrl;

                _shortUrlRepository.UpdateShortUrlDestinationLink(code, newDestinationUrl);
                _shortUrlRepository.SaveChanges();

                return Ok(new 
                { 
                    Message = $"ShortUrl (code: {code}) destination link was updated successfully!",
                    ShortenedUrl = GetBaseUrlWithRequest(HttpContext.Request) + code,
                    OldDestinationUrl = oldDestinationUrl,
                    NewDestinationUrl = shortUrl.DestinationUrl
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Error while updating shorturl (code: {code}) : {ex.Message}");
                return StatusCode(500, new { Message = "Something went wrong"} );
            }
        }

        private string GetBaseUrlWithRequest(HttpRequest request)
        {
            Console.WriteLine($"BaseUrl: {request.Scheme}://{request.Host}{request.Path}/");
            return $"{request.Scheme}://{request.Host}{request.Path}/";
        }
    }
}