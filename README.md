# AspNet.Identity.Dapper
使用Dapper构建的asp.net Identity 用户管理

##使用
### 0
恢复nuget包

### 1 
Clone项目

### 2
更改web.config配置：
```
 <connectionStrings>
    <add name="DefaultConnection" connectionString="Data Source=(LocalDb)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\aspnet-WebDemo-20160726071121.mdf;Initial Catalog=aspnet-WebDemo-20160726071121;Integrated Security=True" providerName="System.Data.SqlClient" />
    <add name="localmysql" connectionString="Server=localhost;Database=hx_crm;Uid=root;Pwd=123456;Convert Zero Datetime=true; pooling=true;min pool size=3;max pool size=100;" providerName="MySql.Data.MySqlClient" />
  </connectionStrings>
  <appSettings>
    <add key="databaseConfName" value="localmysql"/>

    <add key="SmtpServer" value="smtpserver"/>
    <add key="EmailUser" value="emailUserName"/>
    <add key="EmailPassWord" value="emailPassWord"/>
```
链接字符串改为自己的数据库

### 3
在自己的mysql数据库执行建表语句
MySQLIdentity.sql

### 4
启动调试