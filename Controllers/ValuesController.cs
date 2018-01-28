﻿using System;
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
            "server=ardraw-db-instance.crr5w2xqdwfp.us-east-2.rds.amazonaws.com;database=ardraw_db;user=ardraw_user;port=3306;password=hackconcordia;";

        private MySqlConnection _conn;

        public ValuesController()
        {
            _conn = new MySqlConnection(_connStr);
        }
        
        // GET api/values
        [HttpGet("region")]
        public IActionResult Get()
        {
            var drawings = new List<Drawing>();
            _conn.Open();
            
            var command = new MySqlCommand("SELECT * FROM drawings", _conn);
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var drawing = new Drawing
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
            
            _conn.Close();

            return Ok(drawings);
        }

        [HttpGet]
        public IActionResult Regional([FromQuery] decimal lat, [FromQuery] decimal lon)
        {
            var drawings = new List<Drawing>();
            _conn.Open();
            
            var command = new MySqlCommand("SELECT * FROM drawings WHERE latitude >= @lat - 0.05 AND longitude >= @lon - 0.05 AND latitude <= @lat + 0.05 AND longitude <= @lon + 0.05", _conn);
            command.Parameters.Add(new MySqlParameter("lat", lat));
            command.Parameters.Add(new MySqlParameter("lon", lon));
            
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var drawing = new Drawing
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
            
            _conn.Close();

            return Ok(drawings);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] Drawing value)
        {
            _conn.Open();
            
            var command = new MySqlCommand("INSERT INTO drawings(latitude, longitude, altitude, bearing, pathData, color) VALUES @lat, @lon, @altitude, @bearing, @path, @color", _conn);
            command.Parameters.Add(new MySqlParameter("lat", value.Latitude));
            command.Parameters.Add(new MySqlParameter("lon", value.Longitude));
            command.Parameters.Add(new MySqlParameter("altitude", value.Altitude));
            command.Parameters.Add(new MySqlParameter("bearing", value.Bearing));
            command.Parameters.Add(new MySqlParameter("path", value.PathData));
            command.Parameters.Add(new MySqlParameter("color", value.Color));

            command.ExecuteNonQuery();
            
            _conn.Close();
        }
    }
}