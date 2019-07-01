using System;
using AcuCafe;

namespace AcuShop
{
    class Program
    {
        static void Main(string[] args)
        {
            Drink drink = AcuCafe.AcuCafe.OrderDrink(new DrinkMenu() { hasMilk = true, hasSugar = true, type = AvailableDrinks.Expresso, hasChocolate = true});
            Drink c = new AcuCafe.Drink();

            Drink ex = AcuCafe.AcuCafe.OrderDrinkDynamic<Expresso>(new DrinkMenu { hasChocolate = true });
        }
    }
}
