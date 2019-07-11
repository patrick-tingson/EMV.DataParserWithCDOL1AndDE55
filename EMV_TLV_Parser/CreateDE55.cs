using Convertion_XXX;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EMV_XXX
{
    public class CreateDE55
    {
        IEnumerable<TagsDictionary> tagsDictionary = new EMVTagsDictionary().List();
        DE55Tags de55tags;
        List<TNLV> tnlvListForDE55 = new List<TNLV>();

        public CreateDE55(DE55Tags _de55tags)
        {
            //Check if Null
            if (_de55tags == null)
                throw new ArgumentException("Invalid DE55 Tags");

            this.de55tags = _de55tags;

            //Validate tags value
            if (de55tags._5F2A == null)
                throw new ArgumentException("Invalid DE55 Tags 5F2A");
            if (de55tags._5F34 == null)
                throw new ArgumentException("Invalid DE55 Tags 5F34");
            if (de55tags._82 == null)
                throw new ArgumentException("Invalid DE55 Tags 82");
            if (de55tags._84 == null)
                throw new ArgumentException("Invalid DE55 Tags 84");
            if (de55tags._95 == null)
                throw new ArgumentException("Invalid DE55 Tags 95");
            if (de55tags._9A == null)
                throw new ArgumentException("Invalid DE55 Tags 9A");
            if (de55tags._9C == null)
                throw new ArgumentException("Invalid DE55 Tags 9C");
            if (de55tags._9F02 == null)
                throw new ArgumentException("Invalid DE55 Tags 9F02");
            if (de55tags._9F03 == null)
                throw new ArgumentException("Invalid DE55 Tags 9F03");
            if (de55tags._9F09 == null)
                throw new ArgumentException("Invalid DE55 Tags 9F09");
            if (de55tags._9F10 == null)
                throw new ArgumentException("Invalid DE55 Tags 9F10");
            if (de55tags._9F1A == null)
                throw new ArgumentException("Invalid DE55 Tags 9F1A");
            //if (de55tags._9F1E == null)
            //    throw new ArgumentException("Invalid DE55 Tags 9F1E");
            if (de55tags._9F26 == null)
                throw new ArgumentException("Invalid DE55 Tags 9F26");
            if (de55tags._9F27 == null)
                throw new ArgumentException("Invalid DE55 Tags 9F27");
            if (de55tags._9F33 == null)
                throw new ArgumentException("Invalid DE55 Tags 9F33");
            if (de55tags._9F34 == null)
                throw new ArgumentException("Invalid DE55 Tags 9F34");
            if (de55tags._9F35 == null)
                throw new ArgumentException("Invalid DE55 Tags 9F35");
            if (de55tags._9F36 == null)
                throw new ArgumentException("Invalid DE55 Tags 9F36");
            if (de55tags._9F37 == null)
                throw new ArgumentException("Invalid DE55 Tags 9F37");
            if (de55tags._9F41 == null)
                throw new ArgumentException("Invalid DE55 Tags 9F41");
        }

        public IEnumerable<TNLV> Create(ref string messageFormat)
        {
            AddTNLV(Tags.TransactionCurrencyCode, de55tags._5F2A);
            AddTNLV(Tags.ApplicationInterchangeProfile, de55tags._82);
            AddTNLV(Tags.DedicatedFileName, de55tags._84);
            AddTNLV(Tags.TerminalVerificationResults, de55tags._95);
            AddTNLV(Tags.TransactionDate, de55tags._9A);
            AddTNLV(Tags.TransactionType, de55tags._9C);
            AddTNLV(Tags.AmountAuthorised, de55tags._9F02);
            AddTNLV(Tags.ApplicationVersionNumber2, de55tags._9F09);
            AddTNLV(Tags.IssuerApplicationData, de55tags._9F10);
            AddTNLV(Tags.TerminalCountryCode, de55tags._9F1A);
            //AddTNLV(Tags.IFDSerialNumber, de55tags._9F1E);
            AddTNLV(Tags.ApplicationCryptogram, de55tags._9F26);
            AddTNLV(Tags.CryptogramInformationData, de55tags._9F27);
            AddTNLV(Tags.TerminalCapabilities, de55tags._9F33);
            AddTNLV(Tags.CardholderVerificationMethodResults, de55tags._9F34);
            AddTNLV(Tags.TerminalType, de55tags._9F35);
            AddTNLV(Tags.ApplicationTransactionCounter, de55tags._9F36);
            AddTNLV(Tags.UnpredictableNumber, de55tags._9F37);
            AddTNLV(Tags.TransactionSequenceCounter, de55tags._9F41);
            AddTNLV(Tags.PrimaryAccountSequenceNumber, de55tags._5F34);
            AddTNLV(Tags.AmountOther, de55tags._9F03);

            foreach (var tnlv in tnlvListForDE55)
            {
                messageFormat = string.Format("{0}{1}{2}{3}",
                    messageFormat,
                    tnlv.Tags,
                    tnlv.Length,
                    tnlv.Value
                    );
            }

            return tnlvListForDE55;
        }

        void AddTNLV(string tags, string value)
        {
            try
            {
                var _tnlv = tagsDictionary.FirstOrDefault(w => w.Tags == tags);
                tnlvListForDE55.Add(new TNLV
                {
                    Length = Convertion.DecimalToHex(value.Length / 2),
                    Name = _tnlv.Name,
                    Tags = _tnlv.Tags,
                    Value = value
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }


}