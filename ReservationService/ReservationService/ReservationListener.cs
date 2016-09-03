using System;
using Apache.NMS;
using CinemaModelLibrary;

namespace ReservationService
{
    public class ReservationListener : ActiveMqEndpoint
    {
        private ReservationService reservationService;

        public ReservationListener(IConnection connection, ReservationService reservationService) : base(connection)
        {
            this.reservationService = reservationService;
        }

        public ReservationListener(IConnection connection, string queue, ReservationService reservationService) : this(connection, reservationService)
        {
            Start(queue);
        }

        public void Start(string queue)
        {
            base.Start();
            var dest = session.GetDestination(queue);
            var consumer = session.CreateConsumer(dest);
            consumer.Listener += ConsumerOnListener;
        }

        private void ConsumerOnListener(IMessage message)
        {
            if (message is IObjectMessage)
            {
                var body = ((IObjectMessage) message).Body;

                if (body is ReservationDto)
                {
                    reservationService.Consume(body as ReservationDto);
                    message.Acknowledge();
                    Console.WriteLine("Message acknowledged!");
                }
                else
                {
                    Console.WriteLine("Sent object is invalid!");
                }
            }
            else
            {
                Console.WriteLine("Sent message is invalid!");
            }
        }
    }
}