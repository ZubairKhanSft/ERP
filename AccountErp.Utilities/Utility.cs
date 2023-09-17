using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AccountErp.Utilities
{
    public class Utility
    {
        public static TimeZoneInfo GetTimeZone(string timeZoneName)
        {
            return TimeZoneInfo.GetSystemTimeZones().SingleOrDefault(x => x.StandardName.Equals(timeZoneName));
        }

        public static DateTime GetDateTime()
        {
            return DateTime.UtcNow;
        }

        public static DateTime GetDateTime(DateTime dateTime, string timeZoneName)
        {
            //return if timezone name is empty
            if (string.IsNullOrEmpty(timeZoneName))
            {
                dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);

                return dateTime.ToLocalTime();
            }

            //return converted date time
            var timeZone = GetTimeZone(timeZoneName);
            return timeZone != null
                ? TimeZoneInfo.ConvertTime(dateTime, TimeZoneInfo.Utc, timeZone)
                : dateTime.ToLocalTime();
        }

        public static string GetFormattedDate(DateTime? dateTime)
        {
            return dateTime?.ToString(Constants.DateFormat);
        }

        public static string DefaultErrorMessage()
        {
            return "Something went wrong. Please try again after some time";
        }

        public static string GetUniqueFileName(string originalFileName)
        {
            return $"{Guid.NewGuid():N}{Path.GetExtension(originalFileName)}";
        }

        public static void CreateFolder(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public static string GetTempFolder(string basePath)
        {
            var path = $"{basePath}\\temp\\";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path;
        }

        public static string GetInvoiceFolder(string basePath)
        {
            var path = $"{basePath}\\invoice\\";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path;
        }

        public static string GetTempFileUrl(string baseUrl, string fileName)
        {
            return $"{baseUrl}/temp/{fileName}";
        }

        public static string GetEmailTemplateFolder(string basePath, string templateName)
        {
            return $"{basePath}\\templates\\email\\{templateName}";
        }

        public static bool IsValidImage(string extension)
        {
            var imageExtensions = new List<string> { ".JPG", ".JPEG", ".PNG" };
            return imageExtensions.Contains(extension.ToUpper());
        }

        public static string Encrypt(string plainText)
        {
            if (plainText == null) throw new ArgumentNullException("plainText");

            //encrypt data
            //var data = Encoding.Unicode.GetBytes(plainText);
            //byte[] encrypted = ProtectedData.Protect(data, null, DataProtectionScope.CurrentUser);
            const int WorkFactor = 10;
            var HashedPassword = BCrypt.Net.BCrypt.HashPassword(plainText, WorkFactor);
            //return as base64 string
            return HashedPassword;
        }
        public static bool IsValidSingleFile(string file)
        {
            if (file != null && file != "" && file != "string") return true;

            else return false;
        }
        public static bool Decrypt(string password, string cipher)
        {
            try
            {
                if (cipher == null) throw new ArgumentNullException("cipher");

                const int WorkFactor = 10;
                var HashedPassword = BCrypt.Net.BCrypt.HashPassword(password, WorkFactor);
                if (BCrypt.Net.BCrypt.Verify(password, cipher) == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            //parse base64 string
            //byte[] data = Convert.FromBase64String(cipher);

            //decrypt data
            // byte[] decrypted = ProtectedData.Unprotect(data, null, DataProtectionScope.CurrentUser);
            //return Encoding.Unicode.GetString(decrypted);

        }
    }
}
