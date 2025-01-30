using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Etiqa_Assessment_REST_API.Models;
using Etiqa_Assessment_REST_API.Repository;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Xml;

namespace WebApi_NUnitTesting
{
    [TestFixture]
    internal class UserRepositoryTest
    {
        private UserDbContext _context;

        //private UserDbContext _context;
        private UserRepository _userRepository;
        private readonly ILogger<UserRepository> _logger;

        [TearDown]
        public void TearDown()
        {
            // Dispose of the context to free up resources
            _context.Dispose();
        }

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<UserDbContext>()
            .UseInMemoryDatabase(databaseName: "test_db")
            .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
           .Options;

            _context = new UserDbContext(options);
            _userRepository = new UserRepository(_context, _logger);
        }

        [Test]
        public async Task UpdateUserAsync_ShouldUpdateUserInfo()
        {
            // Arrange
            // Add some data
            _context.users.Add(new User 
            { 
                username = "Test",
                mail = "asas.com",
                phonenumber = 28716161,

            });
            // Act
            await _context.SaveChangesAsync();
            // Assert
            // Perform the query
            var result = await _context.users.Where(e => e.username == "Test").ToListAsync();           
            Assert.NotNull(result);
            Assert.AreEqual(1, result.Count);

            // Arrange
            var userDetails = new User
            {
                username = "Santosh",
                mail = "santosh@hotmail.com",
                phonenumber = 987654231,
                skillsets = "asp.net, mvc,sql server",
                hobby = "Playing Cricket"
            };

            //Act
            await _userRepository.AddUserAsync(userDetails);
            // Assert
            var addedUser = await _context.users.FindAsync(userDetails.username);
            Assert.NotNull(addedUser);
            Assert.AreEqual(addedUser.username, userDetails.username);
            Assert.AreEqual(addedUser.mail, userDetails.mail);
            Assert.AreEqual(addedUser.phonenumber, userDetails.phonenumber);
            Assert.AreEqual(addedUser.skillsets, userDetails.skillsets);
            Assert.AreEqual(addedUser.hobby, userDetails.hobby);
        }
    }
}
