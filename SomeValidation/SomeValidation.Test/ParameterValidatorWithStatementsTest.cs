namespace SomeValidation.Test
{
    using NUnit.Framework;
    using System;
    using SomeValidation.Statements;

    [TestFixture]
    public class ParameterValidatorWithStatementsTest
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

        public class CustomerValidator : ParameterValidator<Customer>, IValidator
        {
            public static readonly ParameterInfo Name =         Param(nameof(Customer.Name));
            public static readonly ParameterInfo AddressData =  Param(nameof(Customer.AddressData));
            public static readonly ParameterInfo Age =          Param(nameof(Customer.Age));
            public static readonly ParameterInfo Balance =      Param(nameof(Customer.Balance));

            protected override void Validate(ForName forName, Customer c, params Guid[] ruleSet)
            {          
                this.ShouldNotBe(forName(Name), c.Name).Null();

                Create<AddressValidator>().Validate(forName(AddressData), c.AddressData);

                this.ShouldNotBe(forName(Age), c.Age).Zero();

               Create<MoneyValidator>().Validate(forName(Balance), c.Balance);
            }
        }

        public class AddressValidator : ParameterValidator<Address>, IValidator
        {
            public static readonly ParameterInfo Address1 = Param(nameof(Address));
            public static readonly ParameterInfo PostCode = Param(nameof(Address.PostCode));
            public static readonly ParameterInfo Owner = Param(nameof(Address.Owner));
            public static readonly ParameterInfo Street = Param(nameof(Address.Street));

            protected override void Validate(ForName forName, Address a, params Guid[] ruleSet)
            {   
                if (a == null) { this.ShouldNotBe(forName(Address1), a).Null(); return; }

                this.ShouldNotBe(forName(PostCode), a.PostCode)
                    .NullOrEmpty()
                    .Do(stmt =>
                    {
                        stmt.Length()
                            .AtLeastAndAtMost(0,300)
                            .Positive();
                    });

                Create<CustomerValidator>().Validate(forName(Owner), a.Owner);

                this.ShouldNotBe(forName(Street), a.Street)
                    .Null()
                    .Length()
                        .GreaterThan(300)
                        .LessThan(0);
            }
        }

        public class MoneyValidator : ParameterValidator<Money>, IValidator
        {
            protected override void Validate(ForName forName, Money a, params Guid[] ruleSet)
            {
                this.ShouldBe(forName(), a.Amount)
                    .Positive()
                    .Negative()
                    .GreaterThan(10000);
            }
        }

        [Test]
        public void ValidateTest_ParameterValidators()
        {
            var cust = new Customer();
            cust.AddressData = new Address();
            cust.AddressData.Owner = new Customer();

            var cv = new CustomerValidator();

            string errors = "";

            cv.OnError += vf => errors += string.Format(" -- " + vf.ErrorMessage + "\n", vf.ParameterName);

            cv.Validate("cust", cust);

            AssertContainsInOrder(errors,
                " -- cust.Name should not be null.\n",
                " -- cust.AddressData.PostCode should not be null or empty.\n",
                " -- cust.AddressData.PostCode length should not be at least 0 and at most 300.\n",
                " -- cust.AddressData.Owner.Name should not be null.\n",
                " -- cust.AddressData.Owner.AddressData.Address should not be null.\n",
                " -- cust.AddressData.Owner.Age should not be zero.\n",
                " -- cust.AddressData.Owner.Balance should be positive.\n",
                " -- cust.AddressData.Owner.Balance should be negative.\n",
                " -- cust.AddressData.Owner.Balance should be greater than 10000.\n",
                " -- cust.AddressData.Street should not be null.\n",
                " -- cust.Age should not be zero.\n",
                " -- cust.Balance should be positive.\n",
                " -- cust.Balance should be negative.\n",
                " -- cust.Balance should be greater than 10000.\n");
        }

        [Test]
        public void ValidateAndThrowTest_ParameterValidators()
        {
            var cust = new Customer();
            cust.AddressData = new Address();
            cust.AddressData.Owner = new Customer();

            var cv = new CustomerValidator();

            var ex = Assert.Throws<ValidationException>(() => cv.ValidateAndThrow("cust", cust));

            AssertContainsInOrder(ex.Message,
                " -- cust.Name should not be null.",
                " -- cust.AddressData.PostCode should not be null or empty.",
                " -- cust.AddressData.PostCode length should not be at least 0 and at most 300.",
                " -- cust.AddressData.Owner.Name should not be null.",
                " -- cust.AddressData.Owner.AddressData.Address should not be null.",
                " -- cust.AddressData.Owner.Age should not be zero.",
                " -- cust.AddressData.Owner.Balance should be positive.",
                " -- cust.AddressData.Owner.Balance should be negative.",
                " -- cust.AddressData.Owner.Balance should be greater than 10000.",
                " -- cust.AddressData.Street should not be null.",
                " -- cust.Age should not be zero.",
                " -- cust.Balance should be positive.",
                " -- cust.Balance should be negative.",
                " -- cust.Balance should be greater than 10000.");
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
