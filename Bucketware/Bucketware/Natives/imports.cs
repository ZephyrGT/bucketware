using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Bucketware.Natives
{
    class imports
    {
        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(System.Windows.Forms.Keys vKey);

        public static IntPtr handle = FindWindow(null, BWare.WINDOW_NAME);

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string IpClassName, string IpWindowName);

        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]

        public static extern bool GetWindowRect(IntPtr hwnd, out RECT IpRect);

        public static RECT rect;

        public struct RECT
        {
            public int left, top, right, bottom;
        }
		[DllImport("USER32.DLL")]
		public static extern bool SetForegroundWindow(IntPtr hWnd);
		[DllImport("ntdll.dll")]
		// NtQuerySystemInformation gives us data about all the handlers in the system
		private static extern uint NtQuerySystemInformation(uint SystemInformationClass, IntPtr SystemInformation,
			int SystemInformationLength, ref int nLength);

		[DllImport("kernel32.dll")]
		// dwDesiredAccess sets the process access rights (docs.microsoft.com/en-us/windows/desktop/ProcThread/process-security-and-access-rights)
		// if bInheritHandle is true, processes created by this process will inherit the handle (we don't need this, maybe just set it as a bool)
		// dwProcessId is the PID of the process we want to open with those access rights
		public static extern IntPtr OpenProcess(uint dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, int dwProcessId);

		[DllImport("kernel32.dll")]
		// Returns us a handle to the current process
		public static extern IntPtr GetCurrentProcess();

		[DllImport("kernel32.dll")]
		// Closes a handle
		public static extern int CloseHandle(IntPtr hObject);

		[DllImport("ntdll.dll")]
		// Retrieves information about an object
		// Handle is the object's handle we're getting information from
		// ObjectInformationClass is the type of information we want; ObjectBasicInformation/ObjectTypeInformation, undocumented ObjectNameInformation?
		// ObjectInformation is the buffer where the data is returned to, ObjectInformationLength is the size of that buffer
		// returnLength is a variable where NtQueryObject writes the size of the information returned to us
		public static extern int NtQueryObject(IntPtr Handle, int ObjectInformationClass, IntPtr ObjectInformation,
			int ObjectInformationLength, ref int returnLength);

		[DllImport("kernel32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		// DuplicateHandle duplicates a handle from an external process to ours
		// hSourceProcessHandle is the process we duplicate from, hSourceHandle is the handle we duplicate
		// hTargetProcessHandle is the process we duplicate to, lpTargetHandle is a pointer to a var that receives the new handler
		private static extern bool DuplicateHandle(IntPtr hSourceProcessHandle, ushort hSourceHandle, IntPtr hTargetProcessHandle,
			out IntPtr lpTargetHandle, uint dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, uint dwOptions);

		[DllImport("kernel32.dll")]
		// Access rights, inheritance bool, ID of thread
		static extern IntPtr OpenThread(int dwDesiredAccess, bool bInheritHandle, uint dwThreadId);

		[DllImport("kernel32.dll")]
		// Thread to suspend
		static extern uint SuspendThread(IntPtr hThread);

		[DllImport("kernel32.dll")]
		// Thread to resume
		static extern int ResumeThread(IntPtr hThread);

		[DllImport("user32.dll")]
		// Used for sending keystrokes to new window
		public static extern IntPtr PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll")]
		static extern int SetWindowText(IntPtr hWnd, string text);
	}
}
