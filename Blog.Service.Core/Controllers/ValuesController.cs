using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using Blog.Service.Core.Dto;
using Microsoft.AspNetCore.Authorization;

namespace Blog.Service.Core.Controllers
{
    [ApiVersion("1.0")]
    [Route("/api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        /// <summary>
        /// 获取输入的值
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Policy = "Admin")] //使用该策略可以使得这个api有两个角色
        public ActionResult<IEnumerable<string>> Get([FromQuery]CodeName input)
        {
            return new[] { $"Code:{input.Code}", $"Name:{input.Name}" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}