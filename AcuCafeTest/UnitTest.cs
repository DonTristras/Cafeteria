using Microsoft.VisualStudio.TestTools.UnitTesting;
using AcuCafe;

namespace AcuCafeTest
{
    [TestClass]
    public class UnitTest
    {

        [TestMethod]
        public void IceTeaHasMilkFails()
        {
            //test should return null object
            DrinkMenu Options = new DrinkMenu() { hasMilk = true, type = AvailableDrinks.IceTea };
            Assert.IsNull(AcuCafe.AcuCafe.OrderDrink(Options));
        }

        [TestMethod]
        public void CoffeeHasChocolateTopping()
        {
            //test should return a Coffee with chocolate topping
            DrinkMenu Options = new DrinkMenu() { hasChocolate = true, type = AvailableDrinks.Expresso };
            Assert.AreEqual(AcuCafe.AcuCafe.OrderDrink(Options).HasChocolate, true);
        }

        [TestMethod]
        public void TeaHasNotChocolateTopping()
        {
            //test should return a tee without chocolate
            DrinkMenu Options = new DrinkMenu() { hasChocolate = true, type = AvailableDrinks.Tea };
            Assert.AreEqual(AcuCafe.AcuCafe.OrderDrink(Options).HasChocolate, false);
        }


        [TestMethod]
        public void DynamicTeaWithMilkWithoutChocolate()
        {
            //test should return a dynamic tea type with milk and without chocolate
            DrinkMenu Options = new DrinkMenu() { hasMilk = true, hasChocolate = true };
            Assert.AreEqual(AcuCafe.AcuCafe.OrderDrinkDynamic<Tea>(Options).HasMilk, true);
            Assert.AreEqual(AcuCafe.AcuCafe.OrderDrinkDynamic<Tea>(Options).HasChocolate, false);
        }

        [TestMethod]
        public void DynamicPriceCheckFromBase()
        {
            //test should return check the price of tea with milk is 1.5
            DrinkMenu Options = new DrinkMenu() { hasMilk = true, hasChocolate = true };
            Drink drink = AcuCafe.AcuCafe.OrderDrinkDynamic<Tea>(Options);
            Assert.AreEqual(drink.Cost, 1.5);
        }
    }
}
