using Business.Common.Extensions;
using Business.Test.TestUtility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;

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

            FakePerson newperson = ClassExtensions.ConvertDynamicTo<FakePerson>(tempperson);

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
            var valueList = new List<dynamic>();
            for (var i = 0; i < 10; i++)
            {
                dynamic d = new ExpandoObject();
                d.count = i;
                valueList.Add(d);
            }
            //var results = valueList.Search("count", DynamicSearchOption.Between, Convert.ToDecimal(2), Convert.ToDecimal(8));
            var results = valueList.Filter("count", StringFilterOption.GreaterThan, Convert.ToDecimal(2));

            Console.WriteLine(results.Count());

            //Console.WriteLine("15".IsGreaterThan(20));
            //Console.ReadKey(true);
        }

        [TestMethod]
        public void AddPropertyToExpando()
        {
            var person = FakePerson.CreateKirk();
            dynamic tempperson = person.ToExpando();
            ClassExtensions.AddMember(tempperson, "newValue1", 1);
            Assert.IsTrue(((int)tempperson.newValue1) == 1);

            if (ClassExtensions.TryAddMember(tempperson, "newValue2", 2))
            {
                Assert.IsTrue(((int)tempperson.newValue2) == 2);
                var members = ClassExtensions.GetMetaMemberNames(tempperson);
                Assert.IsNotNull(members);
            }
            else
            {
                Assert.Fail("why?");
            }
        }

        [TestMethod]
        public void ExpandoTryGetMembers()
        {
            var person = FakePerson.CreateKirk();
            dynamic tempperson = person.ToExpando();

            var members = ClassExtensions.GetMetaMemberNames(tempperson);
            var memberTypes = ClassExtensions.GetMemberTypes(tempperson);

            Assert.IsNotNull(members);
            Assert.IsNotNull(memberTypes);
        }

        [TestMethod]
        public void ExpandoTryGetValue()
        {
            var person = FakePerson.CreateKirk();
            dynamic tempperson = person.ToExpando();
            ClassExtensions.AddMember(tempperson, "newValue1", 1);

            var val = 0;
            if (ClassExtensions.TryGetValue(tempperson, "newValue1", out val))
            {
                Assert.IsNotNull(val);
            }
            else
            {
                Assert.IsTrue(val == 0);
            }
        }

        [TestMethod]
        public void ExpandoTryGetValueObject()
        {
            var person = FakePerson.CreateKirk();
            dynamic tempperson = person.ToExpando();
            ClassExtensions.AddMember(tempperson, "newValue1", 1);

            object val;
            if (ClassExtensions.TryGetValue(tempperson, "newValue1", out val))
            {
                Assert.IsNotNull(val);
            }
        }

        [TestMethod]
        public void ExpandoDeleteValue()
        {
            var person = FakePerson.CreateKirk();
            dynamic tempperson = person.ToExpando();
            ClassExtensions.AddMember(tempperson, "newValue1", 1);
            ClassExtensions.DeleteMember(tempperson, "newValue1");
            object val;
            if (ClassExtensions.TryGetValue(tempperson, "newValue1", out val))
            {
                Assert.IsNotNull(val);
            }
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

        #region Dictionary Ideas

        [TestMethod]
        public void NestedDictionaryReadSample()
        {
            var dictionary = new Dictionary<string, Dictionary<Int32, string>>
            {
                {"A", new Dictionary<Int32, string> {{1, "Value 1"}, {2, "Value 2"}, {3, "Value 3"}}},
                {"B", new Dictionary<Int32, string> {{1, "Value 1"}, {2, "Value 2"}, {3, "Value 3"}}},
                {"C", new Dictionary<Int32, string> {{1, "Value 3"}, {2, "Value 4"}, {3, "Value 5"}}}
            };
            var dictionaryToString = dictionary.SelectMany(n => n.Value.Select(o => n.Key + "." + o.Key + "," + o.Value)).ToList(); // transform here with LINQ
            foreach (var d in dictionaryToString)
            {
                var str = d;
                Assert.IsNotNull(str);
            }
        }

        #endregion Dictionary Ideas

        [TestMethod]
        public void DeserializeFromStream()
        {
            //WriteJsonToFilePath
            var kirk = FakePerson.CreateKirk();

            kirk.WriteJsonToFilePath(@"c:\junk\kirk.json");
            FakePerson newKirk;

            using (var fs = File.Open(@"c:\junk\kirk.json", FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                newKirk = fs.JsonDeserialize<FakePerson>();
            }

            Assert.IsNotNull(newKirk);
            Assert.IsNotNull(newKirk.Pets);
        }

        [TestMethod]
        public void CanGetXmlDocument()
        {
            var kirk = FakePerson.CreateKirk();
            var xml = kirk.ToJObjectXml();
            Assert.IsNotNull(xml);
        }
    }
}