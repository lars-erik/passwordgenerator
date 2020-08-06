using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PasswordGenerator.WebSite.Api
{
    [Route("api/[controller]/{isoDate?}/{salt?}")]
    [ApiController]
    public class PasswordsController : ControllerBase
    {
        private readonly Generator generator;

        public PasswordsController(Generator generator)
        {
            this.generator = generator;
        }

        public Dictionary<string, string> Get(string isoDate = null, string salt = null)
        {
            if (!DateTime.TryParseExact(isoDate, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
            {
                date = DateTime.Today;
            }

            return new Dictionary<string, string>
            {
                ["Date"] = isoDate,
                ["Salt"] = salt,
                ["Password"] = generator.Generate(date, salt)
            };
        }
    }
}
