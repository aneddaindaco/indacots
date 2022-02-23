using IndacoProject.Corso.Storage;
using IndacoProject.Corso.Storage.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace IndacoProject.Corso.Test
{
    [TestClass]
    public class MessageRepositoryTest
    {
        private DbConnection _connection;
        private MessageRepository _messageRepository;
        private ApplicationDbContext _dbContext;
        private Mock<ILogger<MessageRepository>> _serviceLoggerMock;

        [TestInitialize]
        public async Task Initialize()
        {
            _serviceLoggerMock = new Mock<ILogger<MessageRepository>>();

            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
           .UseSqlite(_connection).Options;

            _dbContext = new ApplicationDbContext(options);
            _dbContext.Database.EnsureCreated();

            _dbContext.Messages.AddRange(
                new Data.Entities.Message() { Id = Guid.NewGuid().ToString(), Body = "Body 1", Email = "mario.monti@gov.it", Subject = "subject1", Name = "Mario monti" },
                new Data.Entities.Message() { Id = Guid.NewGuid().ToString(), Body = "body 2", Email = "mario.monti2@gov.it", Subject = "subject2", Name = "Mario monti2" },
                new Data.Entities.Message() { Id = Guid.NewGuid().ToString(), Body = "body 3", Email = "mario.monti3@gov.it", Subject = "subject3", Name = "Mario monti3" }
                );
            await _dbContext.SaveChangesAsync();

            _messageRepository = new MessageRepository(_serviceLoggerMock.Object, _dbContext);
        }
        
        [TestMethod]
        public async Task Test01() 
        {
            var count = await _messageRepository.Count(default);
            Assert.AreEqual(3, count);
        }

        [TestCleanup]
        public async Task Cleanup()
        {
            await _dbContext.DisposeAsync();
            _messageRepository.Dispose();
        }
    }
}
