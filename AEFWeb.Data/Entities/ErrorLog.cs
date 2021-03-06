﻿using AEFWeb.Data.Entities.Core;
using System;

namespace AEFWeb.Data.Entities
{
    public class ErrorLog : Entity
    {
        public ErrorLog(Guid id, 
            string message, 
            string exceptionString) : base(id)
        {
            Message = message;
            ExceptionString = exceptionString;
            Date = DateTime.Now;
        }

        public ErrorLog() : base(Guid.NewGuid()) { }

        public string Message { get; private set; }
        public string ExceptionString { get; private set; }
        public DateTime Date { get; private set; }
    }
}
