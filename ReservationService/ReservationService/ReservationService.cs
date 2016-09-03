using System;
using System.Linq;
using CinemaModelLibrary;

namespace ReservationService
{
    public class ReservationService
    {
        private TransactionService transactionService;

        public ReservationService(TransactionService transactionService)
        {
            this.transactionService = transactionService;
        }

        public void Consume(ReservationDto reservationDto)
        {
            Reservation res;
            var success = false;
            using (var db = new Model1Container())
            {
                var reservation =
                    db.ReservationSet.Where(r => r.SeanceId == reservationDto.SeanceId)
                        .SelectMany(r => r.Spots)
                        .Any(s => reservationDto.Spots.Contains(s.Id));

                if (!reservation)
                {
                    var spots = db.SpotSet.Where(spot => reservationDto.Spots.Contains(spot.Id)).ToList();

                    res = new Reservation()
                    {
                        SeanceId = reservationDto.SeanceId,
                        Spots = spots,
                        UserEmail = reservationDto.Email
                    };
                    db.ReservationSet.Add(res);
                    success = true;
                    Console.WriteLine("Reservation succeeded.");
                }
                else
                {
                    Console.WriteLine("At least one spot is already engaged.");
                }
                
                db.SaveChanges();
            }

            if (success)
            {
                transactionService.Send(reservationDto);
            }
        }
    }
}