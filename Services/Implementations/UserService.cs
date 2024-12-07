using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyToDo.Api.Common.Configurations;
using MyToDo.Api.Domain.Entities;
using MyToDo.Api.Services.Interfaces;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MyToDo.Api.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork work;
        private readonly IMapper mapper;
        private readonly JwtSettings jwtSettings;

        public UserService(IUnitOfWork work, IMapper mapper, IOptions<JwtSettings> options)
        {
            this.work = work;
            this.mapper = mapper;
            jwtSettings = options.Value;
        }

        public async Task<ApiResponse> LoginAsync(string username, string password)
        {
            try
            {
                var repository = work.GetRepository<User>();
                var user = await repository.GetFirstOrDefaultAsync(predicate:
                        x => x.UserName.Equals(username));

                if (user == null || !VerifyPassword(password, user.Password))
                    return new ApiResponse("用户名或密码错误");

                // 生成Token
                var accessToken = GenerateJwtToken(user);
                var refreshToken = GenerateRefreshToken();

                // 保存刷新令牌
                user.RefreshToken = refreshToken;
                user.RefreshTokenExpireTime = DateTime.Now.AddDays(7);

                repository.Update(user);
                if (await work.SaveChangesAsync() <= 0)
                {
                    return new ApiResponse("登录失败：更新刷新令牌失败");
                }

                return new ApiResponse(new
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    User = mapper.Map<UserDto>(user)
                });
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }

        public async Task<ApiResponse> RefreshTokenAsync(string refreshToken)
        {
            try
            {
                var repository = work.GetRepository<User>();

                // 查找具有此刷新令牌的用户
                var user = await repository.GetFirstOrDefaultAsync(predicate:
                    x => x.RefreshToken!.Equals(refreshToken));
                if (user == null)
                    return new ApiResponse("无效的刷新令牌");

                // 检查刷新令牌是否过期
                if (user.RefreshTokenExpireTime <= DateTime.Now)
                    return new ApiResponse("刷新令牌已过期");

                // 生成新的访问令牌和刷新令牌
                var newAccessToken = GenerateJwtToken(user);
                var newRefreshToken = GenerateRefreshToken();

                // 更新用户的刷新令牌
                user.RefreshToken = newRefreshToken;
                user.RefreshTokenExpireTime = DateTime.Now.AddDays(7);

                repository.Update(user);
                if (await work.SaveChangesAsync() <= 0)
                    return new ApiResponse("更新令牌失败");

                return new ApiResponse(new
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken,
                    User = mapper.Map<UserDto>(user)
                });
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }

        public async Task<ApiResponse> RegisterAsync(UserDto dto)
        {
            try
            {
                var repository = work.GetRepository<User>();

                // 检查用户是否存在
                var exisUser = await repository.GetFirstOrDefaultAsync(predicate:
                    x => x.UserName.Equals(dto.UserName));

                if (exisUser != null)
                    return new ApiResponse("注册失败，账号已存在");

                // 创建用户
                var user = mapper.Map<User>(dto);
                user.Password = HashPassword(user.Password);
                user.CreateTime = DateTimeOffset.Now.ToUnixTimeSeconds();

                // 生成Token
                var accessToken = GenerateJwtToken(user);
                var refreshToken = GenerateRefreshToken();

                user.RefreshToken = refreshToken;
                user.RefreshTokenExpireTime = DateTime.Now.AddDays(7);

                await repository.InsertAsync(user);
                if (await work.SaveChangesAsync() <= 0)
                    return new ApiResponse("注册失败：创建用户失败");

                return new ApiResponse("注册成功", new
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    User = mapper.Map<UserDto>(user)
                });
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }

        /// <summary>
        /// 生成 JWT 令牌
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
                // 可以添加更多的 Claim
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings.Issuer,
                audience: jwtSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(jwtSettings.ExpireMinutes),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// 生成刷新令牌
        /// </summary>
        /// <returns></returns>
        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        /// <summary>
        /// 返回一个哈希的密码
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        private static string HashPassword(string password)
        {
            // 使用 BCrypt 加密密码
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        /// <summary>
        /// 验证密码是否正确
        /// </summary>
        /// <param name="password"></param>
        /// <param name="hashedPassword"></param>
        /// <returns></returns>
        private static bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
