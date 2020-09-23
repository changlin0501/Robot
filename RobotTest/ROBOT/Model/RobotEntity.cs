using System;
using System.Collections.Generic;
using System.Text;

using MongoDB;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
namespace ROBOT.Model
{
    public class RobotEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }

        [BsonElement("SortNumber")]
        public int SortNumber { get; set; }

        [BsonElement("RobotNumber")]
        public string RobotNumber { get; set; }

        [BsonElement("RobotName")]
        public string RobotName { get; set; }

        [BsonElement("IPAddress")]
        public string IPAddress { get; set; }

        [BsonElement("IsActive")]
        public bool IsActive { get; set; }

        [BsonIgnore]
        public Common.Enum.Robot.RobotStatus Status { get; set; }

        [BsonIgnore]
        public Model.RobotEntity RobotStatus { get; set; }

    }
}
