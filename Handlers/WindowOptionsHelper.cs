﻿using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace WorkLifeBalance.HandlerClasses
{
    public class WindowOptionsHelper
    {
        // Constants for the SetWindowLoc functions
        public const uint SWP_NOSIZE = 0x0001;
        public const uint SWP_NOZORDER = 0x0004;

        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string? lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();


        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);


        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);


        [DllImport("psapi.dll")]
        private static extern uint GetModuleFileNameEx(IntPtr hProcess, IntPtr hModule, StringBuilder lpBaseName, int nSize);


        public static void SetWindowLocation(IntPtr windowHandle, int x, int y)
        {
            // Call SetWindowPos with the SWP_NOSIZE and SWP_NOZORDER flags
            SetWindowPos(windowHandle, IntPtr.Zero, x, y, 0, 0, SWP_NOSIZE | SWP_NOZORDER);
        }

        public static IntPtr GetWindow(string? lpClassName, string lpWindowName)
        {
            return FindWindow(lpClassName, lpWindowName);
        }

        public static string GetWindowTitle(IntPtr hWnd)
        {
            const int nChars = 256;
            StringBuilder windowTitle = new StringBuilder(nChars);
            GetWindowText(hWnd, windowTitle, nChars);
            return windowTitle.ToString();
        }

        public static string GetApplicationName(IntPtr hWnd)
        {
            GetWindowThreadProcessId(hWnd, out uint processId);
            Process process = Process.GetProcessById((int)processId);

            const int nChars = 1024;
            StringBuilder applicationName = new StringBuilder(nChars);
            GetModuleFileNameEx(process.Handle, IntPtr.Zero, applicationName, nChars);

            // Get only the executable file name
            string fileName = System.IO.Path.GetFileName(applicationName.ToString());

            return fileName;
        }
    }
}