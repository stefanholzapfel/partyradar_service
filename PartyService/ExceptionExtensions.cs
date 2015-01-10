using System;
using System.Collections.Generic;

namespace PartyService
{
    public static class ExceptionExtensions
    {
        public static string JoinMessages( this Exception exception )
        {
            var messages = new List<string>();
            Exception exc = exception;
            do
            {
                messages.Add( exc.Message );
                exc = exc.InnerException;

            } while( exc != null );

            return string.Join( Environment.NewLine, messages );
        }
    }
}