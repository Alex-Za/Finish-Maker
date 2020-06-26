using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finish_Maker.Additional_Classes
{
    class FileReader
    {
        private List<List<string>> pathes;
        private List<string> pathes2;
        public FileReader(List<List<string>> pathes)
        {
            this.pathes = pathes;
        }
        public FileReader(List<string> pathes)
        {
            pathes2 = pathes;
        }
        private List<List<string>> categoryData;
        public List<List<string>> CategoryData
        {
            get
            {
                if (categoryData == null)
                {
                    categoryData = ParseCategoryData(pathes2);
                }
                return categoryData;
            }
        }
        private List<List<string>> ParseCategoryData(List<string> pathes)
        {
            List<string> subtypeList = new List<string>();
            List<string> categoryList = new List<string>();
            
            foreach (string path in pathes)
            {
                IEnumerable<string[]> dataFromFile = GetLineFromFile(path);
                int[] categoryColumnPosition = GetCategoryColumnPosition(dataFromFile.First());

                foreach (var line in dataFromFile.Skip(1))
                {
                    int indexForSubtypeSplit = line[categoryColumnPosition[0]].IndexOf(":");
                    int indexForCategorySplit = line[categoryColumnPosition[1]].IndexOf(":");

                    if (indexForSubtypeSplit > 0)
                    {
                        string subtype = line[categoryColumnPosition[0]].Substring(0, indexForSubtypeSplit);
                        if (!subtypeList.Contains(subtype))
                        {
                            subtypeList.Add(subtype);
                        }
                    }
                    else
                    {
                        string subtype = line[categoryColumnPosition[0]];
                        if (!subtypeList.Contains(subtype))
                        {
                            subtypeList.Add(subtype);
                        }
                    }

                    if (indexForCategorySplit > 0)
                    {
                        string category = line[categoryColumnPosition[1]].Substring(0, indexForCategorySplit);
                        if (!categoryList.Contains(category))
                        {
                            categoryList.Add(category);
                        }
                    }
                    else
                    {
                        string category = line[categoryColumnPosition[1]];
                        if (!categoryList.Contains(category))
                        {
                            categoryList.Add(category);
                        }
                    }
                }

            }
            List<List<string>> categoryData = new List<List<string>>();
            categoryData.Add(subtypeList);
            categoryData.Add(categoryList);
            return categoryData;
        }

        private int[] GetCategoryColumnPosition(string[] line)
        {
            string[] columnNames = { "SubTypes", "Order" };
            int[] columnPosition = new int[2] { -1, -1 };

            for (int i = 0; i < columnNames.Length; i++)
            {
                for (int x = 0; x < line.Length; x++)
                {
                    if (columnNames[i] == line[x])
                    {
                        columnPosition[i] = x;
                        break;
                    }
                }

                if (columnPosition[i] == -1)
                {
                    FinishFileData.FileDataError += "Колонка " + columnNames[i] + " отсутствует в експорт линках." + Environment.NewLine;
                }
            }
            return columnPosition;

        }

        private IEnumerable<string[]> GetLineFromFile(string path)
        {
            using (StreamReader reader = new StreamReader(path))
                while (!reader.EndOfStream)
                    yield return reader.ReadLine().Split('|');
        }
    }
}
