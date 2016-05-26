using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace WebFeedReader.Tests.Integration
{
    abstract class TestEntityBase
    {
        public long ID { get; set; }
    }

    class TestEntity : TestEntityBase
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(100)]
        public string Field1 { get; set; }

        [StringLength(100)]
        public string Field2 { get; set; }

        public int Field3 { get; set; }

        public List<TestChildEntity> ChildEntities { get; set; }

        public TestEntity()
        {
            ChildEntities = new List<TestChildEntity>();
        }
    }

    class TestChildEntity : TestEntityBase
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(100)]
        public string Field1 { get; set; }

        [StringLength(100)]
        public string Field2 { get; set; }

        [StringLength(100)]
        public string Field3 { get; set; }
    }
}