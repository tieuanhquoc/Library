using System.Security.Claims;

namespace TieuAnhQuoc.Extensions;

public static class ClaimsExtension
{
    public static string GetUserId(this ClaimsPrincipal principal)
    {
        return principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }

    public static string GetName(this ClaimsPrincipal principal)
    {
        return principal.FindFirst(ClaimTypes.Name)?.Value;
    }

    public static string GetEmail(this ClaimsPrincipal principal)
    {
        return principal.FindFirst(ClaimTypes.Email)?.Value;
    }

    public static string GetMobile(this ClaimsPrincipal principal)
    {
        return principal.FindFirst(ClaimTypes.MobilePhone)?.Value;
    }

    public static string GetRole(this ClaimsPrincipal principal)
    {
        return principal.FindFirst(ClaimTypes.Role)?.Value;
    }

    public static TEnum GetEnum<TEnum>(this ClaimsPrincipal principal, string claimType) where TEnum : struct
    {
        var value = principal.FindFirst(claimType)?.Value;
        Enum.TryParse(value, out TEnum role);
        return role;
    }
}