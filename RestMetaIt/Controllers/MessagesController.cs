using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestMetaIt.Model;

namespace RestMetaIt.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {

        private IMemoryCache _memoryCache;
        private IConfiguration _configuration;
        public MessagesController(IMemoryCache memoryCache, IConfiguration configuration)
        {
            _memoryCache = memoryCache;
            _configuration = configuration;
        }
        private AppParams _appParams;

        [HttpGet]
        public ActionResult<IEnumerable<MetaItMessage>> Get()
        {
            _memoryCache.TryGetValue("MetaItMessages", out List<MetaItMessage> metaItMessages);
            return (metaItMessages is null) ? new List<MetaItMessage>() : metaItMessages.OrderByDescending(o => o.Ts).Take(100).ToList();
        }

        [HttpPost]
        public ActionResult Post([FromBody] string value)
        {
            _appParams = GetAppParams();
            try
            {
                var metaItMessage = JsonConvert.DeserializeObject<MetaItMessage>(value);
                if (_memoryCache.TryGetValue("MetaItMessages", out List<MetaItMessage> metaItMessages))
                {
                    metaItMessages.Add(metaItMessage);
                }
                else
                {
                    metaItMessages = new List<MetaItMessage>() { metaItMessage };
                }
                _memoryCache.Set("MetaItMessages", metaItMessages);
                return Ok("ok");
            }
            catch
            {
                return BadRequest("nok");
            }
        }

        public AppParams GetAppParams()
        {
            try
            {
                return (_configuration != null) ? _configuration.GetSection("AppParams").Get<AppParams>() : new AppParams();
            }
            catch(Exception e)
            {
                throw new NotImplementedException(e.Message);
            }
        }
    }
}
