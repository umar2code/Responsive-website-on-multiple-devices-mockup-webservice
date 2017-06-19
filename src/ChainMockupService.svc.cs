using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Web.Hosting;

namespace chained_mockup_generator
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class ChainMockupService : IChainMockupService
    {

        public string HelloWorld()
        {
            return "Hello World!";
        }

        public string ChainMockup(string Companyname, string Phone_no, string Street, string Zip, string City, string Accentcolor, string Textcolor, string Mainpicture, string Button_CTA_Text, string ID, string LogoImageURL)
        //public string ChainMockup(string Companyname = "Anna Didden", string Phone_no = "798115437", string Street = "Gerbergasse 14", string Zip = "4001", string City = "Basel", string Accentcolor = "#47b403", string Textcolor = "#444444", string Mainpicture = "http://placehold.it/860x500/3369E8/ffffff", string Button_CTA_Text = "Jetzt zum Angebot", string ID = "1", string LogoImageURL = "")
        //public string ChainMockup()
        {
            //string Companyname = "Anna Didden";
            //string Phone_no = "798115437";
            //string Street = "Gerbergasse 14";
            //string Zip = "4001";
            //string City = "Basel";
            //string Accentcolor = "#47b403";
            //string Textcolor = "#444444";
            //string Mainpicture = "http://placehold.it/860x500/3369E8/ffffff";
            //string Button_CTA_Text = "Jetzt zum Angebot";
            //string ID = "1";
            //string LogoImageURL = "";

            WriteLog("Calling params: ");
            WriteLog("Companyname = " + Companyname);
            WriteLog("Phone_no = " + Phone_no);
            WriteLog("Street = " + Street);
            WriteLog("Zip = " + Zip);
            WriteLog("City = " + City);
            WriteLog("Accentcolor = " + Accentcolor);
            WriteLog("Textcolor = " + Textcolor);
            WriteLog("Mainpicture = " + Mainpicture);
            WriteLog("Button_CTA_Text = " + Button_CTA_Text);
            WriteLog("ID = " + ID);
            WriteLog("LogoImageURL = " + LogoImageURL);

            string ret = string.Empty;

            //string CSVseparator = ";";
            string ImageMagickPath = string.Empty;
            bool Logoimage = false;

            string SrcPath = "images";
            bool TransparentBackgroud = false;

            #region reading config file
            WriteLog("reading config file");

            //if (ConfigurationManager.AppSettings["InputFilePath"] != null && ConfigurationManager.AppSettings["InputFilePath"].Length > 0) InputFilePath = ConfigurationManager.AppSettings["InputFilePath"].Trim();
            //if (ConfigurationManager.AppSettings["CSVseparator"] != null && ConfigurationManager.AppSettings["CSVseparator"].Length > 0) CSVseparator = ConfigurationManager.AppSettings["CSVseparator"].Trim();
            if (ConfigurationManager.AppSettings["ImageMagickPath"] != null && ConfigurationManager.AppSettings["ImageMagickPath"].Length > 0) ImageMagickPath = ConfigurationManager.AppSettings["ImageMagickPath"].Trim();
            if (ConfigurationManager.AppSettings["Logoimage"] != null && ConfigurationManager.AppSettings["Logoimage"].Length > 0) Logoimage = (ConfigurationManager.AppSettings["Logoimage"].Trim().ToUpper() == "TRUE");
            //if (ConfigurationManager.AppSettings["SrcPath"] != null && ConfigurationManager.AppSettings["SrcPath"].Length > 0) SrcPath = ConfigurationManager.AppSettings["SrcPath"].Trim();
            if (ConfigurationManager.AppSettings["TransparentBackgroud"] != null && ConfigurationManager.AppSettings["TransparentBackgroud"].Length > 0) TransparentBackgroud = ConfigurationManager.AppSettings["TransparentBackgroud"].Trim().ToUpper() == "TRUE";

            #region image params for phase 1
            WriteLog("image params for phase 1");

            List<ImageParams1> ImageParams1 = new List<ImageParams1>();


            for (int img_num = 1; img_num <= 3; img_num++)
            {
                ImageParams1 image = new ImageParams1();

                int temp;
                string param_name = "img" + img_num + "_type";
                if (ConfigurationManager.AppSettings[param_name] != null && ConfigurationManager.AppSettings[param_name].Length > 0)
                    image.img_type = ConfigurationManager.AppSettings[param_name].Trim();
                else { WriteLog("param read error: " + param_name); continue; }

                param_name = "img" + img_num + "_used_in";
                if (ConfigurationManager.AppSettings[param_name] != null && ConfigurationManager.AppSettings[param_name].Length > 0)
                    image.img_used_in = ConfigurationManager.AppSettings[param_name].Trim().Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                else { WriteLog("param read error: " + param_name); continue; }

                param_name = "img" + img_num + "_width";
                if (ConfigurationManager.AppSettings[param_name] != null && ConfigurationManager.AppSettings[param_name].Length > 0)
                    if (int.TryParse(ConfigurationManager.AppSettings[param_name].Trim(), out temp)) image.img_width = temp;
                    else { WriteLog("param parse error: " + param_name); continue; }
                else { WriteLog("param read error: " + param_name); continue; }

                param_name = "img" + img_num + "_height";
                if (ConfigurationManager.AppSettings[param_name] != null && ConfigurationManager.AppSettings[param_name].Length > 0)
                    if (int.TryParse(ConfigurationManager.AppSettings[param_name].Trim(), out temp)) image.img_height = temp;
                    else { WriteLog("param parse error: " + param_name); continue; }
                else { WriteLog("param read error: " + param_name); continue; }


                param_name = "img" + img_num + "_top_offset";
                if (ConfigurationManager.AppSettings[param_name] != null && ConfigurationManager.AppSettings[param_name].Length > 0)
                    if (int.TryParse(ConfigurationManager.AppSettings[param_name].Trim(), out temp)) image.img_top_offset = temp;
                    else { WriteLog("param parse error: " + param_name); continue; }
                else { WriteLog("param read error: " + param_name); continue; }

                param_name = "img" + img_num + "_inner_width";
                if (ConfigurationManager.AppSettings[param_name] != null && ConfigurationManager.AppSettings[param_name].Length > 0)
                    if (int.TryParse(ConfigurationManager.AppSettings[param_name].Trim(), out temp)) image.img_inner_width = temp;
                    else { WriteLog("param parse error: " + param_name); continue; }
                else { WriteLog("param read error: " + param_name); continue; }


                param_name = "img" + img_num + "_inner_poz";
                if (ConfigurationManager.AppSettings[param_name] != null && ConfigurationManager.AppSettings[param_name].Length > 0)
                    if (int.TryParse(ConfigurationManager.AppSettings[param_name].Trim(), out temp)) image.img_inner_poz = temp;
                    else { WriteLog("param parse error: " + param_name); continue; }
                else { WriteLog("param read error: " + param_name); continue; }


                param_name = "img" + img_num + "_inner_bg_color";
                if (ConfigurationManager.AppSettings[param_name] != null && ConfigurationManager.AppSettings[param_name].Length > 0)
                    image.img_inner_bg_color = ConfigurationManager.AppSettings[param_name].Trim();
                else { WriteLog("param read error: " + param_name); continue; }


                param_name = "img" + img_num + "_head_width";
                if (ConfigurationManager.AppSettings[param_name] != null && ConfigurationManager.AppSettings[param_name].Length > 0)
                    if (int.TryParse(ConfigurationManager.AppSettings[param_name].Trim(), out temp)) image.img_head_width = temp;
                    else { WriteLog("param parse error: " + param_name); continue; }
                else { WriteLog("param read error: " + param_name); continue; }

                param_name = "img" + img_num + "_head_height";
                if (ConfigurationManager.AppSettings[param_name] != null && ConfigurationManager.AppSettings[param_name].Length > 0)
                    if (int.TryParse(ConfigurationManager.AppSettings[param_name].Trim(), out temp)) image.img_head_height = temp;
                    else { WriteLog("param parse error: " + param_name); continue; }
                else { WriteLog("param read error: " + param_name); continue; }


                param_name = "img" + img_num + "_head_font_size";
                if (ConfigurationManager.AppSettings[param_name] != null && ConfigurationManager.AppSettings[param_name].Length > 0)
                    if (int.TryParse(ConfigurationManager.AppSettings[param_name].Trim(), out temp)) image.img_head_font_size = temp;
                    else { WriteLog("param parse error: " + param_name); continue; }
                else { WriteLog("param read error: " + param_name); continue; }

                param_name = "img" + img_num + "_head_left_margin";
                if (ConfigurationManager.AppSettings[param_name] != null && ConfigurationManager.AppSettings[param_name].Length > 0)
                    if (int.TryParse(ConfigurationManager.AppSettings[param_name].Trim(), out temp)) image.img_head_left_margin = temp;
                    else { WriteLog("param parse error: " + param_name); continue; }
                else { WriteLog("param read error: " + param_name); continue; }

                param_name = "img" + img_num + "_head_margin";
                if (ConfigurationManager.AppSettings[param_name] != null && ConfigurationManager.AppSettings[param_name].Length > 0)
                    if (int.TryParse(ConfigurationManager.AppSettings[param_name].Trim(), out temp)) image.img_head_margin = temp;
                    else { WriteLog("param parse error: " + param_name); continue; }
                else { WriteLog("param read error: " + param_name); continue; }

                param_name = "img" + img_num + "_head_color1";
                if (ConfigurationManager.AppSettings[param_name] != null && ConfigurationManager.AppSettings[param_name].Length > 0)
                    image.img_head_color1 = ConfigurationManager.AppSettings[param_name].Trim();
                else { WriteLog("param read error: " + param_name); continue; }

                param_name = "img" + img_num + "_head_color2";
                if (ConfigurationManager.AppSettings[param_name] != null && ConfigurationManager.AppSettings[param_name].Length > 0)
                    image.img_head_color2 = ConfigurationManager.AppSettings[param_name].Trim();
                else { WriteLog("param read error: " + param_name); continue; }

                param_name = "img" + img_num + "_title_font_size";
                if (ConfigurationManager.AppSettings[param_name] != null && ConfigurationManager.AppSettings[param_name].Length > 0)
                    if (int.TryParse(ConfigurationManager.AppSettings[param_name].Trim(), out temp)) image.img_title_font_size = temp;
                    else { WriteLog("param parse error: " + param_name); continue; }
                else { WriteLog("param read error: " + param_name); continue; }

                param_name = "img" + img_num + "_title_top_margin";
                if (ConfigurationManager.AppSettings[param_name] != null && ConfigurationManager.AppSettings[param_name].Length > 0)
                    if (int.TryParse(ConfigurationManager.AppSettings[param_name].Trim(), out temp)) image.img_title_top_margin = temp;
                    else { WriteLog("param parse error: " + param_name); continue; }
                else { WriteLog("param read error: " + param_name); continue; }

                param_name = "img" + img_num + "_title_left_margin";
                if (ConfigurationManager.AppSettings[param_name] != null && ConfigurationManager.AppSettings[param_name].Length > 0)
                    if (int.TryParse(ConfigurationManager.AppSettings[param_name].Trim(), out temp)) image.img_title_left_margin = temp;
                    else { WriteLog("param parse error: " + param_name); continue; }
                else { WriteLog("param read error: " + param_name); continue; }



                param_name = "img" + img_num + "_title_color";
                if (ConfigurationManager.AppSettings[param_name] != null && ConfigurationManager.AppSettings[param_name].Length > 0)
                    image.img_title_color = ConfigurationManager.AppSettings[param_name].Trim();
                else { WriteLog("param read error: " + param_name); continue; }

                param_name = "img" + img_num + "_button_top_margin";
                if (ConfigurationManager.AppSettings[param_name] != null && ConfigurationManager.AppSettings[param_name].Length > 0)
                    if (int.TryParse(ConfigurationManager.AppSettings[param_name].Trim(), out temp)) image.img_button_top_margin = temp;
                    else { WriteLog("param parse error: " + param_name); continue; }
                else { WriteLog("param read error: " + param_name); continue; }

                param_name = "img" + img_num + "_button_font_size";
                if (ConfigurationManager.AppSettings[param_name] != null && ConfigurationManager.AppSettings[param_name].Length > 0)
                    if (int.TryParse(ConfigurationManager.AppSettings[param_name].Trim(), out temp)) image.img_button_font_size = temp;
                    else { WriteLog("param parse error: " + param_name); continue; }
                else { WriteLog("param read error: " + param_name); continue; }

                param_name = "img" + img_num + "_button_height";
                if (ConfigurationManager.AppSettings[param_name] != null && ConfigurationManager.AppSettings[param_name].Length > 0)
                    if (int.TryParse(ConfigurationManager.AppSettings[param_name].Trim(), out temp)) image.img_button_height = temp;
                    else { WriteLog("param parse error: " + param_name); continue; }
                else { WriteLog("param read error: " + param_name); continue; }

                param_name = "img" + img_num + "_button_text_color";
                if (ConfigurationManager.AppSettings[param_name] != null && ConfigurationManager.AppSettings[param_name].Length > 0)
                    image.img_button_text_color = ConfigurationManager.AppSettings[param_name].Trim();
                else { WriteLog("param read error: " + param_name); continue; }

                if (Logoimage)
                {
                    param_name = "img" + img_num + "_logo_width";
                    if (ConfigurationManager.AppSettings[param_name] != null && ConfigurationManager.AppSettings[param_name].Length > 0)
                        if (int.TryParse(ConfigurationManager.AppSettings[param_name].Trim(), out temp)) image.img_logo_width = temp;
                        else { WriteLog("param parse error: " + param_name); continue; }
                    else { WriteLog("param read error: " + param_name); continue; }

                    param_name = "img" + img_num + "_logo_height";
                    if (ConfigurationManager.AppSettings[param_name] != null && ConfigurationManager.AppSettings[param_name].Length > 0)
                        if (int.TryParse(ConfigurationManager.AppSettings[param_name].Trim(), out temp)) image.img_logo_height = temp;
                        else { WriteLog("param parse error: " + param_name); continue; }
                    else { WriteLog("param read error: " + param_name); continue; }

                    param_name = "img" + img_num + "_logo_top_pos";
                    if (ConfigurationManager.AppSettings[param_name] != null && ConfigurationManager.AppSettings[param_name].Length > 0)
                        if (int.TryParse(ConfigurationManager.AppSettings[param_name].Trim(), out temp)) image.img_logo_top_pos = temp;
                        else { WriteLog("param parse error: " + param_name); continue; }
                    else { WriteLog("param read error: " + param_name); continue; }
                }

                ImageParams1.Add(image);
            }

            #endregion

            #region image params for phase2
            WriteLog("image params for phase 2");

            List<ImageParams2> ImageParams2 = new List<ImageParams2>();


            for (int img_num = 1; img_num <= 4; img_num++)
            {
                ImageParams2 image = new ImageParams2();

                int temp;

                string param_name = "img" + img_num + "_name";
                if (ConfigurationManager.AppSettings[param_name] != null && ConfigurationManager.AppSettings[param_name].Length > 0)
                    image.name = ConfigurationManager.AppSettings[param_name].Trim();
                else { WriteLog("param read error: " + param_name); continue; }

                param_name = "img" + img_num + "_screen_x";
                if (ConfigurationManager.AppSettings[param_name] != null && ConfigurationManager.AppSettings[param_name].Length > 0)
                    if (int.TryParse(ConfigurationManager.AppSettings[param_name].Trim(), out temp)) image.screen_x = temp;
                    else { WriteLog("param parse error: " + param_name); continue; }
                else { WriteLog("param read error: " + param_name); continue; }

                param_name = "img" + img_num + "_screen_y";
                if (ConfigurationManager.AppSettings[param_name] != null && ConfigurationManager.AppSettings[param_name].Length > 0)
                    if (int.TryParse(ConfigurationManager.AppSettings[param_name].Trim(), out temp)) image.screen_y = temp;
                    else { WriteLog("param parse error: " + param_name); continue; }
                else { WriteLog("param read error: " + param_name); continue; }

                param_name = "img" + img_num + "_screen_w";
                if (ConfigurationManager.AppSettings[param_name] != null && ConfigurationManager.AppSettings[param_name].Length > 0)
                    if (int.TryParse(ConfigurationManager.AppSettings[param_name].Trim(), out temp)) image.screen_w = temp;
                    else { WriteLog("param parse error: " + param_name); continue; }
                else { WriteLog("param read error: " + param_name); continue; }

                param_name = "img" + img_num + "_screen_h";
                if (ConfigurationManager.AppSettings[param_name] != null && ConfigurationManager.AppSettings[param_name].Length > 0)
                    if (int.TryParse(ConfigurationManager.AppSettings[param_name].Trim(), out temp)) image.screen_h = temp;
                    else { WriteLog("param parse error: " + param_name); continue; }
                else { WriteLog("param read error: " + param_name); continue; }

                param_name = "img" + img_num + "_global_pos_x";
                if (ConfigurationManager.AppSettings[param_name] != null && ConfigurationManager.AppSettings[param_name].Length > 0)
                    if (int.TryParse(ConfigurationManager.AppSettings[param_name].Trim(), out temp)) image.global_pos_x = temp;
                    else { WriteLog("param parse error: " + param_name); continue; }
                else { WriteLog("param read error: " + param_name); continue; }

                param_name = "img" + img_num + "_global_pos_y";
                if (ConfigurationManager.AppSettings[param_name] != null && ConfigurationManager.AppSettings[param_name].Length > 0)
                    if (int.TryParse(ConfigurationManager.AppSettings[param_name].Trim(), out temp)) image.global_pos_y = temp;
                    else { WriteLog("param parse error: " + param_name); continue; }
                else { WriteLog("param read error: " + param_name); continue; }

                param_name = "img" + img_num + "_shadow_x";
                if (ConfigurationManager.AppSettings[param_name] != null && ConfigurationManager.AppSettings[param_name].Length > 0)
                    if (int.TryParse(ConfigurationManager.AppSettings[param_name].Trim(), out temp)) image.shadow_x = temp;
                    else { WriteLog("param parse error: " + param_name); continue; }
                else { WriteLog("param read error: " + param_name); continue; }

                param_name = "img" + img_num + "_shadow_y";
                if (ConfigurationManager.AppSettings[param_name] != null && ConfigurationManager.AppSettings[param_name].Length > 0)
                    if (int.TryParse(ConfigurationManager.AppSettings[param_name].Trim(), out temp)) image.shadow_y = temp;
                    else { WriteLog("param parse error: " + param_name); continue; }
                else { WriteLog("param read error: " + param_name); continue; }

                ImageParams2.Add(image);
            }

            #endregion

            #endregion

            string ID1 = ID;

            string DesktopImageURL = string.Empty;
            string LaptopImageURL = string.Empty;
            string PhoneImageURL = string.Empty;
            string TabletImageURL = string.Empty;

            #region phase1
            WriteLog("phase1");

            bool logo_downloaded = false;

            string orig_image_file_name = HostingEnvironment.MapPath(string.Format("~/temp_{0}_orig_image", ID1)).Replace("\\", "\\\\");
            string logo_image_file_name = HostingEnvironment.MapPath(string.Format("~/temp_{0}_logo_image", ID1)).Replace("\\", "\\\\");

            downloadImage(Mainpicture, orig_image_file_name);

            //#region downloading the orig image
            //using (var client = new WebClient())
            //{
            //    if (File.Exists(orig_image_file_name))
            //    {
            //        try
            //        {
            //            WriteLog("File already exists, deleting before download: " + orig_image_file_name);
            //            File.Delete(orig_image_file_name);
            //        }
            //        catch
            //        {
            //            WriteLog("File already exists, and deleting failed: " + orig_image_file_name);
            //        }
            //    }
            //    try
            //    {
            //        WriteLog("Downloading " + Mainpicture);
            //        client.DownloadFile(Mainpicture, orig_image_file_name);

            //    }
            //    catch (Exception ex)
            //    {
            //        WriteLog("Failed to download " + Mainpicture);
            //        WriteLog(ex.Message);

            //        ret = "Failed to download " + Mainpicture;
            //    }

            //}
            //#endregion

            #region downloading the logo image if needed
            WriteLog("downloading the logo image if needed");
            if (Logoimage && LogoImageURL.Length > 0)
            {

                downloadImage(LogoImageURL, logo_image_file_name);

                //using (var client = new WebClient())
                //{
                //    if (File.Exists(logo_image_file_name))
                //    {
                //        try
                //        {
                //            WriteLog("File already exists, deleting before download: " + logo_image_file_name);
                //            File.Delete(logo_image_file_name);
                //        }
                //        catch
                //        {
                //            WriteLog("File already exists, and deleting failed: " + logo_image_file_name);
                //        }
                //    }
                //    try
                //    {
                //        WriteLog("Downloading " + LogoImageURL);
                //        client.DownloadFile(LogoImageURL, logo_image_file_name);
                //        logo_downloaded = true;

                //    }
                //    catch (Exception ex)
                //    {
                //        WriteLog("Failed to download " + LogoImageURL);
                //        WriteLog(ex.Message);

                //        ret = "Failed to download " + LogoImageURL;
                //    }
                //}
            }
            #endregion


            System.Drawing.Image orig_image_file_jpg = System.Drawing.Image.FromFile(orig_image_file_name);
            int orig_image_file_width = orig_image_file_jpg.Width;
            int orig_image_file_height = orig_image_file_jpg.Height;
            orig_image_file_jpg.Dispose();

            foreach (ImageParams1 i in ImageParams1)
            {
                string button_temp_file_name = HostingEnvironment.MapPath(string.Format("~/temp_{0}_button_{1}x{2}.jpg", ID1, i.img_width, i.img_height)).Replace("\\","\\\\");
                string button_text_file_name = HostingEnvironment.MapPath(string.Format("~/temp_{0}_button_text_{1}x{2}.jpg", ID1, i.img_width, i.img_height)).Replace("\\", "\\\\"); 
                string title_text_file_name = HostingEnvironment.MapPath(string.Format("~/temp_{0}_title_text_{1}x{2}.jpg", ID1, i.img_width, i.img_height)).Replace("\\", "\\\\");
                string logo_image_file_resized = HostingEnvironment.MapPath(string.Format("~/temp_{0}_{1}x{2}_logo_resized.png", ID1, i.img_width, i.img_height)).Replace("\\", "\\\\");

                string out_file_name = HostingEnvironment.MapPath(string.Format("~/{0}_{1}x{2}.jpg", ID1, i.img_width, i.img_height)).Replace("\\", "\\\\");
                string out_file_name_logo = HostingEnvironment.MapPath(string.Format("~/{0}_{1}x{2}_logo.jpg", ID1, i.img_width, i.img_height)).Replace("\\", "\\\\");

                int inner_image_file_height = i.img_inner_width * orig_image_file_height / orig_image_file_width;

                #region generate button text for measurement
                WriteLog("generate button text for measurement");
                var button_text_proc = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = Path.Combine(ImageMagickPath, "magick.exe"),
                        Arguments = string.Format("-density 300x300 -units PixelsPerInch -font Arial -pointsize {0} label:\"{1}\" {2}",
                           i.img_button_font_size / 3,      //0 - button font size
                           Button_CTA_Text,                 //1 - button text
                           button_text_file_name            //2 - button text file name
                            ),
                        UseShellExecute = false,
                        RedirectStandardError = true,
                        CreateNoWindow = true
                    }
                };

                WriteLog("Calling imagemagick to generate button text file:");
                WriteLog(button_text_proc.StartInfo.FileName + " " + button_text_proc.StartInfo.Arguments);

                button_text_proc.Start();
                string button_text_error = button_text_proc.StandardError.ReadToEnd();
                WriteLog(button_text_error);
                button_text_proc.WaitForExit();
                #endregion

                #region generate button image
                WriteLog("generate button image");
                System.Drawing.Image button_text_img = System.Drawing.Image.FromFile(button_text_file_name);
                int button_text_width = button_text_img.Width;
                int button_full_width = button_text_width + i.img_button_font_size * 2;
                button_text_img.Dispose();

                var button_temp_proc = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = Path.Combine(ImageMagickPath, "magick.exe"),
                        Arguments = string.Format("-density 300x300 -units PixelsPerInch -background {0} -fill {1} -font Arial -gravity center -size {2}x{3} -pointsize {4} label:\"{5}\" {6}",
                           Accentcolor,                     //0 - button bg color
                           i.img_button_text_color,         //1 - button text color                                                                              
                           button_full_width,               //2 - full button width
                           i.img_button_height,             //3 - full button height
                           i.img_button_font_size / 3,      //4 - button font size                                       
                           Button_CTA_Text,                 //5 - button text
                           button_temp_file_name            //6 - button temp file name
                            ),
                        UseShellExecute = false,
                        RedirectStandardError = true,
                        CreateNoWindow = true
                    }
                };

                WriteLog("Calling imagemagick to generate button temp file:");
                WriteLog(button_temp_proc.StartInfo.FileName + " " + button_temp_proc.StartInfo.Arguments);

                button_temp_proc.Start();
                string button_temp_error = button_temp_proc.StandardError.ReadToEnd();
                WriteLog(button_temp_error);
                button_temp_proc.WaitForExit();
                #endregion

                #region generate resized logo
                WriteLog("generate resized logo");
                int logo_image_file_width = 0;
                int logo_image_file_height = 0;

                if (logo_downloaded)
                {
                    var resized_img_proc = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = Path.Combine(ImageMagickPath, "magick.exe"),
                            Arguments = string.Format("{0} -resize {1}x{2} {3}",
                               logo_image_file_name,        //0 - downloaded input file
                               i.img_logo_width,            //1 - device screen width
                               i.img_logo_height,           //2 - device screen height
                               logo_image_file_resized      //3 - output resized_img file
                                ),
                            UseShellExecute = false,
                            RedirectStandardError = true,
                            CreateNoWindow = true
                        }
                    };

                    WriteLog("Calling imagemagick to generate resized_img file:");
                    WriteLog(resized_img_proc.StartInfo.FileName + " " + resized_img_proc.StartInfo.Arguments);

                    resized_img_proc.Start();
                    string resized_img_error = resized_img_proc.StandardError.ReadToEnd();
                    WriteLog(resized_img_error);
                    resized_img_proc.WaitForExit();

                    getImageDimensions(logo_image_file_resized, out logo_image_file_width, out logo_image_file_height);
                }

                #endregion

                Process proc = null;

                if (i.img_type == "1")
                {
                    #region type 1 no logo
                    WriteLog("type 1 no logo");
                    proc = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = Path.Combine(ImageMagickPath, "magick.exe"),
                            Arguments = string.Format("-density 300x300 -units PixelsPerInch -size {0}x{1} xc:#FFFFFF -fill {2} -draw \"rectangle {3},{4} {5},{6}\" -fill {7} -draw \"rectangle {8},{9} {10},{11}\" -fill {12} -font Arial -pointsize {13} -annotate +{14}+{15} \"Tel. {16} | {17}, {18} {19}\"  -fill {20} -draw \"rectangle {21},{22} {23},{24}\" -fill {25} -font Arial -pointsize {26} -annotate +{27}+{28} \"{29}\" -draw \"image over {30},{31} {32},{33} '{34}'\"  -draw \"image over {35},{36} {37},{38} '{39}'\" {40}",
                                i.img_width,                                        //0 - output width
                                i.img_height,                                       //1 - output height
                                i.img_head_color1,                                  //2 - head bg color
                                (i.img_width - i.img_head_width) / 2,               //3 - head top left x
                                i.img_top_offset,                                   //4 - head top left y
                                (i.img_width - i.img_head_width) / 2 + i.img_head_width,//5 - head bottom right x
                                i.img_head_height + i.img_top_offset,               //6 - head bottom right y 
                                i.img_head_color2,                                  //7 - head bottom margin color
                                (i.img_width - i.img_head_width) / 2,               //8 - head bottom margin top left x
                                i.img_head_height + i.img_top_offset,               //9 - head bottom margin top left y
                                (i.img_width - i.img_head_width) / 2 + i.img_head_width,//10 - head bottom margin bottom right x
                                i.img_head_height + i.img_head_margin + i.img_top_offset,//11 - head bottom margin bottom right y
                                Textcolor,                                          //12 - head text color
                                i.img_head_font_size / 3,                           //13 - head font size
                                (i.img_width - i.img_head_width) / 2 + i.img_head_left_margin,//14 - head text start position x
                                i.img_head_height / 2 + i.img_head_font_size / 2 / 2 + i.img_top_offset,//15 - head text start position y
                                Phone_no,                                           //16 - head text
                                Street,                                             //17 - head text
                                Zip,                                                //18 - head text
                                City,                                               //19 - head text

                                i.img_inner_bg_color,                               //20 - inner bg color
                                (i.img_width - i.img_head_width) / 2,               //21 - inner bg top left x
                                i.img_head_height + i.img_head_margin + i.img_top_offset,//22 - inner bg top left y
                                (i.img_width - i.img_head_width) / 2 + i.img_head_width,//23 - inner bg bottom right x
                                i.img_inner_poz + inner_image_file_height + i.img_button_top_margin * 2 + i.img_button_height + i.img_top_offset,//24 - inner bg bottom right y


                                i.img_title_color,                                  //25 - title text color
                                i.img_title_font_size / 3,                          //26 - title font size
                                (i.img_width - i.img_head_width) / 2 + i.img_title_left_margin,//27 - title text start position x
                                i.img_title_top_margin + i.img_top_offset,          //28 - title text start position y
                                Companyname,                                        //29 - title

                                (i.img_width - i.img_inner_width) / 2,              //30 - inner image top left x
                                i.img_inner_poz + i.img_top_offset,                 //31 - inner image top left y
                                i.img_inner_width,                                  //32 - inner image width                                            
                                inner_image_file_height,                            //33 - inner image height
                                orig_image_file_name,                               //34 - inner image file name
                                (i.img_width + i.img_inner_width) / 2 - button_full_width,//35 - button position top left x
                                i.img_inner_poz + inner_image_file_height + i.img_button_top_margin + i.img_top_offset,//36 - button position top left y
                                button_full_width,                                  //37 - button width
                                i.img_button_height,                                //38 - button heigth
                                button_temp_file_name,                              //39 - button temp file name                                            
                                out_file_name                                       //40 - output file
                                ),
                            UseShellExecute = false,
                            RedirectStandardError = true,
                            CreateNoWindow = true
                        }
                    };


                    WriteLog("Calling imagemagick:");
                    WriteLog(proc.StartInfo.FileName + " " + proc.StartInfo.Arguments);

                    proc.Start();
                    string error = proc.StandardError.ReadToEnd();
                    WriteLog(error);
                    proc.WaitForExit();
                    #endregion

                    #region type 1 logo
                    WriteLog("type 1 logo");
                    if (logo_downloaded)
                    {
                        proc = new Process
                        {
                            StartInfo = new ProcessStartInfo
                            {
                                FileName = Path.Combine(ImageMagickPath, "magick.exe"),
                                Arguments = string.Format("-density 300x300 -units PixelsPerInch -size {0}x{1} xc:#FFFFFF -fill {2} -draw \"rectangle {3},{4} {5},{6}\" -fill {7} -draw \"rectangle {8},{9} {10},{11}\" -fill {12} -font Arial -pointsize {13} -annotate +{14}+{15} \"Tel. {16} | {17}, {18} {19}\"  -fill {20} -draw \"rectangle {21},{22} {23},{24}\" -draw \"image over {25},{26} {27},{28} '{29}'\" -draw \"image over {30},{31} {32},{33} '{34}'\"  -draw \"image over {35},{36} {37},{38} '{39}'\" {40}",
                                    i.img_width,                                        //0 - output width
                                    i.img_height,                                       //1 - output height
                                    i.img_head_color1,                                  //2 - head bg color
                                    (i.img_width - i.img_head_width) / 2,               //3 - head top left x
                                    i.img_top_offset,                                   //4 - head top left y
                                    (i.img_width - i.img_head_width) / 2 + i.img_head_width,//5 - head bottom right x
                                    i.img_head_height + i.img_top_offset,               //6 - head bottom right y 
                                    i.img_head_color2,                                  //7 - head bottom margin color
                                    (i.img_width - i.img_head_width) / 2,               //8 - head bottom margin top left x
                                    i.img_head_height + i.img_top_offset,               //9 - head bottom margin top left y
                                    (i.img_width - i.img_head_width) / 2 + i.img_head_width,//10 - head bottom margin bottom right x
                                    i.img_head_height + i.img_head_margin + i.img_top_offset,//11 - head bottom margin bottom right y
                                    Textcolor,                                          //12 - head text color
                                    i.img_head_font_size / 3,                           //13 - head font size
                                    (i.img_width - i.img_head_width) / 2 + i.img_head_left_margin,//14 - head text start position x
                                    i.img_head_height / 2 + i.img_head_font_size / 2 / 2 + i.img_top_offset,//15 - head text start position y
                                    Phone_no,                                           //16 - head text
                                    Street,                                             //17 - head text
                                    Zip,                                                //18 - head text
                                    City,                                               //19 - head text

                                    i.img_inner_bg_color,                               //20 - inner bg color
                                    (i.img_width - i.img_head_width) / 2,               //21 - inner bg top left x
                                    i.img_head_height + i.img_head_margin + i.img_top_offset,//22 - inner bg top left y
                                    (i.img_width - i.img_head_width) / 2 + i.img_head_width,//23 - inner bg bottom right x
                                    i.img_inner_poz + inner_image_file_height + i.img_button_top_margin * 2 + i.img_button_height + i.img_top_offset,//24 - inner bg bottom right y

                                    (i.img_width - i.img_head_width) / 2 + i.img_title_left_margin,//25 - logo top left x
                                    i.img_logo_top_pos + i.img_top_offset + ((i.img_logo_height - logo_image_file_height) / 2),//26 - logo top left y vertically centerized
                                    logo_image_file_width,                              //27 - logo width
                                    logo_image_file_height,                             //28 - logo height
                                    logo_image_file_resized,                            //29 - resized logo image name                                        

                                    (i.img_width - i.img_inner_width) / 2,              //30 - inner image top left x
                                    i.img_inner_poz + i.img_top_offset,                 //31 - inner image top left y
                                    i.img_inner_width,                                  //32 - inner image width                                            
                                    inner_image_file_height,                            //33 - inner image height
                                    orig_image_file_name,                               //34 - inner image file name
                                    (i.img_width + i.img_inner_width) / 2 - button_full_width,//35 - button position top left x
                                    i.img_inner_poz + inner_image_file_height + i.img_button_top_margin + i.img_top_offset,//36 - button position top left y
                                    button_full_width,                                  //37 - button width
                                    i.img_button_height,                                //38 - button heigth
                                    button_temp_file_name,                              //39 - button temp file name                                            
                                    out_file_name_logo                                  //40 - output file
                                    ),
                                UseShellExecute = false,
                                RedirectStandardError = true,
                                CreateNoWindow = true
                            }
                        };


                        WriteLog("Calling imagemagick:");
                        WriteLog(proc.StartInfo.FileName + " " + proc.StartInfo.Arguments);


                        proc.Start();
                        error = proc.StandardError.ReadToEnd();
                        WriteLog(error);
                        proc.WaitForExit();
                    }
                    #endregion
                }
                else //if (i.img_type == "2")
                {
                    #region type 2
                    WriteLog("type 2");
                    #region generate title text for measurement
                    WriteLog("generate title text for measurement");
                    var title_text_proc = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = Path.Combine(ImageMagickPath, "magick.exe"),
                            Arguments = string.Format("-density 300x300 -units PixelsPerInch -font Arial -pointsize {0} label:\"{1}\" {2}",
                               i.img_button_font_size / 3,  //0 - button font size
                               Companyname,                 //1 - button text
                               title_text_file_name        //2 - button text file name
                                ),
                            UseShellExecute = false,
                            RedirectStandardError = true,
                            CreateNoWindow = true
                        }
                    };


                    WriteLog("Calling imagemagick to generate title text file:");
                    WriteLog(title_text_proc.StartInfo.FileName + " " + title_text_proc.StartInfo.Arguments);


                    title_text_proc.Start();
                    string title_text_error = title_text_proc.StandardError.ReadToEnd();
                    WriteLog(title_text_error);
                    title_text_proc.WaitForExit();
                    #endregion

                    System.Drawing.Image title_text_img = System.Drawing.Image.FromFile(title_text_file_name);
                    int title_text_width = title_text_img.Width;
                    int title_text_height = title_text_img.Height;
                    int titles_y = i.img_inner_poz + inner_image_file_height + i.img_button_top_margin * 4 + i.img_button_height + i.img_top_offset;
                    title_text_img.Dispose();

                    try
                    {
                        File.Delete(title_text_file_name);
                    }
                    catch { }

                    proc = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = Path.Combine(ImageMagickPath, "magick.exe"),

                            Arguments = string.Format("-density 300x300 -units PixelsPerInch -size {0}x{1} xc:#FFFFFF -fill {2} -draw \"rectangle {3},{4} {5},{6}\" -fill {7} -draw \"rectangle {8},{9} {10},{11}\" -fill {12} -font Arial -pointsize {13} -annotate +{14}+{15} \"{16}\" -fill {17} -font Arial -pointsize {18} -annotate +{19}+{20} \"Telefon\"  -font \"Arial-Bold\" -annotate +{21}+{22} \"{23}\" -annotate +{24}+{25} \"{16}\" -font \"Arial\" -annotate +{26}+{27} \"{28}\" -annotate +{29}+{30} \"{31} {32}\" -fill {33} -draw \"rectangle {34},{35} {36},{37}\" -draw \"image over {38},{39} {40},{41} '{42}'\"  -draw \"image over {43},{44} {45},{46} '{47}'\" -fill {12} -draw \"rectangle {48},{49} {50},{51}\" -fill {12} -draw \"rectangle {52},{53} {54},{55}\" -fill {12} -draw \"rectangle {56},{57} {58},{59}\"  {60}",
                              i.img_width,                                        //0 - output width
                              i.img_height,                                       //1 - output height
                              Accentcolor,                                        //2 - head bg color
                              (i.img_width - i.img_head_width) / 2,               //3 - head top left x
                              i.img_top_offset,                                   //4 - head top left y
                              (i.img_width - i.img_head_width) / 2 + i.img_head_width,//5 - head bottom right x
                              i.img_head_height + i.img_top_offset,               //6 - head bottom right y 
                              i.img_head_color2,                                  //7 - head bottom margin color
                              (i.img_width - i.img_head_width) / 2,               //8 - head bottom margin top left x
                              i.img_head_height + i.img_top_offset,               //9 - head bottom margin top left y
                              (i.img_width - i.img_head_width) / 2 + i.img_head_width,//10 - head bottom margin bottom right x
                              i.img_head_height + i.img_head_margin + i.img_top_offset,//11 - head bottom margin bottom right y
                              i.img_title_color,                                  //12 - head text color
                              i.img_head_font_size / 3,                           //13 - head font size
                              i.img_width / 2 - title_text_width / 2,             //14 - head text start position x                                          
                              i.img_head_height / 2 + title_text_height / 2 + i.img_top_offset,//15 - head text start position y                                          
                              Companyname,                                        //16 - title
                              Textcolor,                                          //17 - text color     
                              i.img_title_font_size / 3,                          //18 - font size
                              (i.img_width - i.img_head_width) / 2 + i.img_title_left_margin,//19 - title text start position x
                              titles_y,                                           //20 - title text start position y
                              (i.img_width - i.img_head_width) / 2 + i.img_title_left_margin + 7 * i.img_title_font_size,//21 - text line1 x
                              titles_y,                                           //22 - text line1 y
                              Phone_no,                                           //23 - phone
                              (i.img_width - i.img_head_width) / 2 + i.img_title_left_margin,//24 - text line2 x
                              titles_y + i.img_title_font_size * 4,               //25 - text line2 y
                              (i.img_width - i.img_head_width) / 2 + i.img_title_left_margin,//26 - text line3 x
                              titles_y + i.img_title_font_size * 8,               //27 - text line3 y           
                              Street,                                             //28 - street                                             
                              (i.img_width - i.img_head_width) / 2 + i.img_title_left_margin,//29 - text line4 x
                              titles_y + i.img_title_font_size * 10,              //30 - text line4 y           
                              Zip,                                                //31 - zip
                              City,                                               //32 - city                                       
                              i.img_inner_bg_color,                               //33 - inner bg color
                              (i.img_width - i.img_head_width) / 2,               //34 - inner bg top left x
                              i.img_head_height + i.img_head_margin + i.img_top_offset,//35 - inner bg top left y
                              (i.img_width - i.img_head_width) / 2 + i.img_head_width,//36 - inner bg bottom right x
                              i.img_inner_poz + inner_image_file_height + i.img_button_top_margin * 2 + i.img_button_height + i.img_top_offset,//37 - inner bg bottom right y
                              (i.img_width - i.img_inner_width) / 2,              //38 - inner image top left x
                              i.img_inner_poz + i.img_top_offset,                 //39 - inner image top left y
                              i.img_inner_width,                                  //40 - inner image width                                            
                              inner_image_file_height,                            //41 - inner image height
                              orig_image_file_name,                               //42 - inner image file name
                              (i.img_width + i.img_inner_width) / 2 - button_full_width,//43 - button position top left x
                              i.img_inner_poz + inner_image_file_height + i.img_button_top_margin + i.img_top_offset,//44 - button position top left y
                              button_full_width,                                  //45 - button width
                              i.img_button_height,                                //46 - button heigth
                              button_temp_file_name,                              //47 - button temp file name                                                                                
                              (i.img_width - i.img_head_width) / 2 + i.img_head_left_margin,                                                            //48 - stripe 1 top left x
                              i.img_head_height / 2 + title_text_height / 2 + i.img_top_offset - i.img_head_font_size,                                  //49 - stripe 1 top left y
                              (i.img_width - i.img_head_width) / 2 + i.img_head_left_margin + i.img_head_font_size,                                     //50 - stripe 1 bottom right x
                              i.img_head_height / 2 + title_text_height / 2 + i.img_top_offset - i.img_head_font_size + i.img_head_font_size / 5,       //51 - stripe 1 bottom right y
                              (i.img_width - i.img_head_width) / 2 + i.img_head_left_margin,                                                            //52 - stripe 2 top left x
                              i.img_head_height / 2 + title_text_height / 2 + i.img_top_offset - i.img_head_font_size / 2 - i.img_head_font_size / 10,  //53 - stripe 2 top left y
                              (i.img_width - i.img_head_width) / 2 + i.img_head_left_margin + i.img_head_font_size,                                     //54 - stripe 2 bottom right x
                              i.img_head_height / 2 + title_text_height / 2 + i.img_top_offset - i.img_head_font_size / 2 + i.img_head_font_size / 10,  //55 - stripe 2 bottom right y
                              (i.img_width - i.img_head_width) / 2 + i.img_head_left_margin,                                                            //56 - stripe 3 top left x
                              i.img_head_height / 2 + title_text_height / 2 + i.img_top_offset - i.img_head_font_size / 5,                              //57 - stripe 3 top left y
                              (i.img_width - i.img_head_width) / 2 + i.img_head_left_margin + i.img_head_font_size,                                     //58 - stripe 3 bottom right x
                              i.img_head_height / 2 + title_text_height / 2 + i.img_top_offset,                                                         //59 - stripe 3 bottom right y
                              out_file_name                                                                                                             //60 - output file
                              ),
                            UseShellExecute = false,
                            RedirectStandardError = true,
                            CreateNoWindow = true
                        }
                    };


                    WriteLog("Calling imagemagick:");
                    WriteLog(proc.StartInfo.FileName + " " + proc.StartInfo.Arguments);


                    proc.Start();
                    string error = proc.StandardError.ReadToEnd();
                    WriteLog(error);
                    proc.WaitForExit();
                    #endregion

                    #region type 2 logo
                    WriteLog("type 2 logo");
                    if (logo_downloaded)
                    {
                        proc = new Process
                        {
                            StartInfo = new ProcessStartInfo
                            {
                                FileName = Path.Combine(ImageMagickPath, "magick.exe"),

                                Arguments = string.Format("-density 300x300 -units PixelsPerInch -size {0}x{1} xc:#FFFFFF -fill {2} -draw \"rectangle {3},{4} {5},{6}\" -fill {7} -draw \"rectangle {8},{9} {10},{11}\" -draw \"image over {12},{13} {14},{15} '{16}'\" -fill {17} -font Arial -pointsize {18} -annotate +{19}+{20} \"Telefon\"  -font \"Arial-Bold\" -annotate +{21}+{22} \"{23}\" -annotate +{24}+{25} \"{61}\" -font \"Arial\" -annotate +{26}+{27} \"{28}\" -annotate +{29}+{30} \"{31} {32}\" -fill {33} -draw \"rectangle {34},{35} {36},{37}\" -draw \"image over {38},{39} {40},{41} '{42}'\"  -draw \"image over {43},{44} {45},{46} '{47}'\" -fill {33} -draw \"rectangle {48},{49} {50},{51}\" -fill {33} -draw \"rectangle {52},{53} {54},{55}\" -fill {33} -draw \"rectangle {56},{57} {58},{59}\"  {60}",
                                  i.img_width,                                        //0 - output width
                                  i.img_height,                                       //1 - output height
                                  "#FFFFFF",                                          //2 - head bg color
                                  (i.img_width - i.img_head_width) / 2,               //3 - head top left x
                                  i.img_top_offset,                                   //4 - head top left y
                                  (i.img_width - i.img_head_width) / 2 + i.img_head_width,//5 - head bottom right x
                                  i.img_head_height + i.img_top_offset,               //6 - head bottom right y 
                                  "#FFFFFF",                                          //7 - head bottom margin color
                                  (i.img_width - i.img_head_width) / 2,               //8 - head bottom margin top left x
                                  i.img_head_height + i.img_top_offset,               //9 - head bottom margin top left y
                                  (i.img_width - i.img_head_width) / 2 + i.img_head_width,//10 - head bottom margin bottom right x
                                  i.img_head_height + i.img_head_margin + i.img_top_offset,//11 - head bottom margin bottom right y


                                  (i.img_width - logo_image_file_width) / 2,          //12 - logo top left x horizontally centerized                                    
                                  i.img_logo_top_pos + i.img_top_offset + ((i.img_logo_height - logo_image_file_height) / 2),//13 - logo top left y vertically centerized
                                  logo_image_file_width,                              //14 - logo width
                                  logo_image_file_height,                             //15 - logo height
                                  logo_image_file_resized,                            //16 - resized logo image name   

                                  //i.img_title_color,                                  //12 - head text color
                                  //i.img_head_font_size / 3,                           //13 - head font size
                                  //i.img_width / 2 - title_text_width / 2,             //14 - head text start position x                                          
                                  //i.img_head_height / 2 + title_text_height / 2 + i.img_top_offset,//15 - head text start position y                                          
                                  //Companyname,                                        //16 - title

                                  Textcolor,                                          //17 - text color     
                                  i.img_title_font_size / 3,                          //18 - font size
                                  (i.img_width - i.img_head_width) / 2 + i.img_title_left_margin,//19 - title text start position x
                                  titles_y,                                           //20 - title text start position y
                                  (i.img_width - i.img_head_width) / 2 + i.img_title_left_margin + 7 * i.img_title_font_size,//21 - text line1 x
                                  titles_y,                                           //22 - text line1 y
                                  Phone_no,                                           //23 - phone
                                  (i.img_width - i.img_head_width) / 2 + i.img_title_left_margin,//24 - text line2 x
                                  titles_y + i.img_title_font_size * 4,               //25 - text line2 y
                                  (i.img_width - i.img_head_width) / 2 + i.img_title_left_margin,//26 - text line3 x
                                  titles_y + i.img_title_font_size * 8,               //27 - text line3 y           
                                  Street,                                             //28 - street                                             
                                  (i.img_width - i.img_head_width) / 2 + i.img_title_left_margin,//29 - text line4 x
                                  titles_y + i.img_title_font_size * 10,              //30 - text line4 y           
                                  Zip,                                                //31 - zip
                                  City,                                               //32 - city                                       
                                  i.img_inner_bg_color,                               //33 - inner bg color
                                  (i.img_width - i.img_head_width) / 2,               //34 - inner bg top left x
                                  i.img_head_height + i.img_head_margin + i.img_top_offset,//35 - inner bg top left y
                                  (i.img_width - i.img_head_width) / 2 + i.img_head_width,//36 - inner bg bottom right x
                                  i.img_inner_poz + inner_image_file_height + i.img_button_top_margin * 2 + i.img_button_height + i.img_top_offset,//37 - inner bg bottom right y
                                  (i.img_width - i.img_inner_width) / 2,              //38 - inner image top left x
                                  i.img_inner_poz + i.img_top_offset,                 //39 - inner image top left y
                                  i.img_inner_width,                                  //40 - inner image width                                            
                                  inner_image_file_height,                            //41 - inner image height
                                  orig_image_file_name,                               //42 - inner image file name
                                  (i.img_width + i.img_inner_width) / 2 - button_full_width,//43 - button position top left x
                                  i.img_inner_poz + inner_image_file_height + i.img_button_top_margin + i.img_top_offset,//44 - button position top left y
                                  button_full_width,                                  //45 - button width
                                  i.img_button_height,                                //46 - button heigth
                                  button_temp_file_name,                              //47 - button temp file name                                                                                
                                  (i.img_width - i.img_head_width) / 2 + i.img_head_left_margin,                                                            //48 - stripe 1 top left x
                                  i.img_head_height / 2 + title_text_height / 2 + i.img_top_offset - i.img_head_font_size,                                  //49 - stripe 1 top left y
                                  (i.img_width - i.img_head_width) / 2 + i.img_head_left_margin + i.img_head_font_size,                                     //50 - stripe 1 bottom right x
                                  i.img_head_height / 2 + title_text_height / 2 + i.img_top_offset - i.img_head_font_size + i.img_head_font_size / 5,       //51 - stripe 1 bottom right y
                                  (i.img_width - i.img_head_width) / 2 + i.img_head_left_margin,                                                            //52 - stripe 2 top left x
                                  i.img_head_height / 2 + title_text_height / 2 + i.img_top_offset - i.img_head_font_size / 2 - i.img_head_font_size / 10,  //53 - stripe 2 top left y
                                  (i.img_width - i.img_head_width) / 2 + i.img_head_left_margin + i.img_head_font_size,                                     //54 - stripe 2 bottom right x
                                  i.img_head_height / 2 + title_text_height / 2 + i.img_top_offset - i.img_head_font_size / 2 + i.img_head_font_size / 10,  //55 - stripe 2 bottom right y
                                  (i.img_width - i.img_head_width) / 2 + i.img_head_left_margin,                                                            //56 - stripe 3 top left x
                                  i.img_head_height / 2 + title_text_height / 2 + i.img_top_offset - i.img_head_font_size / 5,                              //57 - stripe 3 top left y
                                  (i.img_width - i.img_head_width) / 2 + i.img_head_left_margin + i.img_head_font_size,                                     //58 - stripe 3 bottom right x
                                  i.img_head_height / 2 + title_text_height / 2 + i.img_top_offset,                                                         //59 - stripe 3 bottom right y
                                  out_file_name_logo,                                                                                                       //60 - output file
                                  Companyname                                                                                                               //61 - title
                                  ),
                                UseShellExecute = false,
                                RedirectStandardError = true,
                                CreateNoWindow = true
                            }
                        };


                        WriteLog("Calling imagemagick:");
                        WriteLog(proc.StartInfo.FileName + " " + proc.StartInfo.Arguments);


                        proc.Start();
                        error = proc.StandardError.ReadToEnd();
                        WriteLog(error);
                        proc.WaitForExit();
                    }
                    #endregion
                }


                if (i.img_used_in.Contains("Desktop"))
                {
                    if (logo_downloaded) DesktopImageURL = out_file_name_logo;
                    else DesktopImageURL = out_file_name;
                }

                if (i.img_used_in.Contains("Laptop"))
                {
                    if (logo_downloaded) LaptopImageURL = out_file_name_logo;
                    else LaptopImageURL = out_file_name;
                }

                if (i.img_used_in.Contains("Tablet"))
                {
                    if (logo_downloaded) TabletImageURL = out_file_name_logo;
                    else TabletImageURL = out_file_name;
                }

                if (i.img_used_in.Contains("Phone"))
                {
                    if (logo_downloaded) PhoneImageURL = out_file_name_logo;
                    else PhoneImageURL = out_file_name;
                }


                try
                {
                    File.Delete(button_temp_file_name);
                }
                catch { }

                try
                {
                    File.Delete(button_text_file_name);
                }
                catch
                {

                }

                if (logo_downloaded)
                {
                    try
                    {
                        File.Delete(logo_image_file_resized);
                    }
                    catch
                    {

                    }
                }

            }

            try
            {
                File.Delete(orig_image_file_name);
            }
            catch { }

            if (logo_downloaded)
            {
                try
                {
                    File.Delete(logo_image_file_name);
                }
                catch { }
            }

            #endregion

            #region phase2
            WriteLog("phase2");

            string ID2 = ID1;


            string DesktopImage = HostingEnvironment.MapPath(string.Format("~/{0}/temp_{1}_DesktopImage", SrcPath, ID2)).Replace("\\", "\\\\");
            string LaptopImage = HostingEnvironment.MapPath(string.Format("~/{0}/temp_{1}_LaptopImage", SrcPath, ID2)).Replace("\\", "\\\\");
            string PhoneImage = HostingEnvironment.MapPath(string.Format("~/{0}/temp_{1}_PhoneImage", SrcPath, ID2)).Replace("\\", "\\\\");
            string TabletImage = HostingEnvironment.MapPath(string.Format("~/{0}/temp_{1}_TabletImage", SrcPath, ID2)).Replace("\\", "\\\\");

            //string TabletImage = HostingEnvironment.MapPath(string.Format("{0}\\\\temp_{1}_TabletImage", SrcPath, ID2);

            string FinalOutputFile = HostingEnvironment.MapPath(string.Format("~/{0}_Devicemockup_Output.png", ID2)).Replace("\\", "\\\\");

            //downloadimage(desktopimageurl, desktopimage);
            //downloadimage(laptopimageurl, laptopimage);
            //downloadimage(phoneimageurl, phoneimage);
            //downloadimage(tabletimageurl, tabletimage);

            File.Copy(DesktopImageURL, DesktopImage);
            File.Copy(LaptopImageURL, LaptopImage);
            File.Copy(PhoneImageURL, PhoneImage);
            File.Copy(TabletImageURL, TabletImage);

            Dictionary<string, int> GlobalPositions = new Dictionary<string, int>();

            foreach (ImageParams2 i in ImageParams2)
            {
                //1. resize input.jpg
                //2. laptop.png + input-resized.jpg, centerized horizontal and vertical
                //3. + reflection
                //4. + laptop-shine.png + laptop-shadow.png

                GlobalPositions[i.name + "X"] = i.global_pos_x;
                GlobalPositions[i.name + "Y"] = i.global_pos_y;

                string input_image = HostingEnvironment.MapPath(string.Format("~/{0}/temp_{1}_{2}Image", SrcPath, ID2, i.name)).Replace("\\", "\\\\");
                string device_file = HostingEnvironment.MapPath("~/" + SrcPath + "/" + i.name + ".png").Replace("\\", "\\\\");
                string resized_img = HostingEnvironment.MapPath(string.Format("~/{0}/temp_{1}_{2}_resized_img.png", SrcPath, ID2, i.name)).Replace("\\", "\\\\");
                string img_on_screen = HostingEnvironment.MapPath(string.Format("~/{0}/temp_{1}_{2}_img_on_screen.png", SrcPath, ID2, i.name)).Replace("\\", "\\\\");
                string reflection = HostingEnvironment.MapPath(string.Format("~/{0}/temp_{1}_{2}_reflection.png", SrcPath, ID2, i.name)).Replace("\\", "\\\\");
                string shine_and_shadow = HostingEnvironment.MapPath(string.Format("~/{0}/temp_{1}_{2}_shine_and_shadow.png", SrcPath, ID2, i.name)).Replace("\\", "\\\\");


                //string input_image = HostingEnvironment.MapPath(string.Format("{0}\\\\temp_{1}_{2}Image", SrcPath, ID2, i.name);
                //string device_file = HostingEnvironment.MapPath(SrcPath + "\\\\" + i.name + ".png";
                //string resized_img = HostingEnvironment.MapPath(string.Format("{0}\\\\temp_{1}_{2}_resized_img.png", SrcPath, ID2, i.name);
                //string img_on_screen = HostingEnvironment.MapPath(string.Format("{0}\\\\temp_{1}_{2}_img_on_screen.png", SrcPath, ID2, i.name);
                //string reflection = HostingEnvironment.MapPath(string.Format("{0}\\\\temp_{1}_{2}_reflection.png", SrcPath, ID2, i.name);
                //string shine_and_shadow = HostingEnvironment.MapPath(string.Format("{0}\\\\temp_{1}_{2}_shine_and_shadow.png", SrcPath, ID2, i.name);

                int device_fileW, device_fileH;
                int ResizedImgW, ResizedImgH;

                #region generate resized_img
                WriteLog("generate resized_img");
                {
                    var resized_img_proc = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = Path.Combine(ImageMagickPath, "magick.exe"),
                            Arguments = string.Format("{0} -resize {1}x{2} {3}",
                               input_image,        //0 - downloaded input file
                               i.screen_w,         //1 - device screen width
                               i.screen_h,         //2 - device screen height
                               resized_img         //3 - output resized_img file
                                ),
                            UseShellExecute = false,
                            RedirectStandardError = true,
                            CreateNoWindow = true
                        }
                    };


                    WriteLog("Calling imagemagick to generate resized_img file:");
                    WriteLog(resized_img_proc.StartInfo.FileName + " " + resized_img_proc.StartInfo.Arguments);


                    resized_img_proc.Start();
                    string resized_img_error = resized_img_proc.StandardError.ReadToEnd();
                    WriteLog(resized_img_error);
                    resized_img_proc.WaitForExit();

                    getImageDimensions(resized_img, out ResizedImgW, out ResizedImgH);
                }
                #endregion



                #region generate img_on_screen
                WriteLog("generate img_on_screen");
                {
                    var img_on_screen_proc = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = Path.Combine(ImageMagickPath, "magick.exe"),
                            Arguments = string.Format("{0} -draw \"image over {1},{2} {3},{4} '{5}'\" {6}",
                               device_file,                                     //0 - device file
                               i.screen_x + (i.screen_w - ResizedImgW) / 2,     //1 - image x pos (centerized)
                               i.screen_y + (i.screen_h - ResizedImgH) / 2,     //2 - image y pos (centerized)
                               ResizedImgW,                                     //3 - image width
                               ResizedImgH,                                     //4 - image height
                               resized_img,                                     //5 - previously resized image file
                               img_on_screen                                    //6 - output img_on_screen file
                                ),
                            UseShellExecute = false,
                            RedirectStandardError = true,
                            CreateNoWindow = true
                        }
                    };


                    WriteLog("Calling imagemagick to generate img_on_screen file:");
                    WriteLog(img_on_screen_proc.StartInfo.FileName + " " + img_on_screen_proc.StartInfo.Arguments);


                    img_on_screen_proc.Start();
                    string img_on_screen_error = img_on_screen_proc.StandardError.ReadToEnd();
                    WriteLog(img_on_screen_error);
                    img_on_screen_proc.WaitForExit();
                }
                #endregion

                #region generate reflection
                WriteLog("generate reflection");
                {
                    string grad_img = HostingEnvironment.MapPath(string.Format("~/{0}/{1}-grad.png", SrcPath, i.name)).Replace("\\", "\\\\");
                    int GradW, GradH;
                    getImageDimensions(grad_img, out GradW, out GradH);

                    getImageDimensions(device_file, out device_fileW, out device_fileH);

                    var reflection_proc = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = Path.Combine(ImageMagickPath, "magick.exe"),
                            Arguments = string.Format("{0} ( -size {1}x0 xc:none ) ( -clone 0 -flip -crop {1}x{2}+0+0 +repage ) ( -clone 0 -alpha extract -flip -crop {1}x{2}+0+0 +repage {3} -compose multiply -composite ) ( -clone 2 -clone 3 -alpha off -compose copy_opacity -composite ) -delete 2,3 -channel rgba -alpha on -append {4}",
                               img_on_screen,                                         //0 - previously generated img_on_screen file
                               device_fileW,                                          //1 - generated image W
                               GradH,                                                 //2 - reflecion size                                    
                               grad_img,                                              //3 - gradient image
                               reflection                                             //4 - output reflection file
                                ),
                            UseShellExecute = false,
                            RedirectStandardError = true,
                            CreateNoWindow = true
                        }
                    };


                    WriteLog("Calling imagemagick to generate reflection file:");
                    WriteLog(reflection_proc.StartInfo.FileName + " " + reflection_proc.StartInfo.Arguments);


                    reflection_proc.Start();
                    string reflection_error = reflection_proc.StandardError.ReadToEnd();
                    WriteLog(reflection_error);
                    reflection_proc.WaitForExit();
                }
                #endregion

                #region generate shine_and_shadow
                WriteLog("generate shine_and_shadow");
                {
                    string shine_img = HostingEnvironment.MapPath(string.Format("~/{0}/{1}-shine.png", SrcPath, i.name)).Replace("\\", "\\\\");
                    int ShineW, ShineH;
                    getImageDimensions(shine_img, out ShineW, out ShineH);

                    string shadow_img = HostingEnvironment.MapPath(string.Format("~/{0}/{1}-shadow.png", SrcPath, i.name)).Replace("\\", "\\\\");
                    int ShadowW, ShadowH;
                    getImageDimensions(shadow_img, out ShadowW, out ShadowH);

                    var shine_and_shadow_proc = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = Path.Combine(ImageMagickPath, "magick.exe"),
                            Arguments = string.Format("{0} -draw \"image over 0,0 {1},{2} '{3}'\" -draw \"image over 0,0 {4},{5} '{6}'\" {7}",
                               reflection,                          //0 - previously generated reflection file
                               ShineW,                              //1 - shine effect file width
                               ShineH,                              //2 - shine effect file height
                               shine_img,                           //3 - shine effect file
                               ShadowW,                             //4 - shine effect file width
                               ShadowH,                             //5 - shine effect file height
                               shadow_img,                          //6 - shine effect file
                               shine_and_shadow                     //7 - output reflection file
                                ),
                            UseShellExecute = false,
                            RedirectStandardError = true,
                            CreateNoWindow = true
                        }
                    };


                    WriteLog("Calling imagemagick to generate shine_and_shadow file:");
                    WriteLog(shine_and_shadow_proc.StartInfo.FileName + " " + shine_and_shadow_proc.StartInfo.Arguments);


                    shine_and_shadow_proc.Start();
                    string shine_and_shadow_error = shine_and_shadow_proc.StandardError.ReadToEnd();
                    WriteLog(shine_and_shadow_error);
                    shine_and_shadow_proc.WaitForExit();
                }
                #endregion

                try { File.Delete(input_image); }
                catch { }
                try { File.Delete(resized_img); }
                catch { }
                try { File.Delete(img_on_screen); }
                catch { }
                try { File.Delete(reflection); }
                catch { }

            }

            #region generate place all the images on the background
            WriteLog("generate place all the images on the background");
            {
                string Desktop_shine_and_shadow = HostingEnvironment.MapPath(string.Format("~/{0}/temp_{1}_{2}_shine_and_shadow.png", SrcPath, ID2, "Desktop")).Replace("\\", "\\\\");
                string Laptop_shine_and_shadow = HostingEnvironment.MapPath(string.Format("~/{0}/temp_{1}_{2}_shine_and_shadow.png", SrcPath, ID2, "Laptop")).Replace("\\", "\\\\");
                string Phone_shine_and_shadow = HostingEnvironment.MapPath(string.Format("~/{0}/temp_{1}_{2}_shine_and_shadow.png", SrcPath, ID2, "Phone")).Replace("\\", "\\\\");
                string Tablet_shine_and_shadow = HostingEnvironment.MapPath(string.Format("~/{0}/temp_{1}_{2}_shine_and_shadow.png", SrcPath, ID2, "Tablet")).Replace("\\", "\\\\");

                string BackGround = HostingEnvironment.MapPath(string.Format("~/{0}/background.png", SrcPath)).Replace("\\", "\\\\");

                int DesktopW, DesktopH, LaptopW, LaptopH, PhoneW, PhoneH, TabletW, TabletH, BgW, BgH;
                getImageDimensions(Desktop_shine_and_shadow, out DesktopW, out DesktopH);
                getImageDimensions(Laptop_shine_and_shadow, out LaptopW, out LaptopH);
                getImageDimensions(Phone_shine_and_shadow, out PhoneW, out PhoneH);
                getImageDimensions(Tablet_shine_and_shadow, out TabletW, out TabletH);
                getImageDimensions(BackGround, out BgW, out BgH);

                var final_proc = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = Path.Combine(ImageMagickPath, "magick.exe"),
                        Arguments = string.Format("{0} -draw \"image over {1},{2} {3},{4} '{5}'\" -draw \"image over {6},{7} {8},{9} '{10}'\" -draw \"image over {11},{12} {13},{14} '{15}'\" -draw \"image over {16},{17} {18},{19} '{20}'\" {21}",
                           TransparentBackgroud ? string.Format("-size {0}x{1} xc:none", BgW, BgH) : BackGround,   //0 - transparent bg or BackGround file

                           GlobalPositions["DesktopX"],         //1 - Desktop global x pos
                           GlobalPositions["DesktopY"],         //2 - Desktop global y pos
                           DesktopW,                            //3 - Desktop final image width
                           DesktopH,                            //4 - Desktop final image height
                           Desktop_shine_and_shadow,            //5 - Desktop final image

                           GlobalPositions["LaptopX"],          //1 - Laptop global x pos
                           GlobalPositions["LaptopY"],          //2 - Laptop global y pos
                           LaptopW,                             //3 - Laptop final image width
                           LaptopH,                             //4 - Laptop final image height
                           Laptop_shine_and_shadow,             //5 - Laptop final image

                           GlobalPositions["TabletX"],          //1 - Tablet global x pos
                           GlobalPositions["TabletY"],          //2 - Tablet global y pos
                           TabletW,                             //3 - Tablet final image width
                           TabletH,                             //4 - Tablet final image height
                           Tablet_shine_and_shadow,             //5 - Tablet final image                    

                           GlobalPositions["PhoneX"],           //1 - Phone global x pos
                           GlobalPositions["PhoneY"],           //2 - Phone global y pos
                           PhoneW,                              //3 - Phone final image width
                           PhoneH,                              //4 - Phone final image height
                           Phone_shine_and_shadow,              //5 - Phone final image

                           FinalOutputFile
                            ),
                        UseShellExecute = false,
                        RedirectStandardError = true,
                        CreateNoWindow = true
                    }
                };


                WriteLog("Calling imagemagick to generate final file:");
                WriteLog(final_proc.StartInfo.FileName + " " + final_proc.StartInfo.Arguments);


                final_proc.Start();
                string final_error = final_proc.StandardError.ReadToEnd();
                WriteLog(final_error);
                final_proc.WaitForExit();

                ret = "http://localhost/" + Path.GetFileName(FinalOutputFile);

                try { File.Delete(Desktop_shine_and_shadow); }
                catch { }
                try { File.Delete(Laptop_shine_and_shadow); }
                catch { }
                try { File.Delete(Phone_shine_and_shadow); }
                catch { }
                try { File.Delete(Tablet_shine_and_shadow); }
                catch { }
            }
            #endregion


            try { File.Delete(DesktopImage); }
            catch { }
            try { File.Delete(LaptopImage); }
            catch { }
            try { File.Delete(PhoneImage); }
            catch { }
            try { File.Delete(TabletImage); }
            catch { }


            try { File.Delete(DesktopImageURL); }
            catch { }
            try { File.Delete(LaptopImageURL); }
            catch { }
            try { File.Delete(PhoneImageURL); }
            catch { }
            try { File.Delete(TabletImageURL); }
            catch { }

            #endregion


            return ret;
        }

        private void WriteLog(string msg)
        {
            try
            {
                //Console.WriteLine(msg); // for debug

                FileStream fs = new FileStream(Path.Combine(HostingEnvironment.MapPath("~/logs"), DateTime.Now.ToString("yyyy-MM-dd") + ".log"), FileMode.Append, FileAccess.Write);
                StreamWriter m_streamWriter = new StreamWriter(fs);
                m_streamWriter.BaseStream.Seek(0, SeekOrigin.End);
                m_streamWriter.WriteLine(string.Format("[{0}] {1}", DateTime.Now, msg));
                m_streamWriter.Flush();
                m_streamWriter.Close();
            }
            catch (Exception ex)
            {
                //could not open log file                
            }
        }

        private class ImageParams1
        {
            public string img_type { get; set; }
            public string[] img_used_in { get; set; }

            public int img_width { get; set; }
            public int img_height { get; set; }

            public int img_top_offset { get; set; }

            public int img_inner_width { get; set; }
            //public int img_inner_height { get; set; }
            public int img_inner_poz { get; set; }
            public string img_inner_bg_color { get; set; }

            public int img_head_width { get; set; }
            public int img_head_height { get; set; }
            public int img_head_font_size { get; set; }
            public int img_head_left_margin { get; set; }
            public int img_head_margin { get; set; }
            public string img_head_color1 { get; set; }
            public string img_head_color2 { get; set; }

            public int img_title_font_size { get; set; }
            public int img_title_top_margin { get; set; }
            public int img_title_left_margin { get; set; }
            public string img_title_color { get; set; }

            public int img_button_top_margin { get; set; }
            public int img_button_font_size { get; set; }
            public int img_button_height { get; set; }
            public string img_button_text_color { get; set; }

            public int img_logo_width { get; set; }
            public int img_logo_height { get; set; }
            public int img_logo_top_pos { get; set; }
        }

        private class ImageParams2
        {
            public string name { get; set; }

            public int screen_x { get; set; }
            public int screen_y { get; set; }

            public int screen_w { get; set; }
            public int screen_h { get; set; }

            public int global_pos_x { get; set; }
            public int global_pos_y { get; set; }

            public int shadow_x { get; set; }
            public int shadow_y { get; set; }
        }

        private void downloadImage(string url, string filename)
        {
            using (var client = new WebClient())
            {
                if (File.Exists(filename))
                {
                    try
                    {
                        WriteLog("File already exists, deleting before download: " + filename);
                        File.Delete(filename);
                    }
                    catch
                    {
                        WriteLog("File already exists, and deleting failed: " + filename);
                    }
                }
                try
                {
                    WriteLog("Downloading " + url);
                    //client.DownloadFile(url, HostingEnvironment.MapPath("~/" + filename));
                    client.DownloadFile(url, filename);

                }
                catch (Exception ex)
                {
                    WriteLog("Failed to download " + url);
                    WriteLog(ex.Message);
                }
            }
        }

        private void getImageDimensions(string filename, out int Width, out int Height)
        {
            System.Drawing.Image Image_jpg = System.Drawing.Image.FromFile(filename);
            Width = Image_jpg.Width;
            Height = Image_jpg.Height;
            Image_jpg.Dispose();
        }
    }
}
