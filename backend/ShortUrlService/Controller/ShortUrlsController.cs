using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShortUrlService.AsyncDataServices;
using ShortUrlService.Data.Repository;
using ShortUrlService.DTO;
using ShortUrlService.Model;

namespace ShortUrlService.Controller
{
    [ApiController]
    [Route("api/shorturl")]
    public class ShortUrlsController : ControllerBase
    {
        private readonly IShortUrlRepository _shortUrlRepository;
        private readonly IMapper _mapper;

        public ShortUrlsController(IShortUrlRepository shortUrlRepository, IMapper mapper)
        {
            _shortUrlRepository = shortUrlRepository;
            _mapper = mapper;
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public ActionResult GetAllShortUrls(string searchQuery = "", int pageNumber = 1, int pageSize = 5)
        {
            if (string.IsNullOrEmpty(searchQuery)) searchQuery = "";
            if (pageSize > 100) pageSize = 100;
            if (pageNumber < 1)
            {
                return BadRequest(new {Message = "Page number must be greater than 0."});
            }
            
            var shortUrls = _shortUrlRepository.GetAllShortUrls(searchQuery, pageNumber, pageSize);
            
            return Ok(new 
                    {
                        CurrentPage = pageNumber, 
                        PageSize = pageSize,
                        ShortUrls = _mapper.Map<IEnumerable<ShortUrlReadDTO>>(shortUrls)
                    });
        }

        [HttpGet("{code}")]
        public ActionResult VisitShortUrl(string code, [FromQuery]bool avoidRedirection = false)
        {
            ShortUrl shortUrl = _shortUrlRepository.GetShortUrlWithCode(code);
            if (shortUrl == null)
            {
                return NotFound(new { Message = "No ShortUrl mapped to this code"} );
            }

            return avoidRedirection ? 
                Ok(new { Message = "ShortUrl was found", ShortUrl = _mapper.Map<ShortUrlReadDTO>(shortUrl)}):
                new RedirectResult(shortUrl.DestinationUrl);
        }

        [HttpPost]
        public async Task<ActionResult> CreateNewShortUrl(string url, string alias="")
        {
            string baseUrl = GetBaseUrlWithRequest(HttpContext.Request);
            
            try
            {
                if (Uri.IsWellFormedUriString(url, UriKind.Absolute))
                {
                    ShortUrl shortUrlWithAlias = null;
                    string aliasCode = "";
                    if (!string.IsNullOrEmpty(alias)){
                        if (alias.Length <= 20)
                        {
                            if (_shortUrlRepository.DoesAliasExist(alias)){
                                return BadRequest(new { Message = "This alias has been taken already, try a different one." });
                            }
                            else{
                                shortUrlWithAlias = _shortUrlRepository.GetShortUrlWithCode(alias);
                                aliasCode = shortUrlWithAlias == null ? _shortUrlRepository.AddShortUrl(url, alias).GetAwaiter().GetResult().Code:shortUrlWithAlias.Code;
                                _shortUrlRepository.SaveChanges();
                            }
                        }
                        else{
                            return BadRequest(new { Message = "Make sure the alias is at most 20 characters." });
                        }
                    }

                    ShortUrl shortUrl = _shortUrlRepository.GetShortUrlWithDestination(url);
                    
                    if (shortUrl != null)
                    {
                        return string.IsNullOrEmpty(aliasCode) ? 
                            Ok(new { ShortenedUrl = baseUrl + shortUrl.Code, ShortUrl = _mapper.Map<ShortUrlReadDTO>(shortUrl) }):
                            Ok(new { 
                                ShortenedUrl = baseUrl + shortUrl.Code, 
                                ShortenedUrlWithAlias = baseUrl + aliasCode,
                                ShortUrl = _mapper.Map<ShortUrlReadDTO>(shortUrl),
                                ShortUrlWithAlias = _mapper.Map<ShortUrlReadDTO>(shortUrlWithAlias != null ? shortUrlWithAlias:_shortUrlRepository.GetShortUrlWithCode(aliasCode))
                            });
                    }

                    shortUrl = await _shortUrlRepository.AddShortUrl(url);

                    _shortUrlRepository.SaveChanges();
                    return string.IsNullOrEmpty(aliasCode) ? 
                            Ok(new { ShortenedUrl = baseUrl + shortUrl.Code, ShortUrl = _mapper.Map<ShortUrlReadDTO>(shortUrl) }):
                            Ok(new { 
                                ShortenedUrl = baseUrl + shortUrl.Code, 
                                ShortenedUrlWithAlias = baseUrl + aliasCode,
                                ShortUrl = _mapper.Map<ShortUrlReadDTO>(shortUrl),
                                ShortUrlWithAlias = _mapper.Map<ShortUrlReadDTO>(shortUrlWithAlias != null ? shortUrlWithAlias:_shortUrlRepository.GetShortUrlWithCode(aliasCode))
                            });
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
            return $"{request.Scheme}://{request.Host}{request.Path.ToString().TrimEnd('/')}/".Replace("api/", "");
        }
    }
}