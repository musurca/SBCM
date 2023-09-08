using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using static System.Net.Mime.MediaTypeNames;

/*
 * https://stackoverflow.com/questions/25134897/gzip-compression-and-decompression-in-c-sharp
 */

namespace SBCM {
    public class DataFuncs {
        public static string Decompress(string input) {
            byte[] compressed = Convert.FromBase64String(input);
            byte[] decompressed = Decompress(compressed);
            return Encoding.UTF8.GetString(decompressed);
        }

        public static string Compress(string input) {
            byte[] encoded = Encoding.UTF8.GetBytes(input);
            byte[] compressed = Compress(encoded);
            return Convert.ToBase64String(compressed);
        }

        public static byte[] Decompress(byte[] input) {
            using (var source = new MemoryStream(input)) {
                byte[] lengthBytes = new byte[4];
                source.Read(lengthBytes, 0, 4);

                var length = BitConverter.ToInt32(lengthBytes, 0);
                using (var decompressionStream = new GZipStream(source,
                    CompressionMode.Decompress)) {
                    var result = new byte[length];
                    decompressionStream.Read(result, 0, length);
                    return result;
                }
            }
        }

        public static byte[] Compress(byte[] input) {
            using (var result = new MemoryStream()) {
                var lengthBytes = BitConverter.GetBytes(input.Length);
                result.Write(lengthBytes, 0, 4);

                using (var compressionStream = new GZipStream(result,
                    CompressionMode.Compress)) {
                    compressionStream.Write(input, 0, input.Length);
                    compressionStream.Flush();

                }
                return result.ToArray();
            }
        }

        public static ImageCodecInfo GetImageEncoder(ImageFormat format) {
            var codecs = ImageCodecInfo.GetImageDecoders();
            foreach (var codec in codecs) {
                if (codec.FormatID == format.Guid) {
                    return codec;
                }
            }
            return null;
        }

        // quality domain, from lowest to highest: [0L - 100L]
        public static EncoderParameters MakeImageEncoderParameters(Int64 quality) {
            var encoderParameters = new EncoderParameters(1);
            encoderParameters.Param[0] = new EncoderParameter(
                System.Drawing.Imaging.Encoder.Quality,
                quality
            );
            return encoderParameters;
        }

        public static string BitmapToBase64(Bitmap img) {
           return BytesToString(
               ImageToJPEGBytes(img)
           );
        }

        public static Bitmap Base64ToBitmap(string b64) {
            return JPEGBytesToImage(
                StringToBytes(b64)
            );
        }

        public static byte[] ImageToJPEGBytes(System.Drawing.Image img) {
            using (
                var stream = new MemoryStream()
            ) {
                img.Save(
                    stream, 
                    GetImageEncoder(ImageFormat.Jpeg), 
                    MakeImageEncoderParameters(100L)
                );
                return stream.ToArray();
            }
        }

        public static string BytesToString(byte[] buffer) {
            return Convert.ToBase64String(buffer);
        }

        public static byte[] StringToBytes(string buffer) {
            return Convert.FromBase64String(buffer);
        }

        public static Bitmap JPEGBytesToImage(byte[] buffer) {
            Bitmap bitmap;
            using (
                var stream = new MemoryStream(buffer)
            ) {
                System.Drawing.Image img = System.Drawing.Image.FromStream(
                    stream
                );
                bitmap = new Bitmap(img);
            }
            
            return bitmap;
        }
    }
}
