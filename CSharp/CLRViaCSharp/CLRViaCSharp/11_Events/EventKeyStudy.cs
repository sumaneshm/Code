using CLRViaCSharp.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLRViaCSharp._11_Events
{

    //If all the classes start to declare the events and handlers directly, it will require lot of memory to be allocated for that.
    //most of the events wont be used by the developers and in order to optimize the memory, it will store the event handlers in a Dictionary


    public sealed class EventKey { }

    //.net FCL has a class called EventHandlerStore which is used for this purpose, the only difference between EventHandlersStore and EventSet is that the latter is thread-safe.

    public sealed class EventSet
    {
        private Dictionary<EventKey, Delegate> m_events = new Dictionary<EventKey, Delegate>();
        private object locker = new object();

        public void Add(EventKey eventKey, Delegate handler)
        {
            lock (locker)
            {
                Delegate d;
                m_events.TryGetValue(eventKey, out d);
                m_events[eventKey] = Delegate.Combine(d, handler);
            }
        }

        public void Remove(EventKey eventKey, Delegate handler)
        {
            lock (locker)
            {
                Delegate d;
                if (m_events.TryGetValue(eventKey, out d))
                {
                    Delegate removed = Delegate.Remove(d, handler);
                    if (removed != null)
                    {
                        m_events[eventKey] = removed;
                    }
                    else
                    {
                        m_events.Remove(eventKey);
                    }
                }

            }
        }

        public void Raise(EventKey eventKey, object sender, EventArgs eventArgs)
        {
            Delegate d;
            lock (locker)
            {
                if (!m_events.TryGetValue(eventKey, out d))
                {
                    return;
                }
            }

            d.DynamicInvoke(new object[] { sender, eventArgs });
        }
    }

    class FooEventArgs : EventArgs { }

    class EventHoster
    {
        private EventSet m_eventset = new EventSet();

        private EventKey m_fooEventKey = new EventKey();

        public event EventHandler<FooEventArgs> Foo
        {
            add { m_eventset.Add(m_fooEventKey, value); }
            remove { m_eventset.Remove(m_fooEventKey, value); }
        }

        private void OnFoo(FooEventArgs args)
        {
            m_eventset.Raise(m_fooEventKey, this, args);
        }


        public void SimulateFoo()
        {
            FooEventArgs fargs = new FooEventArgs();
            OnFoo(fargs);
        }
    }

    class EventKeyStudy : Study
    {
        public override string StudyName
        {
            get { return "Learn how complex classes with multiple events are implemented"; }
        }

        protected override void PerformStudy()
        {
            EventHoster hoster = new EventHoster();
            hoster.Foo += hoster_Foo;
            hoster.SimulateFoo();
        }
        
        static void hoster_Foo(object sender, FooEventArgs e)
        {
            Console.WriteLine("Foo event was raised");
        }
    }
}
