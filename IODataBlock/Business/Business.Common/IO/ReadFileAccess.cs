using System;
using System.IO;
using System.Runtime.Serialization;
using System.Threading;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Business.Common.IO
{
    public class ReadFileAccess : IDisposable
    {
        #region Class Initialization

        public ReadFileAccess(string filePath)
            : this(new FileInfo(filePath), 180000, TimeSpan.FromSeconds(30))
        {
        }

        public ReadFileAccess(string filePath, TimeSpan lockDuration)
            : this(new FileInfo(filePath), 180000, lockDuration)
        {
        }

        public ReadFileAccess(string filePath, int lockWaitMs, TimeSpan lockDuration)
            : this(new FileInfo(filePath), lockWaitMs, lockDuration)
        {
        }

        public ReadFileAccess(FileInfo fi)
            : this(fi, 180000, TimeSpan.FromSeconds(30))
        {
        }

        public ReadFileAccess(FileInfo fi, TimeSpan lockDuration)
            : this(fi, 180000, lockDuration)
        {
        }

        public ReadFileAccess(FileInfo fi, int lockWaitMs, TimeSpan lockDuration)
        {
            _file = fi;
            _file.Refresh();
            //if (!FileExists()) throw new FileNotFoundException();
            _templockfile = new FileInfo(_file.FullName + ".tlock");
            DefaultLockWaitMs = lockWaitMs;
            DefaultLockDuration = lockDuration;

            AutoEscalateLock = true;  // for now always escalate if possible.

            if (fi.Directory != null && fi.Directory.Exists) IsAccessible = GetLockAccess(DefaultLockWaitMs, lockDuration);
            else if (fi.Directory != null)
                throw new DirectoryNotFoundException($@"DirectoryNotFoundException: {fi.Directory.FullName} not found!");
            else
                throw new DirectoryNotFoundException(@"DirectoryNotFoundException: No directory found!");
        }

        #endregion Class Initialization

        #region Fields and Properties

        private readonly FileInfo _file;

        private readonly FileInfo _templockfile;

        public Timer LockUpdateTimer;

        public int DefaultLockWaitMs { get; set; }

        public TimeSpan DefaultLockDuration { get; set; }

        public bool UpdatingLock { get; set; }

        public bool IsAccessible { get; set; }

        public bool IsLockExpired { get; set; }

        public bool AutoEscalateLock { get; set; }

        public bool LockAcquired { get; set; }



        #endregion Fields and Properties

        #region Public Methods

        public FileStream OpenReadFileStream(int bufferSize = 4096, bool lockStream = true)
        {
            var lockWaitUntil = DateTime.Now.AddMilliseconds(DefaultLockWaitMs);
            while (true)
            {
                try
                {
                    var stream = new FileStream(_file.FullName, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize);
                    if (lockStream) stream.Lock(0, stream.Length);
                    return stream;
                }
                catch (IOException ex)
                {
                    if (!ex.Message.Contains(@"The process cannot access the file") || DateTime.Now >= lockWaitUntil) throw;
                    Thread.Sleep(500);
                }
            }
        }

        public bool TempLockExists()
        {
            _templockfile.Refresh();
            return _templockfile.Exists;
        }

        public bool FileExists()
        {
            _file.Refresh();
            return _file.Exists;
        }

        public void StartLockUpdateTimer()
        {
            if (!(DefaultLockDuration.TotalSeconds >= 30)) return;
            LockUpdateTimer = new Timer(DefaultLockDuration.TotalMilliseconds - 5000);
            LockUpdateTimer.Elapsed += OnLockUpdateTimer;
            LockUpdateTimer.Enabled = true;
        }

        public bool ClearExpiredTempLock(int lockWaitMs)
        {
            if (!IsAccessible && !TempLockExists()) return false;
            var lockFileRemoved = false;

            var lockWaitUntil = DateTime.Now.AddMilliseconds(lockWaitMs);
            while (true)
            {
                Mutex mutex = null;
                var hasMutex = false;
                try
                {
                    bool createdNew;
                    mutex = new Mutex(false, @"Global\" + _file.FullName.GetHashCode(), out createdNew);
                    try
                    {
                        hasMutex = mutex.WaitOne(lockWaitMs, true);
                        if (!hasMutex) return false;
                    }
                    catch (AbandonedMutexException)
                    {
                        // Log the fact the mutex was abandoned in another process, it will still
                        // get acquired.
                    }

                    try
                    {
                        _templockfile.Refresh();
                        if (_templockfile.Exists)
                        {
                            var tlockinfo = new TempLockInfo();
                            var dc = new DataContractSerializer(typeof(TempLockInfo));
                            using (var fs = new FileStream(_templockfile.FullName, FileMode.Open, FileAccess.Read, FileShare.Read | FileShare.Delete))
                            {
                                try
                                {
                                    tlockinfo = (TempLockInfo)dc.ReadObject(fs);
                                }
                                catch (Exception)
                                {
                                    IsLockExpired = true;
                                    _templockfile.Delete();
                                    lockFileRemoved = true;
                                }
                            }
                            if (tlockinfo.ExpirationDate <= DateTime.Now)
                            {
                                IsLockExpired = true;
                                _templockfile.Delete();
                                lockFileRemoved = true;
                            }
                            break;
                        }
                    }
                    catch (IOException ex)
                    {
                        if (!ex.Message.Contains(@"The process cannot access the file") || DateTime.Now >= lockWaitUntil) throw;
                        Thread.Sleep(500);
                    }
                    // ReSharper disable once EmptyGeneralCatchClause
                    catch (Exception)
                    {
                        // since this is a read lock do not throw error.
                        //
                        //throw;
                    }
                }
                finally
                {
                    if (hasMutex)
                    {
                        mutex.ReleaseMutex();
                        mutex.Close();
                    }
                    mutex?.Dispose();
                }
            }
            return lockFileRemoved;
        }

        public bool GetLockAccess(int lockWaitMs, TimeSpan lockDuration)
        {
            bool isAccessible;
            var lockWaitUntil = DateTime.Now.AddMilliseconds(lockWaitMs);
            while (true)
            {
                Mutex mutex = null;
                var hasMutex = false;
                try
                {
                    bool createdNew;
                    mutex = new Mutex(false, @"Global\" + _file.FullName.GetHashCode(), out createdNew);
                    try
                    {
                        hasMutex = mutex.WaitOne(1000, true);
                        if (!hasMutex)
                        {
                            if (DateTime.Now <= lockWaitUntil) continue;
                            else isAccessible = false;
                            break;
                        }
                    }
                    catch (AbandonedMutexException)
                    {
                        // Log the fact the mutex was abandoned in another process, it will still
                        // get acquired.
                    }

                    try
                    {
                        if (TempLockExists())
                        {
                            if (!ClearExpiredTempLock(lockWaitMs) || IsLockExpired)
                            {
                                if (DateTime.Now <= lockWaitUntil)
                                {
                                    Thread.Sleep(500);
                                    continue;
                                }
                                else isAccessible = false;
                                break;
                            }
                        }
                        else if (AutoEscalateLock)
                        {
                            try
                            {
                                using (var fs = new FileStream(_templockfile.FullName, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None))
                                {
                                    var tlockinfo = new TempLockInfo { ExpirationDate = DateTime.Now.Add(lockDuration) };
                                    var dc = new DataContractSerializer(typeof(TempLockInfo));
                                    dc.WriteObject(fs, tlockinfo);
                                    fs.Flush();
                                    LockAcquired = true;
                                }
                            }
                            // ReSharper disable once EmptyGeneralCatchClause
                            catch (Exception)
                            {
                                //throw;
                            }
                        }
                        isAccessible = true;
                        break;
                    }
                    catch (IOException ex)
                    {
                        if (!ex.Message.Contains(@"The process cannot access the file") ||
                            !ex.Message.Contains(@"already exists.") ||
                            DateTime.Now >= lockWaitUntil) throw;
                        Thread.Sleep(500);
                    }
                    catch (Exception)
                    {
                        if (hasMutex)
                        {
                            mutex.ReleaseMutex();
                            hasMutex = false;
                        }
                        RemoveTempLockFile(30000);
                        throw;
                    }
                }
                finally
                {
                    if (hasMutex)
                    {
                        mutex.ReleaseMutex();
                        mutex.Close();
                    }
                    mutex?.Dispose();
                }
            }
            if (LockAcquired) StartLockUpdateTimer();
            return isAccessible;
        }

        public bool UpdateTempLockFile(int lockWaitMs, TimeSpan lockDuration)
        {
            bool lockFileCreated;
            var lockWaitUntil = DateTime.Now.AddMilliseconds(lockWaitMs);
            while (true)
            {
                Mutex mutex = null;
                var hasMutex = false;
                try
                {
                    bool createdNew;
                    mutex = new Mutex(false, @"Global\" + _file.FullName.GetHashCode(), out createdNew);
                    try
                    {
                        hasMutex = mutex.WaitOne(1000, true);
                        if (!hasMutex)
                        {
                            if (DateTime.Now <= lockWaitUntil) continue;
                            else lockFileCreated = false;
                            break;
                        }
                    }
                    catch (AbandonedMutexException)
                    {
                        // Log the fact the mutex was abandoned in another process, it will still
                        // get acquired.
                    }

                    try
                    {
                        using (var fs = new FileStream(_templockfile.FullName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
                        {
                            fs.SetLength(0);
                            fs.Seek(0, SeekOrigin.Begin);
                            var tlockinfo = new TempLockInfo { ExpirationDate = DateTime.Now.Add(lockDuration) };
                            var dc = new DataContractSerializer(typeof(TempLockInfo));
                            dc.WriteObject(fs, tlockinfo);
                            fs.Flush();
                            lockFileCreated = true;
                            Console.WriteLine("Updated Lock!");
                        }
                        break;
                    }
                    catch (IOException ex)
                    {
                        if (!ex.Message.Contains(@"The process cannot access the file") ||
                            !ex.Message.Contains(@"already exists.") ||
                            DateTime.Now >= lockWaitUntil) throw;
                        Thread.Sleep(500);
                    }
                    catch (Exception)
                    {
                        if (hasMutex)
                        {
                            mutex.ReleaseMutex();
                            hasMutex = false;
                        }
                        RemoveTempLockFile(30000);
                        throw;
                    }
                }
                finally
                {
                    if (hasMutex)
                    {
                        mutex.ReleaseMutex();
                        mutex.Close();
                    }
                    mutex?.Dispose();
                }
            }
            return lockFileCreated;
        }

        public bool RemoveTempLockFile(int lockWaitMs)
        {
            if (!TempLockExists()) return true;
            var lockFileRemoved = false;

            try
            {
                var lockWaitUntil = DateTime.Now.AddMilliseconds(lockWaitMs);
                while (true)
                {
                    Mutex mutex = null;

                    var hasMutex = false;
                    try
                    {
                        bool createdNew;
                        mutex = new Mutex(false, @"Global\" + _file.FullName.GetHashCode(), out createdNew);
                        try
                        {
                            hasMutex = mutex.WaitOne(lockWaitMs, true);
                            if (!hasMutex) return false;
                        }
                        catch (AbandonedMutexException)
                        {
                            // Log the fact the mutex was abandoned in another process, it will
                            // still get acquired.
                        }

                        try
                        {
                            _templockfile.Refresh();
                            if (_templockfile.Exists) _templockfile.Delete();
                            lockFileRemoved = true;
                            IsAccessible = false;
                            break;
                        }
                        catch (IOException ex)
                        {
                            if (!ex.Message.Contains(@"The process cannot access the file") || DateTime.Now >= lockWaitUntil) throw;
                            Thread.Sleep(500);
                        }
                    }
                    finally
                    {
                        // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                        if (hasMutex && mutex != null)
                        {
                            mutex.ReleaseMutex();
                            mutex.Close();
                        }
                        mutex?.Dispose();
                    }
                }
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch (Exception)
            {
                //throw;
            }
            return lockFileRemoved;
        }

        #endregion Public Methods

        #region Private Methods

        private void OnLockUpdateTimer(object source, ElapsedEventArgs e)
        {
            if (UpdatingLock) return;
            UpdatingLock = true;
            try
            {
                if (IsAccessible && TempLockExists()) UpdateTempLockFile(DefaultLockWaitMs, DefaultLockDuration);
            }
            finally
            {
                UpdatingLock = false;
            }
        }

        #endregion Private Methods

        #region "Disposal Section"

        // Dispose() calls Dispose(true)
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        //// NOTE: Leave out the finalizer altogether if this class doesn't
        //// own unmanaged resources itself, but leave the other methods
        //// exactly as they are.
        //~ReadFileAccess()
        //{
        //    // Finalizer calls Dispose(false)
        //    Dispose(false);
        //}

        // The bulk of the clean-up code is implemented in Dispose(bool)
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;
            try
            {
                // free managed resources
                if (LockAcquired)
                {
                    if (LockUpdateTimer != null)
                    {
                        IsAccessible = false;
                        LockUpdateTimer.Stop();
                        var waitdt = DateTime.UtcNow.AddSeconds(60);
                        while (UpdatingLock && DateTime.UtcNow <= waitdt)
                        {
                            Thread.Sleep(500);
                        }
                    }
                    RemoveTempLockFile(DefaultLockWaitMs);
                }
                LockUpdateTimer?.Close();
            }
            catch (ObjectDisposedException)
            {
                //ignore
            }
        }

        #endregion "Disposal Section"
    }
}