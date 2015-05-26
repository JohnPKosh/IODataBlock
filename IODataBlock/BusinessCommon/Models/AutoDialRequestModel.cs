using System.Collections.Generic;

namespace BusinessCommon.Models
{
    public class AutoDialRequestModel
    {

        /// <summary>
        /// Gets or sets the transaction identifier. (Optional)
        /// </summary>
        /// <value>
        /// The transaction identifier. (Unique ID)
        /// </value>
        public string transactionId { get; set; }


        /// <summary>
        /// Gets or sets the Calls Per Second. (Optional)
        /// </summary>
        /// <value>
        /// Integer as string. (Default 10)
        /// </value>
        public string cps { get; set; }

        /// <summary>
        /// Gets or sets the numbers to dial with callback URL property.
        /// </summary>
        /// <value>
        /// The numbers to dial.
        /// </value>
        public List<Number> numbers { get; set; }


        /// <summary>
        /// TNs to dial with callback URL property.
        /// </summary>
        public class Number
        {

            /// <summary>
            /// Gets or sets the TN that will be dialed.
            /// </summary>
            /// <value>
            /// The TN to dial.
            /// </value>
            public string number { get; set; }

            /// <summary>
            /// Gets or sets the callback URL where the service will POST results back.
            /// </summary>
            /// <value>
            /// The callback URL.
            /// </value>
            public string callback { get; set; }
        }
    }
}