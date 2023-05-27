using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VIFTechCoding_API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class QuotationController : ControllerBase
    {
        private readonly IJWTManagerRepository _jWTManager;
        public QuotationController(IJWTManagerRepository jWTManager)
        {
            this._jWTManager = jWTManager;
        }
        

        // POST api/<QuotationController>
        [HttpPost]
        public Response Post([FromBody] Parameter param)
        {
           var ageArray= param.Age.Split(",");
            if(ageArray.Length!=0 && int.Parse(ageArray[0]) < 18)
            {
                return null;
            }
            var trpilength = DateTime.Parse(param.end_date).AddDays(1) - DateTime.Parse(param.start_date);
            
            double ageLoad = 0;
            foreach(var age in ageArray)
            {
                if(int.Parse(age)>=18 && int.Parse(age) <= 30)
                {
                    ageLoad +=0.6;
                }
                else if (int.Parse(age) >= 31 && int.Parse(age) <= 40)
                {
                    ageLoad += 0.7;
                }
                else if (int.Parse(age) >= 41 && int.Parse(age) <= 50)
                {
                    ageLoad += 0.8;
                }
                else if (int.Parse(age) >= 51 && int.Parse(age) <= 60)
                {
                    ageLoad += 0.9;
                }
                else if (int.Parse(age) >= 61 && int.Parse(age) <= 70)
                {
                    ageLoad += 1;
                }
            }
            var total = 3 * ageLoad * trpilength.Days;
            Response response=new Response();
            response.total = total;
            response.currency_id=param.currency_id;
            return response;

        }

        [AllowAnonymous]
        [HttpPost]
        [Route("authenticate")]
        public IActionResult Authenticate([FromBody] Users usersdata)
        {
            var token = _jWTManager.Authenticate(usersdata);

            if (token == null)
            {
                return Unauthorized();
            }

            return Ok(token);
        }
    }
}
