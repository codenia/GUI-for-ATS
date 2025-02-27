using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GUI_for_ATS
{
    public static class Helper
    {
        #warning It is recommended to change these three variables to your own random values before using the program.
        static readonly string PasswordHash = "4KrZ$w&1";
        static readonly string SaltKey = "L7g$f§20";
        static readonly string VIKey = "Te2r$W8la&ieAI7%"; // VIKey must be exactly 16 characters long.

        public static string Encrypt(string plainText)
        {
            try
            {
                byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

                if (VIKey.Length != 16)
                    throw new ArgumentException("VIKey must be exactly 16 characters long.");

                using var keyDerivation = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey), 100_000, HashAlgorithmName.SHA512);
                byte[] keyBytes = keyDerivation.GetBytes(32);

                using var aes = Aes.Create();
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.Key = keyBytes;
                aes.IV = Encoding.ASCII.GetBytes(VIKey);

                using var memoryStream = new MemoryStream();
                using (var encryptor = aes.CreateEncryptor())
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                    cryptoStream.FlushFinalBlock();
                }

                return Convert.ToBase64String(memoryStream.ToArray());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Encryption error: " + ex.Message);
                return "";
            }
        }

        public static string Decrypt(string encryptedText)
        {
            try
            {
                byte[] cipherTextBytes = Convert.FromBase64String(encryptedText);

                if (VIKey.Length != 16)
                    throw new ArgumentException("VIKey must be exactly 16 characters long.");

                using var keyDerivation = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey), 100_000, HashAlgorithmName.SHA512);
                byte[] keyBytes = keyDerivation.GetBytes(32);

                using var aes = Aes.Create();
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.Key = keyBytes;
                aes.IV = Encoding.ASCII.GetBytes(VIKey);

                using var memoryStream = new MemoryStream(cipherTextBytes);
                using var decryptor = aes.CreateDecryptor();
                using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
                using var resultStream = new MemoryStream();

                cryptoStream.CopyTo(resultStream);
                return Encoding.UTF8.GetString(resultStream.ToArray());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Decryption error: " + ex.Message);
                return "";
            }
        }

        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbFileInfo, uint uFlags);

        private const uint SHGFI_ICON = 0x100;
        private const uint SHGFI_LARGEICON = 0x0;

        [StructLayout(LayoutKind.Sequential)]

        private struct SHFILEINFO
        {
            public IntPtr hIcon;
            public int iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        }

        public static ImageSource? GetFileIcon(string filePath)
        {
            try
            {
                SHFILEINFO shinfo = new();
                SHGetFileInfo(filePath, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), SHGFI_ICON | SHGFI_LARGEICON);

                if (shinfo.hIcon == IntPtr.Zero)
                    return null;

                return System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(
                    shinfo.hIcon,
                    System.Windows.Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());
            }
            catch
            {
                return Helper.CreatePlaceholderIcon();
            }
        }

        public static ImageSource CreatePlaceholderIcon(int width = 64, int height = 64)
        {
            DrawingVisual drawingVisual = new();
            using (DrawingContext dc = drawingVisual.RenderOpen())
            {
                dc.DrawRectangle(Brushes.LightGray, null, new Rect(0, 0, width, height));
            }

            RenderTargetBitmap bmp = new(width, height, 96, 96, PixelFormats.Pbgra32);
            bmp.Render(drawingVisual);
            return bmp;
        }
    }
}
