using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace FxShow
{
    internal class CustomExceptions
    {
        public class BaseCustomException : ApplicationException
        {
            public string CustomMessage
            {
                get;
                set;
            }
        }

        internal class CustomExceptionOne : BaseCustomException
        {
            public CustomExceptionOne()
            {
                CustomMessage = "Custom Exception One";
            }

            public CustomExceptionOne(string message)
            {
                CustomMessage = message;
            }
        }

        internal class CustomExceptionTwo : BaseCustomException
        {
            public CustomExceptionTwo()
            {
                CustomMessage = "Custom Exception Two";
            }

            public CustomExceptionTwo(string message)
            {
                CustomMessage = message;
            }
        }
    }
}
