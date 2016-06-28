using System;

namespace ConsoleApplication2
{
    public class Worker
    {
        private volatile bool mShouldStop = false;

        public void DoWork()
        {
            while (!mShouldStop)
            {
                Console.WriteLine("Doing somework in the aided thread... ... ");
            }
            Console.WriteLine("Quit the aided thread gracefully... ..."); 
        }

        public void StopWork()
        {
            this.mShouldStop = true;
        }
    }
}