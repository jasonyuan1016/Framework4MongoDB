using System;
using FrameWork.MongoDB.MongoDbConfig;

namespace UnitTestProject1
{
    [Mongo("TestDB1", "Products")]
    public class Product : MongoEntity
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public int CategoryID { get; set; }
        public int Subtype { get; set; }
        public string Description { get; set; }

        public DateTime CreateDateTime { get; set; }

    }
}
