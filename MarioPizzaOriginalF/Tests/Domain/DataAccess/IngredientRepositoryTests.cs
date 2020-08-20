using System.Data;
using MarioPizzaOriginal.Domain;
using MarioPizzaOriginal.Domain.DataAccess;
using MarioPizzaOriginal.Domain.Enums;
using NUnit.Framework;

namespace MarioPizzaOriginal.Tests.Domain.DataAccess
{
    [TestFixture]
    public class IngredientRepositoryTests : DaoTestHelper<IngredientRepository>
    {
        public IngredientRepositoryTests() : base(conn => new IngredientRepository(conn))
        {
        }

        [Test]
        [Ignore("Database is locked :(")]
        public void Get_IngredientExists()
        {
            RunInRollbackTransaction((repository, transaction) =>
                {
                    Assert.That(repository, Is.Not.Null);
                    Assert.That(transaction.Connection.State != ConnectionState.Closed);

                    repository.Add(new Ingredient{IngredientName = "Ogórek", UnitOfMeasureType = UnitOfMeasure.KILOGRAM});
                    transaction.Commit();

                    var newCucumber = repository.Query("select * from Ingredient where IngredientName = 'Ogórek'");
                    Assert.That(newCucumber, Is.Not.Null);
                    Assert.That(newCucumber.Count, Is.EqualTo(1));
                    Assert.That(newCucumber[0].IngredientName, Is.EqualTo("Ogórek"));
                }
            );
        }

        
    }
}
