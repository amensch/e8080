using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using eCPU.Intel8080;

namespace eCPUUnitTests
{
    [TestClass]
    public class i8080RegisterTests
    {
        [TestMethod]
        public void PSWRegTest()
        {
            i8080Registers reg = new i8080Registers();

            Assert.AreEqual( reg.RegA, 0, "init A failed" );
            Assert.AreEqual( reg.RegPSW, 0x0002, "init PSW failed" );

            reg.RegPSW = 0xf287;

            Assert.AreEqual( reg.RegA, 0xf2, "set A failed" );
            Assert.AreEqual( reg.CondRegValue, 0x87, "set cond failed" );

            Assert.AreEqual( reg.RegPSW, 0xf287, "set PSW failed" );
            Assert.IsTrue( reg.CondReg.SignFlag );
            Assert.IsFalse( reg.CondReg.ZeroFlag );
            Assert.IsTrue( reg.CondReg.ParityFlag );
            Assert.IsTrue( reg.CondReg.CarryFlag );
        }
        [TestMethod]
        public void BCRegTest()
        {   
            i8080Registers reg = new i8080Registers();

            Assert.AreEqual( reg.RegB, 0, "init B failed" );
            Assert.AreEqual( reg.RegC, 0, "init C failed" );

            reg.RegBC = 0xf287;

            Assert.AreEqual( reg.RegB, 0xf2, "set B failed" );
            Assert.AreEqual( reg.RegC, 0x87, "set C failed" );

            Assert.AreEqual( reg.RegBC, 0xf287, "set BC failed" );
        }
        [TestMethod]
        public void DERegTest()
        {
            i8080Registers reg = new i8080Registers();

            Assert.AreEqual( reg.RegD, 0, "init D failed" );
            Assert.AreEqual( reg.RegE, 0, "init E failed" );

            reg.RegDE = 0xf287;

            Assert.AreEqual( reg.RegD, 0xf2, "set D failed" );
            Assert.AreEqual( reg.RegE, 0x87, "set E failed" );

            Assert.AreEqual( reg.RegDE, 0xf287, "set DE failed" );
        }
        [TestMethod]
        public void HLRegTest()
        {
            i8080Registers reg = new i8080Registers();

            Assert.AreEqual( reg.RegH, 0, "init H failed" );
            Assert.AreEqual( reg.RegL, 0, "init L failed" );

            reg.RegHL = 0xf287;

            Assert.AreEqual( reg.RegH, 0xf2, "set H failed" );
            Assert.AreEqual( reg.RegL, 0x87, "set L failed" );

            Assert.AreEqual( reg.RegHL, 0xf287, "set HL failed" );
        }
        [TestMethod]
        public void SPTest()
        {
            i8080Registers reg = new i8080Registers();

            reg.SetStackPointer( 0xf2, 0x87 );
            Assert.AreEqual( reg.StackPointer, 0xf287 );
        }
        [TestMethod]
        public void PCTest()
        {
            i8080Registers reg = new i8080Registers();

            reg.SetProgramCounter( 0xf2, 0x87 );
            Assert.AreEqual( reg.ProgramCounter.Value, 0xf287 );
        }

    }
}
