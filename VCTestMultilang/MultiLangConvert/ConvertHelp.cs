using ClosedXML.Excel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MultiLangConvert
{
    public class ConvertHelp
    {
        #region Singleton
        private static readonly Lazy<ConvertHelp> lazy =
        new Lazy<ConvertHelp>(() => new ConvertHelp());
        public static ConvertHelp Instance { get { return lazy.Value; } }
        public ConvertHelp()
        {
            
        }
        #endregion Singleton
        /// <summary>
        /// 產生Json的語言檔
        /// </summary>
        /// <param name="list"></param>
        /// <param name="langs"></param>
        public  void GenJsonFile(List<ResxStrings> list, List<string> langs)
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
                    try
                    {
                        dclang.Add(data.Key, data.Strings[lang]);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message+" key=>" + data.Key + " data=>"+ data.Strings[lang]);
                        throw ex;
                    }
                }
                string json = JsonConvert.SerializeObject(dclang, Formatting.Indented);
                //write string to file
                System.IO.File.WriteAllText(resxPath, json);
            }
        }

        /// <summary>
        /// 產生語言資源檔
        /// </summary>
        /// <param name="list"></param>
        /// <param name="langs"></param>
        public void GenResfile(List<ResxStrings> list, List<string> langs,string pattern= "Resources",bool isCore=false)
        {
            string path = System.IO.Directory.GetCurrentDirectory();
            if(isCore==true)
            {
                path = Path.Combine(path, @"data\resdata\core");
            }
            else
            {
                path = Path.Combine(path, @"data\resdata\mvc");
            }
            
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
                string resxPath = "";
                string mylang = maptowindlang(lang);
                resxPath = Path.Combine(path, pattern + (mylang != "DEFAULT" ? "." + mylang : string.Empty) + ".resx");
                //if (isCore)
                //{
                //    resxPath = Path.Combine(path, pattern + (lang != "DEFAULT" ? "." + lang : string.Empty) + ".resx");
                //}
                //else
                //{
                   
                //    //resxPath = Path.Combine(path, pattern + (lang != "DEFAULT" ? "." + lang : string.Empty) + ".resx");
                //}
               
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
        string maptowindlang(string orglang)
        {
            string mylang = orglang;
            if (mylang == "en-US")
            {
                mylang = "en-us";
            }
            if (mylang == "zh-Hant-TW")
            {
                mylang = "zh-tw";
            }
            if (mylang == "zh-Hans-CN")
            {
                mylang = "zh-cn";
            }
            return mylang;
        }
        public Tuple<List<ResxStrings>, List<string>> ReadExcelData(string xlsxPath)
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
    }
}
