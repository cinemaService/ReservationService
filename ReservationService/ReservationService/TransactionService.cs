using Apache.NMS;
using CinemaModelLibrary;

namespace ReservationService
{
    public class TransactionService : ActiveMqEndpoint
    {
        private IMessageProducer producer;

        public TransactionService(IConnection connection) : base(connection)
        {

        }

        public TransactionService(IConnection connection, string queue) : this(connection)
        {
            Start(queue);
        }

        public void Start(string queue)
        {
            base.Start();
            producer = session.CreateProducer(session.GetDestination(queue));
        }

        public void Send(ReservationDto messageBody)
        {
            var message = session.CreateObjectMessage(messageBody);
            producer.Send(message);
        }
    }
}