using MongoDB.Testing.Mongo;
using System;
using System.Collections.Generic;
using System.Text;

namespace NotesAPI.Tests
{
    public class MongodExeLocator : IMongoExeLocator
    {
        public string Locate()
        {
            return @"C:\Program Files\MongoDB\Server\4.0\bin\mongod.exe";
        }
    }
}
