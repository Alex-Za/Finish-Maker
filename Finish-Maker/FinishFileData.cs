using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finish_Maker
{
    static class FinishFileData
    {
        static FinishFileData()
        {
            FileData = new List<List<string>>();
        }
        public static List<List<string>> FileData { get; set; }
        public static bool ChooseCategoryStatus { get; set; }
        public static string FileDataError { get; set; }
    }
}
