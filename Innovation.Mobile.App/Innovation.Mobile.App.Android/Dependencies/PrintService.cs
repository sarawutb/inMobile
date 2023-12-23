using System.IO;
using Android.Graphics;
using Innovation.Mobile.App.Contracts.Service.Genaral;
using Innovation.Mobile.App.Droid.Dependencies;
using Xamarin.Forms;
using Android.Support.V4.Print;
using Plugin.CurrentActivity;
using Android.Print;
using System.Net.Sockets;
using Android.Content;
using System;
using System.Text;
using Innovation.Mobile.App.Models;
using System.Diagnostics;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using static System.Net.Mime.MediaTypeNames;

[assembly: Dependency(typeof(PrintService))]

namespace Innovation.Mobile.App.Droid.Dependencies
{
    public class PrintService : IPrintService
    {
        private string Commandn;

        public bool PrintImage(Stream img)
        {

            PrintHelper photoPrinter = new PrintHelper(CrossCurrentActivity.Current.Activity);
            photoPrinter.ScaleMode = PrintHelper.ScaleModeFit;
            photoPrinter.Orientation = PrintHelper.OrientationLandscape;
            Bitmap bitmap = BitmapFactory.DecodeStream(img);
            photoPrinter.PrintBitmap("Label", bitmap);

            return true;
        }

        public bool PrintImageA5(Stream img)
        {
            PrintHelper photoPrinter = new PrintHelper(CrossCurrentActivity.Current.Activity);
            photoPrinter.ScaleMode = PrintHelper.ScaleModeFit;
            photoPrinter.Orientation = PrintHelper.OrientationLandscape;
            Bitmap bitmap = BitmapFactory.DecodeStream(img);
            bitmap.GetScaledHeight(100);
            bitmap.GetScaledWidth(200);
            photoPrinter.PrintBitmap("Label", bitmap);


            return true;
        }
        public string CommandPrintPickingList(string port, string address, MaterialPickingDtlVM obj)
        {
            Commandn = "";
            if (string.IsNullOrEmpty(address))
            {
                throw new Exception("กรุณาเลือกเครื่องพิมพ์ลาเบล 4x4");
            }

            if (obj.RmCode.Substring(0, 1) == "I" || obj.RmCode.Substring(0, 1) == "S")
            {
                Commandn = "<STX><ESC>C<ETX>"
               + "<STX><ESC>P<ETX>"
               + "<STX>E1;F1;<ETX>"
               + "<STX>H0;o10,200;f0;c28;h1;w1;d0,30;k50;<ETX>"
               + "<STX>H1;o700,20;f0;c66;h1;w1;d0,30;k15;<ETX>"
               + "<STX>H4;o10,390;f0;c66;h2;w2;d0,30;k30;<ETX>"
               + "<STX>L5;o3,7;l760;w2<ETX> "
               + "<STX>L6;o3,780;l760;w2<ETX>"
               + "<STX>L7;o3,480;l760;w2<ETX>"
               + "<STX>L8;o3,555;l760;w2<ETX>"
               + "<STX>L9;o3,630;l760;w2<ETX>"
               + "<STX>L10;o3,705;l760;w2<ETX>"
               + "<STX>L11;o3,5;f3;l800;w2<ETX>"
               + "<STX>L12;o770,5;f3;l800;w2<ETX>"
               + "<STX>L13;o130,480;f3;l800;w2<ETX>"
               + "<STX>L14;o380,555;f3;l75;w2<ETX>"
               + "<STX>L15;o510,555;f3;l75;w2<ETX>"
               + "<STX>L16;o380,705;f3;l75;w2<ETX>"
               + "<STX>L17;o510,705;f3;l75;w2<ETX>"
               + "<STX>H18;o5,500;f0;c66;h2;w2;d3,OWNER;k10;<ETX>"
               + "<STX>H19;o5,570;f0;c66;h2;w2;d3,LOT NO.;k10;<ETX>"
               + "<STX>H20;o385,570;f0;c66;h2;w2;d3,Batch No.;k10;<ETX>"
               + "<STX>H21;o5,645;f0;c66;h2;w2;d3,WEIGHT;k10;<ETX>"
               + "<STX>H22;o5,720;f0;c66;h2;w2;d3,REC.;k10;<ETX>"
               + "<STX>H23;o385,720;f0;c66;h2;w2;d3,EXP.;k10;<ETX>"
               + "<STX>H24;o150,500;f0;c66;h2;w2;d0,30;k10;<ETX>"
               + "<STX>H25;o150,570;f0;c66;h3;w2;d0,30;k10;<ETX>"
               + "<STX>H26;o515,570;f0;c66;h3;w2;d0,30;k10;<ETX>"
               + "<STX>H27;o150,720;f0;c66;h2;w2;d0,30;k10;<ETX>"
               + "<STX>H28;o150,645;f0;c66;h2;w2;d0,30;k10;<ETX>"
               + "<STX>H29;o515,720;f0;c66;h2;w2;d0,30;k10;<ETX>"
               + "<STX>B30;o150,10;c6;i1;;h200;w2;d0,29;<ETX>"
               + "<STX>R<ETX>"
               + "<STX><ESC>E1<ETX>"
               + "<STX><CAN><ETX>"
               + "<STX>" + obj.RmCode + "<CR><ETX>"
               + "<STX>" + obj.OwnerShortName + "<CR><ETX>"
               + "<STX>" + obj.RmNameBC + "<CR><ETX>"
               + "<STX>" + obj.OwnerName + "<CR><ETX>"
               + "<STX>" + obj.Lotno + "<CR><ETX>"
               + "<STX>" + obj.BatchNo + "<CR><ETX>"
               + "<STX>" + obj.RecordDate + "<CR><ETX>"
               + "<STX>" + obj.Weight + " Kgs." + "<CR><ETX>"
               + "<STX>" + obj.ExpireDate + "<CR><ETX>"
               + "<STX>" + obj.BarcodeData + obj.BatchNoBC + "<CR><ETX>"
               + "<STX><ETB><ETX>"
               ;
            }
            else
            {
                Commandn = "<STX><ESC>C<ETX>"
               + "<STX><ESC>P<ETX>"
               + "<STX>E1;F1;<ETX>"
               + "<STX>H0;o10,10;f0;c28;h1;w1;d0,30;k60;<ETX>"
               + "<STX>H1;o700,20;f0;c66;h1;w1;d0,30;k15;<ETX>"
               + "<STX>H2;o500,120;f0;c66;h2;w1;d0,30;k15;<ETX>"
               + "<STX>H3;o600,120;f0;c66;h2;w1;d0,30;k15;<ETX>"
               + "<STX>H4;o10,400;f0;c66;h2;w2;d0,30;k15;<ETX>"
               + "<STX>H5;o200,520;f0;c66;h2;w2;d0,30;k15;<ETX>"
               + "<STX>H6;o300,520;f0;c66;h2;w2;d0,30;k15;<ETX>"
               + "<STX>H7;o500,520;f0;c66;h2;w2;d0,30;k15;<ETX>"
               + "<STX>L8;o3,7;l760;w2<ETX>"
               + "<STX>L9;o3,780;l760;w2<ETX>"
               + "<STX>L10;o3,570;l760;w2<ETX>"
               + "<STX>L11;o3,625;l760;w2<ETX>"
               + "<STX>L12;o3,680;l760;w2<ETX>"
               + "<STX>L13;o3,730;l760;w2<ETX>"
               + "<STX>L14;o3,5;f3;l800;w2<ETX>"
               + "<STX>L15;o770,5;f3;l800;w2<ETX>"
               + "<STX>L16;o130,570;f3;l800;w2<ETX>"
               + "<STX>L17;o380,680;f3;l800;w2<ETX>"
               + "<STX>L18;o500,680;f3;l800;w2<ETX>"
               + "<STX>H19;o5,580;f0;c66;h2;w2;d3,OWNER;k10;<ETX>"
               + "<STX>H20;o5,635;f0;c66;h2;w2;d3,LOT NO.;k10;<ETX>"
               + "<STX>H21;o5,685;f0;c66;h2;w2;d3,WEIGHT;k10;<ETX>"
               + "<STX>H22;o5,735;f0;c66;h2;w2;d3,REC.;k10;<ETX>"
               + "<STX>H23;o385,685;f0;c66;h2;w2;d3,LOT WT.;k10;<ETX>"
               + "<STX>H24;o385,735;f0;c66;h2;w2;d3,EXP.;k10;<ETX>"
               + "<STX>H25;o150,580;f0;c66;h2;w2;d0,30;k10;<ETX>"
               + "<STX>H26;o150,635;f0;c66;h3;w2;d0,30;k10;<ETX>"
               + "<STX>H27;o150,685;f0;c66;h2;w2;d0,30;k10;<ETX>"
               + "<STX>H28;o150,740;f0;c66;h2;w2;d0,30;k10;<ETX>"
               + "<STX>H29;o510,685;f0;c66;h2;w2;d0,30;k10;<ETX>"
               + "<STX>H30;o510,740;f0;c66;h2;w2;d0,30;k10;<ETX>"
               + "<STX>B31;o150,200;c6;i1;;h150;w2;d0,29;<ETX>"
               + "<STX>R<ETX>"
               + "<STX><ESC>E1<ETX>"
               + "<STX><CAN><ETX>"
               + "<STX>" + obj.RmCode + "<CR><ETX>"
               + "<STX>" + obj.OwnerShortName + "<CR><ETX>"
               + "<STX>" + obj.BOI + "<CR><ETX>"
               + "<STX>" + obj.PILotno + "<CR><ETX>"
               + "<STX>" + obj.RmNameBC + "<CR><ETX>"
               + "<STX>" + obj.RmDG + "<CR><ETX>"
               + "<STX>" + obj.RmTemp + "<CR><ETX>"
               + "<STX>" + obj.SoCRM + "<CR><ETX>"
               + "<STX>" + obj.OwnerName + "<CR><ETX>"
               + "<STX>" + obj.Lotno + "<CR><ETX>"
               + "<STX>" + obj.Weight + " Kgs." + "<CR><ETX>"
               + "<STX>" + obj.RecordDate + "<CR><ETX>"
               + "<STX>" + obj.LotWeight + " Kgs." + "<CR><ETX>"
               + "<STX>" + obj.ExpireDate + "<CR><ETX>"
               + "<STX>" + obj.BarcodeData + "<CR><ETX>"
               + "<STX><ETB><ETX>"
               ;
            }

            byte[] bufferSend = new byte[4096];
            TcpClient cl = new TcpClient();
            try
            {
                cl.Connect(address, Convert.ToInt32(port));
                if (cl.Connected)
                {
                    bufferSend = ASCIIEncoding.ASCII.GetBytes(Commandn);
                    NetworkStream nts = cl.GetStream();
                    if (nts.CanWrite)
                    {
                        nts.Write(bufferSend, 0, bufferSend.Length);
                    }
                    cl.Close();
                }
                return "สำเร็จ";
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("{0}{1}{2}", "การพิมพ์ข้อมูลลาเบลผิดพลาด", Environment.NewLine + "รายละเอียด :", ex.Message));
            }
        }

        public string CommandPrint(string port, string address, MaterialReceivePlanDtlBarcode obj)
        {

            Commandn = "";
            if (string.IsNullOrEmpty(address))
            {
                throw new Exception("กรุณาเลือกเครื่องพิมพ์ลาเบล 4x4");
            }


            int countGHSdtl = 0;
            string ghsNotify = "";

            if (obj.rmGHSlst != null)
            {
                if (obj.rmGHSlst.Notify)
                {
                    ghsNotify = "Danger";
                }
                else
                {
                    ghsNotify = "Warning";
                }

                if (obj.rmGHSlst.RmGHSDtlLst != null && obj.rmGHSlst.RmGHSDtlLst.Count != 0)
                {
                    countGHSdtl = obj.rmGHSlst.RmGHSDtlLst.Count;
                }
            }


            if (obj.rmId.Substring(0, 1) == "I" || obj.rmId.Substring(0, 1) == "S")
            {
                Commandn = "BF ON: BF \"CG Times\",13,0,1,1,1 \n" +
                            "PP 5 , 10 :PX 1200,790,5 \n" +
                            "PP 5 , 10 :PX 700,790,5 \n" +
                            "PP 5 , 10 :PX 640,790,5 \n" +
                            "PP 5 , 10 :PX 580,790,5 \n" +
                            "PP 5 , 10 :PX 520,790,5 \n" +
                            "PP 5 , 10 :PX 460,790,5 \n" +
                            "PP 5 , 10 :PX 400,790,5 \n" +
                            "PP 150, 465 :PX 245,645,5 \n" +
                            "PP 375, 465 :PX 65,155,5 \n" +
                            "PP 530, 585 :PX 65,155,5 \n" +
                            "PP 15  , 660:FT \"CG Times Bold\",11:PT \"OWNER\" \n" +
                            "PP 15  , 600:FT \"CG Times Bold\",11:PT \"LOT NO.\" \n" +
                            "PP 385 , 540:FT \"CG Times Bold\",11:PT \"Batch No.\" \n" +
                            "PP 15  , 540:FT \"CG Times Bold\",11:PT \"WEIGHT\" \n" +
                            "PP 15  , 480:FT \"CG Times Bold\",11:PT \"REC.\" \n" +
                            "PP 385 , 480:FT \"CG Times Bold\",11:PT \"EXP.\" \n" +

                            "PP 20  , 1030:FT \"CG Times Bold\",60:PT \"" + obj.RmCode + "\" \n" +
                            "PP 690 , 1140:FT \"CG Times Bold\",17:PT \"" + obj.OwnerShortName + "\" \n" +
                            "PP 50  , 890:BARSET \"CODE128\",2,1,3,80:PB \"" + obj.barcode + obj.batchNo + "\" \n" +
                            "PP 30  , 820:FT  \"CG Times Bold\",17:PT \"" + obj.rmName + "\" \n" +
                            "PP 190 , 660:FT  \"CG Times Bold\",11:PT \"" + obj.OwnerName + "\" \n" +
                            "PP 170 , 600:FT  \"CG Times Bold\",11:PT \"" + obj.lotNo + "\" \n" +
                            "PP 550 , 600:FT  \"CG Times Bold\",11:PT \"" + obj.batchNo + "\" \n" +
                            "PP 350 , 540:FT  \"CG Times Bold\",11:PT \"" + obj.Weight + " Kgs.\" \n" +
                            "PP 175 , 480:FT  \"CG Times Bold\",11:PT \"" + obj.RecordDate + "\" \n" +
                            "PP 550 , 480:FT  \"CG Times Bold\",11:PT \"" + obj.ExpireDate + "\" \n" +
                            "PP 340 , 420:FT  \"CG Times Bold\",11:PT \"" + ghsNotify + "\" \n";

                if (countGHSdtl != 0) SetCommandImg(obj.rmGHSlst.RmGHSDtlLst, countGHSdtl);

                Commandn = Commandn + "PF \n";
            }
            else
            {
                Commandn = "BF ON: BF \"CG Times\",13,0,1,1,1 \n" +
                            "PP 5 , 10 :PX 1200,790,5 \n" +
                            "PP 5 , 10 :PX 700,790,5 \n" +
                            "PP 5 , 10 :PX 640,790,5 \n" +
                            "PP 5 , 10 :PX 580,790,5 \n" +
                            "PP 5 , 10 :PX 520,790,5 \n" +
                            "PP 5 , 10 :PX 460,790,5 \n" +
                            "PP 5 , 10 :PX 400,790,5 \n" +
                            "PP 150, 465 :PX 245,645,5 \n" +
                            "PP 375, 465 :PX 125,420,5 \n" +
                            "PP 530, 465 :PX 125,265,5 \n" +
                            "PP 15  , 660:FT \"CG Times Bold\",11:PT \"OWNER\" \n" +
                            "PP 15  , 600:FT \"CG Times Bold\",11:PT \"LOT NO.\" \n" +
                            "PP 15  , 540:FT \"CG Times Bold\",11:PT \"WEIGHT\" \n" +
                            "PP 15  , 480:FT \"CG Times Bold\",11:PT \"REC.\" \n" +
                            "PP 385 , 540:FT \"CG Times Bold\",11:PT \"LOT WT.\" \n" +
                            "PP 385 , 480:FT \"CG Times Bold\",11:PT \"EXP.\" \n" +

                            "PP 20  , 1030:FT \"CG Times Bold\",60:PT \"" + obj.RmCode + "\" \n" +
                            "PP 540 , 1050:FT \"CG Times Bold\",19:PT \"" + obj.BOI + "\" \n" +
                            "PP 680 , 1050:FT \"CG Times Bold\",19:PT \"" + obj.piLotNo + "\" \n" +
                            "PP 690 , 1140:FT \"CG Times Bold\",17:PT \"" + obj.OwnerShortName + "\" \n" +
                            "PP 50  , 890:BARSET \"CODE128\",2,1,3,80:PB \"" + obj.barcode + "\" \n" +
                            "PP 30  , 820:FT  \"CG Times Bold\",17:PT \"" + obj.rmName + "\" \n" +
                            "PP 240 , 720:FT  \"CG Times Bold\",14:PT \"" + obj.RmDG + "\" \n" +
                            "PP 390 , 720:FT  \"CG Times Bold\",14:PT \"" + obj.RmTemp + "\" \n" +
                            "PP 540 , 720:FT  \"CG Times Bold\",14:PT \"" + obj.SoCRM + "\" \n" +
                            "PP 190 , 660:FT  \"CG Times Bold\",11:PT \"" + obj.OwnerName + "\" \n" +
                            "PP 350 , 600:FT  \"CG Times Bold\",11:PT \"" + obj.lotNo + "\" \n" +
                            "PP 175 , 540:FT  \"CG Times Bold\",11:PT \"" + obj.Weight + " Kgs.\" \n" +
                            "PP 175 , 480:FT  \"CG Times Bold\",11:PT \"" + obj.RecordDate + "\" \n" +
                            "PP 550 , 540:FT  \"CG Times Bold\",11:PT \"" + obj.LotWeight + " Kgs.\" \n" +
                            "PP 550 , 480:FT  \"CG Times Bold\",11:PT \"" + obj.ExpireDate + "\" \n" +
                            "PP 340 , 420:FT  \"CG Times Bold\",11:PT \"" + ghsNotify + "\" \n";

                if (countGHSdtl != 0) SetCommandImg(obj.rmGHSlst.RmGHSDtlLst, countGHSdtl);

                Commandn = Commandn + "PF \n";
            }


            TcpClient cl = new TcpClient();
            byte[] bufferSend = new byte[4096];
            try
            {
                cl.Connect(address, Convert.ToInt32(port));

                if (cl.Connected)
                {
                    bufferSend = ASCIIEncoding.ASCII.GetBytes(Commandn);
                    NetworkStream nts = cl.GetStream();
                    if (nts.CanWrite)
                    {
                        nts.Write(bufferSend, 0, bufferSend.Length);
                    }
                    cl.Close();
                }
                else
                {
                    throw new Exception(String.Format("{0}{1}", "การเชื่อมต่อเครื่องพิมพ์ลาเบลผิดพลาด \r\n กรุณาตรวจสอบเครื่องพิมพ์ลาเบล หรือติดต่อ IT Support"));
                }
                return "สำเร็จ";
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("{0}{1}{2}", "การเชื่อมต่อเครื่องพิมพ์ลาเบลผิดพลาด \r\nกรุณาตรวจสอบเครื่องพิมพ์ลาเบล หรือติดต่อ IT Support", Environment.NewLine + "รายละเอียด :", ex.Message));
            }

        }

        public bool PrintPdfFile(Stream file)
        {
            try
            {
                if (file.CanSeek)
                    //Reset the position of PDF document stream to be printed
                    file.Position = 0;
                //Create a new file in the Personal folder with the given name
                string createdFilePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Label" + file.Length.ToString());
                //Save the stream to the created file
                using (var dest = System.IO.File.OpenWrite(createdFilePath))
                    file.CopyTo(dest);
                string filePath = createdFilePath;
                PrintManager printManager = (PrintManager)CrossCurrentActivity.Current.Activity.GetSystemService(Context.PrintService);
                PrintDocumentAdapter pda = new CustomPrintDocumentAdapter(filePath);
                //Print with null PrintAttributes
                PrintAttributes attrib = new PrintAttributes.Builder().SetMediaSize(PrintAttributes.MediaSize.UnknownLandscape).Build();
                printManager.Print("Label" + file.Length.ToString(), pda, attrib);
                printManager = null;
                pda = null;
                file.Dispose();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return true;
        }

        private void SetCommandImg(List<RmGHSMatchDtlVM> dtl, int CountDtl)
        {
            int countSetimg = 1;

            dtl = dtl.OrderBy(x => x.DangerLevel).ToList();
            foreach (var lst in dtl)
            {
                if (CountDtl == 1)
                {
                    StrPlusCmd("PP 300 ,130:PM  \"" + lst.FileCenterID + "-196.PCX\" \n");
                }
                else if (CountDtl == 2)
                {
                    switch (countSetimg)
                    {
                        case 1: StrPlusCmd("PP 180 ,130:PM  \"" + lst.FileCenterID + "-196.PCX\" \n"); break;
                        case 2: StrPlusCmd("PP 430 ,130:PM  \"" + lst.FileCenterID + "-196.PCX\" \n"); break;
                    }
                    countSetimg++;
                }
                else if (CountDtl == 3)
                {
                    switch (countSetimg)
                    {
                        case 1: StrPlusCmd("PP 80 ,130:PM  \"" + lst.FileCenterID + "-196.PCX\" \n"); break;
                        case 2: StrPlusCmd("PP 300 ,130:PM  \"" + lst.FileCenterID + "-196.PCX\" \n"); break;
                        case 3: StrPlusCmd("PP 520 ,130:PM  \"" + lst.FileCenterID + "-196.PCX\" \n"); break;
                    }
                    countSetimg++;
                }
                else if (CountDtl == 4)
                {
                    switch (countSetimg)
                    {
                        case 1: StrPlusCmd("PP 20 ,130:PM  \"" + lst.FileCenterID + "-196.PCX\" \n"); break;
                        case 2: StrPlusCmd("PP 310 ,130:PM  \"" + lst.FileCenterID + "-196.PCX\" \n"); break;
                        case 3: StrPlusCmd("PP 400 ,130:PM  \"" + lst.FileCenterID + "-196.PCX\" \n"); break;
                        case 4: StrPlusCmd("PP 590 ,130:PM  \"" + lst.FileCenterID + "-196.PCX\" \n"); break;
                    }
                    countSetimg++;
                }
                else if (CountDtl == 5)
                {
                    switch (countSetimg)
                    {
                        case 1: StrPlusCmd("PP 40 ,150:PM   \"" + lst.FileCenterID + "-128.PCX\" \n"); break;
                        case 2: StrPlusCmd("PP 190 ,150:PM  \"" + lst.FileCenterID + "-128.PCX\" \n"); break;
                        case 3: StrPlusCmd("PP 340 ,150:PM  \"" + lst.FileCenterID + "-128.PCX\" \n"); break;
                        case 4: StrPlusCmd("PP 490 ,150:PM  \"" + lst.FileCenterID + "-128.PCX\" \n"); break;
                        case 5: StrPlusCmd("PP 640 ,150:PM  \"" + lst.FileCenterID + "-128.PCX\" \n"); break;
                    }
                    countSetimg++;
                }
                else if (CountDtl == 6)
                {
                    switch (countSetimg)
                    {
                        case 1: StrPlusCmd("PP 40  ,220:PM  \"" + lst.FileCenterID + "-128.PCX\" \n"); break;
                        case 2: StrPlusCmd("PP 190 ,220:PM  \"" + lst.FileCenterID + "-128.PCX\" \n"); break;
                        case 3: StrPlusCmd("PP 340 ,220:PM  \"" + lst.FileCenterID + "-128.PCX\" \n"); break;
                        case 4: StrPlusCmd("PP 490 ,220:PM  \"" + lst.FileCenterID + "-128.PCX\" \n"); break;
                        case 5: StrPlusCmd("PP 640 ,220:PM  \"" + lst.FileCenterID + "-128.PCX\" \n"); break;
                        case 6: StrPlusCmd("PP 40  ,50:PM   \"" + lst.FileCenterID + "-128.PCX\" \n"); break;
                    }
                    countSetimg++;
                }
                else if (CountDtl == 7)
                {
                    switch (countSetimg)
                    {
                        case 1: StrPlusCmd("PP 40  ,220:PM  \"" + lst.FileCenterID + "-128.PCX\" \n"); break;
                        case 2: StrPlusCmd("PP 190 ,220:PM  \"" + lst.FileCenterID + "-128.PCX\" \n"); break;
                        case 3: StrPlusCmd("PP 340 ,220:PM  \"" + lst.FileCenterID + "-128.PCX\" \n"); break;
                        case 4: StrPlusCmd("PP 490 ,220:PM  \"" + lst.FileCenterID + "-128.PCX\" \n"); break;
                        case 5: StrPlusCmd("PP 640 ,220:PM  \"" + lst.FileCenterID + "-128.PCX\" \n"); break;
                        case 6: StrPlusCmd("PP 40  ,50:PM   \"" + lst.FileCenterID + "-128.PCX\" \n"); break;
                        case 7: StrPlusCmd("PP 190 ,50:PM   \"" + lst.FileCenterID + "-128.PCX\" \n"); break;
                    }
                    countSetimg++;
                }
                else if (CountDtl == 8)
                {
                    switch (countSetimg)
                    {
                        case 1: StrPlusCmd("PP 40  ,220:PM  \"" + lst.FileCenterID + "-128.PCX\" \n"); break;
                        case 2: StrPlusCmd("PP 190 ,220:PM  \"" + lst.FileCenterID + "-128.PCX\" \n"); break;
                        case 3: StrPlusCmd("PP 340 ,220:PM  \"" + lst.FileCenterID + "-128.PCX\" \n"); break;
                        case 4: StrPlusCmd("PP 490 ,220:PM  \"" + lst.FileCenterID + "-128.PCX\" \n"); break;
                        case 5: StrPlusCmd("PP 640 ,220:PM  \"" + lst.FileCenterID + "-128.PCX\" \n"); break;
                        case 6: StrPlusCmd("PP 40  ,50:PM   \"" + lst.FileCenterID + "-128.PCX\" \n"); break;
                        case 7: StrPlusCmd("PP 190 ,50:PM   \"" + lst.FileCenterID + "-128.PCX\" \n"); break;
                        case 8: StrPlusCmd("PP 340 ,50:PM   \"" + lst.FileCenterID + "-128.PCX\" \n"); break;
                    }
                    countSetimg++;
                }
                else
                {
                    switch (countSetimg)
                    {
                        case 1: StrPlusCmd("PP 40  ,220:PM  \"" + lst.FileCenterID + "-128.PCX\" \n"); break;
                        case 2: StrPlusCmd("PP 190 ,220:PM  \"" + lst.FileCenterID + "-128.PCX\" \n"); break;
                        case 3: StrPlusCmd("PP 340 ,220:PM  \"" + lst.FileCenterID + "-128.PCX\" \n"); break;
                        case 4: StrPlusCmd("PP 490 ,220:PM  \"" + lst.FileCenterID + "-128.PCX\" \n"); break;
                        case 5: StrPlusCmd("PP 640 ,220:PM  \"" + lst.FileCenterID + "-128.PCX\" \n"); break;
                        case 6: StrPlusCmd("PP 40  ,50:PM   \"" + lst.FileCenterID + "-128.PCX\" \n"); break;
                        case 7: StrPlusCmd("PP 190 ,50:PM   \"" + lst.FileCenterID + "-128.PCX\" \n"); break;
                        case 8: StrPlusCmd("PP 340 ,50:PM   \"" + lst.FileCenterID + "-128.PCX\" \n"); break;
                        case 9: StrPlusCmd("PP 490 ,50:PM   \"" + lst.FileCenterID + "-128.PCX\" \n"); break;

                    }
                    countSetimg++;
                }
            }
        }

        private void StrPlusCmd(string txt)
        {
            Commandn = Commandn + txt;
        }

        public bool PrintPdfFile(Stream file, MediaSize size = MediaSize.Unknown)
        {
            try
            {
                if (file.CanSeek)
                    file.Position = 0;
                string createdFilePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Label" + file.Length.ToString());
                using (var dest = System.IO.File.OpenWrite(createdFilePath))
                    file.CopyTo(dest);
                string filePath = createdFilePath;
                PrintManager printManager = (PrintManager)CrossCurrentActivity.Current.Activity.GetSystemService(Context.PrintService);
                PrintAttributes.MediaSize mediaSize;
                PrintAttributes.Resolution resolution = new PrintAttributes.Resolution("portrait", "Portrait", 300, 600);
                switch (size)
                {
                    case MediaSize.A4:
                        mediaSize = PrintAttributes.MediaSize.IsoA4;
                        break;
                    case MediaSize.A5:
                        mediaSize = PrintAttributes.MediaSize.IsoA5;
                        resolution = new PrintAttributes.Resolution("landscape", "Landscape", 600, 300);
                        break;
                    case MediaSize.FourXSix:
                        mediaSize = new PrintAttributes.MediaSize("4x6", "4x6", 4000, 6000);
                        break;
                    case MediaSize.FourXFour:
                        mediaSize = new PrintAttributes.MediaSize("4x4", "4x4", 4000, 4000);
                        break;
                    default:
                        mediaSize = PrintAttributes.MediaSize.UnknownPortrait;
                        break;
                }

                PrintAttributes attrib = new PrintAttributes.Builder()
                    .SetMediaSize(mediaSize)
                    //.SetColorMode(PrintColorMode.Monochrome)
                    .SetResolution(resolution)
                    //.SetMinMargins(new PrintAttributes.Margins(10, 10, 10, 10))
                    .Build();
                PrintDocumentAdapter pda = new CustomPrintDocumentAdapter(filePath);
                printManager.Print("Label" + file.Length.ToString(), pda, attrib);
                printManager = null;
                pda = null;
                file.Dispose();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return true;
        }
    }
}