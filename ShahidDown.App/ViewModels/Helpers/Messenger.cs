namespace ShahidDown.App.ViewModels.Helpers
{
    /// <summary>
    /// Represents a messenger, which allows communication between different view-models of the application.
    /// </summary>
    public class Messenger
    {
        private static Messenger? instance;
        private Messenger() { }
        public static Messenger Instance => instance ??= new Messenger();

        private readonly Dictionary<string, Action<object>> callbacks = [];

        public void Register(string messageName, Action<object> callback)
        {
            callbacks[messageName] = callback;
        }

        public void Send(string messageName, object payload)
        {
            if (callbacks.ContainsKey(messageName))
            {
                callbacks[messageName](payload);
            }
        }
    }
}
