namespace GoIoFish.Models
{
    public class LoginProgressMsg
    {
        public LoginState State { get; set; }
        public string Body { get; set; }

        public LoginProgressMsg(LoginState state, string body)
        {
            State = state;
            Body = body;
        }
    }

    public enum LoginState
    {
        LoadingPage,
        QrCodeReady,
        LoginSucceeded,
    }
}