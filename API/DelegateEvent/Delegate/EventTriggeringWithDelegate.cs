namespace API.DelegateEvent.Delegate
{
    public class EventTriggeringWithDelegate
    {
        public static void Main()
        {
            Program2.Main();
        }
    }

    public delegate void EventHandler(string message);

    public class EventPublisher
    {
        public event EventHandler EventOccurred;

        public void DoSomething()
        {
            Console.WriteLine("Doing something important...");
            OnEventOccurred("Event Triggered!");
        }

        protected virtual void OnEventOccurred(string message)
        {
            EventOccurred?.Invoke(message);
        }
    }

    public class EventSubscriber
    {
        public void HandleEvent(string message)
        {
            Console.WriteLine("Received message: " + message);
        }
    }

    public class Program2
    {
        public static void Main()
        {
            EventPublisher publisher = new EventPublisher();
            EventSubscriber subscriber = new EventSubscriber();

            publisher.EventOccurred += subscriber.HandleEvent;
            publisher.DoSomething();
        }
    }

}