using System;
using System.Collections.Generic;
using System.IO;

namespace Solo.Utils
{
    public class Config
    {
        public Dictionary<string, int> Ints { get; private set; }
        public Dictionary<string, bool> Bools { get; private set; }
        public Dictionary<string, string> Strings { get; private set; }

        /// <summary>
        /// The settings class for the game contains dynamic arrays of parameters of the following types: int, bool, string.
        /// <param name="path">Path to the configuration file. The file must contain the parameters written in a separate line according to the following principle: [int|bool|string]:[name]:[value]</param>
        /// </summary>
        public Config(string path)
        {
            Ints = new Dictionary<string, int>();
            Bools = new Dictionary<string, bool>();
            Strings = new Dictionary<string, string>();

            using (StreamReader sr = new StreamReader(path))
            {
                while (sr.Peek() >= 0)
                {
                    string str = sr.ReadLine();
                    str = str.Replace(" ", "");
                    string[] tmp = str.Split(':');

                    switch (tmp[0].ToLower())
                    {
                        case "int":
                            Ints.Add(ReadString(tmp[1].ToLower()), ReadInt(tmp[2]));
                            break;
                        case "bool":
                            Bools.Add(ReadString(tmp[1].ToLower()), ReadBool(tmp[2].ToLower()));
                            break;
                        case "string":
                            Strings.Add(ReadString(tmp[1].ToLower()), ReadString(tmp[2]));
                            break;
                    }
                }
            }
        }

        public void Save(string path)
        {
            using (StreamWriter sw = new StreamWriter(path))
            {
                foreach (string k in Ints.Keys)
                {
                    sw.WriteLine("int: " + k.Replace(" ", "_") + ": " + Ints[k]);
                }

                foreach (string k in Bools.Keys)
                {
                    sw.WriteLine("bool: " + k.Replace(" ", "_") + ": " + Bools[k]);
                }

                foreach (string k in Strings.Keys)
                {
                    sw.WriteLine("string: " + k.Replace(" ", "_") + ": " + Strings[k].Replace(" ", "_"));
                }

                sw.WriteLine(" ");
                sw.WriteLine("// типы данных: int\\bool\\string");
                sw.WriteLine("// on\\true\\1\\yes\\+ - это true, всё остольное false");
                sw.WriteLine("// в string пробелы удаляются, поэтому вместо пробелов нужно ставить '_'.");
                sw.WriteLine("// в именах параметров пробелы удаляются, поэтому вместо пробелов нужно ставить '_', также символы переводятся в нижний регистр");

            }
        }

        /// <summary>
        /// Convert string to int
        /// /// </summary>
        private int ReadInt(string str)
        {
            return Convert.ToInt32(str);
        }
        /// <summary>
        /// Convert string to bool
        /// /// </summary>
        private bool ReadBool(string str)
        {
            if (str == "on" || str == "true" || str == "1" || str == "yes" || str == "+")
                return true;
            else
                return false;
        }
        /// <summary>
        /// Convert string to string. Replace '_' to ' '
        /// /// </summary>
        private string ReadString(string str)
        {
            return str.Replace("_", " ");
        }
    }
}
