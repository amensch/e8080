using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCPU.Intel8080
{
    public class i8080Registers
    {
        // The 8080 CPU has (12) 8 bit registers
        public byte RegA { get;set;}
        private i8080ConditionalRegister _condReg = new i8080ConditionalRegister();
        public byte RegB { get;set;}
        public byte RegC { get;set;}
        public byte RegD { get;set;}
        public byte RegE { get;set;}
        public byte RegH { get;set;}
        public byte RegL { get;set;}
        public UInt16 StackPointer { get;set;}
        private i8080ProgramCounter _pc = new i8080ProgramCounter();

        public i8080Registers()
        {
            RegA = 0;
            RegB = 0;
            RegC = 0;
            RegD = 0;
            RegE = 0;
            RegH = 0;
            RegL = 0;
            StackPointer = 0;
            _pc.Value = 0;
        }

        public i8080ProgramCounter ProgramCounter
        {
            get { return _pc; }
        }

        public i8080ConditionalRegister CondReg
        {
            get
            {
                return _condReg;
            }
        }

        public byte CondRegValue
        {
            get
            {
                return _condReg.Register;
            }
            set
            {
                _condReg.Register = value;
            }
        }

        public UInt16 RegPSW
        {
            get
            {
                return GetValue16( RegA, CondReg.Register );
            }
            set
            {
                RegA = GetHiValue( value );
                CondReg.Register = GetLoValue( value );
            }
        }

        public UInt16 RegBC
        {
            get
            {
                return GetValue16( RegB, RegC );
            }
            set
            {
                RegB = GetHiValue( value );
                RegC = GetLoValue( value );
            }
        }

        public UInt16 RegDE
        {
            get
            {
                return GetValue16( RegD, RegE );
            }
            set
            {
                RegD = GetHiValue( value );
                RegE = GetLoValue( value );
            }
        }

        public UInt16 RegHL
        {
            get
            {
                return GetValue16( RegH, RegL );
            }
            set
            {
                RegH = GetHiValue( value );
                RegL = GetLoValue( value );
            }
        }

        public void SetStackPointer( byte hi, byte lo )
        {
            StackPointer = GetValue16( hi, lo );
        }

        public void SetProgramCounter( byte hi, byte lo )
        {
            ProgramCounter.Value = GetValue16( hi, lo );
        }

        public static UInt16 GetValue16( byte hi, byte lo )
        {
            return ( UInt16 ) ( ( ( UInt32 ) hi << 8 | ( UInt32 ) lo ) & 0xffff );
        }

        public static byte GetHiValue( UInt16 value )
        {
            return ( byte ) ( ( value >> 8 ) & 0xff );
        }

        public static byte GetLoValue( UInt16 value )
        {
            return ( byte ) ( value & 0xff );
        }

    }
}
