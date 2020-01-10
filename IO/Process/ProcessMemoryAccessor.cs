using System;
using System.ComponentModel;
using System.IO;

using SleepHunter.Win32;

namespace SleepHunter.IO.Process
{
    public sealed class ProcessMemoryAccessor : IDisposable
    {
        bool isDisposed;

        public int ProcessId { get; }

        public IntPtr ProcessHandle { get; private set; }

        public ProcessAccess Access { get; }

        public ProcessMemoryAccessor(int processId, ProcessAccess access = ProcessAccess.ReadWrite)
        {
            this.ProcessId = processId;
            this.ProcessHandle = NativeMethods.OpenProcess(access.ToWin32Flags(), false, processId);

            if (ProcessHandle == IntPtr.Zero)
            {
                var error = NativeMethods.GetLastError();
                throw new Win32Exception(error, "Unable to open process.");
            }

            this.Access = access;
        }

        public Stream GetStream()
        {
            return new ProcessMemoryStream(ProcessHandle, Access, leaveOpen: true);
        }

        #region IDisposable Methods
        ~ProcessMemoryAccessor()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        void Dispose(bool isDisposing)
        {
            if (isDisposed)
                return;

            if (isDisposing)
            {
            }

            if (ProcessHandle != IntPtr.Zero)
                NativeMethods.CloseHandle(ProcessHandle);

            ProcessHandle = IntPtr.Zero;
            isDisposed = true;
        }
        #endregion
    }
}
