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

using System;
using System.Linq;
using Business.Common.System.Args;

namespace SelfHostWebApi
{
    /// <summary>
    /// ExitCode Enumeration to be returned as ExitCode for the Application on Exit.
    /// </summary>
    public enum ExitCodeType : int
    {
        Success = 0,
        Failure = 1
    }

    /// <summary>
    /// Primary entry point class of the Application.
    /// </summary>
    class Program
    {
        /// <summary>
        /// Primary entry point method of the Application.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Arguments ArgumentDictionary = null;
            try // Parse Arguments
            {
                ArgumentDictionary = Arguments.CreateArguments(args);
            }
            catch (Exception ex)
            {
                Console.WriteLine(@"Error Parsing Arguments!");
                Console.WriteLine(ex.Message);
                Console.Error.WriteLine(@"Error Parsing Arguments!");
                Console.Error.WriteLine(ex.Message);

                AppMain.Usage();

                if (Environment.UserInteractive && args.Any(x => x.ToLower().Contains("interactive")))
                {
                    Console.WriteLine();
                    Console.WriteLine(@"Press [ENTER] to exit!");
                    Console.Read();
                }
                Environment.Exit((Int32)ExitCodeType.Failure);
            }

            try // Start Application
            {
                if (ArgumentDictionary.Count > 0)
                {
                    if (ArgumentDictionary.Any(x => x.K.ToLower().Contains("help")) || ArgumentDictionary.ContainsKey("?"))
                    {
                        AppMain.Usage();
                        Environment.Exit((Int32)ExitCodeType.Success);
                    }
                }

                //if (Environment.UserInteractive && ArgumentDictionary.Any(x => x.K.ToLower().Contains("interactive")))
                //{
                //    Console.WriteLine();
                //    Console.WriteLine(@"Press [ENTER] to run application!");
                //    Console.Read();
                //}

                Environment.Exit((Int32)AppMain.Start(ArgumentDictionary));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.Error.WriteLine(ex.Message);

                if (Environment.UserInteractive && ArgumentDictionary.Any(x => x.K.ToLower().Contains("interactive")))
                {
                    Console.WriteLine();
                    Console.WriteLine(@"Press [ENTER] to exit!");
                    Console.Read();
                }

                Environment.Exit((Int32)ExitCodeType.Failure);
            }
        }
    }
}
