using EMV_XXX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMV_TLV_Parser_Consume
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("####### SINGLE DATA TLV PARSING");

            //Test Consume of Sigle Data
            var data4 = "703C57126364010001000013D22056015280000000005F20165445535420434152442F4341524420313030303031339F1F0C3532383030303030303030309000";

            var singleParsedTLVinDataList = new TLVParser(data4).Parse();

            foreach (var _tnlv in singleParsedTLVinDataList)
            {
                Console.WriteLine("--" + _tnlv.Tags + " " + _tnlv.Name);

                if (_tnlv.Value.Length > 0)
                    Console.WriteLine("----" + _tnlv.Value);
            }

            Console.WriteLine("");
            Console.WriteLine("");

            Console.WriteLine("####### MULTIPLE DATA TLV PARSING");
            //Test Consume of Multiple Data
            var data1 = "703C57126364010001000013D22056015280000000005F20165445535420434152442F4341524420313030303031339F1F0C3532383030303030303030309000";
            var data2 = "70485F24032205315A0863640100010000218E0A000000000000000002009F0D0590709C98009F0E0520000000009F0F0590709C98005F25031704305F3401009F0702AB005F280206089000";
            var data3 = "703C9F080200968C159F02069F03069F1A0295055F2A029A039C019F37048D198A029F02069F03069F1A0295055F2A029A039C019F3704910A5F300206019000";

            var dataList = new List<string>();
            dataList.Add(data1);
            dataList.Add(data2);
            dataList.Add(data3);
            
            var mutipleParsedTLVinDataList = new TLVParser(dataList).Parse();

            foreach (var _tnlv in mutipleParsedTLVinDataList)
            {
                Console.WriteLine("--" + _tnlv.Tags + " " + _tnlv.Name);

                if (_tnlv.Value.Length > 0)
                    Console.WriteLine("----" + _tnlv.Value);
            }

            Console.ReadLine();
        }
    }
}
