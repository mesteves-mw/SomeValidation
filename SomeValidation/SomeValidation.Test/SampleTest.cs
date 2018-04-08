namespace SomeValidation.Test
{
    using NUnit.Framework;
    using System;
    using System.Threading.Tasks;

    [TestFixture]
    public class ValidatorTest
    {
        public class Customer
        {
            public string Name { get; set; }
            public Address AddressData { get; set; }
            public int Age { get; set; }
            public Money Balance { get; set; }
        }

        public class Address
        {
            public string PostCode { get; set; }
            public string Street { get; set; }
            public Customer Owner { get; set; }
        }

        public struct Money
        {
            public decimal Amount { get; set; }
        }

        public class CustomerValidator : AbstractValidator<Customer>
        {
            public static readonly Guid Name = Guid.NewGuid();
            public static readonly Guid Age = Guid.NewGuid();

            protected override void Validate(Customer c, ForName forName)
            {
                if (c.Name == null) RaiseError(forName("Name"), "{0} is null!");

                Create<AddressValidator>().Validate(c.AddressData, forName("AddressData"));

                if (c.Age == 0) RaiseError(forName("Age"), "{0} is 0!");

                Create<MoneyValidator>().Validate(c.Balance, forName("Balance"));
            }
        }

        public class AddressValidator : AbstractValidator<Address>
        {
            public static readonly Guid PostCode = Guid.NewGuid();
            public static readonly Guid Street = Guid.NewGuid();

            protected override void Validate(Address a, ForName forName)
            {
                if (a == null) { RaiseError(forName("Address"), "{0} is null!"); return; }

                if (a.PostCode == null) RaiseError(forName("PostCode"), "{0} is null!");

                Create<CustomerValidator>().Validate(a.Owner, forName("Owner"));

                if (a.Street == null) RaiseError(forName("Street"), "{0} is null!");
            }
        }

        public class MoneyValidator : AbstractValidator<Money>
        {
            protected override void Validate(Money a, ForName forName)
            {
                if (a.Amount <= 0) RaiseError(forName(), "{0} is negative!");

                if (a.Amount > 10000) RaiseError(forName(), "{0} is higher than max!");
            }
        }

        [Test]
        public void SmokeTest()
        {
            var cust = new Customer();
            cust.AddressData = new Address();
            cust.AddressData.Owner = new Customer();

            var cv = new CustomerValidator();

            string errors = "";

            cv.OnError += (p, e) => errors += string.Format(" -- " + e + "\n", p);

            cv.Validate(cust, "cust");

            AssertContainsInOrder(errors,
                " -- cust.Name is null!\n",
                " -- cust.AddressData.PostCode is null!\n",
                " -- cust.AddressData.Owner.Name is null!\n",
                " -- cust.AddressData.Owner.AddressData.Address is null!\n",
                " -- cust.AddressData.Owner.Age is 0!\n",
                " -- cust.AddressData.Street is null!\n",
                " -- cust.Age is 0!\n");
        }

        [Test]
        public void ParallelTest()
        {
            var cust = new Customer();
            cust.AddressData = new Address();
            cust.AddressData.Owner = new Customer();

            var cv = new CustomerValidator();

            string errors = "";

            cv.OnError += (p, e) => errors += string.Format(" -- " + e + "\n", p);

            Parallel.Invoke(() => cv.Validate(cust, "cust"), () => cv.Validate(cust, "cust2"));
            
            AssertContainsInOrder(errors,
                " -- cust.Name is null!",
                " -- cust2.Name is null!",
                " -- cust2.AddressData.PostCode is null!",
                " -- cust.AddressData.PostCode is null!",
                " -- cust2.AddressData.Owner.Name is null!",
                " -- cust2.AddressData.Owner.AddressData.Address is null!",
                " -- cust2.AddressData.Owner.Age is 0!",
                " -- cust.AddressData.Owner.Name is null!",
                " -- cust.AddressData.Owner.AddressData.Address is null!",
                " -- cust.AddressData.Owner.Age is 0!",
                " -- cust2.AddressData.Owner.Balance is negative!",
                " -- cust2.AddressData.Street is null!",
                " -- cust2.Age is 0!",
                " -- cust.AddressData.Owner.Balance is negative!",
                " -- cust.AddressData.Street is null!",
                " -- cust.Age is 0!",
                " -- cust.Balance is negative!");
        }

        public static void AssertContainsInOrder(string input, params string[] subStrings)
        {
            foreach(string subStr in subStrings)
            {
                Assert.That(input, Contains.Substring(subStr));
                input = input.Replace(subStr, string.Empty);
            }
        }
    }
}
