using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PlaningPokerInLaw.Server.Domain;
using PlaningPokerInLaw.Server.Hubs;
using PlaningPokerInLaw.Shared;

namespace PlaningPokerInLaw.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ParticipantsController : ControllerBase
    {
        private readonly IParticipantsDomainService _participantsDomainService;
        private readonly IHubContext<PokerHub> _pokerHubContext;
        private readonly ConnectionService _hubConnectionService;
        public ParticipantsController(IParticipantsDomainService participantsDomainService, 
            IHubContext<PokerHub> pokerHubContext,
            ConnectionService hubConnectionService)
        {
            _participantsDomainService = participantsDomainService;
            _pokerHubContext = pokerHubContext;
            _hubConnectionService = hubConnectionService;

        }
        // GET: ParticipantsController
        [HttpGet]
        public IEnumerable<Participant> Get()
        {
            return (_participantsDomainService.GetAllParticipants());
        }

        // GET: ParticipantsController/Details/5
        [HttpGet]
        [Route("{Email}")]
        public Participant? Details([FromRoute]string Email)
        {
            return (_participantsDomainService.GetParticipantByEmail(Email));
        }

        // GET: ParticipantsController/Create
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Participant participant)
        {
            try
            {
                var pId = _participantsDomainService.AddParticipant(participant);
                var newParticipant = _participantsDomainService.GetParticipant(pId);

                await _pokerHubContext.Clients.All.SendAsync("LogIn", participant.Email);

                return Ok(newParticipant);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // POST: ParticipantsController/Delete/5
        [HttpDelete]
        [Route("{Email}")]
        public async Task<IActionResult> Delete([FromRoute] string Email)
        {
            var isSuccess = _participantsDomainService.RemoveParticipantByEmail(Email);
            if (isSuccess)
            {
                await _pokerHubContext.Clients.All.SendAsync("LogOut", Email);
                return Ok();
            }
            return BadRequest($"{Email} not found");
        }
    }
}
