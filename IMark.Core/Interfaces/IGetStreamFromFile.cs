using System;
using System.IO;

namespace IMark.Core.Interfaces
{
    public interface IGetStreamFromFile
    {
        Stream LoadFromFile(string filename);
        byte[] LoadBytesFromFile(string filePath);
    }
}
