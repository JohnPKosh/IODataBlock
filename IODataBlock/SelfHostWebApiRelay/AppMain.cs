// ------------------------------------------------------------------
//
// Description:
//
// Author: John Kosh
// E-mail: jkosh@broadvox.com
// Created:
// ------------------------------------------------------------------
//
// Copyright © Broadvox LLC 2010
//
// ------------------------------------------------------------------

using Business.Common.System.App;
using Business.Common.System.Args;
using SelfHostWebApiRelay.RelayBase;
using System;
using System.Dynamic;

namespace SelfHostWebApiRelay
{
    /// <summary>
    /// Primary worker class of the application.
    /// </summary>
    public class AppMain
    {
        #region Class Initialization

        /// <summary>
        /// Default constructor for the AppMain class.
        /// </summary>
        /// <param name="args">Required Arguments instance for the default AppMain constructor.</param>
        public AppMain(Arguments args)
        {
            this.args = args;
        }

        /// <summary>
        /// Primary Start Factory Method called by the Applications Main method to create an instance of AppMain and call the Run method.
        /// </summary>
        /// <param name="args">Required Arguments instance to pass to the default AppMain constructor.</param>
        /// <returns>Returns an ExitCodeType value of Success if no exceptions are unhandled or Failure in the event of an unhandled exception.</returns>
        public static ExitCodeType Start(Arguments args)
        {
            try
            {
                AppMain app = new AppMain(args);
                app.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.Error.WriteLine(ex.Message);
                return ExitCodeType.Failure;
            }
            return ExitCodeType.Success;
        }

        #endregion Class Initialization

        #region Fields / Properties

        /// <summary>
        /// Private Arguments field for accessing arguments in the Application.
        /// </summary>
        private Arguments args = null;

        #endregion Fields / Properties

        #region Helper Methods

        /// <summary>
        /// Displays usage text for the Console application to the output stream.
        /// </summary>
        public static void Usage()
        {
            var usagestr = @"

==============SelfHostWebApiRelay==============

[-InputFilePath:] | [-i:]

* Required:
Full Path of input file to recurse over

[-OutputFilePath:] | [-o:]

* Required:
Full Path of output file to write

[-FromDate:]
* Required:
From date (eg. 2010-07-01)

[-ToDate:]
* Required:
To date (eg. 2010-07-01)

[-DateStrFormat:]
Optional:
Format string for $(DateStr) var
default format is yyyy-MM-dd

[-DateVarFormat:]
Optional:
Format string for $(DateVar) var
default format is yyyyMMdd

[-MergeInto:]
Optional:
Full Path of a merge
template file to apply

[-MergeIntoVar:]
* Required (If MergeInto is specified!):
Name of variable in the MergeInto
file to replace.
eg. MergeIntoVar becomes $(MergeIntoVar)

-OpenSSMS

Optional:
Opens the output file in
Sql Server Management Studio
or default .sql editor.

";

            Console.Write(usagestr);
        }

        #endregion Helper Methods

        /// <summary>
        /// Main Entry Point for Application with no Arguments. PLACE RUN CODE HERE!
        /// </summary>
        public void Run()
        {
            /* Main Entry Point for Application with no Arguments. PLACE RUN CODE HERE! */

            AppBag.Data.Value = new ExpandoObject();
            AppBag.Data.Value.BaseAddress = AddressLoader.LoadBaseAddress();
            AppBag.Data.Value.RelayAddress = AddressLoader.LoadRelayAddress();

            var mgr = new RelayManager();
            mgr.RunServer();

            if (Environment.UserInteractive)
            {
                Console.WriteLine("Hello");
                Console.WriteLine();
                Console.WriteLine(@"Press any key to continue!");
                Console.ReadKey(true);
            }
        }
    }
}