using System;

namespace ImgToAvi
{
    internal class ValidationException : ApplicationException
    {
        public ValidationException(string message) : base(message)
        {
        }
    }
}