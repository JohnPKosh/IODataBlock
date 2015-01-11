using System;
using System.Collections.Generic;
using Business.Common.System;

namespace Business.Test.TestUtility
{
    public class FakePerson : ObjectBase<FakePerson>
    {
        //public static FakePerson CreateFromJson(String json)
        //{
        //    return FromJsonString<FakePerson>(json);
        //}

        public static FakePerson CreateKirk()
        {
            var person = new FakePerson
            {
                Age = 30,
                Birthday = new DateTime(1960, 1, 1),
                FirstName = "James",
                LastName = "Kirk",
                //MiddleInitial = "T",
                NickNames = new List<string> { "Captain", "James" },
                Pets = new List<FakePet> { FakePet.CreateBela(), FakePet.CreateNala() },
                Sex = SexType.Male,
                MyRatings = new Dictionary<string, int> { { "top", 1 }, { "middle", 2 } }
            };
            return person;
        }

        public static FakePerson CreateTKirk()
        {
            var person = new FakePerson
            {
                Age = 30,
                Birthday = new DateTime(1960, 1, 1),
                FirstName = "James",
                LastName = "Kirk",
                MiddleInitial = "T",
                NickNames = new List<string> { "Captain", "James" },
                Pets = new List<FakePet> { FakePet.CreateBela(), FakePet.CreateNala() },
                Sex = SexType.Male,
                MyRatings = new Dictionary<string, int> { { "top", 1 }, { "middle", 2 } }
            };
            return person;
        }

        public String FirstName { get; set; }

        public String LastName { get; set; }

        public string MiddleInitial { get; set; }

        public Int32 Age { get; set; }

        public DateTime Birthday { get; set; }

        public SexType Sex { get; set; }

        public List<String> NickNames { get; set; }

        public List<FakePet> Pets { get; set; }

        public Dictionary<string, int> MyRatings { get; set; }
    }

    public enum SexType
    {
        Male,
        Female,
        Other
    }
}