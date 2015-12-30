using System;

namespace eCPU.Intel8080
{
    public class OpCodeTable
    {
        public UInt16 op { get; set; }
        public string descr { get; set; }
        public UInt16 size { get; set; }

        public OpCodeTable(UInt16 _op, string _descr, UInt16 _size)
        {
            op = _op;
            descr = _descr;
            size = _size;
        }
    }
}
