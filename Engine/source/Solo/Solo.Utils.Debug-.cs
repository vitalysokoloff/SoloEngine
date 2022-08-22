using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Solo.Utils
{
    public static class Debag
    {
        

        public static void Log(string path, string log)
        {
            if (!File.Exists(path))
            {
                File.Create(path);
            }
            using (StreamWriter sw = new StreamWriter(path, append: true))
            {
                sw.WriteLine(log);
            }
        }
       
    }
}
