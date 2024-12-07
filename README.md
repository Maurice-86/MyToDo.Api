# MyToDo.Api 项目文档

## 1. 项目概述

### 1.1 项目介绍
- 基于 .NET 8.0 的待办事项管理系统
- WebApi + JWT 认证的前后端分离架构
- Repository + UnitOfWork 模式的数据访问层
- 使用 Entity Framework Core + SQLite 的数据持久化

### 1.2 主要功能
- 用户认证
  - 注册/登录
  - JWT Token 认证
  - Token 自动刷新
- 待办事项管理
  - 增删改查
  - 状态管理（待办/完成）
- 备忘录管理
  - 增删改查
  - 内容管理

## 2. 技术栈

### 2.1 核心框架
- .NET 8.0
- ASP.NET Core WebApi
- Entity Framework Core 9.0

### 2.2 数据库
- SQLite
- Entity Framework Core
- Repository + UnitOfWork 模式

### 2.3 身份认证
- JWT Bearer Token
- BCrypt.Net-Next (密码加密)

### 2.4 开发工具包
- AutoMapper 13.0 (对象映射)
- FluentValidation (数据验证)
- Swagger/OpenAPI (API文档)

### 2.5 项目依赖包

#### EF Core 相关
- Microsoft.EntityFrameworkCore
- Microsoft.EntityFrameworkCore.Sqlite
- Microsoft.EntityFrameworkCore.Design
- Microsoft.EntityFrameworkCore.Tools
- Microsoft.EntityFrameworkCore.AutoHistory

#### 对象映射
- AutoMapper

#### 数据验证
- FluentValidation
- FluentValidation.AspNetCore
- FluentValidation.DependencyInjectionExtensions

#### 认证相关
- Microsoft.AspNetCore.Authentication.JwtBearer
- BCrypt.Net-Next

#### API文档
- Swashbuckle.AspNetCore

## 3. 项目结构

### 3.1 目录结构
```
MyToDo.Api/
├── Controllers/            # API控制器
│   ├── ToDoController.cs  # 待办事项控制器
│   ├── MemoController.cs  # 备忘录控制器
│   └── UserController.cs  # 用户控制器
├── Domain/                # 领域层
│   └── Entities/         # 实体类
│       ├── BaseEntity.cs # 实体基类
│       ├── ToDo.cs       # 待办事项实体
│       ├── Memo.cs       # 备忘录实体
│       └── User.cs       # 用户实体
├── Infrastructure/          # 基础设施层
│   ├── Context/            # 数据库上下文
│   │   └── MyToDoContext.cs
│   ├── Repository/         # 仓储实现
│   │   ├── Repository.cs   # 通用仓储基类
│   │   ├── ToDoRepository.cs
│   │   ├── MemoRepository.cs
│   │   └── UserRepository.cs
│   └── UnitOfWork/         # 工作单元（https://github.com/Arch/UnitOfWork）
├── Common/                  # 公共类
│   ├── Configurations/     # 配置类
│   │   └── JwtSettings.cs  # JWT配置
│   └── Extensions/         # 扩展方法
│       ├── AutoMapperProFile.cs     # AutoMapper配置文件
│       └── JwtExtensions.cs         # JWT扩展方法
└── Validators/             # 验证器
    ├── UserValidator.cs    # 用户验证器
    ├── TodoValidator.cs    # 待办事项验证器
    └── MemoValidator.cs    # 备忘录验证器
```

### 3.2 层次说明

#### 3.2.1 领域层 (Domain)
- 定义核心业务实体
- 包含业务规则和约束
- 实体间的关系定义

#### 3.2.2 基础设施层 (Infrastructure)
- 数据持久化实现
- 仓储模式实现
- 工作单元实现

#### 3.2.3 应用服务层 (Services)
- 业务逻辑实现
- 数据验证和转换
- 事务处理

#### 3.2.4 表现层 (Controllers)
- API 接口实现
- 请求处理
- 响应封装

## 4. 核心实现

### 4.1 实体定义

#### 基础实体
```csharp
public class BaseEntity
{
    public int Id { get; set; }
    public long CreateTime { get; set; }
    public long UpdateTime { get; set; }
}
```

#### 用户实体
```csharp
public class User : BaseEntity
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpireTime { get; set; }
}
```

### 4.2 数据库上下文
```csharp
public class MyToDoContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<ToDo> ToDos { get; set; }
    public DbSet<Memo> Memos { get; set; }
}
```

### 4.3 仓储实现
- 基于泛型仓储模式
- 支持异步操作
- 集成工作单元

### 4.4 验证器
- 使用 FluentValidation
- 支持模型验证
- 自定义验证规则

## 5. 配置说明

### 5.1 数据库配置
在 `appsettings.json` 中：
```json
{
  "ConnectionStrings": {
    "ToDoConnection": "Data Source=todo.db"
  }
}
```

### 5.2 JWT配置
在 `appsettings.json` 中：
```json:MyToDo.Api/README.md
{
  "JwtSettings": {
    "SecretKey": "your-secret-key-at-least-16-characters",
    "Issuer": "MyToDo.Api",
    "Audience": "MyToDo.Client",
    "ExpireMinutes": 60
  }
}
```

### 5.3 依赖注入配置
在 `Program.cs` 中：
```csharp
// 数据库上下文
builder.Services.AddDbContext<MyToDoContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("ToDoConnection")));

// 仓储和工作单元
builder.Services.AddScoped<IRepository<ToDo>, ToDoRepository>();
builder.Services.AddScoped<IRepository<Memo>, MemoRepository>();
builder.Services.AddScoped<IRepository<User>, UserRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// 应用服务
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IToDoService, ToDoService>();
builder.Services.AddScoped<IMemoService, MemoService>();

// AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProFile));

// FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<UserValidator>();
```

## 6. JWT认证实现

### 6.1 认证配置
```csharp
// 配置 JWT 服务
builder.Services.AddJwtAuthentication(builder.Configuration);
```

### 6.2 认证流程

#### 6.2.1 用户注册
- 路由：`POST /api/User/Register`
- 请求验证
- 密码加密
- 保存用户信息
- 生成Token

#### 6.2.2 用户登录
- 路由：`POST /api/User/Login`
- 验证用户名密码
- 生成访问令牌和刷新令牌
- 更新用户刷新令牌

#### 6.2.3 刷新Token
- 路由：`POST /api/User/RefreshToken`
- 验证刷新令牌
- 生成新的访问令牌和刷新令牌

### 6.3 Token使用

#### 请求头格式
```
Authorization: Bearer {accessToken}
```

#### API保护
```csharp
[Authorize]
[ApiController]
[Route("api/[controller]/[action]")]
public class ToDoController : ControllerBase
{
    // 需要认证的API
}
```

## 7. 开发指南

### 7.1 开发环境
- Visual Studio 2022 或 VS Code
- .NET 8.0 SDK
- SQLite 工具

### 7.2 项目设置
1. 克隆项目
2. 还原NuGet包
3. 配置数据库连接
4. 配置JWT密钥

### 7.3 数据库迁移
```bash
# 添加迁移
Add-Migration InitialCreate

# 更新数据库
Update-Database
```