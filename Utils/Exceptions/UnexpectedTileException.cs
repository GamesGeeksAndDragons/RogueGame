﻿using System;

namespace Utils.Exceptions
{
    public class UnexpectedTileException : Exception
    {
        public UnexpectedTileException(string message) : base(message)
        {
            
        }
    }
}
