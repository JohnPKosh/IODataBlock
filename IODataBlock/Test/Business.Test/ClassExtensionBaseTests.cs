using Business.Common.Extensions;
using Business.Test.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Converters;
using System.Dynamic;

namespace Business.Test
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

            Assert.IsNotNull(dJsonString);
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
            for (int i = 0; i < person.NickNames.Count; i++)
            {
                var nickName = person.NickNames[i];
                var newNickName = newperson.NickNames[i];
                Assert.AreEqual(nickName, newNickName);
            }
            for (int i = 0; i < person.Pets.Count; i++)
            {
                var fido = person.Pets[i];
                var newFido = newperson.Pets[i];
                Assert.AreEqual(fido.Age, newFido.Age);
                Assert.AreEqual(fido.Name, newFido.Name);
                Assert.AreEqual(fido.NickName, newFido.NickName);
            }
            var sex = newperson.Sex;

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
            for (int i = 0; i < person.NickNames.Count; i++)
            {
                var nickName = person.NickNames[i];
                var newNickName = newperson.NickNames[i];
                Assert.AreEqual(nickName, newNickName);
            }
            for (int i = 0; i < person.Pets.Count; i++)
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
    }
}