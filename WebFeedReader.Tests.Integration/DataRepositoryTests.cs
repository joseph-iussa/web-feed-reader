using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Medallion;
using NUnit.Framework;
using WebFeedReader.Persistence;

namespace WebFeedReader.Tests.Integration
{
    [TestFixture]
    class DataRepositoryTests
    {
        private TestPersistenceContext context;
        private TestEntityEqualityComparer testEntityEqComp;

        [Test]
        public void GetList_AppliesFiltering()
        {
            TestEntity entityOne = new TestEntity { Name = "Entity One", Field1 = "*" };
            TestEntity entityTwo = new TestEntity { Name = "Entity Two", Field1 = "**" };

            context.Entities.Add(entityOne);
            context.Entities.Add(entityTwo);
            context.SaveChanges();

            var repo = new DataRepositoryUsingTestPersistenceContext<TestEntity>();

            IList<TestEntity> result = repo.GetList(e => e.Field1.Length > 1);

            Assert.That(result.Contains(entityOne, testEntityEqComp) == false,
                $"Entity present that should have been excluded: {entityOne.Name}");
            Assert.That(result.Contains(entityTwo, testEntityEqComp) == true,
                $"Entity excluded that should have been present: {entityTwo.Name}");
        }

        [Test]
        public void GetList_AppliesOrdering()
        {
            // Entities in order for an ordering of Field1 ASC, Field2 DESC, Field3 ASC.
            List<TestEntity> entitiesInOrder = new List<TestEntity>
            {
                new TestEntity { Name = "Entity One", Field1 = "1", Field2 = "d", Field3 = "1" },
                new TestEntity { Name = "Entity Two", Field1 = "1", Field2 = "c", Field3 = "1" },
                new TestEntity { Name = "Entity Three", Field1 = "1", Field2 = "b", Field3 = "1" },
                new TestEntity { Name = "Entity Four", Field1 = "1", Field2 = "a", Field3 = "1" },
                new TestEntity { Name = "Entity Five", Field1 = "2", Field2 = "y", Field3 = "A" },
                new TestEntity { Name = "Entity Six", Field1 = "2", Field2 = "y", Field3 = "B" },
                new TestEntity { Name = "Entity Seven", Field1 = "2", Field2 = "x", Field3 = "A" },
                new TestEntity { Name = "Entity Eight", Field1 = "2", Field2 = "x", Field3 = "B" }
            };

            // Insert entities into database in random order.
            context.Entities.AddRange(entitiesInOrder.Shuffled());
            context.SaveChanges();

            var repo = new DataRepositoryUsingTestPersistenceContext<TestEntity>();

            IList<TestEntity> result = repo.GetList(orderBy: new OrderByDescription<TestEntity>[]
            {
                new OrderByDescription<TestEntity>(e => e.Field1, true),
                new OrderByDescription<TestEntity>(e => e.Field2, false),
                new OrderByDescription<TestEntity>(e => e.Field3, true),
            });

            Assert.That(entitiesInOrder.SequenceEqual(result, testEntityEqComp),
                "Entities returned in incorrect order");
        }

        [Test]
        public void GetList_AppliesNavigationProperties()
        {
            TestEntity testEntity = new TestEntity { Name = "Test Entity" };
            testEntity.ChildEntities.Add(new TestChildEntity { Name = "Test Child Entity One" });
            testEntity.ChildEntities.Add(new TestChildEntity { Name = "Test Child Entity Two" });
            context.Entities.Add(testEntity);
            context.SaveChanges();

            var repo = new DataRepositoryUsingTestPersistenceContext<TestEntity>();

            IList<TestEntity> result = repo.GetList(navigationProperties: new Expression<Func<TestEntity, object>>[]
            {
                e => e.ChildEntities
            });

            Assert.That(result[0].ChildEntities.Contains(testEntity.ChildEntities[0], testEntityEqComp),
                $"Child entity not loaded: {testEntity.ChildEntities[0].Name}");
            Assert.That(result[0].ChildEntities.Contains(testEntity.ChildEntities[1], testEntityEqComp),
                $"Child entity not loaded: {testEntity.ChildEntities[1].Name}");
        }

        [Test]
        public void GetSingle_AppliesFiltering()
        {
            TestEntity testEntityOne = new TestEntity { Name = "Test Entity One" };
            TestEntity testEntityTwo = new TestEntity { Name = "Test Entity Two" };
            TestEntity testEntityThree = new TestEntity { Name = "Test Entity Three" };
            context.Entities.AddRange(new TestEntity[] { testEntityOne, testEntityTwo, testEntityThree });
            context.SaveChanges();

            var repo = new DataRepositoryUsingTestPersistenceContext<TestEntity>();

            TestEntity resultEntity = repo.GetSingle(e => e.Name == "Test Entity Two");

            Assert.That(testEntityEqComp.Equals(resultEntity, testEntityTwo));
        }

        [Test]
        public void GetSingle_FilterReturnsNoEntities_ReturnsNull()
        {
            context.Entities.Add(new TestEntity { Name = "Test Entity One" });
            context.SaveChanges();

            var repo = new DataRepositoryUsingTestPersistenceContext<TestEntity>();

            TestEntity resultEntity = repo.GetSingle(e => e.Name == "No entity");

            Assert.IsNull(resultEntity);
        }

        [Test]
        public void GetSingle_FilterReturnsManyEntities_Throws()
        {
            TestEntity testEntityOne = new TestEntity { Name = "Test Entity One" };
            TestEntity testEntityTwo = new TestEntity { Name = "Test Entity Two" };
            TestEntity testEntityThree = new TestEntity { Name = "Test Entity Three" };
            context.Entities.AddRange(new TestEntity[] { testEntityOne, testEntityTwo, testEntityThree });
            context.SaveChanges();

            var repo = new DataRepositoryUsingTestPersistenceContext<TestEntity>();

            Assert.Throws<DataRepositoryException>(() => repo.GetSingle(e => e.Name.Length > 0));
        }

        [Test]
        public void GetSingle_AppliesNavigationProperties()
        {
            TestEntity testEntity = new TestEntity { Name = "Test Entity" };
            testEntity.ChildEntities.Add(new TestChildEntity { Name = "Test Child Entity One" });
            testEntity.ChildEntities.Add(new TestChildEntity { Name = "Test Child Entity Two" });
            context.Entities.Add(testEntity);
            context.SaveChanges();

            var repo = new DataRepositoryUsingTestPersistenceContext<TestEntity>();

            TestEntity resultEntity = repo.GetSingle(e => true, e => e.ChildEntities);

            Assert.That(resultEntity.ChildEntities.Contains(testEntity.ChildEntities[0], testEntityEqComp),
                $"Child entity not loaded: {testEntity.ChildEntities[0].Name}");
            Assert.That(resultEntity.ChildEntities.Contains(testEntity.ChildEntities[1], testEntityEqComp),
                $"Child entity not loaded: {testEntity.ChildEntities[1].Name}");
        }

        [Test]
        public void Add_AddsEntities()
        {
            TestEntity testEntityOne = new TestEntity { Name = "Test Entity One" };
            TestEntity testEntityTwo = new TestEntity { Name = "Test Entity Two" };
            TestEntity testEntityThree = new TestEntity { Name = "Test Entity Three" };

            var repo = new DataRepositoryUsingTestPersistenceContext<TestEntity>();

            repo.Add(testEntityOne, testEntityTwo, testEntityThree);

            List<TestEntity> entities = context.Entities.ToList();

            Assert.That(entities.Contains(testEntityOne, testEntityEqComp));
            Assert.That(entities.Contains(testEntityTwo, testEntityEqComp));
            Assert.That(entities.Contains(testEntityThree, testEntityEqComp));
        }

        [Test]
        public void Update_UpdatesEntities()
        {
            TestEntity testEntityOne = new TestEntity { Name = "Test Entity One", Field1 = "not updated" };
            TestEntity testEntityTwo = new TestEntity { Name = "Test Entity Two", Field1 = "not updated" };
            context.Entities.Add(testEntityOne);
            context.Entities.Add(testEntityTwo);
            context.SaveChanges();

            testEntityOne.Field1 = "updated";
            testEntityTwo.Field1 = "updated";

            var repo = new DataRepositoryUsingTestPersistenceContext<TestEntity>();
            repo.Update(testEntityOne, testEntityTwo);

            // Get new test entities from new context to check values have been changed in database.
            // A given context will only ever have a single object reference for a given entity.
            using (var tempContext = new TestPersistenceContext())
            {
                testEntityOne = tempContext.Entities.Single(e => e.Name == "Test Entity One");
                testEntityTwo = tempContext.Entities.Single(e => e.Name == "Test Entity Two");
            }

            Assert.AreEqual("updated", testEntityOne.Field1);
            Assert.AreEqual("updated", testEntityTwo.Field1);
        }

        [Test]
        public void Remove_RemovesEntities()
        {
            TestEntity testEntityOne = new TestEntity { Name = "Test Entity One" };
            TestEntity testEntityTwo = new TestEntity { Name = "Test Entity Two" };
            context.Entities.Add(testEntityOne);
            context.Entities.Add(testEntityTwo);
            context.SaveChanges();

            var repo = new DataRepositoryUsingTestPersistenceContext<TestEntity>();
            repo.Remove(testEntityOne, testEntityTwo);

            Assert.That(context.Entities.Count() == 0);
        }

        [OneTimeSetUp]
        public void OneTimeSetUp()

        {
            Database.SetInitializer(new DropCreateDatabaseAlways<TestPersistenceContext>());
            context = new TestPersistenceContext();
            testEntityEqComp = new TestEntityEqualityComparer();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            context.Dispose();
        }

        [TearDown]
        public void TearDown()
        {
            context.Entities.RemoveRange(context.Entities);
            context.ChildEntities.RemoveRange(context.ChildEntities);
            context.SaveChanges();
        }
    }

    class TestEntityEqualityComparer : EqualityComparer<TestEntityBase>
    {
        public override bool Equals(TestEntityBase entityA, TestEntityBase entityB)
        {
            return entityA.ID == entityB.ID;
        }

        public override int GetHashCode(TestEntityBase obj)
        {
            throw new NotImplementedException();
        }
    }
}