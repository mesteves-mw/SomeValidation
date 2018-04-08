﻿namespace SomeValidation.Test
{
    using NUnit.Framework;
    using System.Threading.Tasks;

    [TestFixture]
    public class ValidatorTest
    {
        public class Customer
        {
            public string Name { get; set; }
            public Address AddressData { get; set; }
            public int Age { get; set; }
        }

        public class Address
        {
            public string PostCode { get; set; }
            public string Street { get; set; }
            public Customer Owner { get; set; }
        }

        public class CustomerValidator : AbstractValidator<Customer>
        {

            public override void Validate(Customer c)
            {
                if (c.Name == null) RaiseError("Name", "{0} is null!");

                Create<AddressValidator>().Validate(c.AddressData, "AddressData");

                if (c.Age == 0) RaiseError("Age", "{0} is 0!");
            }
        }

        public class AddressValidator : AbstractValidator<Address>
        {

            public override void Validate(Address a)
            {
                if (a == null) { RaiseError("Address", "{0} is null!"); return; }

                if (a.PostCode == null) RaiseError("PostCode", "{0} is null!");

                Create<CustomerValidator>().Validate(a.Owner, "Owner");

                if (a.Street == null) RaiseError("Street", "{0} is null!");
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
                " -- cust.Name is null!\n",
                " -- cust.AddressData.PostCode is null!\n",
                " -- cust.AddressData.Owner.Name is null!\n",
                " -- cust.AddressData.Owner.AddressData.Address is null!\n",
                " -- cust.AddressData.Owner.Age is 0!\n",
                " -- cust.AddressData.Street is null!\n",
                " -- cust.Age is 0!\n");

            AssertContainsInOrder(errors,
                " -- cust2.Name is null!\n",
                " -- cust2.AddressData.PostCode is null!\n",
                " -- cust2.AddressData.Owner.Name is null!\n",
                " -- cust2.AddressData.Owner.AddressData.Address is null!\n",
                " -- cust2.AddressData.Owner.Age is 0!\n",
                " -- cust2.AddressData.Street is null!\n",
                " -- cust2.Age is 0!\n");
        }

        public static void AssertContainsInOrder(string input, params string[] subStrings)
        {
            foreach(string subStr in subStrings)
            {
                Assert.That(input, Contains.Substring(subStr));
                input = input.Substring(input.IndexOf(subStr) + subStr.Length);
            }
        }
    }
}
