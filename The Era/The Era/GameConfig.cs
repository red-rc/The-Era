using IniParser;
using IniParser.Model;
using System.IO;

namespace The_Era
{
    public class GameConfig
    {
        public static bool CreatorTool { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public bool Fullscreen { get; set; }
        public string filePath = "config.ini";

        public void LoadConfig()
        {
            if (!File.Exists(filePath))
            {
                CreateConfigFile();
            }

            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile(filePath);

            CreatorTool = bool.Parse(data["Special Feature"]["CreatorTool"]);
            Width = int.Parse(data["Graphics"]["Width"]);
            Height = int.Parse(data["Graphics"]["Height"]);
            Fullscreen = bool.Parse(data["Graphics"]["Fullscreen"]);
        }

        public void EditConfig(string section, string key, string value)
        {
            if (!File.Exists(filePath))
            {
                CreateConfigFile();
            }

            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile(filePath);

            if (data.Sections.ContainsSection(section))
            {
                if (data[section].ContainsKey(key))
                {
                    data[section][key] = value;
                }
                else
                {
                    data[section].AddKey(key, value);
                }
            }
            else
            {
                data.Sections.AddSection(section);
                data[section].AddKey(key, value);
            }

            parser.WriteFile(filePath, data);
        }

        private void CreateConfigFile()
        {
            var data = new IniData();
            data["Special Feature"]["CreatorTool"] = "false";
            data["Graphics"]["Width"] = "1280";
            data["Graphics"]["Height"] = "720";
            data["Graphics"]["Fullscreen"] = "false";

            var parser = new FileIniDataParser();
            parser.WriteFile(filePath, data);
        }
    }
}