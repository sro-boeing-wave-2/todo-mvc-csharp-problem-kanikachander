using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace NotesAPI.Models
{
    public class NotesAPIContext : DbContext
    {
        private readonly IMongoDatabase _database = null;

        public NotesAPIContext(IOptions<Settings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            if (client != null)
                _database = client.GetDatabase(settings.Value.Database);
        }

        public IMongoCollection<Note> Note
        {
            get
            {
                return _database.GetCollection<Note>("Note");
            }
        }
    }
}