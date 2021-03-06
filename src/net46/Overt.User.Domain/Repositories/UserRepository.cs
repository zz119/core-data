﻿using Overt.Core.Data;
using Overt.User.Domain.Contracts;
using Overt.User.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using System.Linq;

namespace Overt.User.Domain.Repositories
{
    public class UserRepository : BaseRepository<UserEntity>, IUserRepository
    {
        public UserRepository() 
            : base() // dbStoreKey 可用于不同数据库切换，连接字符串key前缀：xxx.master xxx.secondary
        {
        }

        public override Func<string> TableNameFunc => () =>
        {
            var tableName = $"{GetMainTableName()}_Test";
            return tableName;
        };

        public override Func<string, string> CreateScriptFunc => (tableName) =>
        {
            return "CREATE TABLE `" + tableName + "` (" +
                   "  `UserId` int(11) NOT NULL AUTO_INCREMENT," +
                   "  `UserName` varchar(200) DEFAULT NULL," +
                   "  `Password` varchar(200) DEFAULT NULL," +
                   "  `RealName` varchar(200) DEFAULT NULL," +
                   "  `AddTime` datetime DEFAULT NULL," +
                   "  `IsSex` bit(1) DEFAULT NULL," +
                   "  `JsonValue` json DEFAULT NULL," +
                   "  `Join` varchar(255) DEFAULT NULL," +
                   "  `ENValue` int(11) DEFAULT NULL," +
                   "  PRIMARY KEY(`UserId`)" +
                   ") ENGINE = InnoDB AUTO_INCREMENT = 3748 DEFAULT CHARSET = utf8mb4; ";
        };

        public async Task<List<string>> OtherSqlAsync()
        {
            // 表名最好使用这个方法获取，支持分表，分表案例详见其他案例
            var tableName = GetTableName();
            var sql = $"select distinct(UserName) from {tableName}";
            return await Execute(async connecdtion =>
            {
                var task = await connecdtion.QueryAsync<string>(sql);
                return task.ToList();
            });
        }
    }
}
