using Convertion_XXX;
using EMV_XXX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMV_XXX
{
    public class TLVParser
    {
        IEnumerable<TagsDictionary> tagsDictionary = new EMVTagsDictionary().List();
        List<string> rawDataList = new List<string>();
        List<TNLV> tnlvList = new List<TNLV>();
        string rawData = "";

        public TLVParser(string _data)
        {
            if (_data.Length == 0)
                throw new ArgumentException("Invalid data");

            this.rawData = _data.Replace(" ", "");
        }

        public TLVParser(List<string> _data)
        {
            if (_data.Count == 0)
                throw new ArgumentException("Invalid data");

            this.rawDataList = _data;
        }

        public IEnumerable<TNLV> Parse()
        {
            bool procedure = (rawData.Length == 0) ? true : false;

            if (procedure)
            {
                foreach (var rawDataInList in rawDataList)
                {
                    ExecuteUpdate(rawDataInList);
                }
            }
            else
                ExecuteUpdate(rawData);

            return tnlvList;
        }

        void ExecuteUpdate(string _rawData)
        {
            try
            {
                var rawList = new List<TNLV>();

                rawList.AddRange(EMVParent(_rawData, ""));

                tnlvList.AddRange(rawList);
            }
            catch (Exception ex)
            {
                Console.WriteLine("EMV Parser Error:" + ex.ToString());
            }
        }

        IEnumerable<TNLV> EMVParent(string _rawData, string template)
        {
            var rawList = new List<TNLV>();
            var data = _rawData;
            var index = 0;
            var len = 0;
            var lenLength = "";
            var startIndexToRead = 0;
            var val = "";
            TNLV _tnlv;

            try
            {
                for (int i = 0; i < _rawData.Length; i++)
                {
                    //Check the 1st 2 Chars
                    for (int ii = 1; ii <= 4; ii++)
                    {
                        var toCheckData = data.Substring(startIndexToRead, ii);

                        if (template.Length == 0)
                            _tnlv = tagsDictionary.FirstOrDefault(w => w.Tags == toCheckData);
                        else
                            _tnlv = tagsDictionary.FirstOrDefault(w => w.Tags == toCheckData && w.Template == template);


                        if (_tnlv != null)
                        {
                            lenLength = _tnlv.Length == null ? data.Substring(i + ii, 2) : _tnlv.Length;
                            len = Convertion.HexToDecimal(lenLength) * 2;
                            val = TagsConvertedValue(_tnlv.Tags, data.Substring(i + ii + lenLength.Length, len));
                            startIndexToRead = i + ii + lenLength.Length + len;
                            i = startIndexToRead - 1;
                            rawList.Add(new TNLV
                            {
                                Length = len.ToString(),
                                Name = _tnlv.Name,
                                Tags = _tnlv.Tags,
                                Value = val
                            });

                            //Check if the Tags is also a Template
                            var checkIfTagIsTemplate = tagsDictionary.FirstOrDefault(w => w.Template == _tnlv.Tags);

                            if (checkIfTagIsTemplate != null)
                                rawList.AddRange(EMVParent(val, _tnlv.Tags));

                            break;
                        }

                        if (ii == 4)
                        {
                            return rawList;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return rawList;
        }

        string TagsConvertedValue(string tag, string value)
        {
            var val = value;

            try
            {
                switch (tag)
                {
                    case "5F20":
                    case "50":
                    case "5F2D":
                    case "9F1F":
                        val = Convertion.HexToAscii(value);
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return val;
        }

    }
}