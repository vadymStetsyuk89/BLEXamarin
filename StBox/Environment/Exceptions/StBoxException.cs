using System;

namespace StBox.Environment.Exceptions
{
    /// <summary>
    /// Describes exceptions that are related to the `StBox`
    /// 
    /// new StBoxException($"{this.GetType().FullName} {nameof(NavigationService.CreatePage)}",exc)
    /// </summary>
    public class StBoxException : Exception
    {
        public StBoxException()
            : base() { }

        public StBoxException(string message)
            : base(message) { }

        public StBoxException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
