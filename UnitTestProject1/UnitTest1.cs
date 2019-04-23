using System;
using System.Collections.Generic;
using System.Linq;
using FrameWork.MongoDB;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using System.Threading.Tasks;

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
                Subtype = 31,
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
            for (int j = 1; j <= 10; j++)
            {
                List<int> i_list = GetRandom(j * 100 + 1, true, j * 100 + 20, true, 12, new Random(), false);
                var listUser = Enumerable.Range(1, 100).Select(i => new Product
                {
                    _id = Guid.NewGuid().ToString("N"),
                    CategoryID = j,
                    Name = string.Format("prtest_{0}_{1}", j, i),
                    Price = 10 * i,
                    Subtype = i_list[i%10],
                    Description = "xxxfjdosaijfoij",
                    CreateDateTime = DateTime.Now
                }).ToList();
                new MongoDbService().BatchAdd<Product>(listUser);
            }
            var qCount = new MongoDbService().List<Product>(x => x.CategoryID == 10).Count;
            Assert.AreEqual(qCount, 100);
        }


        /// <summary>
        /// 根据随机数范围获取一定数量的随机数
        /// </summary>
        /// <param name="minNum">随机数最小值</param>
        /// <param name="minNum">是否包含最小值</param>
        /// <param name="maxNum">随机数最大值</param>
        /// <param name="minNum">是否包含最大值</param>
        /// <param name="ResultCount">随机结果数量</param>
        /// <param name="rm">随机数对象</param>
        /// <param name="isSame">结果是否重复</param>
        /// <returns></returns>
        List<int> GetRandom(int minNum, bool isIncludeMinNum, int maxNum, bool isIncludeMaxNum, int ResultCount, Random rm, bool isSame)
        {
            List<int> randomList = new List<int>();
            int nValue = 0;

            #region 是否包含最大最小值，默认包含最小值，不包含最大值
            if (!isIncludeMinNum) { minNum = minNum + 1; }
            if (isIncludeMaxNum) { maxNum = maxNum + 1; }
            #endregion

            if (isSame)
            {
                for (int i = 0; randomList.Count < ResultCount; i++)
                {
                    nValue = rm.Next(minNum, maxNum);
                    randomList.Add(nValue);
                }
            }
            else
            {
                for (int i = 0; randomList.Count < ResultCount; i++)
                {
                    nValue = rm.Next(minNum, maxNum);
                    //重复判断
                    if (!randomList.Contains(nValue))
                    {
                        randomList.Add(nValue);
                    }
                }
            }
            return randomList;
        }

        /// <summary>
        /// 查询多笔
        /// </summary>
        [TestMethod]
        public void QryList()
        {
            var qryList = new MongoDbService().List<Product>(x => true, null, 100);
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

        /// <summary>
        /// 统计
        /// </summary>
        [TestMethod]
        public void QryGroup()
        {
            List<string> _groupKeys = new List<string>();
            _groupKeys.Add("CategoryID");
            _groupKeys.Add("Subtype");

            List<string> _groupFileds = new List<string>();
            _groupFileds.Add("_sum:{$sum:'$Price'}");
            _groupFileds.Add("_count: {$sum: 1}");
            _groupFileds.Add("_avg: {$avg: '$Price'}");

            List<string> _matchs = new List<string>();
            _matchs.Add("Price:{$gt:20,$lt:200}");
            _matchs.Add("CategoryID:{$gt:5}");

            
            List<BsonDocument> retData = new MongoDbService().GetGroupData<Product>(
                _groupKeys,
                _groupFileds,
                _matchs);
            
            var ss = retData.ToJson();
        }
    }
}
