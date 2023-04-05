using System.Net;

namespace SR.Data;

public interface ICurrentUserService
{
    IPAddress? GetUserIpAddress();
    string? GetFullName();
    string? GetUsername();
}