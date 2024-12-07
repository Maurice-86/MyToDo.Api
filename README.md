# MyToDo.Api ��Ŀ�ĵ�

## ��Ŀ�ܹ�

### 1. ����� (Domain)
λ�ã�`Domain/Entities/`
- `IEntity.cs` - ʵ��ӿڣ���������ֶ�
- `BaseEntity.cs` - ʵ����࣬ʵ�� IEntity  
    ```csharp
    public class BaseEntity : IEntity
    {
        public int Id { get; set; }
        public long CreateTime { get; set; }
        public long UpdateTime { get; set; }
    }  
    ```
- ҵ��ʵ�壺
  - `ToDo.cs` - ��������
  - `Memo.cs` - ����¼
  - `User.cs` - �û�

### 2. ������ʩ�� (Infrastructure)
λ�ã�`Infrastructure/`

#### 2.1 ���ݷ���
- `Context/MyToDoContext.cs` - EF Core ���ݿ�������  
    ```csharp
    public class MyToDoContext : DbContext
    {
        public MyToDoContext(DbContextOptions<MyToDoContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<ToDo> ToDos { get; set; }
        public DbSet<Memo> Memos { get; set; }
    }  
    ```

#### 2.2 �ִ�ʵ��
- `Repository/` - ���� UnitOfWork ģʽ�Ĳִ�ʵ��
  - `ToDoRepository.cs`
  - `MemoRepository.cs`
  - `UserRepository.cs`

### 3. Ӧ�÷���� (Services)
λ�ã�`Services/`
- �ӿڶ��壺
  - `IToDoService.cs`
  - `IMemoService.cs`
  - `IUserService.cs`
- ʵ���ࣺ
  - `ToDoService.cs`
  - `MemoService.cs`
  - `UserService.cs`

### 4. ���ֲ� (Controllers)
λ�ã�`Controllers/`
- `ToDoController.cs`
- `MemoController.cs`
- `UserController.cs`

## �������˵��

### 1. ����ע������
�� `Program.cs` �����ã�

```csharp
var builder = WebApplication.CreateBuilder(args);

// 1. ���ݿ�������ע��
builder.Services.AddDbContext<MyToDoContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("ToDoConnection")));

// 2. �ִ���ע��
builder.Services.AddScoped<IRepository<ToDo>, ToDoRepository>();
builder.Services.AddScoped<IRepository<Memo>, MemoRepository>();
builder.Services.AddScoped<IRepository<User>, UserRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork<MyToDoContext>>();

// 3. �����ע��
builder.Services.AddScoped<IToDoService, ToDoService>();
builder.Services.AddScoped<IMemoService, MemoService>();
builder.Services.AddScoped<IUserService, UserService>();

// 4. AutoMapper ����
builder.Services.AddAutoMapper(typeof(AutoMapperProFile));

// 5. ��� FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<UserValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<TodoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<MemoValidator>();
```

### 2. ���ݿ�����
�� `appsettings.json` �����ã�

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

### 3. AutoMapper ����
�� `Common/Extensions/AutoMapperProFile.cs` �У�

```csharp
public class AutoMapperProFile : Profile
{
    public AutoMapperProFile()
    {
        // User ӳ������
        CreateMap<User, UserDto>().ReverseMap();

        // ToDo ӳ������
        CreateMap<ToDo, TodoDto>().ReverseMap();

        // Memo ӳ������
        CreateMap<Memo, MemoDto>().ReverseMap();
    }
}
```

## ʵ��˵��

### 1. �ִ���ʵ��
- ���� Repository ģʽ
- ʵ�� CRUD ��������
- ֧���Զ����ѯ����
- ʹ�� EF Core �������ݷ���

### 2. �����ʵ��
- ע��ִ��ӿ�
- ʹ�� AutoMapper ���ж���ӳ��
- ͳһ���쳣����
- �������

### 3. ������ʵ��
- RESTful API ���
- ͳһ��·�ɹ���
- ������֤
- ͳһ��Ӧ��ʽ

### 4. ����ģ��
- ʵ��ģ�ͣ��̳� BaseEntity
- DTO ģ�ͣ��������ݴ���
- ʹ�����Ա�ע������֤

## ��������

1. ����ʵ��ģ��
2. �������ݿ�Ǩ��
3. ʵ�ֲִ���
4. ʵ�ַ����
5. ʵ�ֿ�����
6. ��������ע��
7. ���� API

## ����ְ��˵��

### 1. ����� (Domain)
- �������ҵ��ʵ��
- ����ҵ������Լ��
- ʹ�����Ա�ע��֤����

### 2. ������ʩ�� (Infrastructure)
- ʵ�����ݳ־û�
- �ṩ�ִ�ģʽ��װ
- �������ݷ���ϸ��

### 3. Ӧ�÷���� (Services)
- ʵ��ҵ���߼�
- Э���������
- ��������߽�

### 4. ���ֲ� (Controllers)
- ���� HTTP ����
- ������֤
- ����ͳһ��Ӧ��ʽ

## ������������

### 1. ��Ҫ����
- .NET 8.0 SDK
- Visual Studio 2022/VS Code
- SQLite ���ݿ�

### 2. ��Ŀ����

1. Entity Framework Core ��أ�
   - `Microsoft.EntityFrameworkCore`: EF Core ���İ�
   - `Microsoft.EntityFrameworkCore.Sqlite`: SQLite ���ݿ��ṩ����
   - `Microsoft.EntityFrameworkCore.Design`: ���ʱ����֧��
   - `Microsoft.EntityFrameworkCore.Tools`: EF Core �����й���
   - `Microsoft.EntityFrameworkCore.AutoHistory`: �Զ���ʷ��¼֧��

2. ����ӳ�䣺
   - `AutoMapper`: ����֮���ӳ�乤��

3. ��֤��أ�
   - `FluentValidation`: ��������֤����
   - `FluentValidation.AspNetCore`: ASP.NET Core ����
   - `FluentValidation.DependencyInjectionExtensions`: ����ע��֧��

4. API �ĵ���
   - `Swashbuckle.AspNetCore`: Swagger/OpenAPI ֧��

### 3. ���ݿ�Ǩ��
- ���Ǩ��
- �������ݿ�ṹ

## ��Ŀ����˵��

### 1. �״�����
- ��ԭ NuGet ��
- ִ�����ݿ�Ǩ��
- ������Ŀ

### 2. ��������
- ʹ�� Swagger ���Խӿ�
- �鿴��־���
- ���ݿ������֤
