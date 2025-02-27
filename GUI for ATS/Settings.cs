using System.IO;
using System.Reflection;
using System.Xml.Serialization;

namespace GUI_for_ATS
{
    public class Settings
    {
        public string AzureClientId { get; set; }
        public string AzureClientSecret { get; set; }
        public string AzureTenantId { get; set; }
        public string SigntoolPath { get; set; }
        public string AzureCodeSigningDlibDllPath { get; set; }
        public string TimeStampServer { get; set; }
        public string MetadataJsonPath { get; set; }
        public string LastFilePath { get; set; }
        public string LastPath { get; set; }

        public Settings()
        {
            AzureClientId = "";
            AzureClientSecret = "";
            AzureTenantId = "";
            SigntoolPath = "";
            AzureCodeSigningDlibDllPath = "";
            TimeStampServer = "http://timestamp.acs.microsoft.com";
            MetadataJsonPath = "";
            LastFilePath = "";
            LastPath = "";
        }

        public void Save()
        {
            try
            {
                SerializeToXml(this, GetSettingsFilePath());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static Settings? LoadSettings()
        {
            try
            {
                if (File.Exists(GetSettingsFilePath()))
                {
                    try
                    {
                        return DeserializeFromXml<Settings>(GetSettingsFilePath());
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return CreateDefaultSettings();
                    }
                }
                else
                {
                    return CreateDefaultSettings();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return CreateDefaultSettings();
            }
        }

        public static Settings CreateDefaultSettings()
        {
            Settings settings = new();
            settings.Save();
            return settings;
        }

        public static string GetApplicationDataPath()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\" + Assembly.GetExecutingAssembly().GetName().Name?.Replace("-", "_");

            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch { }
            }

            return path;
        }

        public static string GetSettingsFilePath()
        {
            string applicationDataPath = GetApplicationDataPath();

            if (Directory.Exists(applicationDataPath) == false)
            {
                Directory.CreateDirectory(applicationDataPath);
            }

            return applicationDataPath + "\\Settings.xml";
        }

        public static void SerializeToXml<T>(T obj, string fileName)
        {
            using var fileStream = new FileStream(fileName, FileMode.Create);
            var ser = new XmlSerializer(typeof(T));
            ser.Serialize(fileStream, obj);
        }

        public static T? DeserializeFromXml<T>(string xml)
        {
            T? result;
            var ser = new XmlSerializer(typeof(T));
            using (var tr = new StreamReader(xml))
            {
                result = (T?)ser.Deserialize(tr);
            }
            return result;
        }
    }
}
