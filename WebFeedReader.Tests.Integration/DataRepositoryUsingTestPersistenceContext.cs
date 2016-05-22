using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebFeedReader.Persistence;

namespace WebFeedReader.Tests.Integration
{
    class DataRepositoryUsingTestPersistenceContext<T> : DataRepository<T> where T : class
    {
        protected override DbContext GetDbContext()
        {
            return new TestPersistenceContext();
        }
    }
}