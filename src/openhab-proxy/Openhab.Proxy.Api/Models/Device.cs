namespace Openhab.Proxy.Api.Models
{
    /// <summary>
    ///  Represents a specific device (e.g. Blind, Door)
    /// </summary>
    public class Device
    {
        /// <summary>
        /// device id in openhab (Item Id)
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Device description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Device room
        /// </summary>
        public string Room { get; set; }
        /// <summary>
        /// Device zone
        /// </summary>
        public string Zone { get; set; }
        /// <summary>
        /// Device type exposed for dialogflow
        /// (type value in dialogflow metadata)
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// Internal openhab device type
        /// </summary>
        public string OpenhabType { get; set; }
    }
}