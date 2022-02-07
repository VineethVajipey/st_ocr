using System;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace Tesseract.ConsoleDemo
{
    internal class Program
    {

        public static void Main(string[] args)
        {
            Regex cage_rg = new Regex(@"Cage: (.+)");
            Regex pn_rg = new Regex(@"PN: (.+)");
            Regex model_rg = new Regex(@"Model: (.+)");
            Regex serial_rg = new Regex(@"Serial #: (.+)");
            Regex contract_rg = new Regex(@"Contract #: (.+)");
            Regex warranty_rg = new Regex(@"Warranty End: (.+)");
            Device d = new Device();
            var testImagePath = @"C:\Users\vinee\Documents\Swingtech\tesseract-samples-master\src\Tesseract.ConsoleDemo\test2.jpg";
            if (args.Length > 0)
            {
                testImagePath = args[0];
            }

            try
            {
                using (var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default))
                {
                    using (var img = Pix.LoadFromFile(testImagePath))
                    {
                        using (var page = engine.Process(img))
                        {
                            var text = page.GetText();
                            Console.WriteLine("Mean confidence: {0}", page.GetMeanConfidence());

                            Console.WriteLine("Text (GetText): \r\n{0}", text);
                            Console.WriteLine("Text (iterator):");
                            string line = "";
                            string word = "";
                            using (var iter = page.GetIterator())
                            {
                                iter.Begin();

                                do
                                {
                                    do
                                    {
                                        do
                                        {
                                            do
                                            {
                                                if (iter.IsAtBeginningOf(PageIteratorLevel.Block))
                                                {
                                                    Console.WriteLine("<BLOCK>");
                                                }
          
                                                line = iter.GetText(PageIteratorLevel.TextLine);
                                                word = iter.GetText(PageIteratorLevel.Word);
                                                
                                                if(cage_rg.Match(line).Success)
                                                {
                                                    d.cage = cage_rg.Match(line).Groups[1].Value;
                                                } else if (pn_rg.Match(line).Success)
                                                {
                                                    d.PN = pn_rg.Match(line).Groups[1].Value;
                                                } else if (model_rg.Match(line).Success)
                                                {
                                                    d.model = model_rg.Match(line).Groups[1].Value;
                                                } else if (serial_rg.Match(line).Success)
                                                {
                                                    d.serialNum = serial_rg.Match(line).Groups[1].Value;
                                                } else if (contract_rg.Match(line).Success)
                                                {
                                                    d.contractNum = contract_rg.Match(line).Groups[1].Value;
                                                } else if (warranty_rg.Match(line).Success)
                                                {
                                                    d.warrantyEnd = warranty_rg.Match(line).Groups[1].Value;
                                                }

                                                Console.Write(word);
                                                Console.Write(" ");

                                                if (iter.IsAtFinalOf(PageIteratorLevel.TextLine, PageIteratorLevel.TextLine))
                                                {
                                                    Console.WriteLine();
                                                }
                                            } while (iter.Next(PageIteratorLevel.TextLine, PageIteratorLevel.Word));

                                            if (iter.IsAtFinalOf(PageIteratorLevel.Para, PageIteratorLevel.TextLine))
                                            {
                                                Console.WriteLine();
                                            }
                                        } while (iter.Next(PageIteratorLevel.Para, PageIteratorLevel.TextLine));
                                    } while (iter.Next(PageIteratorLevel.Block, PageIteratorLevel.Para));
                                } while (iter.Next(PageIteratorLevel.Block));
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Trace.TraceError(e.ToString());
                Console.WriteLine("Unexpected Error: " + e.Message);
                Console.WriteLine("Details: ");
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("MISTA STOOOOOOOPPID");
            Console.WriteLine("Cage:" + d.cage);
            Console.WriteLine("PN: " + d.PN);
            Console.WriteLine("Model#: " + d.model);
            Console.WriteLine("Serial#: " + d.serialNum);
            Console.WriteLine("Contract#: " + d.contractNum);

            Console.Write("Press any key to continue . . . ");
            Console.ReadKey(true);
        }
        
        class Device {
            public string cage;
            public string PN;
            public string model;
            public string serialNum;
            public string contractNum;
            public string warrantyEnd;
        }

    
    }
}