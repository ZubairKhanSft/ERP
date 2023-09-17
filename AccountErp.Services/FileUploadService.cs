using AccountErp.Infrastructure.Services;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using AccountErp.Utilities;
using System.IO.Packaging;

namespace AccountErp.Services
{
    public class FileUploadService : IFileUploadService
    {
        private readonly IHostingEnvironment _env;
        public FileUploadService(IHostingEnvironment env)
        {
            _env = env;
        }

        public async Task<string> SaveFileAsync(string file)
        {
            string filepath = string.Empty;
            string filename = string.Empty;
            string uploadFolder = string.Empty;
            //string charToUpper = string.Empty;
            //string extension = string.Empty;
            if (IsValidSingleFile(file))
            {
                //if (file.Length > 2 * 1024 * 1024) // 2MB
                //{                            
                //    return BadRequest("Please upload an image file less than 2MB.");
                //}
                string charToUpper = file.Substring(0, 5).ToUpper();
                var bytess = Convert.FromBase64String(file);
                string extension = GetExtension(charToUpper, new MemoryStream(bytess));
                //if (extension == string.Empty) return BadRequest("please upload a valid file");
                if (extension == string.Empty) return "";
                if (FileExtensionMap.ExtensionMap.TryGetValue(extension, out uploadFolder))
                {
                    var myfilename = Guid.NewGuid().ToString();
                    filename = $"{myfilename}{extension}";
                    filepath = Path.Combine(_env.WebRootPath, "Attachments", uploadFolder, filename);

                    Directory.CreateDirectory(Path.GetDirectoryName(filepath));

                    // var bytess = Convert.FromBase64String(file); moved at line 39 to avoid dupication
                    using (var stream = new FileStream(filepath, FileMode.Create))
                    {
                        await stream.WriteAsync(bytess, 0, bytess.Length);
                        await stream.FlushAsync();
                    }
                }
            }
            return uploadFolder + "/" + filename;
        }
        private static bool IsValidSingleFile(string file)
        {
            if (file != null && file != "" && file != "string") return true;

            else return false;
        }
        public class FileExtensionMap
        {
            public static readonly Dictionary<string, string> ExtensionMap = new Dictionary<string, string>
             {
                { ".jpg",   "Images" },
                { ".jpeg",  "Images" },
                { ".png",   "Images" },
                { ".gif",   "Images" },
                { ".pdf",   "Documents" },
                { ".docx",  "Documents" },
                { ".xlsx",  "Documents" },
                { ".txt",   "Documents" },
                { ".doc",   "Documents" },
                { ".zip",   "Documents" },
                { ".exe",   "Documents" },
                { ".csv",   "Documents" },
                { ".mp4",   "Videos" },
                { ".webm", "MediaTemplate"},
                { ".m4a", "Audios"},
                { ".mp3", "Audios"},
                { ".wav", "Audios"}

            };
        }

        public static string GetExtension(string charToUpper, Stream stream)
        {
            switch (charToUpper)
            {
                case "IVBOR":
                    return ".jpg";

                case "/9J/4":
                    return ".png";

                case "JVBER":
                    return ".pdf";
                case "UESDB":
                    return GetExcelExtension(stream);//== ".xlsx" ? "xlsx" : ".docx";
                /*if (GetExcelExtension(stream) == ".xlsx")
                {
                    return ".xlsx";
                }
                else
                {
                    return ".docx";
                }*/

                case "0M8R4":
                    return ".doc";

                case "ZM9YI":
                    return ".txt";

                case "AAAAI":
                    return ".mp4";

                case "AAAAG":
                    return ".m4a";

                case "SUQZA":
                    return ".mp3";

                case "//VQR":
                    return ".mp3";

                case "//VQX":
                    return ".wav";

                case "UMFYI":
                    return ".rar";

                case "TVQQA":
                    return ".exe";

                case "ZMLYC":
                    return ".csv";

                case "R0LGO":
                    return ".gif";

                case "GKXFO":
                    return ".webm";

                default:
                    return string.Empty;
            }
        }
        public static string GetExcelExtension(Stream stream)
        {
            try
            {
                using (var package = Package.Open(stream))
                {
                    // Check if the package contains the required parts for an Excel file
                    if (package.PartExists(new Uri("/xl/workbook.xml", UriKind.Relative)))
                    {
                        return ".xlsx";
                    }
                    else { return ".docx"; }
                }
            }
            catch (Exception)
            {
                // Ignore any exceptions and return an empty string
            }
            return string.Empty;
        }
    }
}
