using PlaningPokerInLaw.Shared;

namespace PlaningPokerInLaw.Server.Domain
{
    public interface IParticipantsDomainService
    {
        int AddParticipant(Participant participant);
        List<Participant> GetAllParticipants();
        Participant? GetParticipant(int participantId);
        Participant? GetParticipantByName(string participantName);
        Participant? GetParticipantByEmail(string participantEmail);
        bool RemoveParticipant(string Name);
        bool RemoveParticipantByEmail(string Email);
    }
}