namespace XamarinFormsBox.AppEnvironment.Arguments.Navigation
{
    public class NavigatedInfoMessageArgs : NavigationBaseArgs
    {
        public NavigatedInfoMessageArgs(string message)
        {
            Message = message;
        }

        public string Message { get; private set; }
    }
}
