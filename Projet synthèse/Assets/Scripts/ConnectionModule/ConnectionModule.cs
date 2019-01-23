
namespace Assets.Scripts.ConnectionModule
{
    public abstract class ConnectionModule
    {
        public static string ARENA_DIRECTORY = "/JSON/arenas/";

        public enum HttpResquest { GET, POST, PUT };
        public abstract string Connect(string _userName, string _password);
    }
}



