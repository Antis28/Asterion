using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Asterion.Models.WebP
{
    public class WebPParams
    {
        public struct Resolution
        {
            public int width;
            public int height;

            public Resolution( int w, int h )
            {
                width = w;
                height = h;
            }

            public bool IsValid()
            {
                if( height > 100 && width > 100 )
                    return true;
                return false;
            }

            public override string ToString()
            {
                return width + " " + height;
            }
        }

        // Качество изображения
        public int quality = 85;
        public int qualityAlpha = 100;
        public Resolution resolution;

        public bool IsProgress = false;
        public bool IsVerbose = false;
        public bool IsQuiet = false;
        public bool IsShort = false;
        public bool IsMetadataCopy = false;
        public bool IsLossless = false;
        public bool IsNoalpha = false;
        public bool IsAlpha_cleanup = false;
        public int Compression
        {
            set
            {
                //(0 = fast, 6 = slowest)
                if( value >= 0 && value <= 6 )
                {
                    compression = value;
                    IsCompressionMethod = true;
                }
                else
                    IsCompressionMethod = false;
            }
            get { return compression; }
        }
        public float PSNR
        {
            set
            {
                //(in dB.typically: 42)
                if( value >= 0 && value <= 100 )
                {
                    psnr = value;
                    IsPSNR = true;
                }
                else
                    IsPSNR = false;
            }
            get { return psnr; }
        }
        public float SNS
        {
            set
            {
                //spatial noise shaping(0:off, 100:max)
                if( value >= 0 && value <= 100 )
                {
                    sns = value;
                    IsSNS = true;
                }
                else
                    IsSNS = false;
            }
            get { return sns; }
        }
        public float FilterStrength
        {
            set
            {
                //filter strength(0 = off..100)
                if( value >= 0 && value <= 100 )
                {
                    filterStrength = value;
                    IsFilterStrength = true;
                }
                else
                    IsFilterStrength = false;
            }
            get
            {
                if( filterStrength >= 0 && filterStrength <= 100 )
                {
                    return filterStrength;
                }
                else
                    return filterStrength = 0;
            }
        }

        private bool IsCompressionMethod = false;
        private int compression = -1;

        private bool IsPSNR = false;
        private float psnr = 42;

        private bool IsSNS = false;
        private float sns = 0;

        private bool IsFilterStrength = false;
        private float filterStrength = 0;





        public string BuildParams( string pathDirectory )
        {
            // Параметры для Webp конвертера
            StringBuilder sb = new StringBuilder();
            if( resolution.IsValid() )
            {
                sb.Append("-resize ");              // -resize
                sb.Append(resolution.ToString());   // <w> <h>
            }
            if( IsProgress )
                sb.Append(" -progress ");   // -progress  report encoding progress 
            if( IsVerbose )
                sb.Append(" -v ");      // -v       verbose, e.g. print encoding/decoding times
            if( IsQuiet )
                sb.Append(" -quiet ");  //-quiet    don't print anything
            if( IsShort )
                sb.Append(" -short "); // -short ................. condense printed message

            //-metadata <string> ..... comma separated list of metadata to
            //copy from the input to the output if present.
            //Valid values: all, none(default), exif, icc, xmp
            if( IsMetadataCopy )
                sb.Append(" -metadata all ");

            //-lossless..............encode image losslessly
            if( IsLossless )
                sb.Append(" -lossless ");
            //-noalpha...............discard any transparency information
            if( IsNoalpha )
                sb.Append(" -noalpha ");
            //-alpha_cleanup.........clean RGB values in transparent area
            if( IsAlpha_cleanup )
                sb.Append(" -alpha_cleanup ");

            //-m<int>...............compression method(0 = fast, 6 = slowest)
            if( IsCompressionMethod )
            {
                sb.Append(" -m ");
                sb.Append(compression);
                sb.Append(" ");
            }
            //-psnr<float>..........target PSNR(in dB.typically: 42)
            if( IsPSNR )
            {
                sb.Append(" -psnr ");
                sb.Append(psnr);
                sb.Append(" ");
            }
            //-sns<int>.............spatial noise shaping (0:off, 100:max)
            if( IsSNS )
            {
                sb.Append(" -sns ");
                sb.Append(sns);
                sb.Append(" ");
            }
            //-f<int>...............filter strength(0 = off..100)
            if( IsFilterStrength )
            {
                sb.Append(" -f ");
                sb.Append(filterStrength);
                sb.Append(" ");
            }


            sb.Append(" -q ");          // -q       качество изображения от 0 до 100
            sb.Append(quality);
            sb.Append(" -alpha_q ");    // -alpha_q качество изображения для альфа канала от 0 до 100   
            sb.Append(qualityAlpha);
            sb.Append(" -o ");          // -o       адрес вывода файла
            sb.Append("\"");
            sb.Append(pathDirectory);
            sb.Append(@"\output\");     //          каталог вывода

            //commandParameters = string.Format(" -q {0} -alpha_q {1} {4} -o \"{2}{3}",
            //        quality,            //{0} -q       качество изображения от 0 до 100
            //        qualityAlpha,       //{1} -alpha_q качество изображения для альфа канала от 0 до 100
            //        pathDirectory,      //{2}  -o      адрес вывода файла
            //        @"\output\",        //{3}          каталог вывода
            //       "-resize " +
            //       resolution.ToString()//{4} -resize
            //    );
            return sb.ToString();
        }
    }
}
