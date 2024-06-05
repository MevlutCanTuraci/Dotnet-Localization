#region Imports
using System.Globalization;
using System.Text.Json;
using System.Xml;
#endregion


namespace App.Mvc.Infrastructer.Helper.Localize
{
    public class LocalizeFileHelper
    {
        private static string _assetsPath => Path.Combine(Directory.GetCurrentDirectory(), "Locates");
        private static string _resourcesPath => Path.Combine(Directory.GetCurrentDirectory(), "Resources");

        private static Dictionary<string, object>? _jsonDict;

        public static void Start()
        {
            _jsonDict = new Dictionary<string, object>();

            var files = GetTranslates();

            foreach (var f in files)
            {
                WriteResx(f);
            }

            Console.WriteLine("Bütün işlemler tamamlandı.");
        }


        static void WriteResx(string file)
        {
            // JSON dosyasını oku
            string jsonString = File.ReadAllText(file);

            // JSON'u bir Dictionary<string, object> olarak deserialize et
            //var jsonDict = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonString);
            _jsonDict = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonString)!;

            // RESX dosyası için bir XmlDocument oluştur
            var resxDoc = new XmlDocument();
            resxDoc.AppendChild(resxDoc.CreateXmlDeclaration("1.0", "utf-8", null));
            var rootElement = resxDoc.CreateElement("root");
            resxDoc.AppendChild(rootElement);

            AddVisualStudioHeaders(resxDoc, rootElement);

            // JSON'u dönüştür ve RESX dosyasına ekle
            foreach (var item in _jsonDict)
            {
                if (item.Key.Contains("=Comments", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (IsGroup(item))
                {
                    ReturnAllGroup(item, resxDoc, rootElement);
                }
                else
                {
                    string commentKey = item.Key + "=Comments";
                    var commentValue = _jsonDict.FirstOrDefault(w => w.Key.Equals(commentKey, StringComparison.OrdinalIgnoreCase)).Value;

                    AddData(resxDoc, rootElement, item.Key, $"{item.Value}", $"{commentValue}");
                }
            }

            var fileInfo = new FileInfo(file);

            // RESX dosyasını kaydet
            string fName = string.Concat("Localize.Resources.", fileInfo.Name.Split(".json")[0], ".resx");
            string resxFilePath = Path.Combine(_resourcesPath, fName);

            using (var writer = XmlWriter.Create(resxFilePath, new XmlWriterSettings { Indent = true }))
            {
                resxDoc.Save(writer);
            }
        }


        //Visual studio tarafında görüntüleyebilmek için xml dökümanına header değerlerini giriyoruz.
        public static void AddVisualStudioHeaders(XmlDocument xml, XmlElement root)
        {
            List<(string name, string value)> headers = new System.Collections.Generic.List<(string name, string value)>
            {
                new ("resmimetype", "text/microsoft-resx"),
                new ("reader", "System.Resources.ResXResourceReader, System.Windows.Forms, Version=2.0.3500.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"), //public key token guid ile oluştur
                new ("writer", "System.Resources.ResXResourceWriter, System.Windows.Forms, Version=2.0.3500.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"),
                //new ("version", "1.3"),
            };

            foreach (var i in headers)
            {
                var resheaderHeader = xml.CreateElement("resheader");
                resheaderHeader.SetAttribute("name", i.name);

                var valueElementHeader = xml.CreateElement("value");
                valueElementHeader.InnerText = i.value;

                resheaderHeader.AppendChild(valueElementHeader);
                root.AppendChild(resheaderHeader);
            }
        }


        static void AddData(XmlDocument xmlDoc, XmlElement rootElement, string key, string value, string comment = "")
        {
            var dataElement = xmlDoc.CreateElement("data");
            dataElement.SetAttribute("name", key);

            var valueElement = xmlDoc.CreateElement("value");
            valueElement.InnerText = value;

            if (!string.IsNullOrEmpty(comment) || !string.IsNullOrWhiteSpace(comment))
            {
                var commentElement = xmlDoc.CreateElement("comment");
                commentElement.InnerText = comment;

                dataElement.AppendChild(commentElement);
            }

            dataElement.AppendChild(valueElement);
            rootElement.AppendChild(dataElement);
        }


        static void ReturnAllGroup(KeyValuePair<string, object> item, XmlDocument xmlDoc, XmlElement rootElement, string comment = "")
        {
            if (IsObject(item) && item.Value is JsonElement jsonElement)
            {
                foreach (var innerElement in jsonElement.EnumerateObject())
                {
                    if (innerElement.Name.Contains("=Comments", StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    if (innerElement.Value.ValueKind == JsonValueKind.Object)
                    {
                        ReturnAllGroup(new KeyValuePair<string, object>(innerElement.Name, innerElement.Value), xmlDoc, rootElement);
                    }
                    else
                    {
                        string commentKey = string.Concat(innerElement.Name, "=Comments");
                        var commentValue = jsonElement.EnumerateObject().FirstOrDefault(w => w.Name.Equals(commentKey, StringComparison.OrdinalIgnoreCase)).Value;

                        if (commentValue.ValueKind == JsonValueKind.String)
                        {
                            comment = commentValue.GetString()!;
                        }

                        AddData(xmlDoc, rootElement, innerElement.Name, $"{innerElement.Value}", $"{comment}");
                    }
                }
            }
        }


        static bool IsGroup(KeyValuePair<string, object> item)
        {
            if (item.Key.EndsWith("-Group", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            else if (item.Value is JsonElement jsonElement && jsonElement.ValueKind == JsonValueKind.Object)
            {
                return true;
            }
            else return false;
        }


        static bool IsObject(KeyValuePair<string, object> item)
        {
            return
            (item.Value is JsonElement jsonElement && jsonElement.ValueKind == JsonValueKind.Object)
            ?
            true : false;
        }


        static List<string> GetTranslates()
        {
            var files = Directory.GetFiles(_assetsPath, "*.json", SearchOption.AllDirectories);
            return files.ToList();
        }


        public static CultureInfo[] GetCultures()
        {
            var myCultureFiles = Directory.GetFiles(_assetsPath, "*.json", SearchOption.AllDirectories);
            var myCultureFileNames = myCultureFiles.Select(filePath => Path.GetFileName(filePath)).ToList();

            string[] cultures = myCultureFileNames
                .Select(s => s.Replace("Localize.Resources.", string.Empty)
                    .Replace(".json", string.Empty)
                ).ToArray();

            CultureInfo[] cInf = cultures.Select(f => new CultureInfo(f)).ToArray();

            return cInf;
        }

    }
}