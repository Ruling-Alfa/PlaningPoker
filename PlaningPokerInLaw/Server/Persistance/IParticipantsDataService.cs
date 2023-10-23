using PlaningPokerInLaw.Shared;

namespace PlaningPokerInLaw.Server.Persistance
{
    public interface IParticipantsDataService
    {
        int AddParticipant(Participant participant);
        List<Participant> GetAllParticipants();
        Participant? GetParticipant(int participantId);
        Participant? GetParticipantByName(string participantName);
        Participant? GetParticipantByEmail(string participantEmail);
        bool RemoveParticipant(Participant participant);
    }
}