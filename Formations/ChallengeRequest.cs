using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Formations
{
    [Serializable]
    public class ChallengeRequest
    {
        private Person _sender;
        private Person _reciever;
        public Person Sender
        {
            get { return _sender; }
            private set { _sender = value; }
        }
        public Person Reciever
        {
            get { return _reciever; }
            private set { _reciever = value; }
        }
        public ChallengeRequest(Person sender, Person reciever)
        {
            this.Sender = sender;
            this.Reciever = reciever;
        }
    }
}
