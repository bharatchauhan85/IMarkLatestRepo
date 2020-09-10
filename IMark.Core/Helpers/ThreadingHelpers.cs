﻿using System;
using Xamarin.Forms;

namespace IMark.Core.Helpers
{
    public static class ThreadingHelpers
    {
        public static void InvokeOnMainThread(Action action)
        {
            if (Device.RuntimePlatform == "Test")
            {
                action?.Invoke();
            }
            else
                Device.BeginInvokeOnMainThread(action);
        }
    }
}
