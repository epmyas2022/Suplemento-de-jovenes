using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace Suplemento.Views
{


    public class FilesData 
    {

        private static string DEFAULT_PATH = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal) ;
        public static  void CreateFile(string path,string nameFile,string data) {

            path = (path == string.Empty) ? DEFAULT_PATH : path;

            nameFile = (nameFile == string.Empty) ? "file" : nameFile;

            var filePath = Path.Combine(path, nameFile);

            if(File.Exists(filePath)) File.Delete(filePath);

         
           File.WriteAllText(filePath, data);

        }


        public static void DeleteFile(string path,string nameFile)
        {
            path = (path == string.Empty) ? DEFAULT_PATH : path;

            nameFile = (nameFile == string.Empty) ? "file" : nameFile;

            var filePath = Path.Combine(path, nameFile);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
        public static string ReadFile(string path, string nameFile)
        {
            path = (path == string.Empty) ? DEFAULT_PATH : path;

            nameFile = (nameFile == string.Empty) ? "file" : nameFile;

            var filePath = Path.Combine(path, nameFile);

            if (!File.Exists(filePath)) return null;

            return File.ReadAllText(filePath);

        }


        public static string ReadFileInPackage(string name, string extension)
        {

            var assembly = System.Reflection.IntrospectionExtensions.GetTypeInfo(typeof(Home)).Assembly;

            var stream = assembly.GetManifestResourceStream("Suplemento." + "db." + name + "." + extension);

            var StreamRead = new StreamReader(stream);

            return StreamRead.ReadToEnd();


        }

    }
}
