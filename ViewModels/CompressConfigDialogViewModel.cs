using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGCompress.ViewModels
{
    public class CompressConfigDialogViewModel
    {

        public enum ImgFormat
        {
            tiff,
            png,
            jp2,
            webp
        }

        private ImgFormat m_Format=ImgFormat.png;
        public ImgFormat MyFormat
        {
            get { return m_Format; }
            set { m_Format = value; }
        }
    }
}
