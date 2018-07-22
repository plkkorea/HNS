using HNS.BusinessSvcs;
using HNS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static HNS.Entities.Product_Info;

namespace HNS.ViewModels
{
    public class ProdCompareViewModel
    {
        //private readonly IProductService _productService;
        private readonly IProductCompareService _productService;
        public ProdCompareViewModel(IProductCompareService productService)
        {
            _productService = productService;
            PopulateProperties();
        }

        public ProdCompareViewModel()
        {

        }

        public void PopulateProperties()
        {
            var products = _productService.GetProducts();

            //LCDSizeAndResolutions = new List<string>() { "None(외부 모니터 별도 구매 Anglog RGB Port,1024x768) ", "B","C" };
            BezelTypes = new List<string>() { "None(OpenFrame Type)", "B1(Bezel Type)", "B2(Bezel Type)" };
            CPUPerformances = new List<string>() { "1Ghz", "667Mhz", "266Mhz" };
            ExtensionPorts1 = new List<string>() { "16핀(Extension Port - I)" };
            ExtensionPorts2 = new List<string>() { "48핀(Extension Port - II)" };
            WiredLans = new List<string>() { "100Mbps", "10Mbps", "None" };
            RAMs = new List<string>() { "64MB", "256MB", "512MB" };
            Flases = new List<string>() { "64MB", "128MB", "256MB" };
            Powers = new List<string> { "DC 9 ~ 24V", "DC 5V only" };
            UARTs = new List<string> { "3Ch", "4Ch" };
            RS485s = new List<string> { "H/W 지원", "S/W 지원" };
            ADCs = new List<string> { "6Ch(12Bit)", "4Ch(12Bit)", "4Ch(10Bit)" };
            USBHosts = new List<string> { "2Ch", "1Ch" };
            CECertifications = new List<string> { "CE 인증 제품" };
            SmartIOs2 = new List<string> { "Smart I/O - II" };
            SmartIOs3 = new List<string> { "Smart I/O - III" };
            SmartVideos = new List<string> { "지원", "미지원" };
            SmartBatteries = new List<string> { "지원", "미지원" };

            //LCDSizeAndResolutions = products.Select(m => m.Screen_size).Distinct().ToList();
            //BezelTypes = products.Select(m => m.B1B2_Bezel).Distinct().ToList();
            //CPUPerformances = products.Select(m => m.CPU).Distinct().ToList();
            //ExtensionPorts = products.Select(m => m.Extension_Port_I_II).Distinct().ToList();
            //WiredLans = products.Select(m => m.WLAN).Distinct().ToList();
            //RAMs = products.Select(m => m.RAM).Distinct().ToList();
            //Flases = products.Select(m => m.Flash).Distinct().ToList();
            //Powers = products.Select(m => m.power).Distinct().ToList();
            //UARTs = products.Select(m => m.PWM).Distinct().ToList();
            //RS485s = products.Select(m => m.RS485).Distinct().ToList();
            //ADCs = products.Select(m => m.AD_Converter).Distinct().ToList();
            //USBHosts = products.Select(m => m.USBHost).Distinct().ToList();
            //CECertifications = products.Select(m => m.certification).Distinct().ToList();
            //SmartIOs = products.Select(m => m.SmartIO_III).Distinct().ToList();
            //SmartVideos = products.Select(m => m.SmartVideo).Distinct().ToList();
            //SmartBatteries = products.Select(m => m.SmartBattery).Distinct().ToList();


        }

        public List<string> LCDSizeAndResolutions { get; set; }
        public List<string> BezelTypes { get; set; }
        public List<string> CPUPerformances { get; set; }
        public List<string> ExtensionPorts1 { get; set; }
        public List<string> ExtensionPorts2 { get; set; }
        public List<string> WiredLans { get; set; }
        public List<string> RAMs { get; set; }
        public List<string> Flases { get; set; }
        public List<string> Powers { get; set; }
        public List<string> UARTs { get; set; }
        public List<string> RS485s { get; set; }
        public List<string> ADCs { get; set; }
        public List<string> USBHosts { get; set; }
        public List<string> CECertifications { get; set; }
        public List<string> SmartIOs2 { get; set; }
        public List<string> SmartIOs3 { get; set; }
        public List<string> SmartVideos { get; set; }
        public List<string> SmartBatteries { get; set; }

        public List<string> SearchResult { get; set; }

        public ProdCompareViewModel Search(ProdCompareViewModel model)
        {
            var products = _productService.GetProducts();

            if (model.ADCs != null && model.ADCs.Count > 0)
            {
                products = products.Where(m => model.ADCs.Contains(m.ADC, new CaseInsensitiveComparer())).ToList();
            }

            if (model.BezelTypes != null && model.BezelTypes.Count > 0)
            {
                products = products.Where(m => model.BezelTypes.Contains(m.Bezel_Type, new CaseInsensitiveComparer())).ToList();
            }

            if (model.CECertifications != null && model.CECertifications.Count > 0)
            {
                products = products.Where(m => model.CECertifications.Contains(m.Certification, new CaseInsensitiveComparer())).ToList();
            }

            if (model.CPUPerformances != null && model.CPUPerformances.Count > 0)
            {
                products = products.Where(m => model.CPUPerformances.Contains(m.CPU, new CaseInsensitiveComparer())).ToList();
            }

            if (model.ExtensionPorts1 != null && model.ExtensionPorts1.Count > 0)
            {
                products = products.Where(m => model.ExtensionPorts1.Contains(m.ExtensionPortI, new CaseInsensitiveComparer())).ToList();
            }

            if (model.ExtensionPorts2 != null && model.ExtensionPorts2.Count > 0)
            {
                products = products.Where(m => model.ExtensionPorts2.Contains(m.ExtensionPortII, new CaseInsensitiveComparer())).ToList();
            }

            if (model.Flases != null && model.Flases.Count > 0)
            {
                products = products.Where(m => model.Flases.Contains(m.FLASH, new CaseInsensitiveComparer())).ToList();
            }

            if (model.LCDSizeAndResolutions != null && model.LCDSizeAndResolutions.Count > 0)
            {
                products = products.Where(m => model.LCDSizeAndResolutions.Contains(m.LCD_Size, new CaseInsensitiveComparer())).ToList();
            }

            if (model.Powers != null && model.Powers.Count > 0)
            {
                products = products.Where(m => model.Powers.Contains(m.POWER, new CaseInsensitiveComparer())).ToList();
            }

            if (model.RAMs != null && model.RAMs.Count > 0)
            {
                products = products.Where(m => model.RAMs.Contains(m.RAM, new CaseInsensitiveComparer())).ToList();
            }

            if (model.RS485s != null && model.RS485s.Count > 0)
            {
                products = products.Where(m => model.RS485s.Contains(m.RS485, new CaseInsensitiveComparer())).ToList();
            }

            if (model.SmartBatteries != null && model.SmartBatteries.Count > 0)
            {
                products = products.Where(m => model.SmartBatteries.Contains(m.SmartBattery, new CaseInsensitiveComparer())).ToList();
            }

            if (model.SmartIOs2 != null && model.SmartIOs2.Count > 0)
            {
                products = products.Where(m => model.SmartIOs2.Contains(m.SmartIO2, new CaseInsensitiveComparer())).ToList();
            }

            if (model.SmartIOs3 != null && model.SmartIOs3.Count > 0)
            {
                products = products.Where(m => model.SmartIOs3.Contains(m.SmartIO3, new CaseInsensitiveComparer())).ToList();
            }

            if (model.SmartVideos != null && model.SmartVideos.Count > 0)
            {
                products = products.Where(m => model.SmartVideos.Contains(m.SmartVideo, new CaseInsensitiveComparer())).ToList();
            }

            if (model.UARTs != null && model.UARTs.Count > 0)
            {
                products = products.Where(m => model.UARTs.Contains(m.UART, new CaseInsensitiveComparer())).ToList();
            }

            if (model.USBHosts != null && model.USBHosts.Count > 0)
            {
                products = products.Where(m => model.USBHosts.Contains(m.USB_HOST, new CaseInsensitiveComparer())).ToList();
            }

            if (model.WiredLans != null && model.WiredLans.Count > 0)
            {
                products = products.Where(m => model.WiredLans.Contains(m.LAN, new CaseInsensitiveComparer())).ToList();
            }

            model.SearchResult = products.Select(m => m.Product_Name).ToList();

            return model;
        }
    }

    public class Option
    {
        public string Name { get; set; }
        public bool Checked { get; set; }
    }
}
