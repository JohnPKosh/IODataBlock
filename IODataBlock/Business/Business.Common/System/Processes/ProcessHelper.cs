using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;

namespace Business.Common.System.Processes
{
    /// <summary>
    /// Wrapper around the Process class to add some convenience methods but
    /// most importantly deal with the complex nature of getting both
    /// StandardOutput and StandardError streams concurrently (this must be done with
    /// callbacks). See http://msdn2.microsoft.com/en-us/library/system.diagnostics.process.standarderror.aspx
    /// </summary>
    public class ProcessHelper:IDisposable
    {
        private Process _mProcess = new Process();
        private TextWriter _mError = Console.Error;
        private TextWriter _mOut = Console.Out;
        private readonly StringBuilder _mOutBuilder;
        private readonly StringBuilder _mErrorBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessHelper"/> class.
        /// </summary>
        public ProcessHelper()
            : this(true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessHelper"/> class.
        /// </summary>
        /// <param name="sendStreamsToStrings">if set to <c>true</c> [send streams to strings].</param>
        public ProcessHelper(bool sendStreamsToStrings)
        {
            _mProcess.StartInfo.WorkingDirectory = Environment.CurrentDirectory;
            _mProcess.ErrorDataReceived += process_ErrorDataReceived;
            _mProcess.OutputDataReceived += process_OutputDataReceived;

            if (sendStreamsToStrings)
            {
                // Caller wants the standard output and error streams
                // to be written in memory to strings that can later
                // be extracted
                _mOutBuilder = new StringBuilder(512);
                _mErrorBuilder = new StringBuilder();

                Out = new StringWriter(_mOutBuilder);
                Error = new StringWriter(_mErrorBuilder);
            }
        }

        /// <summary>
        /// Parses the specified STR.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public static ProcessHelper Parse(string str)
        {
            // Assumes there is an .exe somwhere
            var exeIndex = str.IndexOf(".exe", StringComparison.CurrentCultureIgnoreCase);
            if (exeIndex == -1) throw new NotImplementedException();
            if (str.Length > exeIndex + 4 && str[exeIndex + 4] == '\"')
            {
                exeIndex++;
            }
            string[] pieces = SplitOn(str, exeIndex + 3, true);
            pieces[0] = pieces[0].Trim();
            pieces[1] = pieces[1].Trim();
            if (pieces[0].Length > 0 && pieces[0][0] == '\"')
            {
                pieces[0] = pieces[0].Substring(1);
            }
            if (pieces[0].Length > 0 && pieces[0][pieces[0].Length - 1] == '\"')
            {
                pieces[0] = pieces[0].Substring(0, pieces[0].Length - 1);
            }
            var result = new ProcessHelper {FileName = pieces[0], Arguments = pieces[1]};
            return result;
        }

        public static string[] SplitOn(string str, int index, bool isIndexInFirstPortion)
        {
            string one, two;
            if (index == -1)
            {
                one = str;
                two = "";
            }
            else
            {
                if (index == 0)
                {
                    if (isIndexInFirstPortion)
                    {
                        one = str[0].ToString(CultureInfo.InvariantCulture);
                        two = str.Substring(1);
                    }
                    else
                    {
                        one = "";
                        two = str;
                    }
                }
                else if (index == str.Length - 1)
                {
                    if (isIndexInFirstPortion)
                    {
                        one = str;
                        two = "";
                    }
                    else
                    {
                        one = str.Substring(0, str.Length - 1);
                        two = str[str.Length - 1].ToString(CultureInfo.InvariantCulture);
                    }
                }
                else
                {
                    one = str.Substring(0, isIndexInFirstPortion ? index + 1 : index);
                    two = str.Substring(isIndexInFirstPortion ? index + 1 : index);
                }
            }

            return new[] { one, two };
        }

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>The name of the file.</value>
        public string FileName
        {
            get
            {
                return _mProcess.StartInfo.FileName;
            }
            set
            {
                _mProcess.StartInfo.FileName = value;
            }
        }

        /// <summary>
        /// Gets or sets the arguments.
        /// </summary>
        /// <value>The arguments.</value>
        public string Arguments
        {
            get
            {
                return _mProcess.StartInfo.Arguments;
            }
            set
            {
                _mProcess.StartInfo.Arguments = value;
            }
        }

        /// <summary>
        /// Sets the arguments.
        /// </summary>
        /// <param name="args">The args.</param>
        public void SetArguments(params string[] args)
        {
            if (args != null)
            {
                StringBuilder sb = GetMangledArguments(args);
                Arguments = sb.ToString();
            }
        }

        /// <summary>
        /// Gets the mangled arguments.
        /// </summary>
        /// <param name="args">The args.</param>
        /// <returns></returns>
        private static StringBuilder GetMangledArguments(IEnumerable<string> args)
        {
            var sb = new StringBuilder();
            var c = 0;
            foreach (var val in args)
            {
                if (c > 0)
                {
                    sb.Append(' ');
                }
                bool containsSpace = (val.IndexOf(' ') != -1);
                if (containsSpace)
                {
                    sb.Append('\"');
                }
                sb.Append(val);
                if (containsSpace)
                {
                    sb.Append('\"');
                }
                c++;
            }
            return sb;
        }

        /// <summary>
        /// Adds the arguments.
        /// </summary>
        /// <param name="args">The args.</param>
        public void AddArguments(params string[] args)
        {
            if (args != null)
            {
                StringBuilder sb = GetMangledArguments(args);
                if (!string.IsNullOrEmpty(Arguments))
                {
                    sb.Insert(0, ' ');
                    sb.Insert(0, Arguments);
                }
                Arguments = sb.ToString();
            }
        }

        /// <summary>
        /// Handles the OutputDataReceived event of the process control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataReceivedEventArgs"/> instance containing the event data.</param>
        protected virtual void process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                _mOut.WriteLine(e.Data);
            }
        }

        /// <summary>
        /// Handles the ErrorDataReceived event of the process control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataReceivedEventArgs"/> instance containing the event data.</param>
        protected virtual void process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                _mError.WriteLine(e.Data);
            }
        }

        /// <summary>
        /// Gets or sets the process.
        /// </summary>
        /// <value>The process.</value>
        public Process Process
        {
            get
            {
                return _mProcess;
            }
            set
            {
                _mProcess = value;
            }
        }

        /// <summary>
        /// Gets the start info.
        /// </summary>
        /// <value>The start info.</value>
        public ProcessStartInfo StartInfo
        {
            get
            {
                return _mProcess.StartInfo;
            }
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Start()
        {
            Start(true);
        }

        /// <summary>
        /// Starts the process, begins asynchronous reads on
        /// both standard output and standard error.
        /// </summary>
        /// <param name="useRedirect">if set to <c>true</c> [use redirect].</param>
        public void Start(bool useRedirect)
        {
            // Initialize the asynchronous stuff
            _mProcess.StartInfo.UseShellExecute = false;
            if (useRedirect)
            {
                _mProcess.StartInfo.RedirectStandardError = true;
                _mProcess.StartInfo.RedirectStandardOutput = true;
            }
            _mProcess.StartInfo.CreateNoWindow = true;

            _mProcess.Start();

            if (useRedirect)
            {
                _mProcess.BeginErrorReadLine();
                _mProcess.BeginOutputReadLine();
            }
        }

        /// <summary>
        /// Starts with a timeout of 
        /// milliseconds and does not throw an exception when it sees an error, but returns
        /// the standard error and output.
        /// </summary>
        /// <returns></returns>
        public int StartAndWaitForExit()
        {
            return StartAndWaitForExit(false);
        }

        /// <summary>
        /// Starts the and wait for exit.
        /// </summary>
        /// <param name="timeoutMs">The timeout ms.</param>
        /// <returns></returns>
        public int StartAndWaitForExit(int timeoutMs)
        {
            return StartAndWaitForExit(timeoutMs, false);
        }

        /// <summary>
        /// Starts the process, begins asynchronous reads on
        /// both standard output and standard error, and
        /// waits for the process to exit. The return code
        /// of the process is returned.
        /// </summary>
        /// <param name="timeoutMs">The timeout ms.</param>
        /// <param name="throwOnError">if set to <c>true</c> [throw on error].</param>
        /// <returns>Return code of completed process</returns>
        public int StartAndWaitForExit(int timeoutMs, bool throwOnError)
        {
            Start();

            _mProcess.WaitForExit(timeoutMs);

            int exit = _mProcess.ExitCode;

            if (throwOnError)
            {
                CheckForError(true);
            }

            return exit;
        }

        /// <summary>
        /// Starts the process, begins asynchronous reads on
        /// both standard output and standard error, and
        /// waits for the process to exit. The return code
        /// of the process is returned.
        /// </summary>
        /// <param name="throwOnError">if set to <c>true</c> [throw on error].</param>
        /// <returns>Return code of completed process</returns>
        public int StartAndWaitForExit(bool throwOnError)
        {
            return StartAndWaitForExit(60000, throwOnError);
        }

        /// <summary>
        /// Gets or sets the error.
        /// </summary>
        /// <value>The error.</value>
        public TextWriter Error
        {
            get
            {
                return _mError;
            }
            set
            {
                _mError = value;
            }
        }

        /// <summary>
        /// Gets or sets the out.
        /// </summary>
        /// <value>The out.</value>
        public TextWriter Out
        {
            get
            {
                return _mOut;
            }
            set
            {
                _mOut = value;
            }
        }

        /// <summary>
        /// Gets the exit code.
        /// </summary>
        /// <value>The exit code.</value>
        public int ExitCode
        {
            get
            {
                return _mProcess.ExitCode;
            }
        }

        /// <summary>
        /// Gets the standard output.
        /// </summary>
        /// <value>The standard output.</value>
        public string StandardOutput
        {
            get
            {
                return _mOutBuilder == null ? null : _mOutBuilder.ToString();
            }
        }

        /// <summary>
        /// Gets the standard error.
        /// </summary>
        /// <value>The standard error.</value>
        public string StandardError
        {
            get
            {
                return _mErrorBuilder == null ? null : _mErrorBuilder.ToString();
            }
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override string ToString()
        {
            if (!string.IsNullOrEmpty(FileName))
            {
                string result = FileName;
                if (result.IndexOf(' ') != -1)
                {
                    result = "\"" + result + "\"";
                }

                if (!string.IsNullOrEmpty(Arguments))
                {
                    result += " " + Arguments;
                }
                return result;
            }
            return base.ToString();
        }

        /// <summary>
        /// Checks for error.
        /// </summary>
        /// <returns></returns>
        public string CheckForError()
        {
            return CheckForError(true);
        }

        /// <summary>
        /// Checks for error.
        /// </summary>
        /// <param name="throwOnError">if set to <c>true</c> [throw on error].</param>
        /// <returns></returns>
        public string CheckForError(bool throwOnError)
        {
            string error = null;
            if (ExitCode != 0)
            {
                error = "" + StandardError + "\n" + StandardOutput;
            }
            else if (!string.IsNullOrEmpty(StandardError))
            {
                error = StandardError + "\n" + StandardOutput;
            }

            if (throwOnError && error != null)
            {
                throw new Exception(error);
            }

            return error;
        }


        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
                if(_mProcess != null)_mProcess.Close();
                if (_mError != null) _mError.Close();
                if (_mOut != null) _mOut.Close();
            }
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


    }
}



/*  Sample code


        private static string StartProcessTask()
        {
            var rv = string.Empty;
            try
            {
                var notepad = Path.Combine(Environment.GetEnvironmentVariable("windir"), "notepad.exe");
                ProcessHelper p = new ProcessHelper();
                p.SetArguments(new String[] { @"C:\junk\ErrorLog.txt" });
                p.FileName = notepad;
                p.Start();
                //p.StartAndWaitForExit(600000, true);
                rv = p.StandardOutput;
            }
            catch (Exception ex)
            {                
                throw ex;
            }
            
            return rv;
        }


*/

