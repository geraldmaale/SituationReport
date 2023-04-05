using System.Net;
using Microsoft.AspNetCore.Http;

namespace SR.Data;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor ??
                               throw new ArgumentNullException(nameof(httpContextAccessor));
    }

    public IPAddress? GetUserIpAddress()
    {
        if (_httpContextAccessor.HttpContext != null)
        {
            var ipAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress;
            return ipAddress;
        }
            
        return IPAddress.Any;
    }

    public string? GetUsername()
    {
        if (_httpContextAccessor.HttpContext != null)
        {
            var claims = _httpContextAccessor.HttpContext.User.Claims;
            var name = claims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value;
            return name;
        }

        return null;
    }

    public string? GetFullName()
    {
        if (_httpContextAccessor.HttpContext != null)
        {
            var claims = _httpContextAccessor.HttpContext.User.Claims;
            var name = claims.FirstOrDefault(x=>x.Type == "name")?.Value;
            return name;
        }

        return null;
    }
}