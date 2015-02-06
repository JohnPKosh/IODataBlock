using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using Business.Test.TestUtility;
using Business.Utilities.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Converters;

namespace Business.Test.Common
{
    [TestClass]
    public class ClassExtensionBaseTests
    {
        [TestMethod]
        public void ExpandoObjectToJsonString()
        {
            dynamic d = FakePerson.CreateKirk().ToExpando();
            //string dJsonString = ClassExtensions.ToJsonString(d as ExpandoObject);
            var dJsonString = ((ExpandoObject)d).ToJsonString();
            var newd = dJsonString.ConvertJson<ExpandoObject>(new ExpandoObjectConverter());

            Assert.IsNotNull(newd);
            Assert.IsNotNull(dJsonString);
        }

        [TestMethod]
        public void WriteAndReadBsonDynamic()
        {
            var kirk = FakePerson.CreateKirk();
            var ms = new MemoryStream(kirk.ToBsonByteArray());
            var obj = ms.ConvertBson();
            dynamic expando = obj.ToExpando();
            var name = expando.FirstName;
            Assert.IsNotNull(obj);
        }

        [TestMethod]
        public void WriteAndReadBson()
        {
            var kirk = FakePerson.CreateKirk();
            var ms = new MemoryStream(kirk.ToBsonByteArray());
            var obj = ms.ConvertBson();
            var person = obj.ToObject<FakePerson>();
            var name = person.FirstName;
            Assert.IsNotNull(person.Pets);
            Assert.IsNotNull(obj);
        }

        [TestMethod]
        public void ExpandoObjectFromJsonStringBackToObject()
        {
            var person = FakePerson.CreateKirk();
            var personJson = person.ToJsonString(new ExpandoObjectConverter(), new StringEnumConverter());
            dynamic newperson = personJson.ConvertJsonExpando().ConvertTo<FakePerson>();  // error here in ConvertTo method

            Assert.IsNotNull(newperson);
            Assert.IsInstanceOfType(newperson, typeof(FakePerson));
            Assert.AreEqual(person.Age, newperson.Age);
            Assert.AreEqual(person.Birthday, newperson.Birthday);
            Assert.AreEqual(person.FirstName, newperson.FirstName);
            Assert.AreEqual(person.LastName, newperson.LastName);
            Assert.AreEqual(person.MiddleInitial, newperson.MiddleInitial);
            for (var i = 0; i < person.NickNames.Count; i++)
            {
                var nickName = person.NickNames[i];
                var newNickName = newperson.NickNames[i];
                Assert.AreEqual(nickName, newNickName);
            }
            for (var i = 0; i < person.Pets.Count; i++)
            {
                var fido = person.Pets[i];
                var newFido = newperson.Pets[i];
                Assert.AreEqual(fido.Age, newFido.Age);
                Assert.AreEqual(fido.Name, newFido.Name);
                Assert.AreEqual(fido.NickName, newFido.NickName);
            }

            Assert.AreEqual(person.Sex, newperson.Sex);
        }

        [TestMethod]
        public void ExpandoObjectBackToObject2()
        {
            var person = FakePerson.CreateKirk();
            dynamic tempperson = person.ToExpando();

            tempperson.SomeUnknownProperty = 5; // additional properties are ignored

            FakePerson newperson = ClassExtensionBase.ConvertDynamicTo<FakePerson>(tempperson);

            Assert.IsNotNull(newperson);
            Assert.IsInstanceOfType(newperson, typeof(FakePerson));
            Assert.AreEqual(person.Age, newperson.Age);
            Assert.AreEqual(person.Birthday, newperson.Birthday);
            Assert.AreEqual(person.FirstName, newperson.FirstName);
            Assert.AreEqual(person.LastName, newperson.LastName);
            Assert.AreEqual(person.MiddleInitial, newperson.MiddleInitial);
            for (var i = 0; i < person.NickNames.Count; i++)
            {
                var nickName = person.NickNames[i];
                var newNickName = newperson.NickNames[i];
                Assert.AreEqual(nickName, newNickName);
            }
            for (var i = 0; i < person.Pets.Count; i++)
            {
                var fido = person.Pets[i];
                var newFido = newperson.Pets[i];
                Assert.AreEqual(fido.Age, newFido.Age);
                Assert.AreEqual(fido.Name, newFido.Name);
                Assert.AreEqual(fido.NickName, newFido.NickName);
            }
            foreach (var myRating in person.MyRatings)
            {
                var newrating = newperson.MyRatings[myRating.Key];
                Assert.AreEqual(myRating.Value, newrating);
            }

            Assert.AreEqual(person.Sex, newperson.Sex);
        }

        [TestMethod]
        public void DynamicSearchTest1()
        {
            List<dynamic> valueList = new List<dynamic>();
            for (int i = 0; i < 10; i++)
            {
                dynamic d = new ExpandoObject();
                d.count = i;
                valueList.Add(d);
            }
            //var results = valueList.Search("count", DynamicSearchOption.Between, Convert.ToDecimal(2), Convert.ToDecimal(8));
            var results = valueList.Filter("count", StringFilterOption.GreaterThan, Convert.ToDecimal(2));

            Console.WriteLine(results.Count());

            //Console.WriteLine("15".IsGreaterThan(20));
            Console.ReadKey(true);
        }

        #region Comparison Extension Tests

        [TestMethod]
        public void StringGreaterThan()
        {
            "25".IsGreaterThan(20);
        }

        [TestMethod]
        public void IntGreaterThan()
        {
            15.IsGreaterThan(20);
        }

        [TestMethod]
        public void DoubleGreaterThan()
        {
            15D.IsGreaterThan(20);
        }

        [TestMethod]
        public void StringGreaterThanString()
        {
            var yn = "Hello".IsStringGtString("Aello");
            Assert.IsTrue(yn);
        }

        [TestMethod]
        public void StringNotGreaterThanString()
        {
            var yn = "Hello".IsStringGtString("Jello");
            Assert.IsFalse(yn);
        }

        [TestMethod]
        public void StringNotGreaterThanSameString()
        {
            var yn = "Hello".IsStringGtString("Hello");
            Assert.IsFalse(yn);
        }

        [TestMethod]
        public void StringNotGreaterThanSameStringLowerCase()
        {
            var yn = "Hello".IsStringGtString("hello"); // the "h" comes after "H" in sorting?
            Assert.IsFalse(yn);
        }

        [TestMethod]
        public void StringGreaterThanSameStringUpperCase()
        {
            var yn = "hello".IsStringGtString("Hello"); // the "h" comes after "H" in sorting?
            Assert.IsTrue(yn);
        }

        [TestMethod]
        public void StringNotGreaterThanLongerString()
        {
            var yn = "hell".IsStringGtString("hello");
            Assert.IsFalse(yn);
        }

        [TestMethod]
        public void StringGreaterThanShorterString()
        {
            var yn = "hello".IsStringGtString("hell");
            Assert.IsTrue(yn);
        }

        [TestMethod]
        public void IsStringFilterMatchGreaterThan()
        {
            var yn = "hel".IsStringFilterMatch(StringFilterOption.GreaterThan, new[] { "Hell" });
            Assert.IsTrue(yn);
        }


        [TestMethod]
        public void IsStringFilterMatchLessThan()
        {
            var yn = "Hell".IsStringFilterMatch(StringFilterOption.LessThan, new[] { "hel" });
            Assert.IsTrue(yn);
        }

        #endregion Comparison Extension Tests
    }
}