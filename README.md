# FastNet
基于 ASP.NET Core + EFCore + AdminLTE 开发出来的框架，源代码完全开源。内置后台基础常用功能。
其核心设计目标是开发迅速、代码量少、学习简单、功能强大、轻量级、易扩展，让Web开发更迅速、简单。

## 当前版本
* 基于.Net Core 2.2 开发；
* ORM 使用 EF core 
* SqlServer 数据库 项目含有 fastnet.sql数据生成文件

## 技术介绍
1、前端技术
* AdminLTE作为管理框架
* 数据表格：datatable
* 对话框：layer
* 下拉选择框：jQuery Select2

2、后端技术

* 核心框架：ASP.NET CORE、WEB API
* 持久层框架：EF CORE
* 缓存框架：微软自带Cache、Redis
* 日志管理：NLog、登录日志、操作日志

## 项目结构
![](https://github.com/lovachen/FastNet/raw/master/1553821813(1).jpg)
* Controllers 为Api层，为WebApi所用
* Pages 为页面文件 其中Admin 为后台管理页面，也是我们当前主要实现的页面
* Pages 其它为前台页面，自行实现项目
### 项目分层
* AiBao.Core 项目核心层 引用了 [cts.web.core 类库](https://github.com/lovachen/cts.web.core) ,这也是我封装的一个类库
* AiBao.Entities 数据库实体层，本系统才使用数据库优先的开发模式，对于一般系统来说，优先构建数据开发 通过使用[Scaffold-DbContext](https://docs.microsoft.com/en-us/ef/core/miscellaneous/cli/dotnet)来生成数据库实体
* AiBao.Mapping 数据实体映射。AiBao.Entities 作为数据库实体生成，常常在于数据改动时回重新生成，因此不对实体做任何改动，从而引入了实体映射通过[AutoMapper](https://www.nuget.org/packages/AutoMapper/) 将实体映射转换
* AiBao.Services 服务层 承担着于数据库的操作，缓存的操作于此，通过程序的反射批量注入[DI](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-2.2)。
* AiBao.Web 项目UI层 承载项目所有的页面，Web Api
### 启动默认
系统启动时回将基础的参数配置，菜单配置默认写入数据库，初始化系统。
* 菜单 sitemap.xml 文件进行配置，配置的详情说明于 [cts.web.core 类库](https://github.com/lovachen/cts.web.core)实现。通过读取配置文件，生成菜单写入数据库完成初始化
* 初始化系统参数，系统在启动时可以配置相关的系统参数 
* 系统默认启用 [NLog](https://github.com/NLog/NLog.Web/wiki/Getting-started-with-ASP.NET-Core-2)作为错入日志记录，并写入数据库而不是文件
### 后台管理入口
单一的项目分为 前台，后台，Api等，系统启动时显示默认页面（前台首页），通过连接进入后台(https://localhost:44338/admin)。
数据在生成时默认添加了一个超级管理元账号【admin】密码：abc123
* 密码存储：在系统设计上，密码的存储并非明文。而是Md5（密码+Salt）存储，引入密码盐（Salt）使MD5更散列。
* 登陆实现：登陆时进行两次请求，提一次：通过账号，获取密码盐（Salt）和随机数R，通过jqery.md5插件运算 MD5(MD5（密码+Salt）+R)得到最终提交的密码参数。R记录在数据中，登陆验证时通过数据库记录的用户MD5(存储密码+R）进行匹配。R随机只使用一次，无论登陆成功与否都将清除此R，不可再用.
* 登陆成功后将一些用户常见的信息写入cookie，为了有可能的开发管理App，所有写入cookie 的配置和写入Jwt的配置进行统一，Jwt的说明 [cts.web.core 类库](https://github.com/lovachen/cts.web.core)实现
