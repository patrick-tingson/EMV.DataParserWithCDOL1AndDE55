using System;
using System.Collections.Generic;
using System.Linq;

namespace EMV_XXX
{
    public class CreateCDOL1
    {
        IEnumerable<TagsDictionary> tagsDictionary = new EMVTagsDictionary().List();
        IEnumerable<TNLV> DE55_TNLV = new List<TNLV>();
        List<TNLV> cdolTagList = new List<TNLV>();
        string TAG_8C = "";


        public CreateCDOL1(string _TAG_8C)
        {
            if (_TAG_8C.Length == 0)
                throw new ArgumentException("Invalid TAG_8C");

            this.TAG_8C = _TAG_8C;
        }

        public CreateCDOL1(IEnumerable<TNLV> _DE55_TNLV, string _TAG_8C)
        {
            if (_DE55_TNLV == null)
                throw new ArgumentException("Invalid DE55_TNLV");

            if (_TAG_8C.Length == 0)
                throw new ArgumentException("Invalid TAG_8C");

            this.TAG_8C = _TAG_8C;
            this.DE55_TNLV = _DE55_TNLV;
        }

        public string Create()
        {
            if (DE55_TNLV == null)
                throw new ArgumentException("Invalid DE55_TNLV");

            var result = "";
            try
            {
                var tagSelector = new TagsSelector(DE55_TNLV);

                foreach (var cdol1 in Parse())
                {
                    result = string.Format("{0}{1}",
                        result,
                        tagSelector.Value(cdol1.Tags));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw ex;
            }

            return result;
        }

        public IEnumerable<TNLV> Parse()
        {
            var data = TAG_8C;
            var index = 0;
            var len = 0;
            var lenLength = "";
            var startIndexToRead = 0;
            var val = "";
            TNLV _tnlv;

            try
            {
                for (int i = 0; i < TAG_8C.Length; i++)
                {
                    //Check the 1st 2 Chars
                    for (int ii = 1; ii <= 4; ii++)
                    {
                        var toCheckData = data.Substring(startIndexToRead, ii);

                        _tnlv = tagsDictionary.FirstOrDefault(w => w.Tags == toCheckData);

                        if (_tnlv != null)
                        {
                            lenLength = _tnlv.Length == null ? data.Substring(i + ii, 2) : _tnlv.Length;
                            //len = Convertion.HexToDecimal(lenLength) * 2; 
                            startIndexToRead = i + ii + lenLength.Length;
                            i = startIndexToRead - 1;
                            cdolTagList.Add(new TNLV
                            {
                                Length = lenLength,
                                Name = _tnlv.Name,
                                Tags = _tnlv.Tags,
                            });

                            break;
                        }

                        if (ii == 4)
                        {
                            throw new ArgumentException("Invalid TLV Data");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            foreach (var tnlv in cdolTagList)
            {
                Console.WriteLine(tnlv.Tags + " " + tnlv.Length + " " + tnlv.Name);
            }

            return cdolTagList;
        }
    }


}