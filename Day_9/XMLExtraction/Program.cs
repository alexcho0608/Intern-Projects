using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Xml;
using System.ComponentModel;
using DAL;
using System.Globalization;
using System.Net.Mail;
using System.Net;

namespace XMLExtraction
{
    class Program
    {
        public static BankDBv2Entities context;
        public Timer timer_;
        public Timer timerCurrencies;
        public static IQueryable<Currency> currencies;
        public static IQueryable<CurrenciesExchangeRate> rates;
        public static Dictionary<string, double> xmlCacheData = new Dictionary<string,double>();
        public static Dictionary<string, bool> isoFlagDict = new Dictionary<string, bool>();
        public static bool locker;
        static readonly Random rnd = new Random();

        public void XMLExtraction(object sender, ElapsedEventArgs args)
        {
            lock(xmlCacheData){
                if (!locker)
                {
                    locker = true;
                    XmlDocument xml = new XmlDocument();
                    xml.Load(@"C:\Users\a\Desktop\view-source_www.floatrates.com_daily_eur.xml");
                
                    XmlNodeList nodes = xml.SelectNodes("//channel/item");
                    foreach (XmlNode node in nodes)
                    {
                        try
                        {
                            double value = Convert.ToDouble(node.SelectSingleNode("exchangeRate").InnerText, CultureInfo.InvariantCulture);
                            string name = node.SelectSingleNode("targetCurrency").InnerText;
                            if (!xmlCacheData.ContainsKey(name)) continue;

                            if (xmlCacheData[name] == value) isoFlagDict[name] = false;
                            else isoFlagDict[name] = true;
                            xmlCacheData[name] = value;

                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                    ProcessChanges();
                    locker = false;
                }
            }
        }

        public void ProcessChanges()
        {
            foreach(var flag in isoFlagDict)
            {
                if(!flag.Value) continue;

                var isoCode = flag.Key;

                var isoValue = xmlCacheData[isoCode];

                foreach(var isoElem in xmlCacheData)
                {
                    if (isoElem.Key == isoCode) continue;

                    var currency1 = currencies.FirstOrDefault(e => e.ISOCode == isoCode);
                    var currency2 = currencies.FirstOrDefault(e => e.ISOCode == isoElem.Key);
                    
                     

                    CurrenciesExchangeRate curRate = new CurrenciesExchangeRate()
                    {
                        FromCurrencyID = currency1.ID,
                        ToCurrencyID = currency2.ID,
                        Rate = (decimal) (isoValue / isoElem.Value),
                        EntryDate = GetRandomDate(DateTime.Now,DateTime.MaxValue)
                    };

                    context.CurrenciesExchangeRates.Add(curRate);
                }
                
            }

            context.SaveChanges();
            isoFlagDict["EUR"] = false;
        }

        public void CheckCurrencies(object sender, ElapsedEventArgs args)
        {
            var queryCurrency = context.Currencies.Select(e => e);
            lock (xmlCacheData)
            {
                foreach (var elem in queryCurrency)
                {
                    if (!xmlCacheData.ContainsKey(elem.ISOCode))
                    {
                        xmlCacheData.Add(elem.ISOCode, 0);
                    }
                }
            }
        }

        public static bool IsSupported(string iso)
        {
            return currencies.Any(e => e.ISOCode == iso);
        }

        public static DateTime GetRandomDate(DateTime from, DateTime to)
        {
            var range = to - from;

            var randTimeSpan = new TimeSpan((long)(rnd.NextDouble() * range.Ticks));

            return from + randTimeSpan;
        }

        public static void loadIsoCodes(){

            currencies = context.Currencies.Select(e => e);
            rates = context.CurrenciesExchangeRates.Select(e => e);
            foreach (var cur in currencies.ToList())
            {
                xmlCacheData.Add(cur.ISOCode, 0);
                isoFlagDict.Add(cur.ISOCode, true);
            }
            xmlCacheData["EUR"] = 1;
        }
        
        static void Main(string[] args)
        {
            //locker = false;
            //context = new BankDBv2Entities();
            //loadIsoCodes();

            //Program p = new Program();
            //p.timer_ = new Timer(1000*10);
            //p.timer_.Enabled = true;
            //p.timer_.Elapsed += new ElapsedEventHandler(p.XMLExtraction);

            //p.timerCurrencies = new Timer(1000 * 9);
            //p.timerCurrencies.Enabled = true;
            //p.timerCurrencies.Elapsed += new ElapsedEventHandler(p.CheckCurrencies);

            //var fromAddress = new MailAddress("alexstefanov06@gmail.com", "From Name");
            //var toAddress = new MailAddress("alex_stef@mail.bg", "To Name");
            //const string fromPassword = "";
            //const string subject = "Subject";
            //const string body = "Body";

            //var smtp = new SmtpClient
            //{
            //    Host = "smtp.gmail.com",
            //    Port = 587,
            //    EnableSsl = true,
            //    DeliveryMethod = SmtpDeliveryMethod.Network,
            //    UseDefaultCredentials = false,
            //    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            //};
            //using (var message = new MailMessage(fromAddress, toAddress)
            //{
            //    Subject = subject,
            //    Body = body
            //})
            //{
            //    smtp.Send(message);
            //}



            Console.ReadLine();
        }
    }
}
