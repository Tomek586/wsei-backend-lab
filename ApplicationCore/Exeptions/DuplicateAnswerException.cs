﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Exeptions
{
    public class DuplicateAnswerException : Exception
    {
        public DuplicateAnswerException(string message) : base(message)
        {
        }
    }
}