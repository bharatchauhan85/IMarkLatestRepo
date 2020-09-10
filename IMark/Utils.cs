using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace IMark
{
    public class Utils
    {
        public static SKTypeface GetTypeface(string fullFontName)
        {
            SKTypeface result;

            var assembly = Assembly.GetExecutingAssembly();
            var stream = assembly.GetManifestResourceStream("IMark.Resources.Fonts." + fullFontName);
            if (stream == null)
                return null;

            result = SKTypeface.FromStream(stream);
            return result;
        }
    }
}
