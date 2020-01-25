using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Openhab.Client.Api;
using Openhab.Proxy.Api.Configuration;
using Openhab.Proxy.Api.Models;

namespace Openhab.Proxy.Api.Controllers
{
    [ApiController]
    [AuthorizeWithToken]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase, ITokenController
    {
        private readonly IItemsApi _itemsApi;
        public Guid Uuid { get; set; }
        public string Token { get; set; }
        public string Group { get; set; }

        public HomeController(IItemsApi itemsApi)
        {
            _itemsApi = itemsApi;
        }

        /// <summary>
        /// Get home configuration
        /// </summary>
        /// <remarks></remarks>
        /// <response code="202">Accepted</response>
        /// <response code="500">Internal server error</response>
        [ProducesResponseType(typeof(Home), 200)]
        [ProducesResponseType(500)]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var openhabItems = await _itemsApi.GetItemsAsync(metadata: "dialogflow", tags: Token, recursive: true);
            var zones = openhabItems.Where(ohi => ohi.GroupNames.Count == 1
                                                  && ohi.Type == "Group"
                                                  && ((dynamic)ohi.Metadata?["dialogflow"])?.config.zone != "Internal"
                                                  && ((dynamic)ohi.Metadata?["dialogflow"])?.config.room == null).ToList();
            var rooms = openhabItems.Where(ohi => ohi.GroupNames.Count == 2
                                                  && ohi.Type == "Group"
                                                  && ((dynamic)ohi.Metadata?["dialogflow"])?.config.zone != "Internal"
                                                  && ((dynamic)ohi.Metadata?["dialogflow"])?.config.type == null).ToList();
            var devices = openhabItems.Where(i => ((dynamic)i.Metadata?["dialogflow"])?.config.zone != null
                                                  && ((dynamic)i.Metadata?["dialogflow"])?.config.zone != "Internal").ToList();

            var configuration = new Home
            {
                Id = Group,
                Uuid = Uuid,
                Token = Token,
                Zones = zones.Select(z => new Zone
                {
                    Id = z.Name,
                    Name = ((dynamic)z.Metadata?["dialogflow"])?.config.zone,
                    Description = z.Label,
                    Rooms = rooms.Where(r => r.GroupNames.Contains(z.Name)).Select(r => new Room
                    {
                        Id = r.Name,
                        Name = ((dynamic)r.Metadata?["dialogflow"])?.config.room,
                        Description = r.Label,
                        Devices = devices.Where(d => d.GroupNames.Contains(r.Name)).Select(d => new Device
                        {
                            Id = d.Name,
                            Description = d.Label,
                            Room = ((dynamic)d.Metadata?["dialogflow"])?.config.room,
                            Zone = ((dynamic)d.Metadata?["dialogflow"])?.config.zone,
                            Type = ((dynamic)d.Metadata?["dialogflow"])?.config.type,
                            OpenhabType = d.Type
                        })
                    })
                })
            };

            return Ok(configuration);
        }

        /// <summary>
        /// Get all zones
        /// </summary>
        /// <remarks></remarks>
        /// <response code="202">Accepted</response>
        /// <response code="500">Internal server error</response>
        [ProducesResponseType(typeof(Room), 200)]
        [ProducesResponseType(500)]
        [HttpGet]
        [Route("zones")]
        public async Task<IActionResult> GetZones()
        {
            var openhabItems = await _itemsApi.GetItemsAsync(metadata: "dialogflow", tags: Token, recursive: true);
            var zones = openhabItems.Where(ohi => ohi.GroupNames.Count == 1
                                                  && ohi.Type == "Group"
                                                  && ((dynamic)ohi.Metadata?["dialogflow"])?.config.zone != "Internal"
                                                  && ((dynamic)ohi.Metadata?["dialogflow"])?.config.room == null).ToList();

            var rooms = zones.Select(z => new Zone
            {
                Id = z.Name,
                Name = ((dynamic)z.Metadata?["dialogflow"])?.config.zone,
                Description = z.Label
            });

            return Ok(rooms);
        }

        /// <summary>
        /// Get all rooms
        /// </summary>
        /// <remarks></remarks>
        /// <response code="202">Accepted</response>
        /// <response code="500">Internal server error</response>
        [ProducesResponseType(typeof(Room), 200)]
        [ProducesResponseType(500)]
        [HttpGet]
        [Route("rooms")]
        public async Task<IActionResult> GetRooms()
        {
            var openhabItems = await _itemsApi.GetItemsAsync(metadata: "dialogflow", tags: Token, recursive: true);
            var rooms = openhabItems.Where(ohi => ohi.GroupNames.Count == 2
                                                  && ohi.Type == "Group"
                                                  && ((dynamic)ohi.Metadata?["dialogflow"])?.config.zone != "Internal"
                                                  && ((dynamic)ohi.Metadata?["dialogflow"])?.config.type == null)
                .Select(r => new Room
                {
                    Id = r.Name,
                    Name = ((dynamic)r.Metadata?["dialogflow"])?.config.room,
                    Description = r.Label
                });

            return Ok(rooms);
        }

        /// <summary>
        /// Get all devices
        /// </summary>
        /// <remarks></remarks>
        /// <response code="202">Accepted</response>
        /// <response code="500">Internal server error</response>
        [ProducesResponseType(typeof(Device), 200)]
        [ProducesResponseType(500)]
        [HttpGet]
        [Route("devices")]
        public async Task<IActionResult> GetDevices()
        {
            var openhabItems = await _itemsApi.GetItemsAsync(metadata: "dialogflow", tags: Token, recursive: true);
            var devices = openhabItems.Where(i => ((dynamic)i.Metadata?["dialogflow"])?.config.zone != null
                                                  && ((dynamic)i.Metadata?["dialogflow"])?.config.zone != "Internal")
                .Select(d => new Device
                {
                    Id = d.Name,
                    Description = d.Label,
                    Room = ((dynamic)d.Metadata?["dialogflow"])?.config.room,
                    Zone = ((dynamic)d.Metadata?["dialogflow"])?.config.zone,
                    Type = ((dynamic)d.Metadata?["dialogflow"])?.config.type,
                    OpenhabType = d.Type
                });

            return Ok(devices);
        }
    }
}