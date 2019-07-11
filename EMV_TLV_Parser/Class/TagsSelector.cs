using System;
using System.Collections.Generic;
using System.Linq;

namespace EMV_XXX
{
    internal class TagsSelector
    {
        private IEnumerable<TNLV> tnlv;
        public TagsSelector(IEnumerable<TNLV> _tnlv)
        {
            if (_tnlv == null)
                throw new ArgumentNullException("Invalid TNLV");

            this.tnlv = _tnlv;
        }

        public string Value(string tags)
        {
            var result = tnlv.SingleOrDefault(w => w.Tags == tags);
            return result != null ? result.Value : null;
        }

        public TNLV AllData(string tags)
        {
            var result = tnlv.SingleOrDefault(w => w.Tags == tags);
            return result;
        }
    }


}