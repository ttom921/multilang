using ClosedXML.Excel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class Program
    {
        public class ResxStrings
        {
            public string Key;
            public string Comment;
            public Dictionary<string, string> Strings = new Dictionary<string, string>();
        }
        static void Main(string[] args)
        {
            string path = @".";
            string pattern = "message";
            //ConvExcelToResx(@"FixedMessage.xlsx");
           var retdata= ReadExcelData(@"FixedMessage.xlsx");
            GenResfile(retdata.Item1, retdata.Item2);
            GenJsonFile(retdata.Item1, retdata.Item2);
        }

        private static void GenJsonFile(List<ResxStrings> list, List<string> langs)
        {
            string path = System.IO.Directory.GetCurrentDirectory();
            path = Path.Combine(path, @"data\jsondata");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            else
            {
                Directory.Delete(path, true);
                Directory.CreateDirectory(path);
            }
            
            foreach (string lang in langs)
            {
                string resxPath = Path.Combine(path, (lang != "DEFAULT" ? lang : string.Empty) + ".json");
                Dictionary<string, string> dclang = new Dictionary<string, string>();
                foreach (var data in list)
                {
                    dclang.Add(data.Key, data.Strings[lang]);
                    
                }
                string json = JsonConvert.SerializeObject(dclang, Formatting.Indented);
                //write string to file
                System.IO.File.WriteAllText(resxPath, json);
            }
        }

        private static void  GenResfile(List<ResxStrings> list, List<string> langs)
        {
            string path = System.IO.Directory.GetCurrentDirectory();
            path = Path.Combine(path, @"data\resdata");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            else
            {
                Directory.Delete(path, true);
                Directory.CreateDirectory(path);
            }
            string pattern = "Resources";//Path.GetFileNameWithoutExtension(xlsxPath);
            foreach (string lang in langs)
            {
                string resxPath = Path.Combine(path,
                    pattern + (lang != "DEFAULT" ? "." + lang : string.Empty) + ".resx");
                using (ResXResourceWriter rsxw = new ResXResourceWriter(resxPath))
                {
                    foreach (var data in list)
                    {
                        ResXDataNode node = new ResXDataNode(data.Key, data.Strings[lang]);
                        node.Comment = data.Comment;
                        rsxw.AddResource(node);
                    }
                    rsxw.Generate();
                    rsxw.Close();
                }
            }
        }
        

        
        private static Tuple<List<ResxStrings> , List<string>> ReadExcelData(string xlsxPath)
        {
            var list = new List<ResxStrings>();
            var langs = new List<string>();
            using (XLWorkbook wb = new XLWorkbook(xlsxPath))
            {
                var sht = wb.Worksheets.First();
                int col = 3;
                while (!sht.Cell(1, col).IsEmpty())
                {
                    langs.Add(sht.Cell(1, col).Value.ToString());
                    col++;
                }
                int row = 2;
                while (!sht.Cell(row, 1).IsEmpty())
                {
                    ResxStrings data = new ResxStrings()
                    {
                        Key = sht.Cell(row, 1).Value.ToString(),
                        Comment = sht.Cell(row, 2).Value.ToString()
                    };
                    for (int i = 0; i < langs.Count; i++)
                        data.Strings.Add(langs[i],
                            sht.Cell(row, i + 3).Value.ToString());
                    list.Add(data);
                    row++;
                }
            }
            return Tuple.Create(list, langs);
        }
        //REF: http://msdn.microsoft.com/en-us/library/system.resources.resxdatanode.aspx
        private static void ConvExcelToResx(string xlsxPath)
        {
            var list = new List<ResxStrings>();
            var langs = new List<string>();
            using (XLWorkbook wb = new XLWorkbook(xlsxPath))
            {
                var sht = wb.Worksheets.First();
                int col = 3;
                while (!sht.Cell(1, col).IsEmpty())
                {
                    langs.Add(sht.Cell(1, col).Value.ToString());
                    col++;
                }
                int row = 2;
                while (!sht.Cell(row, 1).IsEmpty())
                {
                    ResxStrings data = new ResxStrings()
                    {
                        Key = sht.Cell(row, 1).Value.ToString(),
                        Comment = sht.Cell(row, 2).Value.ToString()
                    };
                    for (int i = 0; i < langs.Count; i++)
                        data.Strings.Add(langs[i],
                            sht.Cell(row, i + 3).Value.ToString());
                    list.Add(data);
                    row++;
                }
            }
            //Gen resx
            //string path = Path.GetDirectoryName(xlsxPath);
            //if (!Directory.Exists(path))
            //{
            //    Directory.CreateDirectory(path);
            //}
            string path = System.IO.Directory.GetCurrentDirectory();
            path = Path.Combine(path, @"data\resdata");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            else
            {
                Directory.Delete(path, true);
                Directory.CreateDirectory(path);
            }
            string pattern = "Resources";//Path.GetFileNameWithoutExtension(xlsxPath);
            foreach (string lang in langs)
            {
                string resxPath = Path.Combine(path,
                    pattern + (lang != "DEFAULT" ? "." + lang : string.Empty) + ".resx");
                using (ResXResourceWriter rsxw = new ResXResourceWriter(resxPath))
                {
                    foreach (var data in list)
                    {
                        ResXDataNode node = new ResXDataNode(data.Key, data.Strings[lang]);
                        node.Comment = data.Comment;
                        rsxw.AddResource(node);
                    }
                    rsxw.Generate();
                    rsxw.Close();
                }
            }

        }
    }
}
