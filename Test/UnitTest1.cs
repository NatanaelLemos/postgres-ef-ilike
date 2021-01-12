using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Linq.Expressions;
using System.Web;
using FluentAssertions;
using System.ComponentModel.DataAnnotations;

namespace Test
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var options = new DbContextOptionsBuilder<TestDb>()
                .UseInMemoryDatabase($"cars{Guid.NewGuid()}")
                .Options;

            var db = new TestDb(options);

            db.Cars.Add(new Car { Brand = "ford" });
            db.SaveChanges();

            var result = db.Cars.Where(c =>
                EF.Functions.ILike(c.Brand, "ford"))
                .ToList();

            result.Should().HaveCount(1);
        }
    }

    public class TestDb : DbContext
    {
        public TestDb(DbContextOptions<TestDb> options)
            : base(options)
        {
        }

        public DbSet<Car> Cars { get; set; }
    }

    public class Car
    {
        [Key]
        public int Id { get; set; }
        public string Brand { get; set; }
    }
}
