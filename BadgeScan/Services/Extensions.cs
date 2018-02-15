using System;
using System.IO;
using CsvHelper;
using Xamarin.Forms;

namespace BadgeScan
{
    public class Extensions
    {
        public static void LoadFile(string path)
        {
            try
            {
                StreamReader reader = new StreamReader(path);
                var parser = new CsvParser(reader);
                var header = parser.Read();
                //Console.WriteLine($"Header: {string.Join(",", header)}");

                while (true)
                {
                    var row = parser.Read();
                    if (row == null)
                    {
                        break;
                    }
                    //Console.WriteLine($"Row: {string.Join(",", row)}");

                    if (header.Length != row.Length) break;
                    for (var i = 0; i < header.Length; i++) {
                        var key = header[i];
                        var value = row[i];
                        switch(key) {
                            case "Authority":
                                Settings.Authority = value;
                                break;
                            case "Resource":
                                Settings.Resource = value;
                                break;
                            case "ApplicationId":
                                Settings.ApplicationId = value;
                                break;
                            case "SearchAttribute":
                                Settings.SearchAttribute = value;
                                break;
                            case "Keyboard":
                                Settings.Keyboard = value;
                                break;
                            case "UseScanner":
                                bool flag;
                                if (Boolean.TryParse(value, out flag)) Settings.UseScanner = flag;
                                break;
                        }
                        Console.WriteLine($"{key}: {value}");
                    }
                }
                App.Current.MainPage = new NavigationPage(new LoginPage());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"App.LoadFile Error {ex.Message}, {ex.StackTrace}");
            }
        }
    }
}
