using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserService.Application.Interfaces
{
    public interface IFileService
    {
        string[] ReadAllLines(string filePath);
        bool FileExists(string filePath);
    }
}
