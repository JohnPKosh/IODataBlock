using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Business.Test.Validation
{
    [TestClass]
    public class ValidateTn
    {
        [TestMethod]
        public void FailValidationIfNotEnoughDigits()
        {
            var tn = new TnString("000000000");

            // I can get a list of validation errors...
            var erMessages = tn.GetValidationMessages();
            Assert.IsNotNull(erMessages);

            // or I can get full validation results...
            var errors = new List<ValidationResult>();
            if (!tn.TryValidate(errors))
            {
                foreach (var v in errors)
                {
                    Assert.IsNotNull(v);
                }
            }
        }

        [TestMethod]
        public void FailValidationIfNotEnoughDigitsImplicitely()
        {
            // implicitely convert TnString to string
            string tninput = new TnString("000000000");

            // implicitely convert String to TnString
            TnString tn = tninput;

            // I can get a list of validation errors...
            var erMessages = tn.GetValidationMessages();
            Assert.IsNotNull(erMessages);

            // or I can get full validation results...
            var errors = new List<ValidationResult>();

            if (tn.TryValidate(errors)) return;
            foreach (var v in errors)
            {
                Assert.IsNotNull(v);
            }
        }

        [TestMethod]
        public void FailAnyway()
        {
            // implicitely convert TnString to string
            string tninput = new TnString("0000000000");

            // implicitely convert String to TnString
            TnString tn = tninput;

            // I can get a list of validation errors...
            var erMessages = tn.GetValidationMessages();
            Assert.IsNotNull(erMessages);

            // or I can get full validation results...
            var errors = new List<ValidationResult>();

            if (tn.TryValidate(errors)) return;
            foreach (var v in errors)
            {
                Assert.IsNotNull(v);
            }
        }

        [TestMethod]
        public void CreateTnStringSetFromStringsUsingFactory()
        {
            var tns = new List<string>() {"2165132288", "2165132288", "2165132289"};
            var tnslist = TnStringList.Create(tns.AsEnumerable());

            Assert.IsNotNull(tnslist);
        }

        [TestMethod]
        public void FailValidationIfTnStringListLengthEq0()
        {
            var tns = new List<string>();
            var tnslist = TnStringList.Create(tns.AsEnumerable());

            var erMessages = tnslist.GetValidationMessages();
            Assert.IsNotNull(erMessages);

            // or I can get full validation results...
            var errors = new List<ValidationResult>();

            if (tnslist.TryValidate(errors)) return;
            foreach (var v in errors)
            {
                Assert.IsNotNull(v);
            }
        }

        [TestMethod]
        public void FailValidationIfTnStringListHasNullValue()
        {
            //var tns = new List<string>() { "2165132288", "2165132288", "2165132289" };
            //var tnslist = TnStringList.Create(tns.AsEnumerable());

            var tnslist = new TnStringList();
            var erMessages = tnslist.GetValidationMessages();
            Assert.IsNotNull(erMessages);
        }

        [TestMethod]
        public void FailValidationIfAnyTnStringsAreNotValid()
        {
            var tns = new List<string>() { "2165132288", "216513228", "2165132289", "216513228x", "216513228x" };
            var tnslist = TnStringList.Create(tns.AsEnumerable());
            tnslist.Required = true;

            //var tnslist = new TnStringList();
            var erMessages = tnslist.GetValidationMessages();
            Assert.IsNotNull(erMessages);
        }



        [TestMethod]
        public void FailValidationWithFunc()
        {
            var tn = new TnString("0000000000");

            // Create a list of additional validation functions
            var validators = new List<Func<object, ValidationResult>>();
            validators.Add(new Func<object, ValidationResult>(x =>
            {
                if (!(x.GetType() == typeof (TnString))) return new ValidationResult("Object is not a TnString!");
                else
                {
                    var tnstring = (TnString) x;
                    if (tnstring.Value == "0000000000")return new ValidationResult("You really just should not do this crazy stuff!", new[]{"Value"});
                    return null;
                }
            }));

            // or I can get full validation results...
            var errors = new List<ValidationResult>();
            if (!tn.TryValidate(errors, validators))
            {
                foreach (var v in errors)
                {
                    Assert.IsNotNull(v);
                }
            }
        }

    }
}
