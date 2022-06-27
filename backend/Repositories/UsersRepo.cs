using System;
using System.Collections.Generic;
using System.Linq;
using WhaleSpotting.Models.Database;
using WhaleSpotting.Models.Request;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;


namespace WhaleSpotting.Repositories
{
    public interface IUsersRepo
    {
        User Create(CreateUserRequest newUser);
    }

    public class UsersRepo : IUsersRepo
    {
        private readonly WhaleSpottingDbContext _context;

        public UsersRepo(WhaleSpottingDbContext context)
        {
            _context = context;
        }

        public User Create(CreateUserRequest newUser)
        {
            // generate a 128-bit salt using a cryptographically strong random sequence of nonzero values
            byte[] salt = new byte[128 / 8];
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetNonZeroBytes(salt);
            }

            // derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: newUser.Password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8
            ));

            var insertResponse = _context.Users.Add(new User
            {
                Name = newUser.Name,
                HashedPassword = hashed,
                Salt = salt,
                Username = newUser.Username
            });

            _context.SaveChanges();

            return insertResponse.Entity;
        }
    }
}
