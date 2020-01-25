using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Openhab.Client.Api;
using Openhab.Proxy.Api.Configuration;

namespace Openhab.Proxy.Api.Controllers
{
    [ApiController]
    [AuthorizeWithToken]
    [Produces("text/csv")]
    [Route("api/[controller]")]
    public class DialogflowController : ControllerBase, ITokenController
    {
        private readonly IItemsApi _itemsApi;
        public Guid Uuid { get; set; }
        public string Token { get; set; }
        public string Group { get; set; }

        public DialogflowController(IItemsApi itemsApi)
        {
            _itemsApi = itemsApi;
        }

        /// <summary>
        /// Get dialogflow entity configuration for zones
        /// </summary>
        /// <remarks></remarks>
        /// <response code="202">Accepted</response>
        /// <response code="500">Internal server error</response>
        [ProducesResponseType(500)]
        [HttpGet]
        [Route("entities/zone")]
        public async Task<IActionResult> DialogflowZoneEntity()
        {
            var openhabItems = await _itemsApi.GetItemsAsync(metadata: "dialogflow", tags: Token, recursive: true);
            var zones = openhabItems.Where(ohi => ohi.GroupNames.Count == 1
                                                  && ohi.Type == "Group"
                                                  && ((dynamic)ohi.Metadata?["dialogflow"])?.config.zone != "Internal"
                                                  && ((dynamic)ohi.Metadata?["dialogflow"])?.config.room == null).ToList();

            var dialogflowEntityCsv = string.Empty;
            foreach (var zone in zones)
            {
                var zoneMetadata = (string)((dynamic)zone.Metadata?["dialogflow"])?.config.zone;
                var synonyms = $"{zoneMetadata}\"";
                if (zoneMetadata != zoneMetadata.Humanize(LetterCasing.Title))
                    synonyms += $",\"{zoneMetadata.Humanize(LetterCasing.Title)}\"";
                synonyms += $",\"{zone.Name}";
                var entityRow = $"\"{zoneMetadata}\",\"{synonyms}\"";
                dialogflowEntityCsv += entityRow + Environment.NewLine;
            }

            return new ContentResult
            {
                StatusCode = (int)HttpStatusCode.OK,
                Content = dialogflowEntityCsv,
                ContentType = "text/csv"
            };
        }

        /// <summary>
        /// Get dialogflow entity configuration for zones
        /// </summary>
        /// <remarks></remarks>
        /// <response code="202">Accepted</response>
        /// <response code="500">Internal server error</response>
        [ProducesResponseType(500)]
        [HttpGet]
        [Route("entities/room")]
        public async Task<IActionResult> DialogflowRoomEntity()
        {
            var openhabItems = await _itemsApi.GetItemsAsync(metadata: "dialogflow", tags: Token, recursive: true);
            var rooms = openhabItems.Where(ohi => ohi.GroupNames.Count == 2
                                                  && ohi.Type == "Group"
                                                  && ((dynamic)ohi.Metadata?["dialogflow"])?.config.zone != "Internal"
                                                  && ((dynamic)ohi.Metadata?["dialogflow"])?.config.type == null).ToList();

            var dialogflowEntityCsv = string.Empty;
            foreach (var room in rooms)
            {
                var zoneMetadata = (string)((dynamic)room.Metadata?["dialogflow"])?.config.room;
                var synonyms = $"{zoneMetadata}\"";
                if (zoneMetadata != zoneMetadata.Humanize(LetterCasing.Title))
                    synonyms += $",\"{zoneMetadata.Humanize(LetterCasing.Title)}\"";
                synonyms += $",\"{room.Name}";
                var entityRow = $"\"{zoneMetadata}\",\"{synonyms}\"";
                dialogflowEntityCsv += entityRow + Environment.NewLine;
            }

            return new ContentResult
            {
                StatusCode = (int)HttpStatusCode.OK,
                Content = dialogflowEntityCsv,
                ContentType = "text/csv"
            };
        }

        /// <summary>
        /// Get dialogflow entity configuration for device type
        /// </summary>
        /// <remarks></remarks>
        /// <response code="202">Accepted</response>
        /// <response code="500">Internal server error</response>
        [ProducesResponseType(500)]
        [HttpGet]
        [Route("entities/deviceType")]
        public async Task<IActionResult> DialogflowDeviceTypeEntity()
        {
            var openhabItems = await _itemsApi.GetItemsAsync(metadata: "dialogflow", tags: Token, recursive: true);
            var devices = openhabItems.Where(i => ((dynamic)i.Metadata?["dialogflow"])?.config.zone != null
                                                  && ((dynamic)i.Metadata?["dialogflow"])?.config.zone != "Internal"
                                                  && ((dynamic)i.Metadata?["dialogflow"])?.config.room != null
                                                  && ((dynamic)i.Metadata?["dialogflow"])?.config.type != null)
                .Select(d => (string)((dynamic)d.Metadata?["dialogflow"])?.config.type).Distinct();

            var dialogflowEntityCsv = string.Empty;
            foreach (var deviceType in devices)
            {
                var synonyms = $"{deviceType}\"";
                if (deviceType != deviceType.Humanize(LetterCasing.Title))
                    synonyms += $",\"{deviceType.Humanize(LetterCasing.Title)}";
                var entityRow = $"\"{deviceType}\",\"{synonyms}";
                dialogflowEntityCsv += entityRow + Environment.NewLine;
            }

            return new ContentResult
            {
                StatusCode = (int)HttpStatusCode.OK,
                Content = dialogflowEntityCsv,
                ContentType = "text/csv"
            };
        }

        /// <summary>
        /// Get dialogflow entity configuration for rooms
        /// </summary>
        /// <remarks></remarks>
        /// <response code="202">Accepted</response>
        /// <response code="500">Internal server error</response>
        [ProducesResponseType(500)]
        [HttpGet]
        [Route("entities/device")]
        public async Task<IActionResult> DialogflowDeviceEntity()
        {
            var openhabItems = await _itemsApi.GetItemsAsync(metadata: "dialogflow", tags: Token, recursive: true);
            var devices = openhabItems.Where(i => ((dynamic)i.Metadata?["dialogflow"])?.config.type != null && ((dynamic)i.Metadata?["dialogflow"])?.config.zone != "Internal").ToList();

            var dialogflowEntityCsv = string.Join(Environment.NewLine,
                devices.Select(d => $"\"{d.Name}\"" +
                                    $",\"{d.Name}\"" +
                                    $",\"{((dynamic)d.Metadata?["dialogflow"])?.config.room}{((dynamic)d.Metadata?["dialogflow"])?.config.type}\"" +
                                    $",\"{((string)((dynamic)d.Metadata?["dialogflow"])?.config.room).Humanize(LetterCasing.Title)} " +
                                    $"{((string)((dynamic)d.Metadata?["dialogflow"])?.config.type).Humanize(LetterCasing.Title)}\""));

            return new ContentResult
            {
                StatusCode = (int)HttpStatusCode.OK,
                Content = dialogflowEntityCsv,
                ContentType = "text/csv"
            };
        }
    }
}