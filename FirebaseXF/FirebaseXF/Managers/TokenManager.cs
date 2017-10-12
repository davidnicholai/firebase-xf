namespace FirebaseXF
{
    public class TokenManager
    {
        private static TokenManager _instance;

        public static TokenManager Instance
        {
            get { return _instance ?? (_instance = new TokenManager()); }
        }

        public string Token { get; set; } = string.Empty;
    }
}
