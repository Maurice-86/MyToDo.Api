# MyToDo.Api 项目文档

## 项目架构

### 1. 领域层 (Domain)
位置：`Domain/Entities/`
- `IEntity.cs` - 实体接口，定义基础字段
- `BaseEntity.cs` - 实体基类，实现 IEntity  
    ```csharp
    public class BaseEntity : IEntity
    {
        public int Id { get; set; }
        public long CreateTime { get; set; }
        public long UpdateTime { get; set; }
    }  
    ```
- 业务实体：
  - `ToDo.cs` - 待办事项
  - `Memo.cs` - 备忘录
  - `User.cs` - 用户

### 2. 基础设施层 (Infrastructure)
位置：`Infrastructure/`

#### 2.1 数据访问
- `Context/MyToDoContext.cs` - EF Core 数据库上下文  
    ```csharp
    public class MyToDoContext : DbContext
    {
        public MyToDoContext(DbContextOptions<MyToDoContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<ToDo> ToDos { get; set; }
        public DbSet<Memo> Memos { get; set; }
    }  
    ```

#### 2.2 仓储实现
- `Repository/` - 基于 UnitOfWork 模式的仓储实现
  - `ToDoRepository.cs`
  - `MemoRepository.cs`
  - `UserRepository.cs`

### 3. 应用服务层 (Services)
位置：`Services/`
- 接口定义：
  - `IToDoService.cs`
  - `IMemoService.cs`
  - `IUserService.cs`
- 实现类：
  - `ToDoService.cs`
  - `MemoService.cs`
  - `UserService.cs`

### 4. 表现层 (Controllers)
位置：`Controllers/`
- `ToDoController.cs`
- `MemoController.cs`
- `UserController.cs`

## 框架配置说明

### 1. 依赖注入配置
在 `Program.cs` 中配置：

```csharp
var builder = WebApplication.CreateBuilder(args);

// 1. 数据库上下文注册
builder.Services.AddDbContext<MyToDoContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("ToDoConnection")));

// 2. 仓储层注册
builder.Services.AddScoped<IRepository<ToDo>, ToDoRepository>();
builder.Services.AddScoped<IRepository<Memo>, MemoRepository>();
builder.Services.AddScoped<IRepository<User>, UserRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork<MyToDoContext>>();

// 3. 服务层注册
builder.Services.AddScoped<IToDoService, ToDoService>();
builder.Services.AddScoped<IMemoService, MemoService>();
builder.Services.AddScoped<IUserService, UserService>();

// 4. AutoMapper 配置
builder.Services.AddAutoMapper(typeof(AutoMapperProFile));

// 5. 添加 FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<UserValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<TodoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<MemoValidator>();
```

### 2. 数据库配置
在 `appsettings.json` 中配置：

```json
{
  "ConnectionStrings": {
    "ToDoConnection": "Data Source=todo.db"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

### 3. AutoMapper 配置
在 `Common/Extensions/AutoMapperProFile.cs` 中：

```csharp
public class AutoMapperProFile : Profile
{
    public AutoMapperProFile()
    {
        // User 映射配置
        CreateMap<User, UserDto>().ReverseMap();

        // ToDo 映射配置
        CreateMap<ToDo, TodoDto>().ReverseMap();

        // Memo 映射配置
        CreateMap<Memo, MemoDto>().ReverseMap();
    }
}
```

## 实现说明

### 1. 仓储层实现
- 基于 Repository 模式
- 实现 CRUD 基本操作
- 支持自定义查询方法
- 使用 EF Core 进行数据访问

### 2. 服务层实现
- 注入仓储接口
- 使用 AutoMapper 进行对象映射
- 统一的异常处理
- 事务管理

### 3. 控制器实现
- RESTful API 设计
- 统一的路由规则
- 参数验证
- 统一响应格式

### 4. 数据模型
- 实体模型：继承 BaseEntity
- DTO 模型：用于数据传输
- 使用特性标注进行验证

## 开发流程

1. 定义实体模型
2. 配置数据库迁移
3. 实现仓储层
4. 实现服务层
5. 实现控制器
6. 配置依赖注入
7. 测试 API

## 各层职责说明

### 1. 领域层 (Domain)
- 定义核心业务实体
- 包含业务规则和约束
- 使用特性标注验证规则

### 2. 基础设施层 (Infrastructure)
- 实现数据持久化
- 提供仓储模式封装
- 处理数据访问细节

### 3. 应用服务层 (Services)
- 实现业务逻辑
- 协调领域对象
- 处理事务边界

### 4. 表现层 (Controllers)
- 处理 HTTP 请求
- 参数验证
- 返回统一响应格式

## 开发环境配置

### 1. 必要工具
- .NET 8.0 SDK
- Visual Studio 2022/VS Code
- SQLite 数据库

### 2. 项目依赖

1. Entity Framework Core 相关：
   - `Microsoft.EntityFrameworkCore`: EF Core 核心包
   - `Microsoft.EntityFrameworkCore.Sqlite`: SQLite 数据库提供程序
   - `Microsoft.EntityFrameworkCore.Design`: 设计时工具支持
   - `Microsoft.EntityFrameworkCore.Tools`: EF Core 命令行工具
   - `Microsoft.EntityFrameworkCore.AutoHistory`: 自动历史记录支持

2. 对象映射：
   - `AutoMapper`: 对象之间的映射工具

3. 验证相关：
   - `FluentValidation`: 流畅的验证规则
   - `FluentValidation.AspNetCore`: ASP.NET Core 集成
   - `FluentValidation.DependencyInjectionExtensions`: 依赖注入支持

4. API 文档：
   - `Swashbuckle.AspNetCore`: Swagger/OpenAPI 支持

### 3. 数据库迁移
- 添加迁移
- 更新数据库结构

## 项目运行说明

### 1. 首次运行
- 还原 NuGet 包
- 执行数据库迁移
- 启动项目

### 2. 开发调试
- 使用 Swagger 测试接口
- 查看日志输出
- 数据库操作验证
