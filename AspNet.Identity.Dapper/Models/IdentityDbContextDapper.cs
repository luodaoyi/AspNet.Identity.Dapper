using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace AspNet.Identity.Dapper
{
    public class IdentityDbContextDapper :IDisposable
    {

        private IDbConnection _connection;

        /// <summary>
        /// 没有连接字符串名的构造函数，默认使用第一个连接字符串
        /// </summary>
        public IdentityDbContextDapper()
            : this(null)
        {
        }


        /// <summary>
        /// 返回打开的连接
        /// </summary>
        public IDbConnection Connection
        {
            get
            {
                if(_connection.State !=ConnectionState.Open)
                    _connection.Open();

                return _connection;
            }
        }

        /// <summary>
        /// 有连接字符串名的构造函数
        /// </summary>
        /// <param name="connstring"></param>
        public IdentityDbContextDapper(string connstring)
        {
            if(string.IsNullOrEmpty(connstring))
            {
                connstring = ConfigurationManager.ConnectionStrings[0].ConnectionString;
            }
            else
            {
                connstring = ConfigurationManager.ConnectionStrings[connstring].ConnectionString;
            }
            //_connection = new SqlConnection(connstring);
            //这里暂时使用Mysql 后面可以修改任何一种Dapper支持的数据库
            _connection = new MySqlConnection(connstring);
        }

        public void Dispose()
        {
            if(_connection != null)
            {
                if(_connection.State!=ConnectionState.Closed)
                {
                    _connection.Close();
                }
                _connection.Dispose();
                _connection = null;
            }
        }
    }
}
