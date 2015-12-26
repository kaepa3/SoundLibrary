using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoundLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;

namespace SoundLibrary.Tests
{
    [TestClass()]
    public class SoundUtillTests
    {

        [TestMethod()]
        public void PlayTest()
        {
            var obj = new SoundUtill("aho");
            //  正常
            Assert.AreEqual(true, obj.Play("test.wav"));

            //  異常
            Assert.AreEqual(false, obj.Play("tedsaf.wav"));
        }

        [TestMethod()]
        public void StopTest()
        {
            var obj = new SoundUtill();
            //  正常
            Assert.AreEqual(true, obj.Play("test.wav"));
            Assert.AreEqual(true, obj.PlayStop());
            Assert.AreEqual(true, obj.Play("test.wav"));
            Assert.AreEqual(true, obj.PlayStop());
            //  ない場合。
            Assert.AreEqual(false, obj.PlayStop());
        }
        [TestMethod()]
        public void RecTest()
        {
            string str = "./result.wav";
            var obj = new SoundUtill("faaaa");
            //  正常
            Assert.AreEqual(true, obj.Rec());
            System.Threading.Thread.Sleep(4000);
            Assert.AreEqual(true, obj.RecStop(str));
            Assert.AreEqual(true, System.IO.File.Exists(str));

            Debug.WriteLine("debug " + System.IO.Directory.GetCurrentDirectory());
        }

    }
}