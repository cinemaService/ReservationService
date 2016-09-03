using System;
using Apache.NMS;

namespace ReservationService
{
    public abstract class ActiveMqEndpoint : IDisposable
    {
        public IConnection connection { get; set; }

        protected ISession session { get; set; }

        protected IDestination destination { get; set; }

        protected ActiveMqEndpoint(IConnection connection)
        {
            this.connection = connection;
            connection.Start();
        }

        public void Start()
        {
            session = connection.CreateSession(AcknowledgementMode.IndividualAcknowledge);
        }

        public void Close()
        {
            session.Close();
        }

        public void Dispose()
        {
            Close();
        }
    }
}