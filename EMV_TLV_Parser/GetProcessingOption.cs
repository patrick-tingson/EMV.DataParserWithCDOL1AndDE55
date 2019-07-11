using Convertion_XXX;
using System.Collections.Generic;

namespace EMV_XXX
{
    public class GetProcessingOption
    {
        public int ResponseMessageTemplateFormat { get; set; }
        public string SW1 { get; set; }
        public string SW2 { get; set; }
        public string ApplicationInterchangeProfile { get; set; }
        public List<ApplicationFileLocator> ApplicationFileLocatorList { get; set; }

        string data = "";

        public GetProcessingOption(string data)
        {
            if (data.Length > 0)
            {
                this.data = data;
                this.SW1 = data.Substring(data.Length - 4, 2);
                this.SW2 = data.Substring(data.Length - 2, 2);
                if ((SW1 == "90") && (SW2 == "00"))
                {
                    ResponseMessageTemplateFormat = (data.Substring(0, 2) == Tags.ResponseMessageTemplateFormat1) ? 1 : 2;
                    this.data = data.Substring(4, data.Length - 8); //REMOVE RMTF, Length, SW1, SW2
                    ApplicationInterchangeProfile = ParseAIP();
                    ApplicationFileLocatorList = ParseAFL();
                }
            }
        }

        List<ApplicationFileLocator> ParseAFL()
        {
            string parseData = "";
            List<ApplicationFileLocator> response = new List<ApplicationFileLocator>();
            ApplicationFileLocator afl;
            switch (ResponseMessageTemplateFormat)
            {
                case 1:
                    parseData = data.Substring(4, data.Length - 4);
                    break;
                case 2:
                    int l = Convertion.HexToDecimal(data.Substring(data.IndexOf(Tags.ApplicationFileLocator) + 2, 2)) * 2;
                    parseData = data.Substring(data.IndexOf(Tags.ApplicationFileLocator) + 4, l);
                    break;
            }
            for (int i = 0; i <= parseData.Length - 1; i += 8)
            {
                afl = new ApplicationFileLocator();
                afl.ShortFileIdentifier = parseData.Substring(i, 2);
                afl.StartRecord = parseData.Substring(i + 2, 2);
                afl.EndRecord = parseData.Substring(i + 4, 2);
                afl.NumberOfRecords = parseData.Substring(i + 6, 2);
                response.Add(afl);
            }
            return response;
        }

        string ParseAIP()
        {
            string response = "";
            switch (ResponseMessageTemplateFormat)
            {
                case 1:
                    response = data.Substring(0, 4);
                    break;
                case 2:
                    response = data.Substring(4, Convertion.HexToDecimal(data.Substring(2, 2)) * 2);
                    break;
            }
            return response;
        }
    }
}