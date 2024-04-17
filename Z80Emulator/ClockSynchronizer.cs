using System.Diagnostics;

//JJJusing System.Threading;

namespace IngameScript
{
    partial class Program
    {
        /// <summary>
        /// Default implementation of <see cref="IClockSynchronizer"/>.
        /// </summary>
        public class ClockSynchronizer : IClockSynchronizer
        {
            private const int MinMicrosecondsToWait = 10 * 1000;

            public decimal EffectiveClockFrequencyInMHz { get; set; }

            //JJJ private Stopwatch stopWatch = new Stopwatch();

            //JJJ private decimal accummulatedMicroseconds;

            public void Start()
            {
                //JJJstopWatch.Reset();
                //JJJstopWatch.Start();
            }

            public void Stop()
            {
                //JJJstopWatch.Stop();
            }

            public void TryWait(int periodLengthInCycles)
            {
                /* JJJ
                            accummulatedMicroseconds += (periodLengthInCycles / EffectiveClockFrequencyInMHz);

                            var microsecondsPending = (accummulatedMicroseconds - stopWatch.ElapsedMilliseconds);

                            if(microsecondsPending >= MinMicrosecondsToWait)
                            {
                                Thread.Sleep((int)(microsecondsPending / 1000));
                                accummulatedMicroseconds = 0;
                                stopWatch.Reset();
                            }
                */
            }
        }
    }
}