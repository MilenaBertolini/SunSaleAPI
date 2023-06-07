using Main = Domain.Entities.Imagem;
using IService = Application.Interface.Services.IImageMagicService;
using ILogger = Application.Interface.Services.ILoggerService;
using ImageMagick;
using System.Text;

namespace Application.Implementation.Services
{
    public class ImageMagicService : IService
    {
        private readonly ILogger _logger;
        public ImageMagicService(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<Main> TreatAsync(Main input)
        {
            Main output = input;
            try
            {
                string base64Data = input.Arquivo.Substring(input.Arquivo.IndexOf(',') + 1);
                byte[] bytes = Convert.FromBase64String(base64Data);

                var readSettings = new MagickReadSettings() { Format = GetFormat(input.tipo) };
                MagickImage img = new MagickImage(bytes, readSettings);

                if (input.turnTransparent.HasValue && input.turnTransparent.Value)
                {
                    img.ColorFuzz = new Percentage(10);
                    // -transparent white
                    img.Transparent(MagickColors.White);
                }

                if(input.width.HasValue && input.height.HasValue)
                {
                    if (input.width != int.MinValue && input.height != int.MinValue)
                    {
                        img.Resize(input.width.Value, input.height.Value);
                    }
                }

                output.Arquivo = Convert.ToBase64String(img.ToByteArray());

            }
            catch(MagickCorruptImageErrorException ex)
            {
                throw ex;
            }
            catch (MagickMissingDelegateErrorException ex)
            {
                throw ex;
            }
            catch (MagickBlobErrorException ex)
            {
                throw ex;
            }
            catch (Exception e)
            {
                throw e;
            }

            return output;
        }

        private MagickFormat GetFormat(string tipo)
        {
            MagickFormat output = MagickFormat.Png;

            if (tipo.ToLower().Equals("Unknown".ToLower()))
            {
                output = MagickFormat.Unknown;
            }
            else if (tipo.ToLower().Equals("ThreeFr".ToLower()))
            {
                output = MagickFormat.ThreeFr;
            }
            else if (tipo.ToLower().Equals("ThreeG2".ToLower()))
            {
                output = MagickFormat.ThreeG2;
            }
            else if (tipo.ToLower().Equals("ThreeGp".ToLower()))
            {
                output = MagickFormat.ThreeGp;
            }
            else if (tipo.ToLower().Equals("A".ToLower()))
            {
                output = MagickFormat.A;
            }
            else if (tipo.ToLower().Equals("Aai".ToLower()))
            {
                output = MagickFormat.Aai;
            }
            else if (tipo.ToLower().Equals("Ai".ToLower()))
            {
                output = MagickFormat.Ai;
            }
            else if (tipo.ToLower().Equals("APng".ToLower()))
            {
                output = MagickFormat.APng;
            }
            else if (tipo.ToLower().Equals("Art".ToLower()))
            {
                output = MagickFormat.Art;
            }
            else if (tipo.ToLower().Equals("Arw".ToLower()))
            {
                output = MagickFormat.Arw;
            }
            else if (tipo.ToLower().Equals("Ashlar".ToLower()))
            {
                output = MagickFormat.Ashlar;
            }
            else if (tipo.ToLower().Equals("Avi".ToLower()))
            {
                output = MagickFormat.Avi;
            }
            else if (tipo.ToLower().Equals("Avif".ToLower()))
            {
                output = MagickFormat.Avif;
            }
            else if (tipo.ToLower().Equals("Avs".ToLower()))
            {
                output = MagickFormat.Avs;
            }
            else if (tipo.ToLower().Equals("B".ToLower()))
            {
                output = MagickFormat.B;
            }
            else if (tipo.ToLower().Equals("Bayer".ToLower()))
            {
                output = MagickFormat.Bayer;
            }
            else if (tipo.ToLower().Equals("Bayera".ToLower()))
            {
                output = MagickFormat.Bayera;
            }
            else if (tipo.ToLower().Equals("Bgr".ToLower()))
            {
                output = MagickFormat.Bgr;
            }
            else if (tipo.ToLower().Equals("Bgra".ToLower()))
            {
                output = MagickFormat.Bgra;
            }
            else if (tipo.ToLower().Equals("Bgro".ToLower()))
            {
                output = MagickFormat.Bgro;
            }
            else if (tipo.ToLower().Equals("Bmp".ToLower()))
            {
                output = MagickFormat.Bmp;
            }
            else if (tipo.ToLower().Equals("Bmp2".ToLower()))
            {
                output = MagickFormat.Bmp2;
            }
            else if (tipo.ToLower().Equals("Bmp3".ToLower()))
            {
                output = MagickFormat.Bmp3;
            }
            else if (tipo.ToLower().Equals("Brf".ToLower()))
            {
                output = MagickFormat.Brf;
            }
            else if (tipo.ToLower().Equals("C".ToLower()))
            {
                output = MagickFormat.C;
            }
            else if (tipo.ToLower().Equals("Cal".ToLower()))
            {
                output = MagickFormat.Cal;
            }
            else if (tipo.ToLower().Equals("Cals".ToLower()))
            {
                output = MagickFormat.Cals;
            }
            else if (tipo.ToLower().Equals("Canvas".ToLower()))
            {
                output = MagickFormat.Canvas;
            }
            else if (tipo.ToLower().Equals("Caption".ToLower()))
            {
                output = MagickFormat.Caption;
            }
            else if (tipo.ToLower().Equals("Cin".ToLower()))
            {
                output = MagickFormat.Cin;
            }
            else if (tipo.ToLower().Equals("Cip".ToLower()))
            {
                output = MagickFormat.Cip;
            }
            else if (tipo.ToLower().Equals("Clip".ToLower()))
            {
                output = MagickFormat.Clip;
            }
            else if (tipo.ToLower().Equals("Clipboard".ToLower()))
            {
                output = MagickFormat.Clipboard;
            }
            else if (tipo.ToLower().Equals("Cmyk".ToLower()))
            {
                output = MagickFormat.Cmyk;
            }
            else if (tipo.ToLower().Equals("Cmyka".ToLower()))
            {
                output = MagickFormat.Cmyka;
            }
            else if (tipo.ToLower().Equals("Cr2".ToLower()))
            {
                output = MagickFormat.Cr2;
            }
            else if (tipo.ToLower().Equals("Cr3".ToLower()))
            {
                output = MagickFormat.Cr3;
            }
            else if (tipo.ToLower().Equals("Crw".ToLower()))
            {
                output = MagickFormat.Crw;
            }
            else if (tipo.ToLower().Equals("Cube".ToLower()))
            {
                output = MagickFormat.Cube;
            }
            else if (tipo.ToLower().Equals("Cur".ToLower()))
            {
                output = MagickFormat.Cur;
            }
            else if (tipo.ToLower().Equals("Cut".ToLower()))
            {
                output = MagickFormat.Cut;
            }
            else if (tipo.ToLower().Equals("Data".ToLower()))
            {
                output = MagickFormat.Data;
            }
            else if (tipo.ToLower().Equals("Dcm".ToLower()))
            {
                output = MagickFormat.Dcm;
            }
            else if (tipo.ToLower().Equals("Dcr".ToLower()))
            {
                output = MagickFormat.Dcr;
            }
            else if (tipo.ToLower().Equals("Dcraw".ToLower()))
            {
                output = MagickFormat.Dcraw;
            }
            else if (tipo.ToLower().Equals("Dcx".ToLower()))
            {
                output = MagickFormat.Dcx;
            }
            else if (tipo.ToLower().Equals("Dds".ToLower()))
            {
                output = MagickFormat.Dds;
            }
            else if (tipo.ToLower().Equals("Dfont".ToLower()))
            {
                output = MagickFormat.Dfont;
            }
            else if (tipo.ToLower().Equals("Dib".ToLower()))
            {
                output = MagickFormat.Dib;
            }
            else if (tipo.ToLower().Equals("Dng".ToLower()))
            {
                output = MagickFormat.Dng;
            }
            else if (tipo.ToLower().Equals("Dpx".ToLower()))
            {
                output = MagickFormat.Dpx;
            }
            else if (tipo.ToLower().Equals("Dxt1".ToLower()))
            {
                output = MagickFormat.Dxt1;
            }
            else if (tipo.ToLower().Equals("Dxt5".ToLower()))
            {
                output = MagickFormat.Dxt5;
            }
            else if (tipo.ToLower().Equals("Emf".ToLower()))
            {
                output = MagickFormat.Emf;
            }
            else if (tipo.ToLower().Equals("Epdf".ToLower()))
            {
                output = MagickFormat.Epdf;
            }
            else if (tipo.ToLower().Equals("Epi".ToLower()))
            {
                output = MagickFormat.Epi;
            }
            else if (tipo.ToLower().Equals("Eps".ToLower()))
            {
                output = MagickFormat.Eps;
            }
            else if (tipo.ToLower().Equals("Eps2".ToLower()))
            {
                output = MagickFormat.Eps2;
            }
            else if (tipo.ToLower().Equals("Eps3".ToLower()))
            {
                output = MagickFormat.Eps3;
            }
            else if (tipo.ToLower().Equals("Epsf".ToLower()))
            {
                output = MagickFormat.Epsf;
            }
            else if (tipo.ToLower().Equals("Epsi".ToLower()))
            {
                output = MagickFormat.Epsi;
            }
            else if (tipo.ToLower().Equals("Ept".ToLower()))
            {
                output = MagickFormat.Ept;
            }
            else if (tipo.ToLower().Equals("Ept2".ToLower()))
            {
                output = MagickFormat.Ept2;
            }
            else if (tipo.ToLower().Equals("Ept3".ToLower()))
            {
                output = MagickFormat.Ept3;
            }
            else if (tipo.ToLower().Equals("Erf".ToLower()))
            {
                output = MagickFormat.Erf;
            }
            else if (tipo.ToLower().Equals("Exr".ToLower()))
            {
                output = MagickFormat.Exr;
            }
            else if (tipo.ToLower().Equals("Farbfeld".ToLower()))
            {
                output = MagickFormat.Farbfeld;
            }
            else if (tipo.ToLower().Equals("Fax".ToLower()))
            {
                output = MagickFormat.Fax;
            }
            else if (tipo.ToLower().Equals("Ff".ToLower()))
            {
                output = MagickFormat.Ff;
            }
            else if (tipo.ToLower().Equals("File".ToLower()))
            {
                output = MagickFormat.File;
            }
            else if (tipo.ToLower().Equals("Fits".ToLower()))
            {
                output = MagickFormat.Fits;
            }
            else if (tipo.ToLower().Equals("Fl32".ToLower()))
            {
                output = MagickFormat.Fl32;
            }
            else if (tipo.ToLower().Equals("Flv".ToLower()))
            {
                output = MagickFormat.Flv;
            }
            else if (tipo.ToLower().Equals("Fractal".ToLower()))
            {
                output = MagickFormat.Fractal;
            }
            else if (tipo.ToLower().Equals("Ftp".ToLower()))
            {
                output = MagickFormat.Ftp;
            }
            else if (tipo.ToLower().Equals("Fts".ToLower()))
            {
                output = MagickFormat.Fts;
            }
            else if (tipo.ToLower().Equals("Ftxt".ToLower()))
            {
                output = MagickFormat.Ftxt;
            }
            else if (tipo.ToLower().Equals("G".ToLower()))
            {
                output = MagickFormat.G;
            }
            else if (tipo.ToLower().Equals("G3".ToLower()))
            {
                output = MagickFormat.G3;
            }
            else if (tipo.ToLower().Equals("G4".ToLower()))
            {
                output = MagickFormat.G4;
            }
            else if (tipo.ToLower().Equals("Gif".ToLower()))
            {
                output = MagickFormat.Gif;
            }
            else if (tipo.ToLower().Equals("Gif87".ToLower()))
            {
                output = MagickFormat.Gif87;
            }
            else if (tipo.ToLower().Equals("Gradient".ToLower()))
            {
                output = MagickFormat.Gradient;
            }
            else if (tipo.ToLower().Equals("Gray".ToLower()))
            {
                output = MagickFormat.Gray;
            }
            else if (tipo.ToLower().Equals("Graya".ToLower()))
            {
                output = MagickFormat.Graya;
            }
            else if (tipo.ToLower().Equals("Group4".ToLower()))
            {
                output = MagickFormat.Group4;
            }
            else if (tipo.ToLower().Equals("Hald".ToLower()))
            {
                output = MagickFormat.Hald;
            }
            else if (tipo.ToLower().Equals("Hdr".ToLower()))
            {
                output = MagickFormat.Hdr;
            }
            else if (tipo.ToLower().Equals("Heic".ToLower()))
            {
                output = MagickFormat.Heic;
            }
            else if (tipo.ToLower().Equals("Heif".ToLower()))
            {
                output = MagickFormat.Heif;
            }
            else if (tipo.ToLower().Equals("Histogram".ToLower()))
            {
                output = MagickFormat.Histogram;
            }
            else if (tipo.ToLower().Equals("Hrz".ToLower()))
            {
                output = MagickFormat.Hrz;
            }
            else if (tipo.ToLower().Equals("Htm".ToLower()))
            {
                output = MagickFormat.Htm;
            }
            else if (tipo.ToLower().Equals("Html".ToLower()))
            {
                output = MagickFormat.Html;
            }
            else if (tipo.ToLower().Equals("Http".ToLower()))
            {
                output = MagickFormat.Http;
            }
            else if (tipo.ToLower().Equals("Https".ToLower()))
            {
                output = MagickFormat.Https;
            }
            else if (tipo.ToLower().Equals("Icb".ToLower()))
            {
                output = MagickFormat.Icb;
            }
            else if (tipo.ToLower().Equals("Ico".ToLower()))
            {
                output = MagickFormat.Ico;
            }
            else if (tipo.ToLower().Equals("Icon".ToLower()))
            {
                output = MagickFormat.Icon;
            }
            else if (tipo.ToLower().Equals("Iiq".ToLower()))
            {
                output = MagickFormat.Iiq;
            }
            else if (tipo.ToLower().Equals("Info".ToLower()))
            {
                output = MagickFormat.Info;
            }
            else if (tipo.ToLower().Equals("Inline".ToLower()))
            {
                output = MagickFormat.Inline;
            }
            else if (tipo.ToLower().Equals("Ipl".ToLower()))
            {
                output = MagickFormat.Ipl;
            }
            else if (tipo.ToLower().Equals("Isobrl".ToLower()))
            {
                output = MagickFormat.Isobrl;
            }
            else if (tipo.ToLower().Equals("Isobrl6".ToLower()))
            {
                output = MagickFormat.Isobrl6;
            }
            else if (tipo.ToLower().Equals("J2c".ToLower()))
            {
                output = MagickFormat.J2c;
            }
            else if (tipo.ToLower().Equals("J2k".ToLower()))
            {
                output = MagickFormat.J2k;
            }
            else if (tipo.ToLower().Equals("Jng".ToLower()))
            {
                output = MagickFormat.Jng;
            }
            else if (tipo.ToLower().Equals("Jnx".ToLower()))
            {
                output = MagickFormat.Jnx;
            }
            else if (tipo.ToLower().Equals("Jp2".ToLower()))
            {
                output = MagickFormat.Jp2;
            }
            else if (tipo.ToLower().Equals("Jpc".ToLower()))
            {
                output = MagickFormat.Jpc;
            }
            else if (tipo.ToLower().Equals("Jpe".ToLower()))
            {
                output = MagickFormat.Jpe;
            }
            else if (tipo.ToLower().Equals("Jpeg".ToLower()))
            {
                output = MagickFormat.Jpeg;
            }
            else if (tipo.ToLower().Equals("Jpg".ToLower()))
            {
                output = MagickFormat.Jpg;
            }
            else if (tipo.ToLower().Equals("Jpm".ToLower()))
            {
                output = MagickFormat.Jpm;
            }
            else if (tipo.ToLower().Equals("Jps".ToLower()))
            {
                output = MagickFormat.Jps;
            }
            else if (tipo.ToLower().Equals("Jpt".ToLower()))
            {
                output = MagickFormat.Jpt;
            }
            else if (tipo.ToLower().Equals("Json".ToLower()))
            {
                output = MagickFormat.Json;
            }
            else if (tipo.ToLower().Equals("Jxl".ToLower()))
            {
                output = MagickFormat.Jxl;
            }
            else if (tipo.ToLower().Equals("K".ToLower()))
            {
                output = MagickFormat.K;
            }
            else if (tipo.ToLower().Equals("K25".ToLower()))
            {
                output = MagickFormat.K25;
            }
            else if (tipo.ToLower().Equals("Kdc".ToLower()))
            {
                output = MagickFormat.Kdc;
            }
            else if (tipo.ToLower().Equals("Label".ToLower()))
            {
                output = MagickFormat.Label;
            }
            else if (tipo.ToLower().Equals("M".ToLower()))
            {
                output = MagickFormat.M;
            }
            else if (tipo.ToLower().Equals("M2v".ToLower()))
            {
                output = MagickFormat.M2v;
            }
            else if (tipo.ToLower().Equals("M4v".ToLower()))
            {
                output = MagickFormat.M4v;
            }
            else if (tipo.ToLower().Equals("Mac".ToLower()))
            {
                output = MagickFormat.Mac;
            }
            else if (tipo.ToLower().Equals("Map".ToLower()))
            {
                output = MagickFormat.Map;
            }
            else if (tipo.ToLower().Equals("Mask".ToLower()))
            {
                output = MagickFormat.Mask;
            }
            else if (tipo.ToLower().Equals("Mat".ToLower()))
            {
                output = MagickFormat.Mat;
            }
            else if (tipo.ToLower().Equals("Matte".ToLower()))
            {
                output = MagickFormat.Matte;
            }
            else if (tipo.ToLower().Equals("Mef".ToLower()))
            {
                output = MagickFormat.Mef;
            }
            else if (tipo.ToLower().Equals("Miff".ToLower()))
            {
                output = MagickFormat.Miff;
            }
            else if (tipo.ToLower().Equals("Mkv".ToLower()))
            {
                output = MagickFormat.Mkv;
            }
            else if (tipo.ToLower().Equals("Mng".ToLower()))
            {
                output = MagickFormat.Mng;
            }
            else if (tipo.ToLower().Equals("Mono".ToLower()))
            {
                output = MagickFormat.Mono;
            }
            else if (tipo.ToLower().Equals("Mov".ToLower()))
            {
                output = MagickFormat.Mov;
            }
            else if (tipo.ToLower().Equals("Mp4".ToLower()))
            {
                output = MagickFormat.Mp4;
            }
            else if (tipo.ToLower().Equals("Mpc".ToLower()))
            {
                output = MagickFormat.Mpc;
            }
            else if (tipo.ToLower().Equals("Mpeg".ToLower()))
            {
                output = MagickFormat.Mpeg;
            }
            else if (tipo.ToLower().Equals("Mpg".ToLower()))
            {
                output = MagickFormat.Mpg;
            }
            else if (tipo.ToLower().Equals("Mrw".ToLower()))
            {
                output = MagickFormat.Mrw;
            }
            else if (tipo.ToLower().Equals("Msl".ToLower()))
            {
                output = MagickFormat.Msl;
            }
            else if (tipo.ToLower().Equals("Msvg".ToLower()))
            {
                output = MagickFormat.Msvg;
            }
            else if (tipo.ToLower().Equals("Mtv".ToLower()))
            {
                output = MagickFormat.Mtv;
            }
            else if (tipo.ToLower().Equals("Mvg".ToLower()))
            {
                output = MagickFormat.Mvg;
            }
            else if (tipo.ToLower().Equals("Nef".ToLower()))
            {
                output = MagickFormat.Nef;
            }
            else if (tipo.ToLower().Equals("Nrw".ToLower()))
            {
                output = MagickFormat.Nrw;
            }
            else if (tipo.ToLower().Equals("Null".ToLower()))
            {
                output = MagickFormat.Null;
            }
            else if (tipo.ToLower().Equals("O".ToLower()))
            {
                output = MagickFormat.O;
            }
            else if (tipo.ToLower().Equals("Ora".ToLower()))
            {
                output = MagickFormat.Ora;
            }
            else if (tipo.ToLower().Equals("Orf".ToLower()))
            {
                output = MagickFormat.Orf;
            }
            else if (tipo.ToLower().Equals("Otb".ToLower()))
            {
                output = MagickFormat.Otb;
            }
            else if (tipo.ToLower().Equals("Otf".ToLower()))
            {
                output = MagickFormat.Otf;
            }
            else if (tipo.ToLower().Equals("Pal".ToLower()))
            {
                output = MagickFormat.Pal;
            }
            else if (tipo.ToLower().Equals("Palm".ToLower()))
            {
                output = MagickFormat.Palm;
            }
            else if (tipo.ToLower().Equals("Pam".ToLower()))
            {
                output = MagickFormat.Pam;
            }
            else if (tipo.ToLower().Equals("Pango".ToLower()))
            {
                output = MagickFormat.Pango;
            }
            else if (tipo.ToLower().Equals("Pattern".ToLower()))
            {
                output = MagickFormat.Pattern;
            }
            else if (tipo.ToLower().Equals("Pbm".ToLower()))
            {
                output = MagickFormat.Pbm;
            }
            else if (tipo.ToLower().Equals("Pcd".ToLower()))
            {
                output = MagickFormat.Pcd;
            }
            else if (tipo.ToLower().Equals("Pcds".ToLower()))
            {
                output = MagickFormat.Pcds;
            }
            else if (tipo.ToLower().Equals("Pcl".ToLower()))
            {
                output = MagickFormat.Pcl;
            }
            else if (tipo.ToLower().Equals("Pct".ToLower()))
            {
                output = MagickFormat.Pct;
            }
            else if (tipo.ToLower().Equals("Pcx".ToLower()))
            {
                output = MagickFormat.Pcx;
            }
            else if (tipo.ToLower().Equals("Pdb".ToLower()))
            {
                output = MagickFormat.Pdb;
            }
            else if (tipo.ToLower().Equals("Pdf".ToLower()))
            {
                output = MagickFormat.Pdf;
            }
            else if (tipo.ToLower().Equals("Pdfa".ToLower()))
            {
                output = MagickFormat.Pdfa;
            }
            else if (tipo.ToLower().Equals("Pef".ToLower()))
            {
                output = MagickFormat.Pef;
            }
            else if (tipo.ToLower().Equals("Pes".ToLower()))
            {
                output = MagickFormat.Pes;
            }
            else if (tipo.ToLower().Equals("Pfa".ToLower()))
            {
                output = MagickFormat.Pfa;
            }
            else if (tipo.ToLower().Equals("Pfb".ToLower()))
            {
                output = MagickFormat.Pfb;
            }
            else if (tipo.ToLower().Equals("Pfm".ToLower()))
            {
                output = MagickFormat.Pfm;
            }
            else if (tipo.ToLower().Equals("Pgm".ToLower()))
            {
                output = MagickFormat.Pgm;
            }
            else if (tipo.ToLower().Equals("Phm".ToLower()))
            {
                output = MagickFormat.Phm;
            }
            else if (tipo.ToLower().Equals("Pgx".ToLower()))
            {
                output = MagickFormat.Pgx;
            }
            else if (tipo.ToLower().Equals("Picon".ToLower()))
            {
                output = MagickFormat.Picon;
            }
            else if (tipo.ToLower().Equals("Pict".ToLower()))
            {
                output = MagickFormat.Pict;
            }
            else if (tipo.ToLower().Equals("Pix".ToLower()))
            {
                output = MagickFormat.Pix;
            }
            else if (tipo.ToLower().Equals("Pjpeg".ToLower()))
            {
                output = MagickFormat.Pjpeg;
            }
            else if (tipo.ToLower().Equals("Plasma".ToLower()))
            {
                output = MagickFormat.Plasma;
            }
            else if (tipo.ToLower().Equals("Png".ToLower()))
            {
                output = MagickFormat.Png;
            }
            else if (tipo.ToLower().Equals("Png00".ToLower()))
            {
                output = MagickFormat.Png00;
            }
            else if (tipo.ToLower().Equals("Png24".ToLower()))
            {
                output = MagickFormat.Png24;
            }
            else if (tipo.ToLower().Equals("Png32".ToLower()))
            {
                output = MagickFormat.Png32;
            }
            else if (tipo.ToLower().Equals("Png48".ToLower()))
            {
                output = MagickFormat.Png48;
            }
            else if (tipo.ToLower().Equals("Png64".ToLower()))
            {
                output = MagickFormat.Png64;
            }
            else if (tipo.ToLower().Equals("Png8".ToLower()))
            {
                output = MagickFormat.Png8;
            }
            else if (tipo.ToLower().Equals("Pnm".ToLower()))
            {
                output = MagickFormat.Pnm;
            }
            else if (tipo.ToLower().Equals("Pocketmod".ToLower()))
            {
                output = MagickFormat.Pocketmod;
            }
            else if (tipo.ToLower().Equals("Ppm".ToLower()))
            {
                output = MagickFormat.Ppm;
            }
            else if (tipo.ToLower().Equals("Ps".ToLower()))
            {
                output = MagickFormat.Ps;
            }
            else if (tipo.ToLower().Equals("Ps2".ToLower()))
            {
                output = MagickFormat.Ps2;
            }
            else if (tipo.ToLower().Equals("Ps3".ToLower()))
            {
                output = MagickFormat.Ps3;
            }
            else if (tipo.ToLower().Equals("Psb".ToLower()))
            {
                output = MagickFormat.Psb;
            }
            else if (tipo.ToLower().Equals("Psd".ToLower()))
            {
                output = MagickFormat.Psd;
            }
            else if (tipo.ToLower().Equals("Ptif".ToLower()))
            {
                output = MagickFormat.Ptif;
            }
            else if (tipo.ToLower().Equals("Pwp".ToLower()))
            {
                output = MagickFormat.Pwp;
            }
            else if (tipo.ToLower().Equals("Qoi".ToLower()))
            {
                output = MagickFormat.Qoi;
            }
            else if (tipo.ToLower().Equals("R".ToLower()))
            {
                output = MagickFormat.R;
            }
            else if (tipo.ToLower().Equals("RadialGradient".ToLower()))
            {
                output = MagickFormat.RadialGradient;
            }
            else if (tipo.ToLower().Equals("Raf".ToLower()))
            {
                output = MagickFormat.Raf;
            }
            else if (tipo.ToLower().Equals("Ras".ToLower()))
            {
                output = MagickFormat.Ras;
            }
            else if (tipo.ToLower().Equals("Raw".ToLower()))
            {
                output = MagickFormat.Raw;
            }
            else if (tipo.ToLower().Equals("Rgb".ToLower()))
            {
                output = MagickFormat.Rgb;
            }
            else if (tipo.ToLower().Equals("Rgb565".ToLower()))
            {
                output = MagickFormat.Rgb565;
            }
            else if (tipo.ToLower().Equals("Rgba".ToLower()))
            {
                output = MagickFormat.Rgba;
            }
            else if (tipo.ToLower().Equals("Rgbo".ToLower()))
            {
                output = MagickFormat.Rgbo;
            }
            else if (tipo.ToLower().Equals("Rgf".ToLower()))
            {
                output = MagickFormat.Rgf;
            }
            else if (tipo.ToLower().Equals("Rla".ToLower()))
            {
                output = MagickFormat.Rla;
            }
            else if (tipo.ToLower().Equals("Rle".ToLower()))
            {
                output = MagickFormat.Rle;
            }
            else if (tipo.ToLower().Equals("Rmf".ToLower()))
            {
                output = MagickFormat.Rmf;
            }
            else if (tipo.ToLower().Equals("Rsvg".ToLower()))
            {
                output = MagickFormat.Rsvg;
            }
            else if (tipo.ToLower().Equals("Rw2".ToLower()))
            {
                output = MagickFormat.Rw2;
            }
            else if (tipo.ToLower().Equals("Scr".ToLower()))
            {
                output = MagickFormat.Scr;
            }
            else if (tipo.ToLower().Equals("Screenshot".ToLower()))
            {
                output = MagickFormat.Screenshot;
            }
            else if (tipo.ToLower().Equals("Sct".ToLower()))
            {
                output = MagickFormat.Sct;
            }
            else if (tipo.ToLower().Equals("Sfw".ToLower()))
            {
                output = MagickFormat.Sfw;
            }
            else if (tipo.ToLower().Equals("Sgi".ToLower()))
            {
                output = MagickFormat.Sgi;
            }
            else if (tipo.ToLower().Equals("Shtml".ToLower()))
            {
                output = MagickFormat.Shtml;
            }
            else if (tipo.ToLower().Equals("Six".ToLower()))
            {
                output = MagickFormat.Six;
            }
            else if (tipo.ToLower().Equals("Sixel".ToLower()))
            {
                output = MagickFormat.Sixel;
            }
            else if (tipo.ToLower().Equals("SparseColor".ToLower()))
            {
                output = MagickFormat.SparseColor;
            }
            else if (tipo.ToLower().Equals("Sr2".ToLower()))
            {
                output = MagickFormat.Sr2;
            }
            else if (tipo.ToLower().Equals("Srf".ToLower()))
            {
                output = MagickFormat.Srf;
            }
            else if (tipo.ToLower().Equals("Stegano".ToLower()))
            {
                output = MagickFormat.Stegano;
            }
            else if (tipo.ToLower().Equals("StrImg".ToLower()))
            {
                output = MagickFormat.StrImg;
            }
            else if (tipo.ToLower().Equals("Sun".ToLower()))
            {
                output = MagickFormat.Sun;
            }
            else if (tipo.ToLower().Equals("Svg".ToLower()))
            {
                output = MagickFormat.Svg;
            }
            else if (tipo.ToLower().Equals("Svgz".ToLower()))
            {
                output = MagickFormat.Svgz;
            }
            else if (tipo.ToLower().Equals("Text".ToLower()))
            {
                output = MagickFormat.Text;
            }
            else if (tipo.ToLower().Equals("Tga".ToLower()))
            {
                output = MagickFormat.Tga;
            }
            else if (tipo.ToLower().Equals("Thumbnail".ToLower()))
            {
                output = MagickFormat.Thumbnail;
            }
            else if (tipo.ToLower().Equals("Tif".ToLower()))
            {
                output = MagickFormat.Tif;
            }
            else if (tipo.ToLower().Equals("Tiff".ToLower()))
            {
                output = MagickFormat.Tiff;
            }
            else if (tipo.ToLower().Equals("Tiff64".ToLower()))
            {
                output = MagickFormat.Tiff64;
            }
            else if (tipo.ToLower().Equals("Tile".ToLower()))
            {
                output = MagickFormat.Tile;
            }
            else if (tipo.ToLower().Equals("Tim".ToLower()))
            {
                output = MagickFormat.Tim;
            }
            else if (tipo.ToLower().Equals("Tm2".ToLower()))
            {
                output = MagickFormat.Tm2;
            }
            else if (tipo.ToLower().Equals("Ttc".ToLower()))
            {
                output = MagickFormat.Ttc;
            }
            else if (tipo.ToLower().Equals("Ttf".ToLower()))
            {
                output = MagickFormat.Ttf;
            }
            else if (tipo.ToLower().Equals("Txt".ToLower()))
            {
                output = MagickFormat.Txt;
            }
            else if (tipo.ToLower().Equals("Ubrl".ToLower()))
            {
                output = MagickFormat.Ubrl;
            }
            else if (tipo.ToLower().Equals("Ubrl6".ToLower()))
            {
                output = MagickFormat.Ubrl6;
            }
            else if (tipo.ToLower().Equals("Uil".ToLower()))
            {
                output = MagickFormat.Uil;
            }
            else if (tipo.ToLower().Equals("Uyvy".ToLower()))
            {
                output = MagickFormat.Uyvy;
            }
            else if (tipo.ToLower().Equals("Vda".ToLower()))
            {
                output = MagickFormat.Vda;
            }
            else if (tipo.ToLower().Equals("Vicar".ToLower()))
            {
                output = MagickFormat.Vicar;
            }
            else if (tipo.ToLower().Equals("Vid".ToLower()))
            {
                output = MagickFormat.Vid;
            }
            else if (tipo.ToLower().Equals("WebM".ToLower()))
            {
                output = MagickFormat.WebM;
            }
            else if (tipo.ToLower().Equals("Viff".ToLower()))
            {
                output = MagickFormat.Viff;
            }
            else if (tipo.ToLower().Equals("Vips".ToLower()))
            {
                output = MagickFormat.Vips;
            }
            else if (tipo.ToLower().Equals("Vst".ToLower()))
            {
                output = MagickFormat.Vst;
            }
            else if (tipo.ToLower().Equals("WebP".ToLower()))
            {
                output = MagickFormat.WebP;
            }
            else if (tipo.ToLower().Equals("Wbmp".ToLower()))
            {
                output = MagickFormat.Wbmp;
            }
            else if (tipo.ToLower().Equals("Wmf".ToLower()))
            {
                output = MagickFormat.Wmf;
            }
            else if (tipo.ToLower().Equals("Wmv".ToLower()))
            {
                output = MagickFormat.Wmv;
            }
            else if (tipo.ToLower().Equals("Wpg".ToLower()))
            {
                output = MagickFormat.Wpg;
            }
            else if (tipo.ToLower().Equals("X3f".ToLower()))
            {
                output = MagickFormat.X3f;
            }
            else if (tipo.ToLower().Equals("Xbm".ToLower()))
            {
                output = MagickFormat.Xbm;
            }
            else if (tipo.ToLower().Equals("Xc".ToLower()))
            {
                output = MagickFormat.Xc;
            }
            else if (tipo.ToLower().Equals("Xcf".ToLower()))
            {
                output = MagickFormat.Xcf;
            }
            else if (tipo.ToLower().Equals("Xpm".ToLower()))
            {
                output = MagickFormat.Xpm;
            }
            else if (tipo.ToLower().Equals("Xps".ToLower()))
            {
                output = MagickFormat.Xps;
            }
            else if (tipo.ToLower().Equals("Xv".ToLower()))
            {
                output = MagickFormat.Xv;
            }
            else if (tipo.ToLower().Equals("Y".ToLower()))
            {
                output = MagickFormat.Y;
            }
            else if (tipo.ToLower().Equals("Yaml".ToLower()))
            {
                output = MagickFormat.Yaml;
            }
            else if (tipo.ToLower().Equals("Ycbcr".ToLower()))
            {
                output = MagickFormat.Ycbcr;
            }
            else if (tipo.ToLower().Equals("Ycbcra".ToLower()))
            {
                output = MagickFormat.Ycbcra;
            }
            else if (tipo.ToLower().Equals("Yuv".ToLower()))
            {
                output = MagickFormat.Yuv;
            }

            return output;
        }

        private bool IsPngImage(byte[] bytes)
        {
            if (bytes.Length < 8)
                return false;

            byte[] pngSignature = new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };
            for (int i = 0; i < 8; i++)
            {
                if (bytes[i] != pngSignature[i])
                    return false;
            }

            return true;
        }

        public void Dispose()
        {
        }
    }
}
