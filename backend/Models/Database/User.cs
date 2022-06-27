using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WhaleSpotting.Models.Database
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Username { get; set; }

        public string HashedPassword { get; set; }

        public byte[] Salt { get; set; }
    }
}
