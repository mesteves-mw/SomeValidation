namespace SomeValidation.Test
{
    using NUnit.Framework;
    using System;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using System.Linq;

    [TestFixture]
    public class ParameterValidatorTest
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

        public class CustomerValidator : ParameterValidator<Customer>
        {
            public static readonly ParameterInfo Name =         Param(nameof(Customer.Name));
            public static readonly ParameterInfo AddressData =  Param(nameof(Customer.AddressData));
            public static readonly ParameterInfo Age =          Param(nameof(Customer.Age));
            public static readonly ParameterInfo Balance =      Param(nameof(Customer.Balance));

            protected override void Validate(ForName forName, Customer c, params Guid[] ruleSet)
            {               
                this.ShouldNotBeNull(forName(Name), c.Name);

                Create<AddressValidator>().Validate(forName(AddressData), c.AddressData);

                if (c.Age == 0) this.RaiseError(forName(Age), "{0} is 0!");

               Create<MoneyValidator>().Validate(forName(Balance), c.Balance);
            }
        }

        public class AddressValidator : ParameterValidator<Address>
        {
            public static readonly ParameterInfo Address1 = Param(nameof(Address));
            public static readonly ParameterInfo PostCode = Param(nameof(Address.PostCode));
            public static readonly ParameterInfo Owner = Param(nameof(Address.Owner));
            public static readonly ParameterInfo Street = Param(nameof(Address.Street));

            protected override void Validate(ForName forName, Address a, params Guid[] ruleSet)
            {
                if (a == null) { this.RaiseError(forName(Address1), "{0} is null!"); return; }

                if (a.PostCode == null) this.RaiseError(forName(PostCode), "{0} is null!");

                Create<CustomerValidator>().Validate(forName(Owner), a.Owner);

                if (a.Street == null) this.RaiseError(forName(Street), "{0} is null!");
            }
        }

        public class MoneyValidator : ParameterValidator<Money>
        {
            protected override void Validate(ForName forName, Money a, params Guid[] ruleSet)
            {
                if (a.Amount <= 0) this.RaiseError(forName(), "{0} is negative!");

                if (a.Amount > 10000) this.RaiseError(forName(), "{0} is higher than max!");
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
                " -- cust.Name is null!\n",
                " -- cust.AddressData.PostCode is null!\n",
                " -- cust.AddressData.Owner.Name is null!\n",
                " -- cust.AddressData.Owner.AddressData.Address is null!\n",
                " -- cust.AddressData.Owner.Age is 0!\n",
                " -- cust.AddressData.Street is null!\n",
                " -- cust.Age is 0!\n");
        }

        [Test]
        public void ValidateTest_ParameterValidatorsParallel()
        {
            var cust = new Customer();
            cust.AddressData = new Address();
            cust.AddressData.Owner = new Customer();

            var cv = new CustomerValidator();

            //Handling raise of errors
            var errors = new List<IValidationError>();
            cv.OnError += errors.Add;

            var custParamInfo = new ParameterInfo("cust");

            Parallel.Invoke(() => cv.Validate(custParamInfo, cust), () => cv.Validate("cust2", cust));

            //var postCodeFailure = errors.FirstOrDefault(vf => (vf as ValidationError).ParameterGuid.GetValueOrDefault() == AddressValidator.PostCode);
            //Assert.That(postCodeFailure.ParameterName, Contains.Substring("PostCode"));

            bool matchCustAddressOwnerName = false;

            foreach (var error in errors)
            { 
                switch ((error as ParameterValidationError)?.Parameter)
                {
                    case ParameterInfo pi when pi == CustomerValidator.Name:
                        break;

                    case ParameterInfo pi when pi == custParamInfo / CustomerValidator.AddressData / AddressValidator.Owner / CustomerValidator.Name:
                        matchCustAddressOwnerName = true;
                        break;
                }
            }

            Assert.That(matchCustAddressOwnerName, $"Failed to {matchCustAddressOwnerName}");

            //Handle validation failure list
            var errorMessage = " -- " + string.Join("\r\n -- ", errors.Select(vf => string.Format(vf.ErrorMessage, vf.ParameterName)));
            
            AssertContainsInOrder(errorMessage,
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

        [Test]
        public void ValidateAndThrowTest_ParameterValidators()
        {
            var cust = new Customer();
            cust.AddressData = new Address();
            cust.AddressData.Owner = new Customer();

            var cv = new CustomerValidator();

            var ex = Assert.Throws<ValidationException>(() => cv.ValidateAndThrow("cust", cust));

            AssertContainsInOrder(ex.Message,
                " -- cust.Name is null!",
                " -- cust.AddressData.PostCode is null!",
                " -- cust.AddressData.Owner.Name is null!",
                " -- cust.AddressData.Owner.AddressData.Address is null!",
                " -- cust.AddressData.Owner.Age is 0!",
                " -- cust.AddressData.Street is null!",
                " -- cust.Age is 0!");
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
