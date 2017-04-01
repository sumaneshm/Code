using CSharpInDepth.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpInDepth._12_LinqBeyondCollections
{
    class IntroToRx : Study
    {
        // Rx aka Reactive extension is reverse of conventional linq to objects
        // where linq to objects are always pull model that is subscribers will have to constantly ask whether there is any data present or not,
        // in Rx case, the server will push the data and hence it is called as "reactive" extension.

        public override string StudyName
        {
            get { return "A simple introduction to Reactive eXtention"; }
        }

        protected override void PerformStudy()
        {
            SimpleSelectUsingObservables();
            RxGroupBy();
            FlatteningRxData();
        }

        private void FlatteningRxData()
        {
            var qry = from x in Observable.Range(1, 3)
                      from y in Observable.Range(1, x)
                      select new { x, y };

            qry.Subscribe(Console.WriteLine);
        }

        private void RxGroupBy()
        {
            var qry = from n in Observable.Range(1, 10)
                      group n by n % 3;

            // It is a bit complex to subscribe to the grouped rx case
            // we have to subscribe the full query once and we have to subscribe to individual group
            // It keeps on stream results and hence is very good if we have to group a lot of data as Linq to object buffers all the output for the entire group
            // before sending it to the client
            qry.Subscribe(g => g.Subscribe(x => Console.WriteLine("Value : {0}, Group : {1}", x, g.Key)));

        }

        private void SimpleSelectUsingObservables()
        {
            DrawHeader("First look at observables");

            // Observable.Range -> generates numbers from 1 to 10 and returns IObservable<int>
            // IObservable.Subscribe => Clients have to subscribe themselves to receive updates from the server.

            var numbers = Observable.Range(1, 10);
            numbers.Subscribe(
                    x => Console.WriteLine("Data : {0}", x),
                    y => Console.WriteLine("OnError : {0}", y),
                    () => Console.WriteLine("Completed")
                );
        }
    }
}
