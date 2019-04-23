using System;
using System.Collections.Generic;
using FrameWork.MongoDB.MongoDbConfig;
using MongoDB.Bson.Serialization.Attributes;


namespace UnitTestProject1
{
    [Mongo("TestDB1", "Products")]
    public class Product : MongoEntity
    {
        public string Name { get; set; }
        public Decimal Price { get; set; }
        public int CategoryID { get; set; }
        public string Description { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreateDateTime { get; set; }

    }
}
