﻿using System.Windows;
using System.Text;
using System;

namespace Procurement
{
    public partial class App : Application
    {
        public App()
        {
            Application.Current.DispatcherUnhandledException += new System.Windows.Threading.DispatcherUnhandledExceptionEventHandler(Current_DispatcherUnhandledException);
        }

        void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            System.IO.File.AppendAllText("DebugInfo.log", getEnvironementDetails());
            System.IO.File.AppendAllText("DebugInfo.log", e.Exception.ToString());
            MessageBox.Show("There was an unhandled error - Sorry! Please send the debuginfo.log to one of us devs :)");
        }

        private string getEnvironementDetails()
        {
            StringBuilder builder = new StringBuilder();
            try
            {
                builder.AppendLine("CurrentCulture: " + System.Threading.Thread.CurrentThread.CurrentCulture.ToString());
                builder.AppendLine("CurrentUICulture: " + System.Threading.Thread.CurrentThread.CurrentUICulture.ToString());
            }
            catch (Exception ex)
            {
                builder.AppendLine(ex.ToString());
            }

            return builder.ToString();
        }
    }
}