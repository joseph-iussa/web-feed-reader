using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFeedReader.Persistence
{
    public class DataRepositoryException : Exception
    {
        public DataRepositoryException(string msg, Exception ex) : base(msg, ex)
        {
        }
    }
}