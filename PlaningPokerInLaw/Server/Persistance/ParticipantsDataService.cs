using PlaningPokerInLaw.Shared;

namespace PlaningPokerInLaw.Server.Persistance
{
    public class ParticipantsDataService : IParticipantsDataService
    {
        private readonly List<Participant> _participants;
        private static int _lastId = -1;
        private object _participantLock { get; set; }
        public ParticipantsDataService()
        {
            _participants = new List<Participant>()
            {
                new Participant()
                {
                    Id = _lastId++,
                    Name = "Neel",
                    Email = "neel@email.com",
                    isModerator = true
                },
                new Participant()
                {
                    Id = _lastId++,
                    Name = "Jay",
                    Email = "jay@email.com"
                }
            };
            _participantLock = new object();
        }

        public List<Participant> GetAllParticipants()
        {
            return _participants;
        }
        public Participant? GetParticipant(int participantId)
        {
            return _participants.FirstOrDefault(p => p.Id == participantId);
        }
        public Participant? GetParticipantByName(string participantName)
        {
            return _participants.FirstOrDefault(p => p.Name == participantName);
        }

        public Participant? GetParticipantByEmail(string participantEmail)
        {
            return _participants.FirstOrDefault(p => p.Email == participantEmail);
        }

        public int AddParticipant(Participant participant)
        {
            if (_participants.Any(p => p.Name == participant.Name || p.Email == participant.Email))
            {
                throw new ParticipantExistsException(participant);
            }
            lock (_participantLock)
            {
                participant.Id = _lastId++;
                _participants.Add(participant);
            }
            return participant.Id;
        }

        public bool RemoveParticipant(Participant participant)
        {
            var isSuccess = false;
            var existingParticipant = _participants.FirstOrDefault(p =>
                p.Name == participant.Name && p.Email == participant.Email);
            if (existingParticipant is not null)
            {
                _participants.Remove(existingParticipant);
                isSuccess = true;
            }
            return isSuccess;
        }
    }
}
