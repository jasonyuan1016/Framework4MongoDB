using System;
using System.Linq;
using FrameWork.MongoDB;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        
        /// <summary>
        /// 新增-修改-删除
        /// </summary>
        [TestMethod]
        public void Add_Update_Delete()
        {
            var dbService = new MongoDbService();

            var id = Guid.NewGuid().ToString();
            dbService.Add(new Product
            {
                _id = id,
                CategoryID = 3,
                Price = 7000,
                Name = "Iphone XS",
                CreateDateTime = DateTime.Now
            });

            var itm = dbService.Get<Product>(x => x._id.Equals(id));

            itm.Description = "fldsakjflksadjdflka";
            dbService.Update<Product>(itm);

            var itmCheck = dbService.Get<Product>(x => x._id.Equals(id));

            dbService.Delete<Product>(itmCheck);
        }

        /// <summary>
        /// 批量写入
        /// </summary>
        [TestMethod]
        public void BatchAdd()
        {
            var listUser = Enumerable.Range(0, 100).Select(i => new Product
            {
                _id = Guid.NewGuid().ToString("N"),
                CategoryID = 1,
                Name = "pptest" + i,
                Price = 10 * i,
                Description = "xxxfjdosaijfoij",
                CreateDateTime = DateTime.Now
            }).ToList();

            new MongoDbService().BatchAdd<Product>(listUser);
        }

        /// <summary>
        /// 查询多笔
        /// </summary>
        [TestMethod]
        public void QryList()
        {
            var qryList = new MongoDbService().List<Product>(x => true);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        [TestMethod]
        public void QryPageList()
        {
            int pageIndex = 1;
            int pageSize = 20;
            var pageData = new MongoDbService().PageList<Product>(x => x.CategoryID == 1, x => new Product() { _id = x._id, Name = x.Name, Price = x.Price }, pageIndex, pageSize, x => x.CreateDateTime, true);
        }
    }
}
