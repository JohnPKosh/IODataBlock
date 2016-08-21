using System.Runtime.Serialization;

namespace Business.Common.System.Args
{
    [DataContract(Namespace = @"http://www.broadvox.com/Args/", Name = "A")]
    public struct Arg
    {
        [DataMember]
        public string K { get; set; }

        [DataMember]
        public string V { get; set; }

        //public void SetValue(Object value, Func<Object, String> converter = null)
        //{
        //    if (converter != null)
        //    {
        //        this.V = converter(value);
        //    }
        //    else
        //    {
        //        this.V = value.ToString();
        //    }
        //}

        //public void SetValue(IConvertible value, String FormatString)
        //{
        //    if (value is IFormattable)
        //        this.V = ((IFormattable)value).ToString(FormatString, CultureInfo.CurrentCulture);
        //    else if (value != null)
        //        this.V = value.ToString();
        //    else
        //        this.V = String.Empty;

        //}
    }
}
