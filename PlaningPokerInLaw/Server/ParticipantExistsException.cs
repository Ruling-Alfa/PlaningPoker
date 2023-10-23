using PlaningPokerInLaw.Shared;

namespace PlaningPokerInLaw.Server
{
    public class ParticipantExistsException : Exception
    {
        public ParticipantExistsException(Participant participant) :
            base($"Participant ({participant.Name} - {participant.Email}) already Exists")
        {

        }
    }
}
