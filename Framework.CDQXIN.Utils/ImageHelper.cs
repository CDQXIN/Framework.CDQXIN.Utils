using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Framework.CDQXIN.Utils
{
    /// <summary>
    /// 图片工具类
    /// </summary>
    public class ImageHelper
    {
        #region 保存上传图片的方法
        /// <summary>
        /// 保存上传图片的方法
        /// </summary>
        /// <param name="file">上传的图片</param>
        /// <param name="imageLocalPath">图片物理路径</param>
        /// <param name="imageDbPath">图片数据库路径</param>
        /// <returns>上传结果</returns>
        public static int SavePicToServer(HttpPostedFile file, string imageLocalPath, ref string imageDbPath)
        {
            string errorMsg = "";
            return SavePicToServer(file, false, imageLocalPath, ref imageDbPath, new int[0], null, ref errorMsg);
        }
        /// <summary>
        /// 保存上传图片的方法
        /// </summary>
        /// <param name="file">上传的图片</param>
        /// <param name="imageLocalPath">图片物理路径</param>
        /// <param name="imageDbPath">图片数据库路径</param>
        /// <param name="errorMsg">异常信息</param>
        /// <returns>上传结果</returns>
        public static int SavePicToServer(HttpPostedFile file, string imageLocalPath, ref string imageDbPath, ref string errorMsg)
        {
            return SavePicToServer(file, false, imageLocalPath, ref imageDbPath, new int[0], null, ref errorMsg);
        }
        /// <summary>
        /// 保存上传图片的方法
        /// </summary>
        /// <param name="file">上传的图片</param>
        /// <param name="needAbbr">是否需要缩略图</param>
        /// <param name="imageLocalPath">图片物理路径</param>
        /// <param name="imageDbPath">图片数据库路径</param>
        /// <param name="width">不同规格的缩略图宽度</param>
        /// <param name="suffix">不同规格的缩略图后缀</param>
        /// <returns>上传结果</returns>
        public static int SavePicToServer(HttpPostedFile file, bool needAbbr, string imageLocalPath, ref string imageDbPath, int[] width, string[] suffix)
        {
            string errorMsg = "";
            return SavePicToServer(file, imageLocalPath, ref imageDbPath, needAbbr, width, suffix, false, WaterMarkType.Unknown, WaterMarkPosition.Unknown, null, ref errorMsg);
        }
        /// <summary>
        /// 保存上传图片的方法
        /// </summary>
        /// <param name="file">上传的图片</param>
        /// <param name="needAbbr">是否需要缩略图</param>
        /// <param name="imageLocalPath">图片物理路径</param>
        /// <param name="imageDbPath">图片数据库路径</param>
        /// <param name="width">不同规格的缩略图宽度</param>
        /// <param name="suffix">不同规格的缩略图后缀</param>
        /// <param name="errorMsg">异常信息</param>
        /// <returns>上传结果</returns>
        public static int SavePicToServer(HttpPostedFile file, bool needAbbr, string imageLocalPath, ref string imageDbPath, int[] width, string[] suffix, ref string errorMsg)
        {
            return SavePicToServer(file, imageLocalPath, ref imageDbPath, needAbbr, width, suffix, false, WaterMarkType.Unknown, WaterMarkPosition.Unknown, null, ref errorMsg);
        }
        /// <summary>
        /// 保存上传图片的方法
        /// </summary>
        /// <param name="file">上传的图片</param>
        /// <param name="imageLocalPath">图片物理路径</param>
        /// <param name="imageDbPath">图片数据库路径</param>
        /// <param name="needWaterMark">是否需要水印</param>
        /// <param name="type">水印类型</param>
        /// <param name="position">水印位置</param>
        /// <param name="content">水印内容</param>
        /// <returns>上传结果</returns>
        public static int SavePicToServer(HttpPostedFile file, string imageLocalPath, ref string imageDbPath, bool needWaterMark, WaterMarkType type, WaterMarkPosition position, string content)
        {
            string errorMsg = "";
            return SavePicToServer(file, imageLocalPath, ref imageDbPath, false, new int[0], null, needWaterMark, type, position, content, ref errorMsg);
        }
        /// <summary>
        /// 保存上传图片的方法
        /// </summary>
        /// <param name="file">上传的图片</param>
        /// <param name="imageLocalPath">图片物理路径</param>
        /// <param name="imageDbPath">图片数据库路径</param>
        /// <param name="needWaterMark">是否需要水印</param>
        /// <param name="type">水印类型</param>
        /// <param name="position">水印位置</param>
        /// <param name="content">水印内容</param>
        /// <param name="errorMsg">异常信息</param>
        /// <returns>上传结果</returns>
        public static int SavePicToServer(HttpPostedFile file, string imageLocalPath, ref string imageDbPath, bool needWaterMark, WaterMarkType type, WaterMarkPosition position, string content, ref string errorMsg)
        {
            return SavePicToServer(file, imageLocalPath, ref imageDbPath, false, new int[0], null, needWaterMark, type, position, content, ref errorMsg);
        }
        /// <summary>
        /// 保存上传图片的方法
        /// </summary>
        /// <param name="file">上传的图片</param>
        /// <param name="imageLocalPath">图片物理路径</param>
        /// <param name="imageDbPath">图片数据库路径</param>
        /// <param name="needAbbr">是否需要缩略图</param>
        /// <param name="width">不同规格的缩略图宽度</param>
        /// <param name="suffix">不同规格的缩略图后缀</param>
        /// <param name="needWaterMark">是否需要水印</param>
        /// <param name="type">水印类型</param>
        /// <param name="position">水印位置</param>
        /// <param name="content">水印内容</param>
        /// <returns>上传结果</returns>
        public static int SavePicToServer(HttpPostedFile file, string imageLocalPath, ref string imageDbPath, bool needAbbr, int[] width, string[] suffix, bool needWaterMark, WaterMarkType type, WaterMarkPosition position, string content)
        {
            string errorMsg = "";
            return SavePicToServer(file, imageLocalPath, ref imageDbPath, needAbbr, width, suffix, needWaterMark, type, position, content, ref errorMsg);
        }
        /// <summary>
        /// 保存上传图片的方法
        /// </summary>
        /// <param name="file">上传的图片</param>
        /// <param name="imageLocalPath">图片物理路径</param>
        /// <param name="imageDbPath">图片数据库路径</param>
        /// <param name="needAbbr">是否需要缩略图</param>
        /// <param name="width">不同规格的缩略图宽度</param>
        /// <param name="suffix">不同规格的缩略图后缀</param>
        /// <param name="needWaterMark">是否需要水印</param>
        /// <param name="type">水印类型</param>
        /// <param name="position">水印位置</param>
        /// <param name="content">水印内容</param>
        /// <param name="errorMsg">异常信息</param>
        /// <returns>上传结果</returns>
        public static int SavePicToServer(HttpPostedFile file, string imageLocalPath, ref string imageDbPath, bool needAbbr, int[] width, string[] suffix, bool needWaterMark, WaterMarkType type, WaterMarkPosition position, string content, ref string errorMsg)
        {
            int rtn = 1;
            try
            {
                imageDbPath = StringHelper.GetCurrentDateString();

                FileHelper.CreateDirectory(imageLocalPath + imageDbPath);

                string suffixName = FileHelper.GetSuffix(file.FileName);
                imageDbPath += @"\" + FileHelper.BuildFileNameByTime(suffixName);
                file.SaveAs(imageLocalPath + imageDbPath);

                if (needWaterMark)
                    AddWaterMark(imageLocalPath, ref imageDbPath, type, position, content);

                //压缩图片失败
                if (!CompressionImage(imageLocalPath + imageDbPath, 1024))
                {
                    errorMsg = "压缩图片失败。";
                    rtn = 0;
                }
                else if (needAbbr)
                {
                    for (int i = 0; i < width.Length; i++)
                    {
                        int state = 0;//0:表示复制 1:表示按宽度压缩 2:表示按高度压缩

                        Image objImg = Image.FromFile(imageLocalPath + imageDbPath);
                        if (objImg.Width < width[i] && objImg.Height < width[i])
                            state = 0;
                        else if (objImg.Width > objImg.Height)
                            state = 1;
                        else
                            state = 2;

                        objImg.Dispose();
                        if (state == 0)
                            File.Copy(imageLocalPath + imageDbPath, imageLocalPath + imageDbPath.Replace(".", suffix[i] + "."));
                        else if (state == 1)
                            AbbreviatoryImageWidth(imageLocalPath + imageDbPath, width[i], imageLocalPath + imageDbPath.Replace(".", suffix[i] + "."));
                        else
                            AbbreviatoryImageHeight(imageLocalPath + imageDbPath, width[i], imageLocalPath + imageDbPath.Replace(".", suffix[i] + "."));
                    }
                }
                imageDbPath = imageDbPath.Replace(@"\", @"/");
            }
            catch (Exception ex)
            {
                errorMsg = ex.StackTrace;
                rtn = 0;
            }
            return rtn;
        }
        #endregion

        #region 获得有效的图片地址的方法
        /// <summary>
        /// 获得有效的图片地址的方法
        /// </summary>
        /// <param name="url">数据库存储的图片地址</param>
        /// <param name="needSmallPic">是否需要规格图片</param>
        /// <returns>有效的图片地址</returns>
        public static string GetValidImageUrl(string url, bool needRegularPic)
        {
            return GetValidImageUrl(url, ConfigHelper.GetAppSetting("ImageServerPath"), ConfigHelper.GetAppSetting("DefaultImageServerPath"), needRegularPic);
        }
        /// <summary>
        /// 获得有效的图片地址的方法
        /// </summary>
        /// <param name="url">数据库存储的图片地址</param>
        /// <param name="picServerPath">图片服务器路径</param>
        /// <param name="defaultUrl">默认图片路径</param>
        /// <param name="needSmallPic">是否需要规格图片</param>
        /// <returns>有效的图片地址</returns>
        public static string GetValidImageUrl(string url, string picServerPath, string defaultUrl, bool needRegularPic)
        {
            if (needRegularPic)
                return GetValidImageUrl(url, "_small.", picServerPath, defaultUrl);

            if (url != "")
                return picServerPath + url;
            else
                return defaultUrl;
        }
        /// <summary>
        /// 获得有效的图片地址的方法
        /// </summary>
        /// <param name="url">数据库存储的图片地址</param>
        /// <param name="urlSuffix">图片路径后缀</param>
        /// <returns>有效的图片地址</returns>
        public static string GetValidImageUrl(string url, string urlSuffix)
        {
            return GetValidImageUrl(url, urlSuffix, ConfigHelper.GetAppSetting("ImageServerPath"), ConfigHelper.GetAppSetting("DefaultImageServerPath"));
        }
        /// <summary>
        /// 获得有效的图片地址的方法
        /// </summary>
        /// <param name="url">数据库存储的图片地址</param>
        /// <param name="urlSuffix">图片路径后缀</param>
        /// <param name="picServerPath">图片服务器路径</param>
        /// <param name="defaultUrl">默认图片路径</param>
        /// <returns>有效的图片地址</returns>
        public static string GetValidImageUrl(string url, string urlSuffix, string picServerPath, string defaultUrl)
        {
            url = url.Replace(".", urlSuffix);

            if (url != "")
                return picServerPath + url;
            else
                return defaultUrl;
        }
        #endregion

        #region 图片压缩的方法
        /// <summary>
        /// 图片压缩的方法
        /// </summary>
        /// <param name="_srcImage">原有图片路径</param>
        /// <param name="width">要压缩的宽度</param>
        /// <returns>true:成功 false:失败</returns>
        public static bool CompressionImage(string _srcImage, int width)
        {
            if (CheckSize(_srcImage, width))
            {
                if (ImageHelper.AbbreviatoryImageWidth(_srcImage, width, _srcImage.Replace(".", "_.")))
                {
                    GC.Collect();
                    //FileHelper.RemoveFile(_srcImage);
                    //FileHelper.ReNameFile(_srcImage.Replace(".", "_."), _srcImage, false);
                    System.IO.File.Copy(_srcImage.Replace(".", "_."), _srcImage, true);
                    System.IO.File.Delete(_srcImage.Replace(".", "_."));
                    return true;
                }
                else
                    return false;
            }
            else
                return true;
        }

        /// <summary>
        /// 图片压缩的方法
        /// </summary>
        /// <param name="_srcImage">原有图片路径</param>
        /// <param name="width">要压缩的宽度</param>
        /// <returns>true:成功 false:失败</returns>
        public static Stream CompressionImage(Stream _srcImage, int width, string saveFile)
        {
            Stream stream = new MemoryStream();
            if (CheckSize(_srcImage, width))
            {
                stream = ImageHelper.AbbreviatoryImageWidth(_srcImage, width, saveFile);
                return stream;
            }
            else
            {
                Image img = Image.FromStream(_srcImage);
                img.Save(saveFile);
                FileStream fileStream = new FileStream(saveFile, FileMode.Open);
                return fileStream;
            }
        }

        #endregion

        #region 按宽度压缩图片的方法
        /// <summary>
        /// 按宽度压缩图片的方法
        /// </summary>
        /// <param name="_srcImage">图片完整路径</param>
        /// <param name="_width">图片压缩宽度</param>
        /// <param name="_fileName">压缩后的文件名</param>
        /// <returns>压缩结果</returns>
        public static bool AbbreviatoryImageWidth(string _srcImage, int _width, string _fileName)
        {
            bool rtn = true;
            Image srcImage = null;
            try
            {
                srcImage = Image.FromFile(_srcImage);
                Image.GetThumbnailImageAbort callb = new Image.GetThumbnailImageAbort(ThumbnailCallback);
                int srcwidth = srcImage.Width;
                decimal srcheight = Convert.ToDecimal(srcImage.Height);
                decimal rate = Convert.ToDecimal(srcwidth) / _width;
                int tgtheight = Convert.ToInt32(srcImage.Height / rate);

                rtn = CompressImage(_srcImage, new Size(_width, tgtheight), _fileName);
            }
            catch
            {
                rtn = false;
            }
            finally
            {
                srcImage.Dispose();
            }

            return rtn;
        }

        /// <summary>
        /// 按宽度压缩图片的方法
        /// </summary>
        /// <param name="_srcImage">图片完整路径</param>
        /// <param name="_width">图片压缩宽度</param>
        /// <param name="_fileName">压缩后的文件名</param>
        /// <returns>压缩结果</returns>
        public static Stream AbbreviatoryImageWidth(Stream _srcImage, int _width, string saveFile)
        {
            Stream newstream = new MemoryStream();
            Image srcImage = null;
            try
            {
                srcImage = Image.FromStream(_srcImage);
                Image.GetThumbnailImageAbort callb = new Image.GetThumbnailImageAbort(ThumbnailCallback);
                int srcwidth = srcImage.Width;
                decimal srcheight = Convert.ToDecimal(srcImage.Height);
                decimal rate = Convert.ToDecimal(srcwidth) / _width;
                int tgtheight = Convert.ToInt32(srcImage.Height / rate);

                newstream = CompressImage(_srcImage, new Size(_width, tgtheight), saveFile);
            }
            catch
            {
                newstream.Dispose();
            }
            return newstream;
        }
        #endregion

        #region 按高度压缩图片的方法
        /// <summary>
        /// 按高度压缩图片的方法
        /// </summary>
        /// <param name="_srcImage">图片完整路径</param>
        /// <param name="_height">图片压缩高度</param>
        /// <param name="_fileName">压缩后的文件名</param>
        /// <returns>压缩结果</returns>
        public static bool AbbreviatoryImageHeight(string _srcImage, int _height, string _fileName)
        {
            bool rtn = true;
            Image srcImage = null;
            try
            {
                srcImage = Image.FromFile(_srcImage);
                Image.GetThumbnailImageAbort callb = new Image.GetThumbnailImageAbort(ThumbnailCallback);
                int srcHeight = srcImage.Height;
                decimal srcWidth = Convert.ToDecimal(srcImage.Width);
                decimal rate = Convert.ToDecimal(srcHeight) / _height;
                int tgtWidth = Convert.ToInt32(srcImage.Width / rate);

                rtn = CompressImage(_srcImage, new Size(tgtWidth, _height), _fileName);
            }
            catch
            {
                rtn = false;
            }
            finally
            {
                srcImage.Dispose();
            }

            return rtn;
        }
        #endregion

        #region 创建缩略图的方法
        /// <summary>
        /// 创建缩略图的方法
        /// </summary>
        /// <param name="_srcImage">图片完整路径</param>
        /// <param name="_width">图片压缩宽度</param>
        public static void CreateAbbreviatoryImage(string _srcImage, int _width)
        {
            try
            {
                Image srcImage = Image.FromFile(_srcImage);
                Image.GetThumbnailImageAbort callb = new Image.GetThumbnailImageAbort(ThumbnailCallback);
                int srcwidth = srcImage.Width;
                decimal srcheight = Convert.ToDecimal(srcImage.Height);
                decimal rate = Convert.ToDecimal(srcwidth) / _width;
                int tgtheight = Convert.ToInt32(srcImage.Height / rate);

                Image tgtImage;
                tgtImage = srcImage.GetThumbnailImage(_width, tgtheight, callb, new System.IntPtr());

                string Suffix = "";
                string _abbImage = GetAbbrevaiatoryImageName(_srcImage, ref Suffix);
                tgtImage.Save(_abbImage, (Suffix == "") ? ImageFormat.Jpeg : GetImageType(Suffix));
                srcImage.Dispose();
                tgtImage.Dispose();
            }
            catch (Exception ex)
            {
                string strException = ex.Message;
            }
        }
        /// <summary>
        /// 创建缩略图的方法
        /// </summary>
        /// <param name="_srcImage">图片完整路径</param>
        /// <param name="_width">压缩宽度</param>
        /// <param name="_abbImage">压缩图片名称</param>
        /// <returns>压缩结果</returns>
        public static bool CreateAbbreviatoryImage(string _srcImage, int _width, ref string _abbImage)
        {
            bool rtn = false;
            try
            {
                Image srcImage = Image.FromFile(_srcImage);
                Image.GetThumbnailImageAbort callb = new Image.GetThumbnailImageAbort(ThumbnailCallback);
                int srcwidth = srcImage.Width;
                decimal srcheight = Convert.ToDecimal(srcImage.Height);
                decimal rate = Convert.ToDecimal(srcwidth) / _width;
                int tgtheight = Convert.ToInt32(srcImage.Height / rate);

                Image tgtImage;
                tgtImage = srcImage.GetThumbnailImage(_width, tgtheight, callb, new System.IntPtr());

                string Suffix = "";
                _abbImage = GetAbbrevaiatoryImageName(_srcImage, ref Suffix);
                tgtImage.Save(_abbImage, (Suffix == "") ? ImageFormat.Jpeg : GetImageType(Suffix));
                srcImage.Dispose();
                tgtImage.Dispose();

                rtn = true;
            }
            catch (Exception ex)
            {
                string strException = ex.Message;
                rtn = false;
            }

            return rtn;
        }
        #endregion

        #region 判断图片大小是否合格的方法
        /// <summary>
        /// 判断图片大小是否合格的方法
        /// </summary>
        /// <param name="_width">允许宽度</param>
        /// <param name="_height">允许高度</param>
        /// <param name="_srcImage">图片完整路径</param>
        /// <returns>判断结果</returns>
        public static bool RegularPicSize(int _width, int _height, string _srcImage)
        {
            bool rtn = false;
            string suffix = FileHelper.GetSuffix(_srcImage);
            if (ExcludeRegular.IndexOf(suffix) != -1) return true;
            System.Drawing.Image srcImage = System.Drawing.Image.FromFile(_srcImage);
            if (srcImage.Width <= _width && srcImage.Height <= _height)
                rtn = true;
            srcImage.Dispose();

            return rtn;
        }
        #endregion

        #region 获得压缩图片名称的方法
        /// <summary>
        /// 获得压缩图片名称的方法
        /// </summary>
        /// <param name="_srcImage">图片完整路径</param>
        /// <returns>压缩结果</returns>
        public static string GetAbbrevaiatoryImageName(string _srcImage)
        {
            string _suffix = "";
            return GetAbbrevaiatoryImageName(_srcImage, ref _suffix);
        }
        /// <summary>
        /// 获得压缩图片名称的方法
        /// </summary>
        /// <param name="_srcImage">图片完整路径</param>
        /// <param name="_suffix">扩展名</param>
        /// <returns>压缩结果</returns>
        public static string GetAbbrevaiatoryImageName(string _srcImage, ref string _suffix)
        {
            int index = _srcImage.LastIndexOf(".");
            _suffix = _srcImage.Substring(index, _srcImage.Length - index);
            _srcImage = _srcImage.Substring(0, index);
            _srcImage += "_small";
            _srcImage += _suffix;

            return _srcImage;
        }
        #endregion

        #region 添加水印的方法
        /// <summary>
        /// 添加水印的方法
        /// </summary>
        /// <param name="imageLocalPath">图片物理路径</param>
        /// <param name="imageDbPath">图片数据库路径</param>
        /// <param name="type">水印类型</param>
        /// <param name="position">水印位置</param>
        /// <param name="content">水印内容</param>
        public static void AddWaterMark(string imageLocalPath, ref string imageDbPath, WaterMarkType type, WaterMarkPosition position, string content)
        {
            string oldPath = imageLocalPath + imageDbPath;
            Image image = Image.FromFile(oldPath);

            Bitmap b = new Bitmap(image.Width, image.Height, PixelFormat.Format24bppRgb);
            Graphics g = Graphics.FromImage(b);
            g.Clear(Color.White);
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.CompositingQuality = CompositingQuality.HighQuality;

            g.DrawImage(image, 0, 0, image.Width, image.Height);

            switch (type)
            {
                //如果是图片              
                case WaterMarkType.Image:
                    AddWatermarkImage(g, content, position, image.Width, image.Height);
                    break;
                //如果是文字                    
                case WaterMarkType.Text:
                    AddWatermarkText(g, content, position, image.Width, image.Height);
                    break;
            }

            int index = imageDbPath.LastIndexOf("\\");
            imageDbPath = imageDbPath.Insert(index + 1, "wm");

            b.Save(imageLocalPath + imageDbPath);
            b.Dispose();
            image.Dispose();

            FileHelper.RemoveFile(oldPath);
        }
        #endregion

        #region 私有方法

        private const string ExcludeRegular = ".swf";

        #region 判断图片宽度是否大于指定宽度的方法
        private static bool CheckSize(string _srcImage, int width)
        {
            bool rtn = false;
            Image objImg = Image.FromFile(_srcImage);
            if (objImg.Width > width)
            {
                rtn = true;
            }
            objImg.Dispose();

            return rtn;
        }

        private static bool CheckSize(Stream _srcImage, int width)
        {
            bool rtn = false;
            Image objImg = Image.FromStream(_srcImage);
            if (objImg.Width > width)
            {
                rtn = true;
            }
            objImg.Dispose();

            return rtn;
        }

        #endregion

        #region 压缩图片的方法
        private static bool CompressImage(string fileName, Size newSize, string newFile)
        {
            bool rtn = true;

            Image img = null;
            Bitmap outBmp = null;

            try
            {
                img = Image.FromFile(fileName);
                ImageFormat thisFormat = img.RawFormat;
                outBmp = new Bitmap(img, newSize);
                Graphics g = Graphics.FromImage(outBmp);
                // 设置画布的描绘质量
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(img, new Rectangle(0, 0, newSize.Width, newSize.Height), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel);
                g.Dispose();
                // 以下代码为保存图片时,设置压缩质量
                EncoderParameters encoderParams = new EncoderParameters();
                long[] quality = new long[1];
                quality[0] = 90;
                EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
                encoderParams.Param[0] = encoderParam;
                //获得包含有关内置图像编码解码器的信息的ImageCodecInfo 对象.
                ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo jpegICI = null;
                for (int i = 0; i < arrayICI.Length; i++)
                {
                    if (arrayICI[i].FormatDescription.Equals("JPEG"))
                    {
                        jpegICI = arrayICI[i];
                        //设置JPEG编码
                        break;
                    }
                }
                if (jpegICI != null)
                {
                    outBmp.Save(newFile, jpegICI, encoderParams);
                }
                else
                {
                    outBmp.Save(newFile, thisFormat);
                }
            }
            catch
            {
                rtn = false;
            }
            finally
            {
                if (img != null)
                {
                    img.Dispose();
                }

                if (outBmp != null)
                {
                    outBmp.Dispose();
                }
            }

            return rtn;

            //bool rtn = true;

            //try
            //{
            //    ASPJPEGLib.IASPJpeg objJpeg = new ASPJPEGLib.ASPJpeg();
            //    objJpeg.Open(fileName);

            //    //保证截取原图下缩略图成比例的原图
            //    decimal width = 0;
            //    decimal height = 0;
            //    if (objJpeg.OriginalWidth < objJpeg.OriginalHeight)
            //    {
            //        height = newSize.Height;
            //        width = Convert.ToInt32(height / objJpeg.OriginalHeight * objJpeg.OriginalWidth);
            //    }
            //    else
            //    {
            //        width = newSize.Width;
            //        height = Convert.ToInt32(width / objJpeg.OriginalWidth * objJpeg.OriginalHeight);
            //    }

            //    objJpeg.Width = Convert.ToInt32(width);
            //    objJpeg.Height = Convert.ToInt32(height);

            //    objJpeg.Save(newFile);
            //}
            //catch
            //{
            //    rtn = false;
            //}

            //return rtn;
        }

        private static Stream CompressImage(Stream fileName, Size newSize, string saveFile)
        {
            Image img = null;
            Bitmap outBmp = null;

            try
            {
                img = Image.FromStream(fileName);
                ImageFormat thisFormat = img.RawFormat;
                outBmp = new Bitmap(img, newSize);
                Graphics g = Graphics.FromImage(outBmp);
                // 设置画布的描绘质量
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(img, new Rectangle(0, 0, newSize.Width, newSize.Height), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel);
                g.Dispose();
                // 以下代码为保存图片时,设置压缩质量
                EncoderParameters encoderParams = new EncoderParameters();
                long[] quality = new long[1];
                quality[0] = 90;
                EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
                encoderParams.Param[0] = encoderParam;
                //获得包含有关内置图像编码解码器的信息的ImageCodecInfo 对象.
                ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo jpegICI = null;
                for (int i = 0; i < arrayICI.Length; i++)
                {
                    if (arrayICI[i].FormatDescription.Equals("JPEG"))
                    {
                        jpegICI = arrayICI[i];
                        //设置JPEG编码
                        break;
                    }
                }
                if (jpegICI != null)
                {
                    outBmp.Save(saveFile, jpegICI, encoderParams);
                }
                else
                {
                    outBmp.Save(saveFile, thisFormat);
                }
                FileStream fileStream = new FileStream(saveFile, FileMode.Open);
                return fileStream;
            }
            catch
            {
            }
            return null;
        }

        #endregion

        #region 获得图片编码解码器信息的方法
        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }
        #endregion

        #region 获得类型字符串的方法
        private static string GetMimeType(string _file)
        {
            string rtn = "image/jpeg";
            if (_file.ToLower().IndexOf(".gif") > 0)
                rtn = "image/gif";
            else if (_file.ToLower().IndexOf(".png") > 0)
                rtn = "image/png";

            return rtn;
        }
        #endregion

        #region ThumbnailCallback
        private static bool ThumbnailCallback()
        {
            return false;
        }
        #endregion

        #region 获取图片格式的方法
        private static ImageFormat GetImageType(string _ContentType)
        {
            _ContentType = _ContentType.ToLower();
            ImageFormat format;
            if (_ContentType == ".jpeg" || _ContentType == ".jpg")
            {
                format = ImageFormat.Jpeg;
            }
            else if (_ContentType == ".gif")
            {
                format = ImageFormat.Gif;
            }
            else if (_ContentType == ".bmp")
            {
                format = ImageFormat.Bmp;
            }
            else if (_ContentType == ".tiff")
            {
                format = ImageFormat.Tiff;
            }
            else if (_ContentType == ".png")
            {
                format = ImageFormat.Png;
            }
            else
            {
                format = ImageFormat.MemoryBmp;
            }

            return format;
        }
        #endregion

        #region 加水印文字的方法
        /// <summary>
        /// 加水印文字的方法
        /// </summary>
        /// <param name="picture">Graphics 对象</param>
        /// <param name="_watermarkText">水印文字内容</param>
        /// <param name="_watermarkPosition">水印位置</param>
        /// <param name="_width">被加水印图片的宽</param>
        /// <param name="_height">被加水印图片的高</param>
        private static void AddWatermarkText(Graphics picture, string _watermarkText, WaterMarkPosition _watermarkPosition, int _width, int _height)
        {
            int[] sizes = new int[] { 40, 36, 32, 28, 24, 20, 16, 12 };
            Font crFont = null;
            SizeF crSize = new SizeF();
            for (int i = 0; i < 8; i++)
            {
                crFont = new Font("arial", sizes[i], FontStyle.Bold);
                crSize = picture.MeasureString(_watermarkText, crFont);

                if ((ushort)crSize.Width < (ushort)_width)
                    break;
            }

            float xpos = 0;
            float ypos = 0;

            switch (_watermarkPosition)
            {
                case WaterMarkPosition.TopLeft:
                    xpos = ((float)_width * (float).01) + (crSize.Width / 2);
                    ypos = (float)_height * (float).01;
                    break;
                case WaterMarkPosition.TopRight:
                    xpos = ((float)_width * (float).99) - (crSize.Width / 2);
                    ypos = (float)_height * (float).01;
                    break;
                case WaterMarkPosition.BottomRight:
                    xpos = ((float)_width * (float).99) - (crSize.Width / 2);
                    ypos = ((float)_height * (float).99) - crSize.Height;
                    break;
                case WaterMarkPosition.BottomLeft:
                    xpos = ((float)_width * (float).01) + (crSize.Width / 2);
                    ypos = ((float)_height * (float).99) - crSize.Height;
                    break;
            }

            StringFormat StrFormat = new StringFormat();
            StrFormat.Alignment = StringAlignment.Center;

            SolidBrush semiTransBrush2 = new SolidBrush(Color.FromArgb(153, 0, 0, 0));
            picture.DrawString(_watermarkText, crFont, semiTransBrush2, xpos + 1, ypos + 1, StrFormat);

            SolidBrush semiTransBrush = new SolidBrush(Color.FromArgb(153, 255, 255, 255));
            picture.DrawString(_watermarkText, crFont, semiTransBrush, xpos, ypos, StrFormat);

            semiTransBrush2.Dispose();
            semiTransBrush.Dispose();
        }
        #endregion

        #region 加水印图片的方法
        /// <summary>
        /// 加水印图片的方法
        /// </summary>
        /// <param name="picture">Graphics 对象</param>
        /// <param name="WaterMarkPicPath">水印图片的地址</param>
        /// <param name="_watermarkPosition">水印位置</param>
        /// <param name="_width">被加水印图片的宽</param>
        /// <param name="_height">被加水印图片的高</param>
        private static void AddWatermarkImage(Graphics picture, string WaterMarkPicPath, WaterMarkPosition _watermarkPosition, int _width, int _height)
        {
            Image watermark = new Bitmap(WaterMarkPicPath);

            ImageAttributes imageAttributes = new ImageAttributes();
            ColorMap colorMap = new ColorMap();

            colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
            colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);
            ColorMap[] remapTable = { colorMap };

            imageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);

            float[][] colorMatrixElements = {
                                                new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},
                                                new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},
                                                new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f},
                                                new float[] {0.0f,  0.0f,  0.0f,  0.3f, 0.0f},
                                                new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}
                                            };

            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);

            imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

            int xpos = 0;
            int ypos = 0;
            int WatermarkWidth = 0;
            int WatermarkHeight = 0;
            double bl = 1d;
            //计算水印图片的比率
            //取背景的1/4宽度来比较
            if ((_width > watermark.Width * 4) && (_height > watermark.Height * 4))
            {
                bl = 1;
            }
            else if ((_width > watermark.Width * 4) && (_height < watermark.Height * 4))
            {
                bl = Convert.ToDouble(_height / 4) / Convert.ToDouble(watermark.Height);

            }
            else

                if ((_width < watermark.Width * 4) && (_height > watermark.Height * 4))
            {
                bl = Convert.ToDouble(_width / 4) / Convert.ToDouble(watermark.Width);
            }
            else
            {
                if ((_width * watermark.Height) > (_height * watermark.Width))
                {
                    bl = Convert.ToDouble(_height / 4) / Convert.ToDouble(watermark.Height);
                }
                else
                {
                    bl = Convert.ToDouble(_width / 4) / Convert.ToDouble(watermark.Width);
                }

            }

            WatermarkWidth = Convert.ToInt32(watermark.Width * bl);
            WatermarkHeight = Convert.ToInt32(watermark.Height * bl);

            switch (_watermarkPosition)
            {
                case WaterMarkPosition.TopLeft:
                    xpos = 10;
                    ypos = 10;
                    break;
                case WaterMarkPosition.TopRight:
                    xpos = _width - WatermarkWidth - 10;
                    ypos = 10;
                    break;
                case WaterMarkPosition.BottomRight:
                    xpos = _width - WatermarkWidth - 10;
                    ypos = _height - WatermarkHeight - 10;
                    break;
                case WaterMarkPosition.BottomLeft:
                    xpos = 10;
                    ypos = _height - WatermarkHeight - 10;
                    break;
            }

            picture.DrawImage(watermark, new Rectangle(xpos, ypos, WatermarkWidth, WatermarkHeight), 0, 0, watermark.Width, watermark.Height, GraphicsUnit.Pixel, imageAttributes);

            watermark.Dispose();
            imageAttributes.Dispose();
        }
        #endregion

        #endregion
    }

    #region 水印类型
    /// <summary>
    /// 水印类型
    /// </summary>
    public enum WaterMarkType
    {
        Image,
        Text,
        Unknown
    }
    #endregion

    #region 图片水印的位置
    /// <summary>
    /// 图片水印的位置
    /// </summary>
    public enum WaterMarkPosition
    {
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight,
        Unknown
    }
    #endregion
}
