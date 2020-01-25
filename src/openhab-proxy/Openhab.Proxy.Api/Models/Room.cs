using System.Collections.Generic;

namespace Openhab.Proxy.Api.Models
{
    /// <summary>
    /// Represents a specific room (e.g. Office, LivingRoom)
    /// </summary>
    public class Room
    {
        /// <summary>
        /// Room id in openhab
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Room sub name in openhab
        /// (room value in dialogflow metadata)
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Room description/label
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Room devices
        /// </summary>
        public IEnumerable<Device> Devices { get; set; }
    }
}