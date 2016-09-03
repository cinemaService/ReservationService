using System;
using Apache.NMS;
using Autofac;

namespace ReservationService
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();

            builder.Register(c => new NMSConnectionFactory(new Uri("activemq:tcp://localhost:61616")).CreateConnection()).InstancePerLifetimeScope();
            builder.RegisterType<ReservationService>().InstancePerLifetimeScope();
            builder.RegisterType<ReservationListener>().WithParameter("queue", "queue://reservation").InstancePerLifetimeScope();
            builder.RegisterType<TransactionService>().WithParameter("queue", "queue://transaction").InstancePerLifetimeScope();

            using (var scope = builder.Build())
            {
                var listener = scope.Resolve<ReservationListener>();
                Console.ReadKey();
            }

            Console.WriteLine("End of program");
        }
    }
}
