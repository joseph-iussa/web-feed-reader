using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace WebFeedReader.Tests.Integration
{
    class TestPersistenceContext : DbContext
    {
        public DbSet<TestEntity> Entities { get; set; }
        public DbSet<TestChildEntity> ChildEntities { get; set; }
    }
}