using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apache.NMS;
using Apache.NMS.Util;
using CinemaModelLibrary;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            var uri = new Uri("activemq:tcp://localhost:61616");
            var connectionFactory = new NMSConnectionFactory(uri);

            using (var connection = connectionFactory.CreateConnection())
            using (var session = connection.CreateSession())
            {
                var dest = SessionUtil.GetDestination(session, "queue://test");

                using (var producer = session.CreateProducer(dest))
                {
                    connection.Start();
                    producer.DeliveryMode = MsgDeliveryMode.Persistent;
                    int i = 0;
                    while (Console.ReadKey().KeyChar != 'q')
                    {
                        int[] a = {1, 2, 5};
                        var msg = session.CreateObjectMessage(new ReservationDto()
                        {
                            SeanceId = 1,
                            Spots = a,
                            Email = "mautokar@gmail.com"
                        });
                        producer.Send(msg);
                        Console.WriteLine("Send");
                    }
                }
            }
        }
    }
}
