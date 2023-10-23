using Microsoft.AspNetCore.Razor.TagHelpers;
using PlaningPokerInLaw.Server.Persistance;
using PlaningPokerInLaw.Shared;
using System.Xml.Linq;

namespace PlaningPokerInLaw.Server.Domain
{
    public class ParticipantsDomainService : IParticipantsDomainService
    {
        private readonly IParticipantsDataService _participantsDataService;
        public ParticipantsDomainService(IParticipantsDataService participantsDataService)
        {
            _participantsDataService = participantsDataService;
        }

        public List<Participant> GetAllParticipants()
        {
            return _participantsDataService.GetAllParticipants();
        }

        public Participant? GetParticipant(int participantId)
        {
            return _participantsDataService.GetParticipant(participantId);
        }
        public Participant? GetParticipantByName(string participantName)
        {
            return _participantsDataService.GetParticipantByName(participantName.Trim());
        }

        public Participant? GetParticipantByEmail(string participantEmail)
        {
            return _participantsDataService.GetParticipantByEmail(participantEmail.Trim());
        }

        public int AddParticipant(Participant participant)
        {
            participant.Name = participant.Name.Trim();
            participant.Email = participant.Email.Trim();
            return _participantsDataService.AddParticipant(participant);
        }

        public bool RemoveParticipant(string Name)
        {
            var participantName = Name?.Trim();
            var participant = _participantsDataService.GetParticipantByName(participantName);
            return _participantsDataService.RemoveParticipant(participant);

        }

        public bool RemoveParticipantByEmail(string Email)
        {
            var participantEmail = Email?.Trim();
            var participant = _participantsDataService.GetParticipantByEmail(participantEmail);
            return _participantsDataService.RemoveParticipant(participant);
        }
    }
}
