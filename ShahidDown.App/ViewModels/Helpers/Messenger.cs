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

        private readonly Dictionary<string, List<Action<object>>> callbacks = [];

        public void Register(string messageName, Action<object> callback)
        {
            if (!callbacks.TryAdd(messageName, [callback]))
            {
                callbacks[messageName].Add(callback);
            } 
        }

        public void Send(string messageName, object payload)
        {
            if (callbacks.TryGetValue(messageName, out List<Action<object>>? callbackList))
            {
                foreach (Action<object> callback in callbackList)
                {
                    callback(payload);
                }
            }
        }
    }
}
