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
            callbacks.Add(messageName, callback);
        }

        public void Send(string messageName, object payload)
        {
            if (callbacks.TryGetValue(messageName, out Action<object>? callback))
            {
                callback(payload);
            }
        }
    }
}
