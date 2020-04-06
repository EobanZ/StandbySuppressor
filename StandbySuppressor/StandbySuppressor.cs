using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StandbySuppressor
{
    public class StandbySuppressor
    {

        #region ApiStuff
        //Main Source: https://decatec.de/programmierung/c-sharp-windows-standby-unterdruecken/

        private const int POWER_REQUEST_CONTEXT_VERSION = 0;
        private const int POWER_REQUEST_CONTEXT_SIMPLE_STRING = 0x1;
        private const int POWER_REQUEST_CONTEXT_DETAILED_STRING = 0x2;

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern IntPtr PowerCreateRequest(ref POWER_REQUEST_CONTEXT Context);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool PowerSetRequest(IntPtr PowerRequestHandle, PowerRequestType RequestType);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool PowerClearRequest(IntPtr PowerRequestHandle, PowerRequestType RequestType);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct POWER_REQUEST_CONTEXT
        {
            public UInt32 Version;
            public UInt32 Flags;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string SimpleReasonString;
        }

        internal enum PowerRequestType
        {
            PowerRequestDisplayRequired = 0,
            PowerRequestSystemRequired,
            PowerRequestAwayModeRequired,
            PowerRequestExecutionRequired
        }

        // Check if power avaiability requests are supported on the current platform.
        [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        internal static extern IntPtr LoadLibrary(string dllToLoad);

        private const uint ES_SYSTEM_REQUIRED = 0x00000001;
        private const uint ES_DISPLAY_REQUIRED = 0x00000002;
        private const uint ES_USER_PRESENT = 0x00000004; // Only supported by Windows XP/Windows Server 2003
        private const uint ES_AWAYMODE_REQUIRED = 0x00000040; // Not supported by Windows XP/Windows Server 2003
        private const uint ES_CONTINUOUS = 0x80000000;

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern uint SetThreadExecutionState(uint esFlags);

        #endregion
        public static bool isStandbySuppressed = false;
        public static SuppresionMode usedMethod;
        public static bool keepScreenOn = false;

        private static IntPtr currentPowerRequest;
        private static Thread MouseMoverThread;

        public enum SuppresionMode
        {
            PowerAvailabilityRequests,
            SetThreadExecutionState,
            MouseMover
        }


        public static bool DisableStandby(SuppresionMode mode = SuppresionMode.PowerAvailabilityRequests, bool keepScreenEnabled = false)
        {
            keepScreenOn = keepScreenEnabled;
            bool result = false;
            switch (mode)
            {
                case SuppresionMode.PowerAvailabilityRequests:
                    if (PowerAvailabilityRequestsSupported())
                        result = StandbySuppressor.DisableStandbyPowerAvailability();
                    break;
                case SuppresionMode.SetThreadExecutionState:
                    result = StandbySuppressor.DisableStandbySetThreadExec();
                    break;
                case SuppresionMode.MouseMover:
                    result = StandbySuppressor.DisableStandbyMoveMouse();
                    break;
            }

            if (result == true)
                usedMethod = mode;
            else
                MessageBox.Show("Error disableing standby", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            return result;
        }

        public static bool EnableStandby()
        {
            bool result = false;

            if (isStandbySuppressed != true)
                return result;

            switch (usedMethod)
            {
                case SuppresionMode.PowerAvailabilityRequests:
                    result = EnableStandbyPowerAvailablity();
                    break;
                case SuppresionMode.SetThreadExecutionState:
                    result = EnableStandbySetThreadExec();
                    break;
                case SuppresionMode.MouseMover:
                    result = EnableStandbyMoveMouse();
                    break;
            }
            return result;
        }

        private static bool DisableStandbyPowerAvailability()
        {
            // Clear current power request if there is any.
            if (currentPowerRequest != IntPtr.Zero)
            {
                PowerClearRequest(currentPowerRequest, PowerRequestType.PowerRequestSystemRequired);
                currentPowerRequest = IntPtr.Zero;
            }

            // Create new power request.
            POWER_REQUEST_CONTEXT pContext;
            pContext.Flags = POWER_REQUEST_CONTEXT_SIMPLE_STRING;
            pContext.Version = POWER_REQUEST_CONTEXT_VERSION;
            // This is the reason for standby suppression. It is shown when the command "powercfg -requests" is executed.
            pContext.SimpleReasonString = "Standby suppressed by StandbySuppressor.exe";

            currentPowerRequest = PowerCreateRequest(ref pContext);

            if (currentPowerRequest == IntPtr.Zero)
            {
                // Failed to create power request.
                var error = Marshal.GetLastWin32Error();

                if (error != 0)
                    throw new Win32Exception(error);
            }

            bool success = PowerSetRequest(currentPowerRequest, PowerRequestType.PowerRequestSystemRequired);
            if (keepScreenOn)
                success = PowerSetRequest(currentPowerRequest, PowerRequestType.PowerRequestDisplayRequired);

            if (!success)
            {
                // Failed to set power request.
                currentPowerRequest = IntPtr.Zero;
                var error = Marshal.GetLastWin32Error();

                if (error != 0)
                    throw new Win32Exception(error);
            }

            return success;
        }

        private static bool DisableStandbySetThreadExec()
        {

            uint flags = keepScreenOn ? ES_CONTINUOUS | ES_SYSTEM_REQUIRED | ES_DISPLAY_REQUIRED : ES_CONTINUOUS | ES_SYSTEM_REQUIRED;

            uint success = SetThreadExecutionState(flags);


            if (success == 0)
            {
                // Failed to suppress standby
                var error = Marshal.GetLastWin32Error();

                if (error != 0)
                    throw new Win32Exception(error);
            }

            return success != 0 ? true : false;
        }

        private static bool DisableStandbyMoveMouse()
        {
            if(MouseMoverThread == null)
                MouseMoverThread = new Thread(MouseMoverThreadFunction);

            if (MouseMoverThread.IsAlive)
                MouseMoverThread.Abort();

            MouseMoverThread.IsBackground = true;

            MouseMoverThread.Start();

            return true;
        }

        private static bool EnableStandbyPowerAvailablity()
        {
            // Only try to clear power request if any power request is set.
            if (currentPowerRequest != IntPtr.Zero)
            {
                var success = PowerClearRequest(currentPowerRequest, PowerRequestType.PowerRequestSystemRequired);
                if (keepScreenOn)
                    success = PowerClearRequest(currentPowerRequest, PowerRequestType.PowerRequestDisplayRequired);

                if (!success)
                {
                    // Failed to clear power request.
                    currentPowerRequest = IntPtr.Zero;
                    var error = Marshal.GetLastWin32Error();

                    if (error != 0)
                        throw new Win32Exception(error);
                }
                else
                {
                    currentPowerRequest = IntPtr.Zero;
                }
                return success;
            }
            else
                return false;
        }

        private static bool EnableStandbySetThreadExec()
        {
            var success = SetThreadExecutionState(ES_CONTINUOUS);

            if (success == 0)
            {
                // Failed to enable standby
                var error = Marshal.GetLastWin32Error();

                if (error != 0)
                    throw new Win32Exception(error);
            }

            return success != 0 ? true : false;
        }

        private static bool EnableStandbyMoveMouse()
        {
            if (MouseMoverThread == null)
                return false;

            if (MouseMoverThread.IsAlive)
                MouseMoverThread.Abort();
            else
                return false;
                

            return true;
        }

        private static bool PowerAvailabilityRequestsSupported()
        {
            var ptr = LoadLibrary("kernel32.dll");
            var ptr2 = GetProcAddress(ptr, "PowerSetRequest");

            if (ptr2 == IntPtr.Zero)
            {
                // Power availability requests NOT suppoted.  
                MessageBox.Show("PowerAvailability Method not supported on your system", "Method not supportet", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else
            {
                // Power availability requests ARE suppoted.                
                return true;
            }
        }

        private static void MouseMoverThreadFunction()
        {
            while(true)
            {
                InputSimulator.MoveMouse(2, 0);
                Thread.Sleep(20000);
                InputSimulator.MoveMouse(-2, 0);
                Thread.Sleep(20000);
            }
        }
    }
}
