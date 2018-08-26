using System;

namespace Utils.Exceptions
{
    public class DirectionNotFoundException : Exception
    {
        public DirectionNotFoundException(string message) : base(message)
        {
            
        }
    }
}
