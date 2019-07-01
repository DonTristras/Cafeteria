using System;
using System.Text;

namespace AcuCafe
{
    //This enum will allow us to predefine the Drink options
    public enum AvailableDrinks { Expresso, Tea, IceTea };

    //Let's create a class that defines the existing options, this will help to mantain it in the future
    public class DrinkMenu
    {
        public AvailableDrinks type;
        public bool hasMilk = false;
        public bool hasSugar = false;
        public bool hasChocolate = false;
    }

    public class AcuCafe
    {
        // add a class comment to notify library consumers there is a improved version
        /// <summary>
        /// This method has been deprecated, please use OrderDrink(DrinkMenu option)
        /// </summary>
        /// <param name="type"></param>
        /// <param name="hasMilk"></param>
        /// <param name="hasSugar"></param>
        /// <returns></returns>
        public static Drink OrderDrink(string type, bool hasMilk, bool hasSugar)
        {
            Drink drink = new Drink();
            if (type == "Expresso")
            {
                drink = new Expresso();
            }
            else if (type == "HotTea")
                drink = new Tea();
            else if (type == "IceTea")
                drink = new IceTea();

            try
            {
                drink.HasMilk = hasMilk;
                drink.HasSugar = hasSugar;
                drink.Prepare(); //Remove parameter as it was not used in the method
            }
            catch (Exception ex)
            {
                Console.WriteLine("We are unable to prepare your drink.");
                System.IO.File.WriteAllText(@"c:\Error.txt", ex.ToString());
            }

            return drink;
        }

        //Let's create a an overloaded alternative method for the updated version
        public static Drink OrderDrink(DrinkMenu option)
        {
            Drink drink = new Drink();
            if (option.type == AvailableDrinks.Expresso)
            {
                drink = new Expresso();
            }
            else if (option.type == AvailableDrinks.Tea)
            {
                drink = new Tea();
            }
            else if (option.type == AvailableDrinks.IceTea)
            {
                drink = new IceTea();
            }
                

            try
            {
                drink.Prepare(); //Remove type as it is not used
                drink.HasMilk = option.hasMilk;
                drink.HasSugar = option.hasSugar;
                drink.HasChocolate = option.hasChocolate;
                return drink;
            }
            catch (Exception ex)
            {
                Console.WriteLine("We are unable to prepare your drink: " + ex.ToString());
                System.IO.File.WriteAllText(@"c:\Error.txt", ex.ToString());
            }
            //Let's return nothing if something fails
            return null;
        }

        //If we want to make it even more DRY and easier to mantain, we could use dynamic T types with a base class filter, so it allows only derivated classes and avoid runtime errors
        //and get rid off the factory pattern. We would need to remove AvailiableDrinks from the DrinkMenu. This would require to refactor code using this library 
        public static T OrderDrinkDynamic<T>(DrinkMenu option) where T : Drink, new()
        {
            try
            {
                T drink = new T();
                drink.Prepare(); //Remove type as it is not used
                drink.HasMilk = option.hasMilk;
                drink.HasSugar = option.hasSugar;
                drink.HasChocolate = option.hasChocolate;
                return drink;
            }
            catch (Exception ex)
            {
                Console.WriteLine("We are unable to prepare your drink: " + ex.ToString());
                System.IO.File.WriteAllText(@"c:\Error.txt", ex.ToString());
            }
            //Let's return nothing if something fails
            return null;
        }

    }

    public class Drink
    {
        //Instanciate baseclass with false values, as we asume there are no extras by default
        public Drink() {
            _HasMilk = false;
            _HasChocolate = false;
            _HasSugar = false;
        }

        public const double MilkCost = 0.5;
        public const double SugarCost = 0.5;
        //let's asume chocolate is free for the moment
        public const double ChocoCost = 0;
        //We add all properties in base class, having below a protected property which will be only accessible to subtypes
        //By default we make properties read only, in this way we don't need to write override for each property in the subtype
        //We also change the new for properties we want to override as it will pic up the parent property when 
        //https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/polymorphism
        protected bool _HasMilk;
        public virtual bool HasMilk
        {
            get { return _HasMilk; }
            set { }
        }

        protected bool _HasChocolate;
        public virtual bool HasChocolate
        {
            get { return _HasChocolate; }
            set { }
        }

        protected bool _HasSugar;
        public virtual bool HasSugar
        {
            get { return _HasSugar; }
            set {  }
        }

        public virtual string Description
        {
            get;
        }

        //I move Cost property to the base class to avoid duplicated code, use inhertiance instead
        public virtual double BaseCost { get; set; }
        public double Cost
        {
            get
            {
                double cost = BaseCost;
                if (HasMilk)
                    cost += MilkCost;
                if (HasSugar)
                    cost += SugarCost;
                if (HasChocolate)
                    cost += ChocoCost;
                return cost;
            }
        }

        public void Prepare()
        {   // https://stackoverflow.com/questions/73883/string-vs-stringbuilder
            //Let's use stringbuilder to improve the performance
            StringBuilder message = new StringBuilder();
            message.AppendFormat("We are preparing the following drink for you: {0}", Description);
            if (HasMilk)
                message.Append(" with milk");
            else
                message.Append(" without milk");

            if (HasSugar)
                message.Append(" with sugar");
            else
                message.Append(" without sugar");

            Console.WriteLine(message);
        }
    }

    public class Expresso : Drink
    {
        //Set up constants as read only properties
        public override string Description
        {
            get { return "Expresso"; }
        }

        public override double BaseCost {
            get { return 1.8;  }
        }

        //We override the setter of each property we want to make writable
        public override bool HasMilk
        {
            set { _HasMilk = value; }
        }
        public override bool HasSugar
        {
            set { _HasSugar = value; }
        }
        //Having the chocolate defined in the base class, then we can override the setter to be able to satisfy liskov principle
        public override bool HasChocolate
        {
            set { _HasChocolate = value; }
        }
    }

    public class Tea : Drink
    {
        //Set up constants as read only properties
        public override string Description
        {
            get { return "Tea"; }
        }

        public override double BaseCost
        {
            get { return 1; }
        }

        //We override the setter of each property we want to make writable
        public override bool HasMilk
        {
            set { _HasMilk = value; }
        }
        public override bool HasSugar
        {
            set { _HasSugar = value; }
        }
    }

    public class IceTea : Drink
    {
        //Set up constants as read only properties
        public override double BaseCost
        {
            get { return 1; }
        }
        public override string Description
        {
            get { return "Ice tea"; }
        }
        public override bool HasSugar
        {
            set { _HasSugar = value; }
        }

        //Override the HasMilk property to avoid adding milk, throw an exception so the barista sees the raised error 
        //In this case I asumed we wan't to explictly stop the process to create the Ice Tea and notify the barista via an raised exception
        //however, if we want to raise an exception by default, we would add this part to Drink class
        public override bool HasMilk
        {
            set
            {
                if (value == true) { throw new Exception("We don't serve ice tea with milk"); };
            }
        }
    }
}