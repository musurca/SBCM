using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

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

        public static EncoderParameters MakeImageEncoderParameters(Int64 quality) {
            var encoderParameters = new EncoderParameters(1);
            encoderParameters.Param[0] = new EncoderParameter(
                System.Drawing.Imaging.Encoder.Quality,
                quality
            );
            return encoderParameters;
        }

        public static byte[] ImageToJPEGBytes(Image img) {
            using (var stream = new MemoryStream()) {
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
            Image img = Image.FromStream(new MemoryStream(buffer));
            Bitmap bitmap = new Bitmap(img);
            return bitmap;
        }
    }
}
