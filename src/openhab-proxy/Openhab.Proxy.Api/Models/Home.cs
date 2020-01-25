using System;
using System.Collections.Generic;

namespace Openhab.Proxy.Api.Models
{
    /// <summary>
    /// Home configuration including zones, rooms and devices
    /// </summary>
    public class Home
    {
        /// <summary>
        /// Home id in openhab
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Access Token UUID
        /// </summary>
        public Guid Uuid { get; set; }
        /// <summary>
        /// Namespace value for dialogflow (internal)
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// Home Zones
        /// </summary>
        public IEnumerable<Zone> Zones { get; set; }
    }
}
