using System;

namespace Openhab.Proxy.Api.Configuration
{
    public interface ITokenController
    {
        Guid Uuid { get; set; }
        string Token { get; set; }
        string Group { get; set; }
    }
}
