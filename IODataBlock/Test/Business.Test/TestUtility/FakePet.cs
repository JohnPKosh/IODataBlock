using Business.Common.System;
using System;

namespace Business.Test.TestUtility
{
    public class FakePet : ObjectBase<FakePet>
    {
        //public static FakePet CreateFromJson(String json)
        //{
        //    return FromJsonString<FakePet>(json);
        //}

        public static FakePet CreateBela()
        {
            var fido = new FakePet { Name = "Bela", Age = 6, NickName = "Dummee" };
            return fido;
        }

        public static FakePet CreateNala()
        {
            var fido = new FakePet { Name = "Nala", Age = 2, NickName = "Dumb Dumb Doo Doo Head" };
            return fido;
        }

        public String Name { get; set; }

        public Int32 Age { get; set; }

        public String NickName { get; set; }
    }
}