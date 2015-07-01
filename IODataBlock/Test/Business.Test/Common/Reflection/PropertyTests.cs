using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Business.Common.Extensions;
using Business.Common.Reflection;
using Business.Test.TestUtility;
using Fasterflect;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace Business.Test.Common.Reflection
{
    [TestClass]
    public class PropertyTests
    {
        [TestMethod]
        public void TestFakePersonPropNames()
        {
            var _props = FakePerson.CreateKirk().GetLazyPropertyInfo();
            var propnames = _props.Value.Select(prop => prop.Key).ToList();
            if (!propnames.Any())
            {
                Assert.Fail("no data?");
            }
        }

        [TestMethod]
        public void TestFakePersonPropValues()
        {
            var _props = FakePerson.CreateKirk().GetLazyPropertyInfo();
            var valuesOnly = _props.Value.Select(prop => prop.Value).ToList();
            if (!valuesOnly.Any())
            {
                Assert.Fail("no data?");
            }

            foreach (var memberGetter in valuesOnly)
            {
                var newkirk = memberGetter.Invoke(FakePerson.CreateKirk());
                if (newkirk == null)
                {
                    //Assert.Fail("no data?");
                }
            }
        }


        [TestMethod]
        public void TestFakePersonPropValuesByName()
        {
            var kirk = FakePerson.CreateKirk();
            var _props = kirk.GetLazyPropertyInfo();
            var propnames = _props.Value.Select(prop => prop.Key).ToList();
            if (!propnames.Any())
            {
                Assert.Fail("no data?");
            }

            foreach (var propname in propnames)
            {
                var memberGetter = _props.Value.FirstOrDefault(prop => prop.Key == propname).Value;
                var val = memberGetter.Invoke(kirk);
                if (val == null)
                {
                    
                    //Assert.Fail("no data?");
                }
            }
        }

        [TestMethod]
        public void TestFakePersonPropValuesByName2()
        {
            var kirk = FakePerson.CreateKirk();
            var _props = kirk.GetLazyPropertyInfo();
            var propnames = _props.Value.Select(prop => prop.Key).ToList();

            foreach (var propname in propnames)
            {
                var val = kirk.TryGetPropertyValue(propname);
                if (val == null)
                {
                    //Assert.Fail("no data?");
                }
            }

            var name = kirk.TryGetFieldValue("FirstName") as string;
            var props2 = kirk.GetType().FieldsAndPropertiesWith(Flags.InstancePublic).Where(x => !x.Type().InheritsOrImplements(typeof(ICollection<>)) && !x.Type().InheritsOrImplements(typeof(Enum)));
            foreach (var memberInfo in props2)
            {
                var membername = memberInfo.Name;
                if (String.IsNullOrWhiteSpace(membername))
                {
                    //Assert.Fail("no data?");
                }
            }
        }

        [TestMethod]
        public void TestPersonJObject()
        {
            var person = FakePerson.CreateKirk();
            JObject kirk = person;
            var props2 = kirk.GetType().FieldsAndPropertiesWith(Flags.InstancePublic).Where(x => !x.Type().InheritsOrImplements(typeof(ICollection<>)) && !x.Type().InheritsOrImplements(typeof(Enum)));
            foreach (var memberInfo in props2)
            {
                var membername = memberInfo.Name;
                if (String.IsNullOrWhiteSpace(membername))
                {
                    //Assert.Fail("no data?");
                }
            }
        }

        [TestMethod]
        public void TestPersonDynamic()
        {
            var person = FakePerson.CreateKirk();
            JObject jKirk = person;
            dynamic expandoKirk = person.ToExpando();
            dynamic dynKirk = new {FirstName = "James", LastName = "Kirk"};
            var dynKirkType = dynKirk.GetType();

            var jkirkisdynamic = jKirk.GetType().InheritsOrImplements<IDynamicMetaObjectProvider>();
            var expandoKirkType = expandoKirk.GetType();
            var expandoKirkisdynamic = expandoKirkType.IsGenericType;
            var dynKirkisdynamic = dynKirkType.IsGenericType;

            if (jkirkisdynamic && expandoKirkisdynamic && dynKirkisdynamic)
            {
                // yeah!
            }
        }

    }
}
