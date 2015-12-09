using eCPU.Machine8080;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace eCPUUnitTests
{
    [TestClass]
    public class ShiftRegisterTests
    {
        [TestMethod]
        public void TestShiftRegister()
        {
            ShiftOffsetDevice offset = new ShiftOffsetDevice();
            ShiftDevice device = new ShiftDevice(offset);

            device.Write(0xaa);
            Assert.AreEqual(device.Read(), 0xaa, "First write failed");

            offset.Write(0x04);
            Assert.AreEqual(device.Read(), 0xa0, "First shift failed");

            device.Write(0xbb);
            Assert.AreEqual(device.Read(), 0xba, "Second write failed");

            device.Write(0xc4);
            offset.Write(0x00);
            Assert.AreEqual(device.Read(), 0xc4, "Third write failed");
        }
    }
}
