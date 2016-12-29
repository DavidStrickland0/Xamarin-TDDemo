﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamarinTDDemo.Babysitter
{
    public static class Calculator
    {

        public static decimal Calulate(IEnumerable<HourlyRate> rates, IEnumerable<Timing> timings)
        {
            decimal charge = 0;

            var totalChargeableTime = timings.Aggregate(TimeSpan.Zero, (total, timing) => total + timing.Time);

            var remainingTimings = timings;
            var hoursCharged = new TimeSpan();
            //Assumes After Midnight Time is more valuable then Prebedtime which is more valuable then post bedtime.
            while (hoursCharged < totalChargeableTime)
            {
                charge = charge + chargeHighestValueHour(rates,remainingTimings);
                hoursCharged= hoursCharged.Add(new TimeSpan(1,0,0));
            }
            return charge;
        }

        public static decimal Calulate(IEnumerable<HourlyRate> rates, Times times)
        {
            throw new NotImplementedException();
        }

        private static decimal chargeHighestValueHour(IEnumerable<HourlyRate> rates, IEnumerable<Timing> remainingTimings)
        {
            //Concat the two Collections so we can work with Time and rate together
            var appendedRates = remainingTimings
                .Join(rates, t => t.Catagory, r => r.Catagory,
                (timing, rate) => new { timing.Catagory, timing.Time, rate.Rate });

            //Finds which of the remaining Hours have the highest value
            var hourToCharge = appendedRates.Where(aR => aR.Time > TimeSpan.Zero).OrderBy(ar => ar.Rate).Last();

            //Updated the remainingTimings collection removing the hour we are charging
            var timingToChange = remainingTimings.Where(rT => rT.Catagory == hourToCharge.Catagory).First();
            timingToChange.Time = timingToChange.Time.Subtract(new TimeSpan(1, 0, 0));
            return hourToCharge.Rate;
        }


        private static TimeSpan ToFullHours(this TimeSpan timespan)
        {
            if (timespan.Milliseconds > 0)
                timespan = timespan.Subtract(new TimeSpan(0, 0, 0, timespan.Milliseconds));
                timespan = timespan.Add(new TimeSpan(0, 0, 1));
            if (timespan.Seconds > 0)
                timespan = timespan.Subtract(new TimeSpan(0, 0, timespan.Seconds, 0));
                timespan = timespan.Add(new TimeSpan(0, 1, 0));
            if (timespan.Minutes > 0)
                timespan = timespan.Subtract(new TimeSpan(0, timespan.Minutes, 0, 0));
                timespan = timespan.Add(new TimeSpan(1, 0, 0));
            return timespan;
        }

    }
}
