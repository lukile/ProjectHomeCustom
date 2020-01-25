using System.Collections.Generic;

namespace Openhab.Proxy.Api.Models
{
    /// <summary>
    /// Represents a specific zone (e.g. GroundFloor, Outside)
    /// </summary>
    public class Zone
    {
        /// <summary>
        /// Zone id in openhab
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Zone sub name in openhab
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Zone description/label
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Zone rooms
        /// </summary>
        public IEnumerable<Room> Rooms { get; set; }
    }
}