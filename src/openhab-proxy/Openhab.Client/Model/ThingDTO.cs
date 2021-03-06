/* 
 * openHAB REST API
 *
 * No description provided (generated by Swagger Codegen https://github.com/swagger-api/swagger-codegen)
 *
 * OpenAPI spec version: 2.5
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace Openhab.Client.Model
{
    /// <summary>
    /// ThingDTO
    /// </summary>
    [DataContract]
    public partial class ThingDTO :  IEquatable<ThingDTO>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ThingDTO" /> class.
        /// </summary>
        /// <param name="label">label.</param>
        /// <param name="bridgeUID">bridgeUID.</param>
        /// <param name="configuration">configuration.</param>
        /// <param name="properties">properties.</param>
        /// <param name="uID">uID.</param>
        /// <param name="thingTypeUID">thingTypeUID.</param>
        /// <param name="channels">channels.</param>
        /// <param name="location">location.</param>
        public ThingDTO(string label = default(string), string bridgeUID = default(string), Dictionary<string, object> configuration = default(Dictionary<string, object>), Dictionary<string, string> properties = default(Dictionary<string, string>), string uID = default(string), string thingTypeUID = default(string), List<ChannelDTO> channels = default(List<ChannelDTO>), string location = default(string))
        {
            Label = label;
            BridgeUID = bridgeUID;
            Configuration = configuration;
            Properties = properties;
            UID = uID;
            ThingTypeUID = thingTypeUID;
            Channels = channels;
            Location = location;
        }
        
        /// <summary>
        /// Gets or Sets Label
        /// </summary>
        [DataMember(Name="label", EmitDefaultValue=false)]
        public string Label { get; set; }

        /// <summary>
        /// Gets or Sets BridgeUID
        /// </summary>
        [DataMember(Name="bridgeUID", EmitDefaultValue=false)]
        public string BridgeUID { get; set; }

        /// <summary>
        /// Gets or Sets Configuration
        /// </summary>
        [DataMember(Name="configuration", EmitDefaultValue=false)]
        public Dictionary<string, object> Configuration { get; set; }

        /// <summary>
        /// Gets or Sets Properties
        /// </summary>
        [DataMember(Name="properties", EmitDefaultValue=false)]
        public Dictionary<string, string> Properties { get; set; }

        /// <summary>
        /// Gets or Sets UID
        /// </summary>
        [DataMember(Name="UID", EmitDefaultValue=false)]
        public string UID { get; set; }

        /// <summary>
        /// Gets or Sets ThingTypeUID
        /// </summary>
        [DataMember(Name="thingTypeUID", EmitDefaultValue=false)]
        public string ThingTypeUID { get; set; }

        /// <summary>
        /// Gets or Sets Channels
        /// </summary>
        [DataMember(Name="channels", EmitDefaultValue=false)]
        public List<ChannelDTO> Channels { get; set; }

        /// <summary>
        /// Gets or Sets Location
        /// </summary>
        [DataMember(Name="location", EmitDefaultValue=false)]
        public string Location { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class ThingDTO {\n");
            sb.Append("  Label: ").Append(Label).Append("\n");
            sb.Append("  BridgeUID: ").Append(BridgeUID).Append("\n");
            sb.Append("  Configuration: ").Append(Configuration).Append("\n");
            sb.Append("  Properties: ").Append(Properties).Append("\n");
            sb.Append("  UID: ").Append(UID).Append("\n");
            sb.Append("  ThingTypeUID: ").Append(ThingTypeUID).Append("\n");
            sb.Append("  Channels: ").Append(Channels).Append("\n");
            sb.Append("  Location: ").Append(Location).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
  
        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public virtual string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input)
        {
            return Equals(input as ThingDTO);
        }

        /// <summary>
        /// Returns true if ThingDTO instances are equal
        /// </summary>
        /// <param name="input">Instance of ThingDTO to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(ThingDTO input)
        {
            if (input == null)
                return false;

            return 
                (
                    Label == input.Label ||
                    (Label != null &&
                    Label.Equals(input.Label))
                ) && 
                (
                    BridgeUID == input.BridgeUID ||
                    (BridgeUID != null &&
                    BridgeUID.Equals(input.BridgeUID))
                ) && 
                (
                    Configuration == input.Configuration ||
                    Configuration != null &&
                    Configuration.SequenceEqual(input.Configuration)
                ) && 
                (
                    Properties == input.Properties ||
                    Properties != null &&
                    Properties.SequenceEqual(input.Properties)
                ) && 
                (
                    UID == input.UID ||
                    (UID != null &&
                    UID.Equals(input.UID))
                ) && 
                (
                    ThingTypeUID == input.ThingTypeUID ||
                    (ThingTypeUID != null &&
                    ThingTypeUID.Equals(input.ThingTypeUID))
                ) && 
                (
                    Channels == input.Channels ||
                    Channels != null &&
                    Channels.SequenceEqual(input.Channels)
                ) && 
                (
                    Location == input.Location ||
                    (Location != null &&
                    Location.Equals(input.Location))
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hashCode = 41;
                if (Label != null)
                    hashCode = hashCode * 59 + Label.GetHashCode();
                if (BridgeUID != null)
                    hashCode = hashCode * 59 + BridgeUID.GetHashCode();
                if (Configuration != null)
                    hashCode = hashCode * 59 + Configuration.GetHashCode();
                if (Properties != null)
                    hashCode = hashCode * 59 + Properties.GetHashCode();
                if (UID != null)
                    hashCode = hashCode * 59 + UID.GetHashCode();
                if (ThingTypeUID != null)
                    hashCode = hashCode * 59 + ThingTypeUID.GetHashCode();
                if (Channels != null)
                    hashCode = hashCode * 59 + Channels.GetHashCode();
                if (Location != null)
                    hashCode = hashCode * 59 + Location.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        /// To validate all properties of the instance
        /// </summary>
        /// <param name="validationContext">Validation context</param>
        /// <returns>Validation Result</returns>
        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            yield break;
        }
    }

}
