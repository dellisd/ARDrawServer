using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace ARDrawServer.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {

        private string _connStr =
            "server=ardraw-db-instance.crr5w2xqdwfp.us-east-2.rds.amazonaws.com;user=ardraw_user;database=ardraw_db;port=3306;password=;";

        private MySqlConnection _conn;

        public ValuesController()
        {
            _conn = new MySqlConnection(_connStr);
        }
        
        // GET api/values
        [HttpGet]
        public IActionResult Get()
        {
            var drawings = new List<Drawing>();
            _conn.Open();
            
            var command = new MySqlCommand("SELECT * FROM drawings", _conn);
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var drawing = new Drawing()
                    {
                        Id = reader.GetInt32(0),
                        Latitude = reader.GetDecimal(1),
                        Longitude = reader.GetDecimal(2),
                        Altitude = reader.GetDecimal(3),
                        Bearing = reader.GetDecimal(4),
                        Color = reader.GetInt32(6),
                        PathData = reader.GetString(5)
                    };
                    
                    drawings.Add(drawing);
                }
            }

            return Ok(drawings);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
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