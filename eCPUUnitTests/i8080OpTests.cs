using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using eCPU.Intel8080;

namespace eCPUUnitTests
{
    [TestClass]
    public class i8080OpTests
    {
        private UInt16 _initProgramCounter = 0x0000;

        [TestMethod]
        public void TestLXIandSTAX()
        {
            i8080 cpu = InitLoadAndExecute(
                new byte[] 
                            { 0x01, 0x78, 0xab,     // LXI B
                              0x11, 0x2a, 0xf0,     // LXI D
                              0x21, 0xaa, 0x44,     // LXI HL
                              0x31, 0x10, 0x3d } );

            Assert.AreEqual(cpu.Registers.RegBC, 0xab78, "BC failed");
            Assert.AreEqual(cpu.Registers.RegDE, 0xf02a, "DE failed");
            Assert.AreEqual(cpu.Registers.RegHL, 0x44aa, "HL failed");
            Assert.AreEqual(cpu.Registers.StackPointer, 0x3d10, "SP failed");

            LoadAndExecute(new byte[] 
                                { 0x3e, 0x25,     // MVI A
                                  0x02,
                                  0x12 }, cpu);

            Assert.AreEqual(cpu.Memory[0xab78], 0x25, "BC store failed");
            Assert.AreEqual(cpu.Memory[0xf02a], 0x25, "DE store failed");
        }

        [TestMethod]
        public void TestADI()
        {

            i8080 cpu = new i8080();
            cpu.Registers.RegA = 0x14;

            LoadAndExecute(new byte[] { 0xc6, 0x42 }, cpu);
            Assert.AreEqual(cpu.Registers.RegA, 0x56, "op failed");
            Assert.IsFalse(cpu.Registers.CondReg.CarryFlag, "carry failed");
            Assert.IsFalse(cpu.Registers.CondReg.AuxCarryFlag, "aux carry failed");
            Assert.IsFalse(cpu.Registers.CondReg.ZeroFlag, "zero failed");
            Assert.IsTrue(cpu.Registers.CondReg.ParityFlag, "parity failed");
            Assert.IsFalse(cpu.Registers.CondReg.SignFlag, "sign failed");

            LoadAndExecute(new byte[] { 0xc6, 0xbe }, cpu);
            Assert.AreEqual(cpu.Registers.RegA, 0x14, "ADI 2 failed");
            Assert.IsTrue(cpu.Registers.CondReg.CarryFlag, "carry 2 failed");
            Assert.IsTrue(cpu.Registers.CondReg.AuxCarryFlag, "aux carry 2 failed");
            Assert.IsFalse(cpu.Registers.CondReg.ZeroFlag, "zero 2 failed");
            Assert.IsTrue(cpu.Registers.CondReg.ParityFlag, "parity 2 failed");
            Assert.IsFalse(cpu.Registers.CondReg.SignFlag, "sign 2 failed");
        }

        [TestMethod]
        public void TestACI()
        {

            i8080 cpu = new i8080();
            cpu.Registers.RegA = 0x56;
            cpu.Registers.CondReg.CarryFlag = false;

            LoadAndExecute(new byte[] { 0xce, 0xbe }, cpu);
            Assert.AreEqual(cpu.Registers.RegA, 0x14, "op failed");
            Assert.AreEqual(cpu.Registers.RegA, 0x14, "ACI 1 failed");
            Assert.IsTrue(cpu.Registers.CondReg.CarryFlag, "carry 1 failed");
            Assert.IsTrue(cpu.Registers.CondReg.AuxCarryFlag, "aux carry 1 failed");
            Assert.IsFalse(cpu.Registers.CondReg.ZeroFlag, "zero 1 failed");
            Assert.IsTrue(cpu.Registers.CondReg.ParityFlag, "parity 1 failed");
            Assert.IsFalse(cpu.Registers.CondReg.SignFlag, "sign 1 failed");

            LoadAndExecute(new byte[] { 0xce, 0x42 }, cpu);
            Assert.AreEqual(cpu.Registers.RegA, 0x57, "ACI 2 failed");
            Assert.IsFalse(cpu.Registers.CondReg.CarryFlag, "carry 2 failed");
            Assert.IsFalse(cpu.Registers.CondReg.AuxCarryFlag, "aux carry 2 failed");
            Assert.IsFalse(cpu.Registers.CondReg.ZeroFlag, "zero 2 failed");
            Assert.IsFalse(cpu.Registers.CondReg.ParityFlag, "parity 2 failed");
            Assert.IsFalse(cpu.Registers.CondReg.SignFlag, "sign 2 failed");
        }

        [TestMethod]
        public void TestADD()
        {
            i8080 cpu = new i8080();
            cpu.Registers.RegD = 0x2e;
            cpu.Registers.RegA = 0x6c;

            LoadAndExecute(new byte[] { 0x82 }, cpu);
            Assert.AreEqual(cpu.Registers.RegA, 0x9a, "op failed");
            Assert.IsFalse(cpu.Registers.CondReg.CarryFlag, "carry failed");
            Assert.IsTrue(cpu.Registers.CondReg.AuxCarryFlag, "aux carry failed");
            Assert.IsFalse(cpu.Registers.CondReg.ZeroFlag, "zero failed");
            Assert.IsTrue(cpu.Registers.CondReg.ParityFlag, "parity failed");
            Assert.IsTrue(cpu.Registers.CondReg.SignFlag, "sign failed");
        }

        [TestMethod]
        public void TestADC_NoCarry()
        {
            i8080 cpu = new i8080();
            cpu.Registers.RegC = 0x3d;
            cpu.Registers.RegA = 0x42;
            cpu.Registers.CondReg.CarryFlag = false;

            LoadAndExecute(new byte[] { 0x89 }, cpu);
            Assert.AreEqual(cpu.Registers.RegA, 0x7f, "op failed");
            Assert.IsFalse(cpu.Registers.CondReg.CarryFlag, "carry failed");
            Assert.IsFalse(cpu.Registers.CondReg.ZeroFlag, "zero failed");
            Assert.IsFalse(cpu.Registers.CondReg.ParityFlag, "parity failed");
            Assert.IsFalse(cpu.Registers.CondReg.SignFlag, "sign failed");
            Assert.IsFalse(cpu.Registers.CondReg.AuxCarryFlag, "aux carry failed");
        }

        [TestMethod]
        public void TestADC_WithCarry()
        {
            i8080 cpu = new i8080();
            cpu.Registers.RegC = 0x3d;
            cpu.Registers.RegA = 0x42;
            cpu.Registers.CondReg.CarryFlag = true;

            LoadAndExecute(new byte[] { 0x89 }, cpu);
            Assert.AreEqual(cpu.Registers.RegA, 0x80, "op failed");
            Assert.IsFalse(cpu.Registers.CondReg.CarryFlag, "carry failed");
            Assert.IsFalse(cpu.Registers.CondReg.ZeroFlag, "zero failed");
            Assert.IsFalse(cpu.Registers.CondReg.ParityFlag, "parity failed");
            Assert.IsTrue(cpu.Registers.CondReg.SignFlag, "sign failed");
            Assert.IsTrue(cpu.Registers.CondReg.AuxCarryFlag, "aux carry failed");
        }
        
        [TestMethod]
        public void TestSUB()
        {
            i8080 cpu = new i8080();
            cpu.Registers.RegA = 0x3e;

            LoadAndExecute(new byte[] { 0x97 }, cpu);
            Assert.AreEqual(cpu.Registers.RegA, 0x00, "op failed");
            Assert.IsFalse(cpu.Registers.CondReg.CarryFlag, "carry failed");
            Assert.IsTrue(cpu.Registers.CondReg.AuxCarryFlag, "aux carry failed");
            Assert.IsTrue(cpu.Registers.CondReg.ZeroFlag, "zero failed");
            Assert.IsTrue(cpu.Registers.CondReg.ParityFlag, "parity failed");
            Assert.IsFalse(cpu.Registers.CondReg.SignFlag, "sign failed");
        }

        [TestMethod]
        public void TestSUI()
        {
            i8080 cpu = new i8080();
            cpu.Registers.RegA = 0x00;

            LoadAndExecute(new byte[] { 0xd6, 0x01 }, cpu);
            Assert.AreEqual(cpu.Registers.RegA, 0xff, "op failed");
            Assert.IsTrue(cpu.Registers.CondReg.CarryFlag, "carry failed");
            Assert.IsFalse(cpu.Registers.CondReg.AuxCarryFlag, "aux carry failed");
            Assert.IsFalse(cpu.Registers.CondReg.ZeroFlag, "zero failed");
            Assert.IsTrue(cpu.Registers.CondReg.ParityFlag, "parity failed");
            Assert.IsTrue(cpu.Registers.CondReg.SignFlag, "sign failed");
        }

        [TestMethod]
        public void TestSBI_NoCarry()
        {
            i8080 cpu = new i8080();
            cpu.Registers.RegA = 0x00;
            cpu.Registers.CondReg.CarryFlag = false;

            LoadAndExecute(new byte[] { 0xde, 0x01 }, cpu);
            Assert.AreEqual(cpu.Registers.RegA, 0xff, "op failed");
            Assert.IsTrue(cpu.Registers.CondReg.CarryFlag, "carry failed");
            Assert.IsFalse(cpu.Registers.CondReg.AuxCarryFlag, "aux carry failed");
            Assert.IsFalse(cpu.Registers.CondReg.ZeroFlag, "zero failed");
            Assert.IsTrue(cpu.Registers.CondReg.ParityFlag, "parity failed");
            Assert.IsTrue(cpu.Registers.CondReg.SignFlag, "sign failed");
        }

        [TestMethod]
        public void TestSBI1_WithCarry()
        {
            i8080 cpu = new i8080();
            cpu.Registers.RegA = 0x00;
            cpu.Registers.CondReg.CarryFlag = true;

            LoadAndExecute(new byte[] { 0xde, 0x01 }, cpu);
            Assert.AreEqual(cpu.Registers.RegA, 0xfe, "op failed");
            Assert.IsTrue(cpu.Registers.CondReg.CarryFlag, "carry failed");
            Assert.IsFalse(cpu.Registers.CondReg.AuxCarryFlag, "aux carry failed");
            Assert.IsFalse(cpu.Registers.CondReg.ZeroFlag, "zero failed");
            Assert.IsFalse(cpu.Registers.CondReg.ParityFlag, "parity failed");
            Assert.IsTrue(cpu.Registers.CondReg.SignFlag, "sign failed");
        }

        [TestMethod]
        public void TestSBB()
        {
            i8080 cpu = new i8080();
            cpu.Registers.RegA = 0x04;
            cpu.Registers.RegL = 0x02;
            cpu.Registers.CondReg.CarryFlag = true;

            LoadAndExecute(new byte[] { 0x9d }, cpu);
            Assert.AreEqual(cpu.Registers.RegA, 0x01, "op failed");
            Assert.IsFalse(cpu.Registers.CondReg.CarryFlag, "carry failed");
            Assert.IsTrue(cpu.Registers.CondReg.AuxCarryFlag, "aux carry failed");
            Assert.IsFalse(cpu.Registers.CondReg.ZeroFlag, "zero failed");
            Assert.IsFalse(cpu.Registers.CondReg.ParityFlag, "parity failed");
            Assert.IsFalse(cpu.Registers.CondReg.SignFlag, "sign failed");
        }

        [TestMethod]
        public void TestANI()
        {
            i8080 cpu = new i8080();
            cpu.Registers.RegA = 0x3a;

            LoadAndExecute(new byte[] { 0xe6, 0x0f }, cpu);
            Assert.AreEqual(cpu.Registers.RegA, 0x0a, "op failed");
            Assert.IsFalse(cpu.Registers.CondReg.CarryFlag, "carry failed");
            Assert.IsFalse(cpu.Registers.CondReg.AuxCarryFlag, "aux carry failed");
            Assert.IsFalse(cpu.Registers.CondReg.ZeroFlag, "zero failed");
            Assert.IsTrue(cpu.Registers.CondReg.ParityFlag, "parity failed");
            Assert.IsFalse(cpu.Registers.CondReg.SignFlag, "sign failed");
        }

        [TestMethod]
        public void TestXRI()
        {
            i8080 cpu = new i8080();
            cpu.Registers.RegA = 0x3b;

            LoadAndExecute(new byte[] { 0xee, 0x81 }, cpu);
            Assert.AreEqual(cpu.Registers.RegA, 0xba, "op failed");
            Assert.IsFalse(cpu.Registers.CondReg.CarryFlag, "carry failed");
            Assert.IsFalse(cpu.Registers.CondReg.AuxCarryFlag, "aux carry failed");
            Assert.IsFalse(cpu.Registers.CondReg.ZeroFlag, "zero failed");
            Assert.IsFalse(cpu.Registers.CondReg.ParityFlag, "parity failed");
            Assert.IsTrue(cpu.Registers.CondReg.SignFlag, "sign failed");
        }

        [TestMethod]
        public void TestORI()
        {
            i8080 cpu = new i8080();
            cpu.Registers.RegA = 0xb5;

            LoadAndExecute(new byte[] { 0xf6, 0x0f }, cpu);
            Assert.AreEqual(cpu.Registers.RegA, 0xbf, "op failed");
            Assert.IsFalse(cpu.Registers.CondReg.CarryFlag, "carry failed");
            Assert.IsFalse(cpu.Registers.CondReg.AuxCarryFlag, "aux carry failed");
            Assert.IsFalse(cpu.Registers.CondReg.ZeroFlag, "zero failed");
            Assert.IsFalse(cpu.Registers.CondReg.ParityFlag, "parity failed");
            Assert.IsTrue(cpu.Registers.CondReg.SignFlag, "sign failed");
        }

        [TestMethod]
        public void TestCPI()
        {
            i8080 cpu = new i8080();
            cpu.Registers.RegA = 0x4a;

            LoadAndExecute(new byte[] { 0xfe, 0x40 }, cpu);
            Assert.AreEqual(cpu.Registers.RegA, 0x4a, "op failed");
            Assert.IsFalse(cpu.Registers.CondReg.CarryFlag, "carry failed");
            Assert.IsTrue(cpu.Registers.CondReg.AuxCarryFlag, "aux carry failed");
            Assert.IsFalse(cpu.Registers.CondReg.ZeroFlag, "zero failed");
            Assert.IsTrue(cpu.Registers.CondReg.ParityFlag, "parity failed");
            Assert.IsFalse(cpu.Registers.CondReg.SignFlag, "sign failed");
        }

        [TestMethod]
        public void TestDAA()
        {
            i8080 cpu = new i8080();
            cpu.Registers.RegA = 0x9b;
            cpu.Registers.CondReg.CarryFlag = false;
            cpu.Registers.CondReg.AuxCarryFlag = false;

            LoadAndExecute(new byte[] { 0x27 }, cpu);
            Assert.AreEqual(cpu.Registers.RegA, 0x01, "op failed");
            Assert.IsTrue(cpu.Registers.CondReg.CarryFlag, "carry failed");
            Assert.IsTrue(cpu.Registers.CondReg.AuxCarryFlag, "aux carry failed");
            Assert.IsFalse(cpu.Registers.CondReg.ZeroFlag, "zero failed");
            Assert.IsFalse(cpu.Registers.CondReg.ParityFlag, "parity failed");
            Assert.IsFalse(cpu.Registers.CondReg.SignFlag, "sign failed");
        }

        [TestMethod]
        public void TestANA()
        {
            i8080 cpu = new i8080();
            cpu.Registers.RegA = 0xfc;
            cpu.Registers.RegC = 0x0f;

            LoadAndExecute(new byte[] { 0xa1 }, cpu);
            Assert.AreEqual(cpu.Registers.RegA, 0x0c, "op failed");
            Assert.IsFalse(cpu.Registers.CondReg.CarryFlag, "carry failed");
            Assert.IsFalse(cpu.Registers.CondReg.ZeroFlag, "zero failed");
            Assert.IsTrue(cpu.Registers.CondReg.ParityFlag, "parity failed");
            Assert.IsFalse(cpu.Registers.CondReg.SignFlag, "sign failed");
        }

        [TestMethod]
        public void TestXRA()
        {
            i8080 cpu = new i8080();
            cpu.Registers.RegA = 0xff;

            LoadAndExecute(new byte[] { 0xaf }, cpu);
            Assert.AreEqual(cpu.Registers.RegA, 0x00, "op failed");
            Assert.IsFalse(cpu.Registers.CondReg.CarryFlag, "carry failed");
            Assert.IsFalse(cpu.Registers.CondReg.AuxCarryFlag, "aux carry failed");
            Assert.IsTrue(cpu.Registers.CondReg.ZeroFlag, "zero failed");
            Assert.IsTrue(cpu.Registers.CondReg.ParityFlag, "parity failed");
            Assert.IsFalse(cpu.Registers.CondReg.SignFlag, "sign failed");

            cpu.Registers.RegA = 0x5c;
            cpu.Registers.RegE = 0x78;
            LoadAndExecute(new byte[] { 0xab }, cpu);

            Assert.AreEqual(cpu.Registers.RegA, 0x24, "op failed");
            Assert.IsFalse(cpu.Registers.CondReg.CarryFlag, "carry failed");
            Assert.IsFalse(cpu.Registers.CondReg.ZeroFlag, "zero failed");
            Assert.IsTrue(cpu.Registers.CondReg.ParityFlag, "parity failed");
            Assert.IsFalse(cpu.Registers.CondReg.SignFlag, "sign failed");
        }

        [TestMethod]
        public void TestORA()
        {
            i8080 cpu = new i8080();
            cpu.Registers.RegA = 0x33;
            cpu.Registers.RegC = 0x0f;

            LoadAndExecute(new byte[] { 0xb1 }, cpu);
            Assert.AreEqual(cpu.Registers.RegA, 0x3f, "op failed");
            Assert.IsFalse(cpu.Registers.CondReg.CarryFlag, "carry failed");
            Assert.IsFalse(cpu.Registers.CondReg.AuxCarryFlag, "aux carry failed");
            Assert.IsFalse(cpu.Registers.CondReg.ZeroFlag, "zero failed");
            Assert.IsTrue(cpu.Registers.CondReg.ParityFlag, "parity failed");
            Assert.IsFalse(cpu.Registers.CondReg.SignFlag, "sign failed");
        }

        [TestMethod]
        public void TestCMP()
        {
            i8080 cpu = new i8080();
            cpu.Registers.RegA = 0x0a;
            cpu.Registers.RegE = 0x05;

            LoadAndExecute(new byte[] { 0xbb }, cpu);
            Assert.IsFalse(cpu.Registers.CondReg.CarryFlag, "carry failed");
            Assert.IsFalse(cpu.Registers.CondReg.ZeroFlag, "zero failed");
            Assert.IsTrue(cpu.Registers.CondReg.ParityFlag, "parity failed");
            Assert.IsFalse(cpu.Registers.CondReg.SignFlag, "sign failed");

            cpu.Registers.RegA = 0x02;
            cpu.Registers.RegE = 0x05;

            LoadAndExecute(new byte[] { 0xbb }, cpu);
            Assert.IsTrue(cpu.Registers.CondReg.CarryFlag, "carry failed");
            Assert.IsFalse(cpu.Registers.CondReg.ZeroFlag, "zero failed");
            Assert.IsFalse(cpu.Registers.CondReg.ParityFlag, "parity failed");
            Assert.IsTrue(cpu.Registers.CondReg.SignFlag, "sign failed");
        }

        [TestMethod]
        public void TestRLC()
        {
            i8080 cpu = new i8080();
            cpu.Registers.RegA = 0xf2;
            LoadAndExecute(new byte[] { 0x07 }, cpu);
            Assert.AreEqual(cpu.Registers.RegA, 0xe5, "op failed");
            Assert.IsTrue(cpu.Registers.CondReg.CarryFlag, "carry failed");
        }

        [TestMethod]
        public void TestRRC()
        {
            i8080 cpu = new i8080();
            cpu.Registers.RegA = 0xf2;
            LoadAndExecute(new byte[] { 0x0f }, cpu);
            Assert.AreEqual(cpu.Registers.RegA, 0x79, "op failed");
            Assert.IsFalse(cpu.Registers.CondReg.CarryFlag, "carry failed");
        }

        [TestMethod]
        public void TestRAL()
        {
            i8080 cpu = new i8080();
            cpu.Registers.RegA = 0xb5;
            LoadAndExecute(new byte[] { 0x17 }, cpu);
            Assert.AreEqual(cpu.Registers.RegA, 0x6a, "op failed");
            Assert.IsTrue(cpu.Registers.CondReg.CarryFlag, "carry failed");
        }

        [TestMethod]
        public void TestRAR()
        {
            i8080 cpu = new i8080();
            cpu.Registers.RegA = 0x6a;
            cpu.Registers.CondReg.CarryFlag = true;
            LoadAndExecute(new byte[] { 0x1f }, cpu);
            Assert.AreEqual(cpu.Registers.RegA, 0xb5, "op failed");
            Assert.IsFalse(cpu.Registers.CondReg.CarryFlag, "carry failed");
        }

        [TestMethod]
        public void TestDAD()
        {
            i8080 cpu = new i8080();
            cpu.Registers.RegB = 0x33;
            cpu.Registers.RegC = 0x9f;
            cpu.Registers.RegH = 0xa1;
            cpu.Registers.RegL = 0x7b;
            LoadAndExecute(new byte[] { 0x09 }, cpu);
            Assert.AreEqual(cpu.Registers.RegHL, 0xd51a, "op failed");
            Assert.IsFalse(cpu.Registers.CondReg.CarryFlag, "carry failed");
        }

        [TestMethod]
        public void TestXCHG()
        {
            i8080 cpu = new i8080();
            cpu.Registers.RegD = 0x33;
            cpu.Registers.RegE = 0x55;
            cpu.Registers.RegH = 0x00;
            cpu.Registers.RegL = 0xff;
            LoadAndExecute(new byte[] { 0xeb }, cpu);
            Assert.AreEqual(cpu.Registers.RegDE, 0x00ff, "op1 failed");
            Assert.AreEqual(cpu.Registers.RegHL, 0x3355, "op2 failed");
        }

        [TestMethod]
        public void TestXTHL()
        {
            i8080 cpu = new i8080();
            cpu.Registers.StackPointer = 0x10ad;
            cpu.Registers.RegHL = 0x0b3c;
            cpu.Memory[cpu.Registers.StackPointer] = 0xf0;
            cpu.Memory[cpu.Registers.StackPointer+1] = 0x0d;
            LoadAndExecute(new byte[] { 0xe3 }, cpu);
            Assert.AreEqual(cpu.Registers.StackPointer, 0x10ad, "op1 failed");
            Assert.AreEqual(cpu.Memory[cpu.Registers.StackPointer], 0x3c, "op1 failed");
            Assert.AreEqual(cpu.Memory[cpu.Registers.StackPointer+1], 0x0b, "op1 failed");
            Assert.AreEqual(cpu.Registers.RegHL, 0x0df0, "op2 failed");
        }

        [TestMethod]
        public void TestShortProgram()
        {
            i8080 cpu = InitLoadAndExecute(
                new byte[] {
                0x3e,0x02,
                0x4f,
                0xc6,0x04,
                0x47,
                0x11,0x41,0x01,
                0x21,0x69,0x00, 
                0x19,
                0x76 });

            Assert.AreEqual(cpu.Registers.RegC, 0x02, "C failed");
            Assert.AreEqual(cpu.Registers.RegB, 0x06, "B failed");
            Assert.AreEqual(cpu.Registers.RegD, 0x01, "D failed");
            Assert.AreEqual(cpu.Registers.RegE, 0x41, "E failed");
            Assert.AreEqual(cpu.Registers.RegH, 0x01, "H failed");
            Assert.AreEqual(cpu.Registers.RegL, 0xaa, "L failed");
        }

        [TestMethod]
        public void TestPushAndPop()
        {
            i8080 cpu = new i8080();
            cpu.Registers.ProgramCounter.Value = _initProgramCounter;

            // load registers
            byte[] program = new byte[]
            {
                0x31, 0x00, 0x20,       // LXI SP, $2000
                0x3e, 0x01,             // MVI A, $01
                0x06, 0x02,             // MVI B, $01
                0x0e, 0x04,             // MVI C, $01
                0x16, 0x08,             // MVI D, $01
                0x1e, 0x11,             // MVI E, $01
                0x26, 0x21,             // MVI H, $01
                0x2e, 0x41,             // MVI L, $01
                0x37,                   // STC (set carry)
            };

            LoadAndExecute(program, cpu);

            // test loading
            Assert.AreEqual(cpu.Registers.RegA, 0x01, "A load failed");
            Assert.AreEqual(cpu.Registers.RegB, 0x02, "B load failed");
            Assert.AreEqual(cpu.Registers.RegC, 0x04, "C load failed");
            Assert.AreEqual(cpu.Registers.RegD, 0x08, "D load failed");
            Assert.AreEqual(cpu.Registers.RegE, 0x11, "E load failed");
            Assert.AreEqual(cpu.Registers.RegH, 0x21, "H load failed");
            Assert.AreEqual(cpu.Registers.RegL, 0x41, "L load failed");
            Assert.IsTrue(cpu.Registers.CondReg.CarryFlag, "carry flag set failed");

            // push registers and clear them
            program = new byte[]
            {
                0xc5,               // PUSH BC
                0xd5,               // PUSH DE
                0xe5,               // PUSH HL
                0xf5,               // PUSH PSW
                0x3e, 0x00,         // MVI A, $00
                0x06, 0x00,         // MVI B, $00
                0x0e, 0x00,         // MVI C, $00
                0x16, 0x00,         // MVI D, $00
                0x1e, 0x00,         // MVI E, $00
                0x26, 0x00,         // MVI H, $00
                0x2e, 0x00,         // MVI L, $00
                0x3f,               // CMC (complement carry)
           };

            LoadAndExecute(program, cpu);

            // test push and clear
            Assert.AreEqual(cpu.Registers.RegA, 0x00, "A push failed");
            Assert.AreEqual(cpu.Registers.RegB, 0x00, "B push failed");
            Assert.AreEqual(cpu.Registers.RegC, 0x00, "C push failed");
            Assert.AreEqual(cpu.Registers.RegD, 0x00, "D push failed");
            Assert.AreEqual(cpu.Registers.RegE, 0x00, "E push failed");
            Assert.AreEqual(cpu.Registers.RegH, 0x00, "H push failed");
            Assert.AreEqual(cpu.Registers.RegL, 0x00, "L push failed");
            Assert.IsFalse(cpu.Registers.CondReg.CarryFlag, "carry flag complement failed");

            // pop registers
            program = new byte[]
            {
                0xf1,               // POP PSW
                0xe1,               // POP HL
                0xd1,               // POP DE
                0xc1,               // POP BC
            };

            //LoadAndExecute(program, cpu);
            LoadInstructions(program, cpu);
            cpu.ExecuteNext();
            cpu.ExecuteNext();
            cpu.ExecuteNext();
            cpu.ExecuteNext();

            // test pop
            Assert.AreEqual(cpu.Registers.RegA, 0x01, "A pop failed");
            Assert.AreEqual(cpu.Registers.RegB, 0x02, "B pop failed");
            Assert.AreEqual(cpu.Registers.RegC, 0x04, "C pop failed");
            Assert.AreEqual(cpu.Registers.RegD, 0x08, "D pop failed");
            Assert.AreEqual(cpu.Registers.RegE, 0x11, "E pop failed");
            Assert.AreEqual(cpu.Registers.RegH, 0x21, "H pop failed");
            Assert.AreEqual(cpu.Registers.RegL, 0x41, "L pop failed");
            Assert.IsTrue(cpu.Registers.CondReg.CarryFlag, "carry flag pop failed");

        }

        [TestMethod]
        public void TestZeroFlag()
        {
            i8080 cpu = new i8080();
            cpu.Registers.RegB = 0x01;
            cpu.Registers.ProgramCounter.Value = 0x00;
            cpu.Memory[0x00] = 0x05;
            cpu.ExecuteNext();

            Assert.IsTrue(cpu.Registers.CondReg.ZeroFlag, "failed");
        }

        [TestMethod]
        public void TestINX()
        {
            i8080 cpu = new i8080();
            cpu.Registers.RegDE = 0x05;
            cpu.Registers.ProgramCounter.Value = 0x00;
            cpu.Memory[0x00] = 0x13;
            cpu.Memory[0x01] = 0x13;
            cpu.Memory[0x02] = 0x13;
            cpu.Memory[0x03] = 0x13;
            cpu.Memory[0x04] = 0x13;
            cpu.ExecuteNext();
            cpu.ExecuteNext();
            cpu.ExecuteNext();
            cpu.ExecuteNext();
            cpu.ExecuteNext();
            Assert.AreEqual(cpu.Registers.RegDE, 0x0a);


            cpu.Registers.RegDE = 0xfe;
            cpu.Memory[0x05] = 0x13;
            cpu.Memory[0x06] = 0x13;
            cpu.Memory[0x07] = 0x13;
            cpu.Memory[0x08] = 0x13;
            cpu.Memory[0x09] = 0x13;
            cpu.ExecuteNext();
            cpu.ExecuteNext();
            cpu.ExecuteNext();
            cpu.ExecuteNext();
            cpu.ExecuteNext();
            Assert.AreEqual(cpu.Registers.RegDE, 0x103);
        }

        [TestMethod]
        public void TestInterrupt()
        {
            i8080 cpu = new i8080();
            cpu.Registers.ProgramCounter.Value = 0x0123;
            cpu.Registers.StackPointer = 0x2000;

            LoadAndExecute(new byte[] { 0xfb }, cpu);  // enable interrupts

            cpu.AddInterrupt(0xcf);  // RST 1
            cpu.ExecuteNext();      // should execute the interrupt

            Assert.AreEqual(cpu.Registers.ProgramCounter.Value, 0x08, "program counter is wrong");
            Assert.AreEqual(cpu.Registers.StackPointer, 0x1ffe, "stack is wrong");
        }

        [TestMethod]
        public void ShowUnimplementedInstructions()
        {
            i8080 cpu = new i8080();
            cpu.ReportNotImplemented();
        }

        private i8080 InitAndLoad( byte[] program )
        {
            i8080 cpu = new i8080();
            cpu.Registers.ProgramCounter.Value = _initProgramCounter;
            LoadInstructions(program, cpu);
            return cpu;
        }

        private i8080 InitLoadAndExecute(byte[] program)
        {
            i8080 cpu = new i8080();
            cpu.Registers.ProgramCounter.Value = _initProgramCounter;
            LoadAndExecute(program, cpu);
            return cpu;
        }
        
        private void LoadInstructions(byte[] reg, i8080 cpu)
        {
            for( int i=0; i < reg.Length; i++ )
            {
                cpu.Memory[cpu.Registers.ProgramCounter.Value + i] = reg[i];
            }
        }

        private void LoadAndExecute(byte[] reg, i8080 cpu)
        {
            LoadInstructions(reg, cpu);
            for (int i = 0; i < reg.Length; i++)
            {
                cpu.ExecuteNext();
            }
        }


    }
}
