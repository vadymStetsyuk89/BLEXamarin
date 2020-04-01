using System;

namespace StBox.Environment.Helpers.JWTUtils
{
    public class SignatureVerificationException : Exception
    {
        public SignatureVerificationException(string message)
            : base(message) { }
    }
}
