using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using eCPU.Intel8080;

namespace eCPUUnitTests
{
    [TestClass]
    public class i8080ConditionalRegisterTests
    {
        [TestMethod]
        public void TestRegisterInit()
        {
            i8080ConditionalRegister reg = new i8080ConditionalRegister();
            Assert.AreEqual( reg.Register, 0x02 );
        }

        [TestMethod]
        public void TestCarryFlag()
        {
            i8080ConditionalRegister reg = new i8080ConditionalRegister();

            // test init
            Assert.IsFalse( reg.CarryFlag );

            reg.CarryFlag = true;
            Assert.IsTrue( reg.CarryFlag );

            reg.CarryFlag = false;
            Assert.IsFalse( reg.CarryFlag );

        }
        [TestMethod]
        public void TestZeroFlag()
        {
            i8080ConditionalRegister reg = new i8080ConditionalRegister();

            // test init
            Assert.IsFalse( reg.ZeroFlag );

            reg.ZeroFlag = true;
            Assert.IsTrue( reg.ZeroFlag );

            reg.ZeroFlag = false;
            Assert.IsFalse( reg.ZeroFlag );

        }
        [TestMethod]
        public void TestParityFlag()
        {
            i8080ConditionalRegister reg = new i8080ConditionalRegister();

            // test init
            Assert.IsFalse( reg.ParityFlag );

            reg.ParityFlag = true;
            Assert.IsTrue( reg.ParityFlag );

            reg.ParityFlag = false;
            Assert.IsFalse( reg.ParityFlag );

        }
        [TestMethod]
        public void TestSignFlag()
        {
            i8080ConditionalRegister reg = new i8080ConditionalRegister();

            // test init
            Assert.IsFalse( reg.SignFlag );

            reg.SignFlag = true;
            Assert.IsTrue( reg.SignFlag );

            reg.SignFlag = false;
            Assert.IsFalse( reg.SignFlag );

        }
        [TestMethod]
        public void TestAuxCarryFlag()
        {
            i8080ConditionalRegister reg = new i8080ConditionalRegister();

            // test init
            Assert.IsFalse( reg.AuxCarryFlag );

            reg.AuxCarryFlag = true;
            Assert.IsTrue( reg.AuxCarryFlag );

            reg.AuxCarryFlag = false;
            Assert.IsFalse( reg.AuxCarryFlag );
        }
        [TestMethod]
        public void TestCalcCarryFlag()
        {
            i8080ConditionalRegister reg = new i8080ConditionalRegister();

            reg.CalcCarryFlag( 0x017e );
            Assert.IsTrue( reg.CarryFlag );

            reg.CalcCarryFlag( 0x0023 );
            Assert.IsFalse( reg.CarryFlag );
        }
        [TestMethod]
        public void TestCalcZeroFlag()
        {
            i8080ConditionalRegister reg = new i8080ConditionalRegister();

            reg.CalcZeroFlag( 0 );
            Assert.IsTrue( reg.ZeroFlag );

            reg.CalcZeroFlag( 0x0023 );
            Assert.IsFalse( reg.ZeroFlag );
        }
        [TestMethod]
        public void TestCalcSignFlag()
        {
            i8080ConditionalRegister reg = new i8080ConditionalRegister();

            reg.CalcSignFlag( 0x018e );
            Assert.IsTrue( reg.SignFlag );

            reg.CalcSignFlag( 0x0067 );
            Assert.IsFalse( reg.SignFlag );
        }
        [TestMethod]
        public void TestCalcParityFlag()
        {
            i8080ConditionalRegister reg = new i8080ConditionalRegister();

            reg.CalcParityFlag( 0x0035 );
            Assert.IsTrue( reg.ParityFlag );

            reg.CalcParityFlag( 0x0037 );
            Assert.IsFalse( reg.ParityFlag );

            reg.CalcParityFlag( 0 );
            Assert.IsTrue( reg.ParityFlag );

            reg.CalcParityFlag( 0xff );
            Assert.IsTrue( reg.ParityFlag );

        }
    }
}
