using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using System.Windows.Forms;

namespace OX.Copyable.Tests
{
    class SolidBrushProvider : InstanceProvider<SolidBrush>
    {
        public override SolidBrush CreateTypedCopy(SolidBrush toBeCopied)
        {
            return new SolidBrush(toBeCopied.Color);
        }
    }

    [TestClass]
    public class CopyWithInstanceProviderTests
    {
        [TestMethod]
        public void CopyForm()
        {
            Form1 form = new Form1();
            Form1 copy = (Form1)form.Copy();
            Assert.AreNotSame(form, copy);
        }

        [TestMethod]
        public void CopyBrushWithInstanceProvider()
        {
            SolidBrush a = new SolidBrush(Color.Red);
            SolidBrush b = (SolidBrush)a.Copy();
            Assert.AreNotSame(a, b);
        }

        [TestMethod]
        public void CopyWithSuppliedInstance()
        {
            SolidBrush a = new SolidBrush(Color.Red);
            a.Color = Color.Black;
            SolidBrush b = new SolidBrush(Color.Red);
            a.Copy(b);
            Assert.AreNotSame(a, b);
            Assert.AreEqual(a.Color, b.Color);

            Size sz = new Size(2, 2);
            Assert.AreEqual(4, sz.GetArea());
        }
    }

    static class SizeExtensions
    {
        public static int GetArea(this Size sz)
        {
            return sz.Width * sz.Height;
        }
    }
}
