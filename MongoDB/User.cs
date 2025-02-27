﻿using System;
using System.Collections.Generic;
using FrameWork.MongoDB.MongoDbConfig;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDB
{
    [Mongo("TestDB1", "User")]
    public class User : MongoEntity
    {
        public string Name { get; set; }

        public int Age { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime BirthDateTime { get; set; }

        public User Son { get; set; }

        public Sex Sex { get; set; }

        public List<int> NumList { get; set; }

        public List<string> AddressList { get; set; }
    }

    public enum Sex
    {
        Man = 1,
        Woman = 2
    }
}
