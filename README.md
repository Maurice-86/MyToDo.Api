# MyToDo.Api ��Ŀ�ĵ�

## 1. ��Ŀ����

### 1.1 ��Ŀ����
- ���� .NET 8.0 �Ĵ����������ϵͳ
- WebApi + JWT ��֤��ǰ��˷���ܹ�
- Repository + UnitOfWork ģʽ�����ݷ��ʲ�
- ʹ�� Entity Framework Core + SQLite �����ݳ־û�

### 1.2 ��Ҫ����
- �û���֤
  - ע��/��¼
  - JWT Token ��֤
  - Token �Զ�ˢ��
- �����������
  - ��ɾ�Ĳ�
  - ״̬��������/��ɣ�
- ����¼����
  - ��ɾ�Ĳ�
  - ���ݹ���

## 2. ����ջ

### 2.1 ���Ŀ��
- .NET 8.0
- ASP.NET Core WebApi
- Entity Framework Core 9.0

### 2.2 ���ݿ�
- SQLite
- Entity Framework Core
- Repository + UnitOfWork ģʽ

### 2.3 �����֤
- JWT Bearer Token
- BCrypt.Net-Next (�������)

### 2.4 �������߰�
- AutoMapper 13.0 (����ӳ��)
- FluentValidation (������֤)
- Swagger/OpenAPI (API�ĵ�)

### 2.5 ��Ŀ������

#### EF Core ���
- Microsoft.EntityFrameworkCore
- Microsoft.EntityFrameworkCore.Sqlite
- Microsoft.EntityFrameworkCore.Design
- Microsoft.EntityFrameworkCore.Tools
- Microsoft.EntityFrameworkCore.AutoHistory

#### ����ӳ��
- AutoMapper

#### ������֤
- FluentValidation
- FluentValidation.AspNetCore
- FluentValidation.DependencyInjectionExtensions

#### ��֤���
- Microsoft.AspNetCore.Authentication.JwtBearer
- BCrypt.Net-Next

#### API�ĵ�
- Swashbuckle.AspNetCore

## 3. ��Ŀ�ṹ

### 3.1 Ŀ¼�ṹ
```
MyToDo.Api/
������ Controllers/            # API������
��   ������ ToDoController.cs  # �������������
��   ������ MemoController.cs  # ����¼������
��   ������ UserController.cs  # �û�������
������ Domain/                # �����
��   ������ Entities/         # ʵ����
��       ������ BaseEntity.cs # ʵ�����
��       ������ ToDo.cs       # ��������ʵ��
��       ������ Memo.cs       # ����¼ʵ��
��       ������ User.cs       # �û�ʵ��
������ Infrastructure/          # ������ʩ��
��   ������ Context/            # ���ݿ�������
��   ��   ������ MyToDoContext.cs
��   ������ Repository/         # �ִ�ʵ��
��   ��   ������ Repository.cs   # ͨ�òִ�����
��   ��   ������ ToDoRepository.cs
��   ��   ������ MemoRepository.cs
��   ��   ������ UserRepository.cs
��   ������ UnitOfWork/         # ������Ԫ��https://github.com/Arch/UnitOfWork��
������ Common/                  # ������
��   ������ Configurations/     # ������
��   ��   ������ JwtSettings.cs  # JWT����
��   ������ Extensions/         # ��չ����
��       ������ AutoMapperProFile.cs     # AutoMapper�����ļ�
��       ������ JwtExtensions.cs         # JWT��չ����
������ Validators/             # ��֤��
    ������ UserValidator.cs    # �û���֤��
    ������ TodoValidator.cs    # ����������֤��
    ������ MemoValidator.cs    # ����¼��֤��
```

### 3.2 ���˵��

#### 3.2.1 ����� (Domain)
- �������ҵ��ʵ��
- ����ҵ������Լ��
- ʵ���Ĺ�ϵ����

#### 3.2.2 ������ʩ�� (Infrastructure)
- ���ݳ־û�ʵ��
- �ִ�ģʽʵ��
- ������Ԫʵ��

#### 3.2.3 Ӧ�÷���� (Services)
- ҵ���߼�ʵ��
- ������֤��ת��
- ������

#### 3.2.4 ���ֲ� (Controllers)
- API �ӿ�ʵ��
- ������
- ��Ӧ��װ

## 4. ����ʵ��

### 4.1 ʵ�嶨��

#### ����ʵ��
```csharp
public class BaseEntity
{
    public int Id { get; set; }
    public long CreateTime { get; set; }
    public long UpdateTime { get; set; }
}
```

#### �û�ʵ��
```csharp
public class User : BaseEntity
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpireTime { get; set; }
}
```

### 4.2 ���ݿ�������
```csharp
public class MyToDoContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<ToDo> ToDos { get; set; }
    public DbSet<Memo> Memos { get; set; }
}
```

### 4.3 �ִ�ʵ��
- ���ڷ��Ͳִ�ģʽ
- ֧���첽����
- ���ɹ�����Ԫ

### 4.4 ��֤��
- ʹ�� FluentValidation
- ֧��ģ����֤
- �Զ�����֤����

## 5. ����˵��

### 5.1 ���ݿ�����
�� `appsettings.json` �У�
```json
{
  "ConnectionStrings": {
    "ToDoConnection": "Data Source=todo.db"
  }
}
```

### 5.2 JWT����
�� `appsettings.json` �У�
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

### 5.3 ����ע������
�� `Program.cs` �У�
```csharp
// ���ݿ�������
builder.Services.AddDbContext<MyToDoContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("ToDoConnection")));

// �ִ��͹�����Ԫ
builder.Services.AddScoped<IRepository<ToDo>, ToDoRepository>();
builder.Services.AddScoped<IRepository<Memo>, MemoRepository>();
builder.Services.AddScoped<IRepository<User>, UserRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Ӧ�÷���
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IToDoService, ToDoService>();
builder.Services.AddScoped<IMemoService, MemoService>();

// AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProFile));

// FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<UserValidator>();
```

## 6. JWT��֤ʵ��

### 6.1 ��֤����
```csharp
// ���� JWT ����
builder.Services.AddJwtAuthentication(builder.Configuration);
```

### 6.2 ��֤����

#### 6.2.1 �û�ע��
- ·�ɣ�`POST /api/User/Register`
- ������֤
- �������
- �����û���Ϣ
- ����Token

#### 6.2.2 �û���¼
- ·�ɣ�`POST /api/User/Login`
- ��֤�û�������
- ���ɷ������ƺ�ˢ������
- �����û�ˢ������

#### 6.2.3 ˢ��Token
- ·�ɣ�`POST /api/User/RefreshToken`
- ��֤ˢ������
- �����µķ������ƺ�ˢ������

### 6.3 Tokenʹ��

#### ����ͷ��ʽ
```
Authorization: Bearer {accessToken}
```

#### API����
```csharp
[Authorize]
[ApiController]
[Route("api/[controller]/[action]")]
public class ToDoController : ControllerBase
{
    // ��Ҫ��֤��API
}
```

## 7. ����ָ��

### 7.1 ��������
- Visual Studio 2022 �� VS Code
- .NET 8.0 SDK
- SQLite ����

### 7.2 ��Ŀ����
1. ��¡��Ŀ
2. ��ԭNuGet��
3. �������ݿ�����
4. ����JWT��Կ

### 7.3 ���ݿ�Ǩ��
```bash
# ���Ǩ��
Add-Migration InitialCreate

# �������ݿ�
Update-Database
```