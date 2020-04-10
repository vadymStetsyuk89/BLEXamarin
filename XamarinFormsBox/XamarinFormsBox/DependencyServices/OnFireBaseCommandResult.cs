namespace XamarinFormsBox.DependencyServices
{
    public class OnFireBaseCommandResult<TPayload> : OnFireBaseCommandResult
    {
        public OnFireBaseCommandResult(TPayload payload, bool isSuccessful, string message)
            : base(isSuccessful, message)
        {
            Payload = payload;
        }

        public TPayload Payload { get; private set; }
    }

    public class OnFireBaseCommandResult
    {
        public OnFireBaseCommandResult(bool isSuccessful, string message)
        {
            IsSuccessful = isSuccessful;
            Message = message;
        }

        public bool IsSuccessful { get; private set; }

        public string Message { get; private set; }
    }
}
