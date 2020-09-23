using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using ROBOT.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ROBOT.Common
{
    public class RobotMasterHelper
    {
        /// <summary>
        /// 找出机器人列表
        /// </summary>
        /// <returns></returns>
        public static List<Model.RobotEntity> GetRobotMasterList()
        {
            try
            {
                string robotMasterCollectionName = Common.ConfigFileHandler.GetAppConfig("RobotMasterCollectionName");

                var collection = Common.MongodbHandler.GetInstance().GetCollection(robotMasterCollectionName);
                //条件是可用的
                var newFilter = Builders<BsonDocument>.Filter.Eq("IsActive", true);

                //排序
                var getDocuments = Common.MongodbHandler.GetInstance().Find(collection, newFilter).Sort(Builders<BsonDocument>.Sort.Ascending("SortNumber")).ToList();

                if (getDocuments != null && getDocuments.Count > 0)
                {
                    List<Model.RobotEntity> robotList = new List<Model.RobotEntity>();

                    foreach (var item in getDocuments)
                    {
                        Model.RobotEntity robotEntity = BsonSerializer.Deserialize<Model.RobotEntity>(item);

                        robotEntity.Status = Robot.RobotStatus.Default;

                        robotList.Add(robotEntity);
                    }

                    return robotList;
                }

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// 插入机器人
        /// </summary>
        /// <param name="robotEntity"></param>
        /// <returns></returns>
        public static bool InsertRobotMasterEntity(Model.RobotEntity robotEntity)
        {
            try
            {
                string robotMasterCollectionName = Common.ConfigFileHandler.GetAppConfig("RobotMasterCollectionName");

                var collection = MongodbHandler.GetInstance().mc_MongoDatabase.GetCollection<Model.RobotEntity>(robotMasterCollectionName);
                collection.InsertOne(robotEntity);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 插入机器人列表
        /// </summary>
        /// <param name="robotEntitys"></param>
        /// <returns></returns>
        public static bool InsertRobotMasterList(List<Model.RobotEntity> robotEntitys)
        {
            try
            {
                List<bool> result = new List<bool>();
                foreach (var item in robotEntitys)
                {
                    result.Add(InsertRobotMasterEntity(item));
                }

                //全部成功
                if (result.All(t => t)) return true;

                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 修改机器人实体
        /// </summary>
        /// <param name="robotEntity"></param>
        /// <returns></returns>
        public static bool UpdateRobotMasterEntity(Model.RobotEntity robotEntity)
        {
            try
            {
                string robotMasterCollectionName = Common.ConfigFileHandler.GetAppConfig("RobotMasterCollectionName");
                var collection = MongodbHandler.GetInstance().GetCollection(robotMasterCollectionName);

                var filterID = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(robotEntity.id));
                BsonDocument update = robotEntity.ToBsonDocument();
                //不能更新ID
                update.Remove("_id");

                //更新
                BsonDocument updateResult = collection.FindOneAndUpdate(filterID, update);

                if (updateResult != null)
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 修改机器人列表
        /// </summary>
        /// <param name="robotEntityList"></param>
        /// <returns></returns>
        public static bool UpdateRobotMasterEntity(List<Model.RobotEntity> robotEntityList)
        {
            try
            {
                List<bool> result = new List<bool>();
                foreach (var item in robotEntityList)
                {
                    result.Add(UpdateRobotMasterEntity(item));
                }

                //全部成功
                if (result.All(t => t)) return true;

                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 删除机器人
        /// </summary>
        /// <param name="robotEntity"></param>
        /// <returns></returns>
        public static bool RemoveRobotMasterEntity(Model.RobotEntity robotEntity)
        {
            try
            {
                string robotMasterCollectionName = Common.ConfigFileHandler.GetAppConfig("RobotMasterCollectionName");
                var collection = MongodbHandler.GetInstance().GetCollection(robotMasterCollectionName);

                var filterID = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(robotEntity.id));

                //删除
                BsonDocument updateResult = collection.FindOneAndDelete(filterID);

                if (updateResult != null)
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
