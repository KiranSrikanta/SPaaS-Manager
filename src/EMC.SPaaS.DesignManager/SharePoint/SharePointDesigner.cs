using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace EMC.SPaaS.DesignManager
{
    public class SharePointDesigner : IDesigner<SharePointDesign>
    {
        SharePointDesign _design;
        public SharePointDesign Design
        {
            get
            {
                return _design;
            }
        }

        public SharePointDesigner()
        {
            _design = new SharePointDesign();
        }

        public SharePointDesigner(SharePointDesign design)
        {
            _design = design;
        }
    }
}
