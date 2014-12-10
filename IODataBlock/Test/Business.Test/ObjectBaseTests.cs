using Business.Common.Extensions;
using Business.Test.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;

namespace Business.Test
{
    [TestClass]
    public class ObjectBaseTests
    {
        [TestMethod]
        public void CanGetJsonStringFromFakePet()
        {
            var fido = FakePet.CreateBela();
            var fidoJson = fido.ToJson();
            Assert.IsTrue(!String.IsNullOrWhiteSpace(fidoJson));
        }

        [TestMethod]
        public void CanDeserializeJsonStringFromFakePet()
        {
            var fido = FakePet.CreateBela();
            var fidoJson = fido.ToJson();
            var newFido = JsonConvert.DeserializeObject<FakePet>(fidoJson);

            Assert.IsInstanceOfType(newFido, typeof(FakePet));
            Assert.AreEqual(fido.Age, newFido.Age);
            Assert.AreEqual(fido.Name, newFido.Name);
            Assert.AreEqual(fido.NickName, newFido.NickName);
        }

        [TestMethod]
        public void CanDeserializeJsonStringFromFakePetCreate()
        {
            var fido = FakePet.CreateBela();
            var fidoJson = fido.ToJson();
            var newFido = FakePet.CreateFromJson(fidoJson);

            Assert.IsInstanceOfType(newFido, typeof(FakePet));
            Assert.AreEqual(fido.Age, newFido.Age);
            Assert.AreEqual(fido.Name, newFido.Name);
            Assert.AreEqual(fido.NickName, newFido.NickName);
        }

        [TestMethod]
        public void CanGetJsonStringFromFakePerson()
        {
            var person = FakePerson.CreateKirk();
            var personJson = person.ToJson();
            Assert.IsTrue(!String.IsNullOrWhiteSpace(personJson));
        }

        [TestMethod]
        public void CanDeserializeJsonStringFromFakePerson()
        {
            var person = FakePerson.CreateKirk();
            var personJson = person.ToJson();
            var newperson = JsonConvert.DeserializeObject<FakePerson>(personJson);

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
            Assert.AreEqual(person.Sex, newperson.Sex);
        }

        [TestMethod]
        public void CanDeserializeJsonStringFromFakePersonCreate()
        {
            var person = FakePerson.CreateKirk();
            var personJson = person.ToJson();
            var newperson = FakePerson.CreateFromJson(personJson);

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
            Assert.AreEqual(person.Sex, newperson.Sex);
        }

        [TestMethod]
        public void CanDeserializeJsonStringFromFakePersonUsingConvertJson()
        {
            var person = FakePerson.CreateKirk();
            var personJson = person.ToJson();
            var newperson = personJson.ConvertJson<FakePerson>();

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
            Assert.AreEqual(person.Sex, newperson.Sex);
        }

        [TestMethod]
        public void CanPopulateFakePerson()
        {
            var person = FakePerson.CreateKirk();
            var personJson = person.ToJson();
            var newperson = new FakePerson();

            newperson.PopulateFromJson(personJson);

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
            Assert.AreEqual(person.Sex, newperson.Sex);
        }

        [TestMethod]
        public void CanPopulateFakePersonAndSaveToFilePath()
        {
            var person = FakePerson.CreateKirk();
            var personJson = person.ToJson();
            var newperson = new FakePerson();

            newperson.PopulateFromJson(personJson);

            newperson.WriteJsonToFilePath(@"c:\junk\newperson.txt");

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
            Assert.AreEqual(person.Sex, newperson.Sex);
        }

        [TestMethod]
        public void ReadFakePersonFromJsonFile()
        {
            var person = FakePerson.CreateKirk();
            var newperson = new FileInfo(@"c:\junk\newperson.txt").ReadJsonFile<FakePerson>();

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
            Assert.AreEqual(person.Sex, newperson.Sex);
        }

        [TestMethod]
        public void ReadFakePersonFromJsonFileWithSettings()
        {
            var person = FakePerson.CreateKirk();
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.DefaultValueHandling = DefaultValueHandling.Populate;
            var newperson = new FileInfo(@"c:\junk\newperson.txt").ReadJsonFile<FakePerson>();

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
            Assert.AreEqual(person.Sex, newperson.Sex);
        }

        [TestMethod]
        public void ReadFakePersonFromJsonFileWithJsonConverter()
        {
            var person = FakePerson.CreateKirk();
            person.WriteJsonToFilePath(@"c:\junk\newpersonWithEnum.txt", new StringEnumConverter());

            var newperson = new FileInfo(@"c:\junk\newpersonWithEnum.txt").ReadJsonFile<FakePerson>(new StringEnumConverter());

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
            Assert.AreEqual(person.Sex, newperson.Sex);
        }

        [TestMethod]
        public void ReadFromJsonFileWithJsonConverter()
        {
            var sexes = new List<SexType>()
            {
                SexType.Female,
                SexType.Male
            };
            sexes.WriteJsonToFilePath(@"c:\junk\SexTypeWithEnum.txt", new StringEnumConverter());
            var newsexes = new FileInfo(@"c:\junk\SexTypeWithEnum.txt").ReadJsonFile<List<SexType>>(new StringEnumConverter() { AllowIntegerValues = false });
            Assert.AreEqual(sexes[0], newsexes[0]);
        }

        [TestMethod]
        public void JObjectConvertJsonWithJsonConverter()
        {
            var person = FakePerson.CreateKirk();
            var personJson = person.ToJsonString(new StringEnumConverter());
            var jobjectperson = personJson.ConvertJson();

            FakePerson newperson = jobjectperson.ToJToken().ToObject<FakePerson>();

            //newperson.PopulateFromJson(personJson);

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
            Assert.AreEqual(person.Sex, newperson.Sex);
        }

        [TestMethod]
        public void ObjectConvertJsonWithJsonConverter()
        {
            var person = FakePerson.CreateKirk();
            var personJson = person.ToJsonString(new StringEnumConverter());
            var jobjectperson = personJson.ConvertJson(typeof(FakePerson));

            FakePerson newperson = jobjectperson.ToJToken().ToObject<FakePerson>();

            //newperson.PopulateFromJson(personJson);

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
            Assert.AreEqual(person.Sex, newperson.Sex);
        }

        [TestMethod]
        public void ExpandoObjectFromJsonString()
        {
            var person = FakePerson.CreateKirk();
            var personJson = person.ToJsonString(new ExpandoObjectConverter(), new StringEnumConverter());
            dynamic newperson = personJson.ConvertJsonExpando();

            Assert.IsNotNull(newperson);
            Assert.IsInstanceOfType(newperson, typeof(ExpandoObject));
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

            Assert.AreEqual(person.Sex, Enum.Parse(typeof(SexType), newperson.Sex));
        }

        [TestMethod]
        public void ExpandoObjectBackToObject()
        {
            var person = FakePerson.CreateKirk();
            var personJson = person.ToJsonString(new ExpandoObjectConverter());
            dynamic tempperson = personJson.ConvertJsonExpando();

            var tempstring = ClassExtensions.ToJsonString(tempperson);
            var newperson = FakePerson.CreateFromJson(tempstring);

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
            Assert.AreEqual(person.Sex, newperson.Sex);
        }

        [TestMethod]
        public void ExpandoObjectBackToObject3()
        {
            dynamic fido = new ExpandoObject();
            fido.Age = 40;
            fido.Name = "Fido";
            var fidoJson = ClassExtensions.ToJsonString((object)fido);

            var newfido = FakePet.CreateFromJson(fidoJson);
            var newexpando = FakePet.CreateFromExpando(fido);

            var newfido2 = ClassExtensions.ConvertExpandoTo<FakePet>(fido);

            Assert.IsNotNull(newfido);

            //var person = FakePerson.CreateKirk();
            //dynamic tempperson = person.ToExpando();

            //FakePet.CreateBela();
            //FakePet newperson = ClassExtensionBase.ConvertDynamicTo<FakePet>(tempperson);
        }
    }
}