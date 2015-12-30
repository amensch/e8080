using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Collections.Concurrent;

using eCPU.CPU;

namespace eCPU.Intel8080
{
    public class i8080
    {

        private delegate int OpCodeAction();
        private OpCodeAction[] _opActions = new OpCodeAction[0x100];

        private byte[] _memory = new byte[0x10000];
        private i8080Registers _regs;
        private ConcurrentQueue<byte> _interrupts;

        // I/O Ports
        private Dictionary<byte, IInputDevice> _inputDevices;
        private Dictionary<byte, IOutputDevice> _outputDevices;

        private bool enable_interrupts = false;

        // for debugging only -- keep track of the previous instruction
        private UInt16 _lastpc = 0x00;

        public i8080() 
        {
            Clear();
            for( int i = 0; i < _opActions.Length; i++ )
            {
                _opActions[i] = () => { return Op_NotImplemented(); } ;
            }
            LoadOpCodeActions();
        }

        public i8080Registers Registers
        {
            get { return _regs; }
        }

        public byte[] Memory
        {
            get { return _memory; }
        }

        public void Clear()
        {
            _regs = new i8080Registers();
            enable_interrupts = false;
            _interrupts = new ConcurrentQueue<byte>();
            ZeroArray(ref _memory);
            _inputDevices = new Dictionary<byte, IInputDevice>();
            _outputDevices = new Dictionary<byte, IOutputDevice>();
        }

        public void AddInterrupt(byte op_code)
        {
            _interrupts.Enqueue(op_code);
        }

        public void AddInputDevice(IInputDevice device, byte port)
        {
            if (_inputDevices.ContainsKey(port))
            {
                _inputDevices.Remove(port);
            }
            _inputDevices.Add(port, device);
        }

        public void AddOutputDevice(IOutputDevice device, byte port)
        {
            if (_outputDevices.ContainsKey(port))
            {
                _outputDevices.Remove(port);
            }
            _outputDevices.Add(port, device);
        }

        public void ReportNotImplemented()
        {
            // CPU will need to be reinitialized after this
            for( int i = 0; i < _opActions.Length; i++ )
            {
                try
                {
                    int test = _opActions[ i ]();
                } 
                catch( NotImplementedException )
                {
                    Debug.WriteLine( i.ToString( "X2" ) + " not implemented" );
                }
                catch( Exception )
                {
                    Debug.WriteLine(i.ToString("X2") + " threw an exception");
                }
            }
        }

        public int ExecuteNext()
        {

            // regarding the cpu testing binary
            // 0x0689 bad
            // 0x069B pass//if( pc == 0x0689 )
            //{
            //    Debug.WriteLine("fail");
            //}
            //if( pc == 0x069B )
            //{
            //    Debug.WriteLine("pass");
            //}


            UInt16 pc = _regs.ProgramCounter.Value;
            byte interrupt_op = 0x00;
            int cycles = 0;

            // First check for an interrupt. 
            if( _interrupts.TryDequeue(out interrupt_op))
            {
                if (enable_interrupts)
                {
                    enable_interrupts = false;
                    cycles = _opActions[interrupt_op]();
                }
            }

            // If no interrupt occurred then execute the instruction
            // in the program counter
            if (cycles == 0)
            {
                cycles = _opActions[_memory[_regs.ProgramCounter.Value]]();

                // if prior actions haven't altered the program counter then
                // increment based on the size of the current instruction
                if (pc == _regs.ProgramCounter.Value)
                {
                    _regs.ProgramCounter.Increment(_memory[_regs.ProgramCounter.Value]);
                }
            }

            // preserve the previous instruction for debugging
            _lastpc = pc;
            return cycles;
        }

        public void LoadInstructions(byte[] reg, UInt16 initProgramCounter)
        {
            Clear();
            _regs.ProgramCounter.Value = initProgramCounter;
            for (int i = 0; i < reg.Length; i++)
            {
                _memory[_regs.ProgramCounter.Value + i] = reg[i];
            }
        }

        private void LoadOpCodeActions()
        {

            #region NOPs
            _opActions[ 0x00 ] = () =>
            {
                return Op_NOP();
            };
            _opActions[ 0x08 ] = () =>
            {
                return Op_NOP();
            };
            _opActions[ 0x10 ] = () =>
            {
                return Op_NOP();
            };
            _opActions[ 0x18 ] = () =>
            {
                return Op_NOP();
            };
            _opActions[ 0x20 ] = () =>
            {
                return Op_NOP();
            };
            _opActions[ 0x28 ] = () =>
            {
                return Op_NOP();
            };
            _opActions[ 0x30 ] = () =>
            {
                return Op_NOP();
            };
            _opActions[ 0x38 ] = () =>
            {
                return Op_NOP();
            };
            #endregion

            #region LXI - load register pair with contents of memory pointed to by pc+1 and pc+2
            _opActions[ 0x01 ] = () =>
            {
                _regs.RegBC = i8080Registers.GetValue16(_memory[_regs.ProgramCounter.Value + 2],
                                                    _memory[_regs.ProgramCounter.Value + 1]);
                return Op_LXI();
            };

            _opActions[ 0x11 ] = () =>
            {
                _regs.RegDE = i8080Registers.GetValue16(_memory[_regs.ProgramCounter.Value + 2],
                                                    _memory[_regs.ProgramCounter.Value + 1]);
                return Op_LXI();
            };

            _opActions[ 0x21 ] = () =>
            {
                _regs.RegHL = i8080Registers.GetValue16(_memory[_regs.ProgramCounter.Value + 2],
                                                    _memory[_regs.ProgramCounter.Value + 1]);
                return Op_LXI();
            };

            _opActions[ 0x31 ] = () =>
            {
                _regs.StackPointer = i8080Registers.GetValue16(_memory[_regs.ProgramCounter.Value + 2],
                                                    _memory[_regs.ProgramCounter.Value + 1]);
                return Op_LXI();
            };
            #endregion

            #region STAX - store A into memory address pointed to in register pair
            _opActions[ 0x02 ] = () => { return Op_STAX( _regs.RegBC ); } ;
            _opActions[ 0x12 ] = () => { return Op_STAX( _regs.RegDE ); };
            #endregion

            #region INX - Increment Register Pair (no flags)
            _opActions[ 0x03 ] = () =>
            {
                _regs.RegBC += 1;
                return Op_INX();
            };

            _opActions[ 0x13 ] = () =>
            {
                _regs.RegDE += 1;
                return Op_INX();
            };

            _opActions[ 0x23 ] = () =>
            {
                _regs.RegHL += 1;
                return Op_INX();
            };

            _opActions[ 0x33 ] = () =>
            {
                _regs.StackPointer += 1;
                return Op_INX();
            };
            #endregion

            #region DCX - decrement register pair (no flags)
            _opActions[ 0x0b ] = () =>
            {
                _regs.RegBC -= 1;
                return Op_DCX();
            };

            _opActions[ 0x1b ] = () =>
            {
                _regs.RegDE -= 1;
                return Op_DCX();
            };

            _opActions[ 0x2b ] = () =>
            {
                _regs.RegHL -= 1;
                return Op_DCX();
            };

            _opActions[ 0x3b ] = () =>
            {
                _regs.StackPointer -= 1;
                return Op_DCX();
            };
            #endregion

            #region INR - increment  8 bit register (with flags)
            _opActions[ 0x04 ] = () =>
            {
                _regs.RegB = Increment( _regs.RegB );
                return Op_INR();
            };
            _opActions[ 0x14 ] = () =>
            {
                _regs.RegD = Increment( _regs.RegD );
                return Op_INR();
            };
            _opActions[ 0x24 ] = () =>
            {
                _regs.RegH = Increment( _regs.RegH );
                return Op_INR();
            };
            _opActions[ 0x34 ] = () =>  // use contents of address pointed to by HL
            {
                _memory[ _regs.RegHL ] = Increment( _memory[ _regs.RegHL ] );
                return 10;  // memory op is extra clock cycles
            };
            _opActions[ 0x0c ] = () =>
            {
                _regs.RegC = Increment( _regs.RegC );
                return Op_INR();
            };
            _opActions[ 0x1c ] = () =>
            {
                _regs.RegE = Increment( _regs.RegE );
                return Op_INR();
            };
            _opActions[ 0x2c ] = () =>
            {
                _regs.RegL = Increment( _regs.RegL );
                return Op_INR();
            };
            _opActions[ 0x3c ] = () =>
            {
                _regs.RegA = Increment( _regs.RegA );
                return Op_INR();
            };
            #endregion

            #region DCR - decrement  8 bit register (with flags)
            _opActions[ 0x05 ] = () =>
            {
                _regs.RegB = Decrement( _regs.RegB );
                return Op_DCR();
            };
            _opActions[ 0x15 ] = () =>
            {
                _regs.RegD = Decrement( _regs.RegD );
                return Op_DCR();
            };
            _opActions[ 0x25 ] = () =>
            {
                _regs.RegH = Decrement( _regs.RegH );
                return Op_DCR();
            };
            _opActions[ 0x35 ] = () =>  // use contents of address pointed to by HL
            {
                _memory[ _regs.RegHL ] = Decrement( _memory[ _regs.RegHL ] );
                return 10;  // memory op is extra clock cycles
            };
            _opActions[ 0x0d ] = () =>
            {
                _regs.RegC = Decrement( _regs.RegC );
                return Op_DCR();
            };
            _opActions[ 0x1d ] = () =>
            {
                _regs.RegE = Decrement( _regs.RegE );
                return Op_DCR();
            };
            _opActions[ 0x2d ] = () =>
            {
                _regs.RegL = Decrement( _regs.RegL );
                return Op_DCR();
            };
            _opActions[ 0x3d ] = () =>
            {
                _regs.RegA = Decrement( _regs.RegA );
                return Op_DCR();
            };
            #endregion

            #region MVI - move immediate byte to register
            _opActions[ 0x06 ] = () =>
            {
                _regs.RegB = _memory[ _regs.ProgramCounter.Value + 1 ];
                return Op_MVI();
            };
            _opActions[ 0x16 ] = () =>
            {
                _regs.RegD = _memory[_regs.ProgramCounter.Value + 1];
                return Op_MVI();
            };
            _opActions[ 0x26 ] = () =>
            {
                _regs.RegH = _memory[_regs.ProgramCounter.Value + 1];
                return Op_MVI();
            };
            _opActions[ 0x36 ] = () =>
            {
                _memory[_regs.RegHL] = _memory[_regs.ProgramCounter.Value + 1];
                return 10;  // memory op is extra clock cycles
            };
            _opActions[ 0x0e ] = () =>
            {
                _regs.RegC = _memory[_regs.ProgramCounter.Value + 1];
                return Op_MVI();
            };
            _opActions[ 0x1e ] = () =>
            {
                _regs.RegE = _memory[_regs.ProgramCounter.Value + 1];
                return Op_MVI();
            };
            _opActions[ 0x2e ] = () =>
            {
                _regs.RegL = _memory[_regs.ProgramCounter.Value + 1];
                return Op_MVI();
            };
            _opActions[ 0x3e ] = () =>
            {
                _regs.RegA = _memory[_regs.ProgramCounter.Value + 1];
                return Op_MVI();
            };
            #endregion

            #region immediate instructions
            // ADI -- immediate add where addend is the next byte after the instruction
            _opActions[0xc6] = () =>
            {
                _regs.RegA = Add(_regs.RegA, _memory[_regs.ProgramCounter.Value + 1]);
                return Op_Immediate();
            };
            // SUI -- subtract immediate
            _opActions[0xd6] = () =>
            {
                _regs.RegA = Subtract(_regs.RegA, _memory[_regs.ProgramCounter.Value + 1]);
                return Op_Immediate();
            };
            // ANI -- logical AND between A and immediate
            _opActions[0xe6] = () =>
            {
                _regs.RegA = LogicalAND(_regs.RegA, _memory[_regs.ProgramCounter.Value + 1]);
                return Op_Immediate();
            };
            // ORI -- logical OR between A and immediate
            _opActions[0xf6] = () =>
            {
                _regs.RegA = LogicalOR(_regs.RegA, _memory[_regs.ProgramCounter.Value + 1]);
                return Op_Immediate();
            }; 
            // ACI - add immediate to accumulator with carry
            _opActions[0xce] = () =>
            {
                _regs.RegA = AddWithCarry(_regs.RegA, _memory[_regs.ProgramCounter.Value + 1]);
                return Op_Immediate();
            };
            // SBI -- subtract immediate with borrow
            _opActions[0xde] = () =>
            {
                _regs.RegA = SubtractWithCarry(_regs.RegA, _memory[_regs.ProgramCounter.Value + 1]);
                return Op_Immediate();
            };
            // XRI -- XOR immediate 
            _opActions[0xee] = () =>
            {
                _regs.RegA = LogicalXOR(_regs.RegA, _memory[_regs.ProgramCounter.Value + 1]);
                return Op_Immediate();
            };
            // CPI -- Compare A to immediate 
            _opActions[0xfe] = () =>
            {
                Compare(_regs.RegA, _memory[_regs.ProgramCounter.Value + 1]);
                return Op_Immediate();
            };
            #endregion

            #region MOV - move 8 bit reg to 8 bit reg

            // MOV - dest B
            _opActions[ 0x40 ] = () => { return Op_MOV() ; } ;
            _opActions[ 0x41 ] = () => 
            {
                _regs.RegB = _regs.RegC;
                return Op_MOV() ;
            };
            _opActions[ 0x42 ] = () => 
            { 
                _regs.RegB = _regs.RegD;
                return Op_MOV() ;
            };
            _opActions[ 0x43 ] = () => 
            { 
                _regs.RegB = _regs.RegE; 
                return Op_MOV() ;
            }	;
            _opActions[ 0x44 ] = () => 
            {
                _regs.RegB = _regs.RegH; 
                return Op_MOV() ;
            }	;
            _opActions[ 0x45 ] = () => 
            {
                _regs.RegB = _regs.RegL;
                return Op_MOV() ;
            };
            _opActions[ 0x46 ] = () => 
            {
                _regs.RegB = _memory[_regs.RegHL]; 
                return Op_MOVM() ;
            }	;
            _opActions[ 0x47 ] = () => 
            {
                _regs.RegB = _regs.RegA; 
                return Op_MOV() ;
            };

            // MOV - dest C
            _opActions[ 0x48 ] = () => 
            {
                _regs.RegC = _regs.RegB; 
                return Op_MOV() ;
            }	;
            _opActions[ 0x49 ] = () => { return Op_MOV() ; } ;
            _opActions[ 0x4a ] = () => 
            {
                _regs.RegC = _regs.RegD; 
                return Op_MOV() ;
            }	;
            _opActions[ 0x4b ] = () => 
            {
                _regs.RegC = _regs.RegE; 
                return Op_MOV() ;
            }	;
            _opActions[ 0x4c ] = () => 
            {
                _regs.RegC = _regs.RegH; 
                return Op_MOV() ;
            }	;
            _opActions[ 0x4d ] = () => 
            {
                _regs.RegC = _regs.RegL; 
                return Op_MOV() ;
            }	;
            _opActions[ 0x4e ] = () => 
            {
                _regs.RegC = _memory[_regs.RegHL]; 
                return Op_MOVM() ;
            }	;
            _opActions[ 0x4f ] = () => 
            {
                _regs.RegC = _regs.RegA; 
                return Op_MOV() ;
            }	;

            // MOV - dest D
            _opActions[ 0x50 ] = () => 
            {
                _regs.RegD = _regs.RegB; 
                return Op_MOV() ;
            }	;
            _opActions[ 0x51 ] = () => 
            {
                _regs.RegD = _regs.RegC; 
                return Op_MOV() ;
            }	;
            _opActions[ 0x52 ] = () => { return Op_MOV() ; } ;
            _opActions[ 0x53 ] = () => 
            {
                _regs.RegD = _regs.RegE; 
                return Op_MOV() ;
            }	;
            _opActions[ 0x54 ] = () => 
            {
                _regs.RegD = _regs.RegH; 
                return Op_MOV() ;
            }	;
            _opActions[ 0x55 ] = () => 
            {
                _regs.RegD = _regs.RegL; 
                return Op_MOV() ;
            }	;
            _opActions[ 0x56 ] = () => 
            {
                _regs.RegD = _memory[_regs.RegHL]; 
                return Op_MOVM() ;
            }	;
            _opActions[ 0x57 ] = () => 
            {
                _regs.RegD = _regs.RegA; 
                return Op_MOV() ;
            }	;

            // MOV - dest E
            _opActions[ 0x58 ] = () => 
            {
                _regs.RegE = _regs.RegB; 
                return Op_MOV() ;
            };	
            _opActions[ 0x59 ] = () => 
            {
                _regs.RegE = _regs.RegC; 
                return Op_MOV() ;
            };	
            _opActions[ 0x5a ] = () => 
            {
                _regs.RegE = _regs.RegD; 
                return Op_MOV() ;
            };	
            _opActions[ 0x5b ] = () => { return Op_MOV() ; } ;
            _opActions[ 0x5c ] = () => 
            {
                _regs.RegE = _regs.RegH; 
                return Op_MOV() ;
            };	
            _opActions[ 0x5d ] = () => 
            {
                _regs.RegE = _regs.RegL; 
                return Op_MOV() ;
            };	
            _opActions[ 0x5e ] = () => 
            {
                _regs.RegE = _memory[_regs.RegHL]; 
                return Op_MOVM() ;
            }	;
            _opActions[ 0x5f ] = () => 
            {
                _regs.RegE = _regs.RegA; 
                return Op_MOV() ;
            };

            // MOV - dest H
            _opActions[ 0x60 ] = () => 
            {
                _regs.RegH = _regs.RegB; 
                return Op_MOV() ;
            };
            _opActions[ 0x61 ] = () => 
            {
                _regs.RegH = _regs.RegC; 
                return Op_MOV() ;
            };
            _opActions[ 0x62 ] = () => 
            {
                _regs.RegH = _regs.RegD; 
                return Op_MOV() ;
            };
            _opActions[ 0x63 ] = () => 
            {
                _regs.RegH = _regs.RegE; 
                return Op_MOV() ;
            };
            _opActions[ 0x64 ] = () => { return Op_MOV() ; } ;
            _opActions[ 0x65 ] = () => 
            {
                _regs.RegH = _regs.RegL; 
                return Op_MOV() ;
            };
            _opActions[ 0x66 ] = () => 
            {
                _regs.RegH = _memory[_regs.RegHL]; 
                return Op_MOVM() ;
            }	;
            _opActions[ 0x67 ] = () => 
            {
                _regs.RegH = _regs.RegA; 
                return Op_MOV() ;
            };

            // MOV - dest L
            _opActions[ 0x68 ] = () => 
            {
                _regs.RegL = _regs.RegB; 
                return Op_MOV() ;
            };
            _opActions[ 0x69 ] = () => 
            {
                _regs.RegL = _regs.RegC; 
                return Op_MOV() ;
            };
            _opActions[ 0x6a ] = () => 
            {
                _regs.RegL = _regs.RegD; 
                return Op_MOV() ;
            };
            _opActions[ 0x6b ] = () => 
            {
                _regs.RegL = _regs.RegE; 
                return Op_MOV() ;
            };
            _opActions[ 0x6c ] = () => 
            {
                _regs.RegL = _regs.RegH; 
                return Op_MOV() ;
            };
            _opActions[ 0x6d ] = () => { return Op_MOV() ; } ;
            _opActions[ 0x6e ] = () => 
            {
                _regs.RegL = _memory[_regs.RegHL]; 
                return Op_MOVM() ;
            }	;
            _opActions[ 0x6f ] = () => 
            {
                _regs.RegL = _regs.RegA; 
                return Op_MOV() ;
            };

            // MOV - dest M
            _opActions[ 0x70 ] = () => 
            {
                _memory[_regs.RegHL] = _regs.RegB; 
                return Op_MOVM() ;
            };
            _opActions[ 0x71 ] = () => 
            {
                _memory[_regs.RegHL] = _regs.RegC; 
                return Op_MOVM() ;
            };
            _opActions[ 0x72 ] = () => 
            {
                _memory[_regs.RegHL] = _regs.RegD; 
                return Op_MOVM() ;
            };
            _opActions[ 0x73 ] = () => 
            {
                _memory[_regs.RegHL] = _regs.RegE; 
                return Op_MOVM() ;
            };
            _opActions[ 0x74 ] = () => 
            {
                _memory[_regs.RegHL] = _regs.RegH; 
                return Op_MOVM() ;
            };
            _opActions[ 0x75 ] = () => 
            {
                _memory[_regs.RegHL] = _regs.RegL; 
                return Op_MOVM() ;
            };
            _opActions[ 0x77 ] = () => 
            {
                _memory[_regs.RegHL] = _regs.RegA; 
                return Op_MOVM() ;
            };

            // MOV - dest A
            _opActions[ 0x78 ] = () => 
            {
                _regs.RegA = _regs.RegB;
                return Op_MOV() ;
            };
            _opActions[ 0x79 ] = () => 
            {
                _regs.RegA = _regs.RegC;
                return Op_MOV();
            };
            _opActions[ 0x7a ] = () => 
            {
                _regs.RegA = _regs.RegD;
                return Op_MOV();
            };
            _opActions[ 0x7b ] = () => 
            {
                _regs.RegA = _regs.RegE;
                return Op_MOV();
            };
            _opActions[ 0x7c ] = () => 
            {
                _regs.RegA = _regs.RegH;
                return Op_MOV();
            };
            _opActions[ 0x7d ] = () => 
            {
                _regs.RegA = _regs.RegL;
                return Op_MOV();
            };
            _opActions[ 0x7e ] = () => 
            {
                _regs.RegA = _memory[_regs.RegHL];
                return Op_MOVM();
            };
            _opActions[ 0x7f ] = () => { return Op_MOV() ; };
            #endregion

            #region ADD - add 8 bit register to A and store in A
            _opActions[ 0x80 ] = () => { return Op_ADD(_regs.RegB) ; };
            _opActions[ 0x81 ] = () => { return Op_ADD(_regs.RegC) ; };
            _opActions[ 0x82 ] = () => { return Op_ADD(_regs.RegD) ; };
            _opActions[ 0x83 ] = () => { return Op_ADD(_regs.RegE) ; };
            _opActions[ 0x84 ] = () => { return Op_ADD(_regs.RegH) ; };
            _opActions[ 0x85 ] = () => { return Op_ADD(_regs.RegL) ; };
            _opActions[ 0x86 ] = () => { return Op_ADDMemory(_memory[ _regs.RegHL ] ) ; };
            _opActions[ 0x87 ] = () => { return Op_ADD(_regs.RegA) ; };
            #endregion

            #region ADC - add 8 bit register with carry to A and store in A
            _opActions[ 0x88 ] = () =>
            {
                return Op_ADC( _regs.RegB );
            };
            _opActions[ 0x89 ] = () =>
            {
                return Op_ADC( _regs.RegC );
            };
            _opActions[ 0x8a ] = () =>
            {
                return Op_ADC( _regs.RegD );
            };
            _opActions[ 0x8b ] = () =>
            {
                return Op_ADC( _regs.RegE );
            };
            _opActions[ 0x8c ] = () =>
            {
                return Op_ADC( _regs.RegH );
            };
            _opActions[ 0x8d ] = () =>
            {
                return Op_ADC( _regs.RegL );
            };
            _opActions[ 0x8e ] = () =>
            {
                return Op_ADCMemory( _memory[ _regs.RegHL ] );
            };
            _opActions[ 0x8f ] = () =>
            {
                return Op_ADC( _regs.RegA );
            };
            #endregion

            #region SUB - subtract 8 bit register from A and store in A
            _opActions[ 0x90 ] = () =>
            {
                return Op_SUB( _regs.RegB );
            };
            _opActions[ 0x91 ] = () =>
            {
                return Op_SUB( _regs.RegC );
            };
            _opActions[ 0x92 ] = () =>
            {
                return Op_SUB( _regs.RegD );
            };
            _opActions[ 0x93 ] = () =>
            {
                return Op_SUB( _regs.RegE );
            };
            _opActions[ 0x94 ] = () =>
            {
                return Op_SUB( _regs.RegH );
            };
            _opActions[ 0x95 ] = () =>
            {
                return Op_SUB( _regs.RegL );
            };
            _opActions[ 0x96 ] = () =>
            {
                return Op_SUBMemory( _memory[ _regs.RegHL ] );
            };
            _opActions[ 0x97 ] = () =>
            {
                return Op_SUB( _regs.RegA );
            };
            #endregion

            #region SBB - subtract 8 bit register with borrow from A and store in A
            _opActions[ 0x98 ] = () =>
            {
                return Op_SBB( _regs.RegB );
            };
            _opActions[ 0x99 ] = () =>
            {
                return Op_SBB( _regs.RegC );
            };
            _opActions[ 0x9a ] = () =>
            {
                return Op_SBB( _regs.RegD );
            };
            _opActions[ 0x9b ] = () =>
            {
                return Op_SBB( _regs.RegE );
            };
            _opActions[ 0x9c ] = () =>
            {
                return Op_SBB( _regs.RegH );
            };
            _opActions[ 0x9d ] = () =>
            {
                return Op_SBB( _regs.RegL );
            };
            _opActions[ 0x9e ] = () =>
            {
                return Op_SBBMemory( _memory[ _regs.RegHL ] );
            };
            _opActions[ 0x9f ] = () =>
            {
                return Op_SBB( _regs.RegA );
            };
            #endregion

            #region ANA - logical AND with register and A and store in A
            _opActions[ 0xa0 ] = () =>
            {
                return Op_ANA( _regs.RegB );
            };
            _opActions[ 0xa1 ] = () =>
            {
                return Op_ANA( _regs.RegC );
            };
            _opActions[ 0xa2 ] = () =>
            {
                return Op_ANA( _regs.RegD );
            };
            _opActions[ 0xa3 ] = () =>
            {
                return Op_ANA( _regs.RegE );
            };
            _opActions[ 0xa4 ] = () =>
            {
                return Op_ANA( _regs.RegH );
            };
            _opActions[ 0xa5 ] = () =>
            {
                return Op_ANA( _regs.RegL );
            };
            _opActions[ 0xa6 ] = () =>
            {
                return Op_ANAMemory( _memory[ _regs.RegHL ] );
            };
            _opActions[ 0xa7 ] = () =>
            {
                return Op_ANA( _regs.RegA );
            };
            #endregion

            #region XRA - logical XOR with register and A and store in A
            _opActions[ 0xa8 ] = () =>
            {
                return Op_XRA( _regs.RegB );
            };
            _opActions[ 0xa9 ] = () =>
            {
                return Op_XRA( _regs.RegC );
            };
            _opActions[ 0xaa ] = () =>
            {
                return Op_XRA( _regs.RegD );
            };
            _opActions[ 0xab ] = () =>
            {
                return Op_XRA( _regs.RegE );
            };
            _opActions[ 0xac ] = () =>
            {
                return Op_XRA( _regs.RegH );
            };
            _opActions[ 0xad ] = () =>
            {
                return Op_XRA( _regs.RegL );
            };
            _opActions[ 0xae ] = () =>
            {
                return Op_XRAMemory( _memory[ _regs.RegHL ] );
            };
            _opActions[ 0xaf ] = () =>
            {
                return Op_XRA( _regs.RegA );
            };
            #endregion

            #region ORA - logical OR with register and A and store in A
            _opActions[ 0xb0 ] = () =>
            {
                return Op_ORA( _regs.RegB );
            };
            _opActions[ 0xb1 ] = () =>
            {
                return Op_ORA( _regs.RegC );
            };
            _opActions[ 0xb2 ] = () =>
            {
                return Op_ORA( _regs.RegD );
            };
            _opActions[ 0xb3 ] = () =>
            {
                return Op_ORA( _regs.RegE );
            };
            _opActions[ 0xb4 ] = () =>
            {
                return Op_ORA( _regs.RegH );
            };
            _opActions[ 0xb5 ] = () =>
            {
                return Op_ORA( _regs.RegL );
            };
            _opActions[ 0xb6 ] = () =>
            {
                return Op_ORAMemory( _memory[ _regs.RegHL ] );
            };
            _opActions[ 0xb7 ] = () =>
            {
                return Op_ORA( _regs.RegA );
            };
            #endregion

            #region CMP - compare register with A, modify flags only
            _opActions[ 0xb8 ] = () =>
            {
                return Op_CMP( _regs.RegB );
            };
            _opActions[ 0xb9 ] = () =>
            {
                return Op_CMP( _regs.RegC );
            };
            _opActions[ 0xba ] = () =>
            {
                return Op_CMP( _regs.RegD );
            };
            _opActions[ 0xbb ] = () =>
            {
                return Op_CMP( _regs.RegE );
            };
            _opActions[ 0xbc ] = () =>
            {
                return Op_CMP( _regs.RegH );
            };
            _opActions[ 0xbd ] = () =>
            {
                return Op_CMP( _regs.RegL );
            };
            _opActions[ 0xbe ] = () =>
            {
                return OpCMPMemory( _memory[ _regs.RegHL ] );
            };
            _opActions[ 0xbf ] = () =>
            {
                return Op_CMP( _regs.RegA );
            };
            #endregion

            #region DAD - add double register to HL and store in HL
            _opActions[ 0x09 ] = () =>
            {
                return Op_DAD( _regs.RegBC );
            };
            _opActions[ 0x19 ] = () =>
            {
                return Op_DAD( _regs.RegDE );
            };
            _opActions[ 0x29 ] = () =>
            {
                return Op_DAD( _regs.RegHL );
            };
            _opActions[ 0x39 ] = () =>
            {
                return Op_DAD( _regs.StackPointer );
            };
            #endregion

            #region Rotate Instructions (RLC,RAL,RRC,RAR)
            _opActions[0x07] = () => // RLC rotate left
            {
                // carry bit equal to high bit
                _regs.CondReg.CarryFlag = ((_regs.RegA & 0x80) == 0x80);

                // shift left
                _regs.RegA = (byte)((_regs.RegA << 1) & 0xff);

                // low bit equals old high bit (which is now carry bit)
                if (_regs.CondReg.CarryFlag) _regs.RegA = (byte)(_regs.RegA | 0x01);
                return Op_Rotate();
            };
            _opActions[0x17] = () => // RAL rotate left through carry
            {
                // shift left into a 16 bit int
                UInt16 temp = (UInt16)(_regs.RegA << 1);

                // set accumulator to first 8 bits of temp
                _regs.RegA = (byte)(temp & 0xff);

                // set low bit to the carry bit
                if (_regs.CondReg.CarryFlag) _regs.RegA = (byte)(_regs.RegA | 0x01);

                // set carry bit equal to the original high bit
                _regs.CondReg.CarryFlag = (temp > 0x0ff); 
                return Op_Rotate();
            };
            _opActions[0x0f] = () => // RRC rotate right
            {
                // carry bit equal to low bit
                _regs.CondReg.CarryFlag = ((_regs.RegA & 0x01) == 0x01);

                // shift right
                _regs.RegA = (byte)((_regs.RegA >> 1) & 0xff);

                // high bit equals old low bit (which is now carry bit)
                if (_regs.CondReg.CarryFlag) _regs.RegA = (byte)(_regs.RegA | 0x80);
                return Op_Rotate();
            };
            _opActions[0x1f] = () => // RAR rotate right through carry
            {
                // low order bit replaces carry bit and
                // original carry bit replaces the high bit

                // shift right into temp
                byte temp = (byte)(_regs.RegA >> 1);

                // set high bit of temp to carry bit
                if (_regs.CondReg.CarryFlag) temp = (byte)(temp | 0x80);

                // set carry bit to original low bit
                _regs.CondReg.CarryFlag = ((byte)(_regs.RegA & 0x01) == 0x01);

                // set accumulator to temp
                _regs.RegA = temp;
                return Op_Rotate();
            };
            #endregion

            #region Store Instructions (STA, SHLD)
            
            // STA - store accumulator direct (store A into the memory address formed by the immediate address
            _opActions[0x32] = () =>
            {
                _memory[GetImmediateAddr()] = _regs.RegA;
                return 13;
            };
            // SHLD - Store H and L direct
            _opActions[0x22] = () =>
            {
                _memory[GetImmediateAddr()] = _regs.RegL;
                _memory[GetImmediateAddr() + 1] = _regs.RegH;
                return 16;
            }; // (adr) <-L; (adr+1)<-H"

            #endregion

            #region Load Instructions (LDAX,LDA,LHLD,SPHL,PCHL)
            // LDAX - load contents of memory address BC into A
            _opActions[0x0a] = () =>
            {
                _regs.RegA = _memory[_regs.RegBC];
                return 7;
            };

            // LDAX - load contents of memory address DE into A
            _opActions[0x1a] = () =>
            {
                _regs.RegA = _memory[_regs.RegDE];
                return 7;
            };
            // LAD - load accumulator direct (contents of memory at immediate address replaces the contents of a)
            _opActions[0x3a] = () =>
            {
                _regs.RegA = _memory[GetImmediateAddr()];
                return 13;
            };
            // LHLD - load H and L direct
            _opActions[0x2a] = () =>
            {
                _regs.RegL = _memory[GetImmediateAddr()];
                _regs.RegH = _memory[GetImmediateAddr() + 1];
                return 16;
            }; // L <- (adr); H<-(adr+1)"

            _opActions[0xf9] = () => // SPHL - load stack pointer with HL
            {
                _regs.StackPointer = _regs.RegHL;
                return 5;
            };

            _opActions[0xe9] = () => // PCHL - load program counter with HL
            {
                _regs.ProgramCounter.Value = _regs.RegHL;
                return 5;
            };
            #endregion

            #region Carry Instructions (STC,CMC)
            // STC - set carry
            _opActions[0x37] = () => 
            {
                _regs.CondReg.CarryFlag = true ;
                return 4;
            };
            // CMC - complement carry
            _opActions[0x3f] = () =>
            {
                _regs.CondReg.CarryFlag = !_regs.CondReg.CarryFlag;
                return 4;
            };
            #endregion

            #region Push Instructions
            // PUSH BC
            _opActions[0xc5] = () => 
            { 
                return Op_Push(_regs.RegBC) ; 
            }; 
            // PUSH DE
            _opActions[0xd5] = () =>
            {
                return Op_Push(_regs.RegDE);
            }; 
            // PUSH HL
            _opActions[0xe5] = () =>
            {
                return Op_Push(_regs.RegHL);
            }; 
            // PUSH PSW
            _opActions[0xf5] = () =>
            {
                return Op_Push(_regs.RegPSW);
            }; 
            #endregion

            #region Pop Instructions
            // POP BC
            _opActions[0xc1] = () =>
            {
                UInt16 pop = PopRegister();
                _regs.RegC = i8080Registers.GetLoValue(pop);
                _regs.RegB = i8080Registers.GetHiValue(pop);
                return Op_Pop();
            };
            // POP DE
            _opActions[0xd1] = () =>
            {
                UInt16 pop = PopRegister();
                _regs.RegE = i8080Registers.GetLoValue(pop);
                _regs.RegD = i8080Registers.GetHiValue(pop);
                return Op_Pop();
            };
            // POP HL
            _opActions[0xe1] = () =>
            {
                UInt16 pop = PopRegister();
                _regs.RegL = i8080Registers.GetLoValue(pop);
                _regs.RegH = i8080Registers.GetHiValue(pop);
                return Op_Pop();
            };
            // POP PSW
            _opActions[0xf1] = () =>
            {
                UInt16 pop = PopRegister();
                _regs.RegPSW = i8080Registers.GetLoValue(pop);
                _regs.RegA = i8080Registers.GetHiValue(pop);
                return Op_Pop();
            };
            #endregion

            #region Jump Instructions
            // JNZ
            _opActions[0xc2] = () =>
            {
                return Op_Jump(!_regs.CondReg.ZeroFlag);
            };
            // JNC
            _opActions[0xd2] = () =>
            {
                return Op_Jump(!_regs.CondReg.CarryFlag);
            };
            // JPO
            _opActions[0xe2] = () =>
            {
                return Op_Jump(!_regs.CondReg.ParityFlag);
            };
            // JP
            _opActions[0xf2] = () =>
            {
                return Op_Jump(!_regs.CondReg.SignFlag);
            };
            // JMP
            _opActions[0xc3] = () =>
            {
                return Op_Jump(true);
            };
            // JZ
            _opActions[0xca] = () =>
            {
                return Op_Jump(_regs.CondReg.ZeroFlag);
            };
            // JC
            _opActions[0xda] = () =>
            {
                return Op_Jump(_regs.CondReg.CarryFlag);
            };
            // JPE
            _opActions[0xea] = () =>
            {
                return Op_Jump(_regs.CondReg.ParityFlag);
            };
            // JM
            _opActions[0xfa] = () =>
            {
                return Op_Jump(_regs.CondReg.SignFlag);
            };

            // JMP -- alternate to 0xc3, this instruction should not be used
            _opActions[0xcb] = _opActions[0xc3];

            #endregion

            #region Call Instructions
            // CNZ
            _opActions[0xc4] = () =>
            {
                return Op_Call(!_regs.CondReg.ZeroFlag);
            };
            // CNC
            _opActions[0xd4] = () =>
            {
                return Op_Call(!_regs.CondReg.CarryFlag);
            };
            // CPO
            _opActions[0xe4] = () =>
            {
                return Op_Call(!_regs.CondReg.ParityFlag);
            };
            // CP
            _opActions[0xf4] = () =>
            {
                return Op_Call(!_regs.CondReg.SignFlag);
            };

            // CZ
            _opActions[0xcc] = () =>
            {
                return Op_Call(_regs.CondReg.ZeroFlag);
            };
            // CC
            _opActions[0xdc] = () =>
            {
                return Op_Call(_regs.CondReg.CarryFlag);
            };
            // CPE
            _opActions[0xec] = () =>
            {
                return Op_Call(_regs.CondReg.ParityFlag);
            };
            // CM
            _opActions[0xfc] = () =>
            {
                return Op_Call(_regs.CondReg.SignFlag);
            };

            // CALL
            _opActions[0xcd] = () =>
            {
                return Op_Call(true);
            };

            // alternative CALL instructions that should not be used
            _opActions[0xdd] = _opActions[0xcd];
            _opActions[0xed] = _opActions[0xcd];
            _opActions[0xfd] = _opActions[0xcd];
            #endregion

            #region Return Instructions
            // RNZ
            _opActions[0xc0] = () =>
            {
                return Op_Return(!_regs.CondReg.ZeroFlag);
            };
            // RNC
            _opActions[0xd0] = () =>
            {
                return Op_Return(!_regs.CondReg.CarryFlag);
            };
            // RPO
            _opActions[0xe0] = () =>
            {
                return Op_Return(!_regs.CondReg.ParityFlag);
            };
            // RP
            _opActions[0xf0] = () =>
            {
                return Op_Return(!_regs.CondReg.SignFlag);
            };
            // RZ
            _opActions[0xc8] = () =>
            {
                return Op_Return(_regs.CondReg.ZeroFlag);
            };
            // RC
            _opActions[0xd8] = () =>
            {
                return Op_Return(_regs.CondReg.CarryFlag);
            };
            // RPE
            _opActions[0xe8] = () =>
            {
                return Op_Return(_regs.CondReg.ParityFlag);
            };
            // RM
            _opActions[0xf8] = () =>
            {
                return Op_Return(_regs.CondReg.SignFlag);
            };
            // RET
            _opActions[0xc9] = () =>
            {
                return Op_Return(true);
            };

            // alternative RET instruction that should not be used
            _opActions[0xd9] = _opActions[0xc9];

            #endregion

            #region Special Instructions

            _opActions[0x27] = () => // DAA
            {
                byte ls4 = (byte)(_regs.RegA & 0x0f);

                if( (byte)(_regs.RegA & 0x0f) > 0x09 || _regs.CondReg.AuxCarryFlag )
                {
                    _regs.RegA += 0x06;
                    if( ls4 + 0x06 > 0x10 )
                    {
                        _regs.CondReg.AuxCarryFlag = true;
                    }
                    else
                    {
                        _regs.CondReg.AuxCarryFlag = false;
                    }
                }
                else
                {
                    _regs.CondReg.AuxCarryFlag = false;
                }

                if( ( byte ) ( _regs.RegA & 0xf0 ) > 0x90 || _regs.CondReg.CarryFlag )
                {
                    UInt16 answer = (UInt16)(_regs.RegA + 0x60);
                    _regs.CondReg.CalcCarryFlag(answer);
                    _regs.CondReg.CalcZeroFlag(answer);
                    _regs.CondReg.CalcParityFlag(answer);
                    _regs.CondReg.CalcSignFlag(answer);
                    _regs.RegA = (byte)(answer & 0xff);
                }
                else
                {
                    _regs.CondReg.CarryFlag = false;
                }
                return 4;

            };
            _opActions[0x76] = () => // HLT
            {
                // halt state, does not increment the program counter
                return 7; 
            };

            _opActions[0x2f] = () => // CMA - complement accumulator
            {
                _regs.RegA = (byte)~_regs.RegA;
                return 4;
            };

            _opActions[0xe3] = () => // XTHL - exchange stack with HL
            {
                // contents of L exchanged with contents of memory pointed to by sp
                // contents of H are exchanged with contents of memory pointed to by sp+1
                byte temp = _memory[_regs.StackPointer];
                _memory[_regs.StackPointer] = _regs.RegL;
                _regs.RegL = temp;

                temp = _memory[_regs.StackPointer + 1];
                _memory[_regs.StackPointer + 1] = _regs.RegH;
                _regs.RegH = temp;

                return 18;
            };

            _opActions[0xeb] = () => // XCHG - exchange DE and HL
            {
                // H <-> D and L <-> E
                byte temp = _regs.RegD;
                _regs.RegD = _regs.RegH;
                _regs.RegH = temp;

                temp = _regs.RegE;
                _regs.RegE = _regs.RegL;
                _regs.RegL = temp;

                return 5;
            };

            #endregion

            #region Input/Output device instructions

            // IN   data from port is stored in the accumulator
            _opActions[0xdb] = () =>
            {
                IInputDevice device;
                byte port = _memory[_regs.ProgramCounter.Value + 1];
                if (_inputDevices.TryGetValue(port, out device))
                {
                    _regs.RegA = device.Read();
                }
                else
                {
                    // if no device is attached, zero out the register
                    _regs.RegA = 0;
                }
                return 10;
            };

            // OUT  accumulator is written out to the port
            _opActions[0xd3] = () =>
            {
                IOutputDevice device;
                byte port = _memory[_regs.ProgramCounter.Value + 1];
                if (_outputDevices.TryGetValue(port, out device))
                {
                    device.Write(_regs.RegA);
                }
                return 10;
            };
            #endregion

            #region Interrupt Instructions

            // EI
            _opActions[0xfb] = () =>
            {
                enable_interrupts = true;
                return 4;
            };

            // DI
            _opActions[0xf3] = () =>
            {
                enable_interrupts = false;
                return 4;
            };

            // RST 0
            _opActions[0xc7] = () =>
            {
                return Op_RST(0);
            };
            _opActions[0xcf] = () =>
            {
                return Op_RST(1);
            };
            _opActions[0xd7] = () =>
            {
                return Op_RST(2);
            };
            _opActions[0xdf] = () =>
            {
                return Op_RST(3);
            };
            _opActions[0xe7] = () =>
            {
                return Op_RST(4);
            };
            _opActions[0xef] = () =>
            {
                return Op_RST(5);
            };
            _opActions[0xf7] = () =>
            {
                return Op_RST(6);
            };
            _opActions[0xff] = () =>
            {
                return Op_RST(7);
            };
            #endregion
        }

        #region OpCode Helper Functions

        private int Op_NotImplemented()
        {
            throw new NotImplementedException( "Instruction " + _memory[ _regs.ProgramCounter.Value ].ToString("X2") + " not implemented" );
        }

        private int Op_NOP()
        {
            return 4;
        }

        private int Op_LXI()
        {
            return 10;
        }

        private int Op_STAX( UInt16 addr )
        {
            _memory[addr] = _regs.RegA;
            return 7;
        }

        private int Op_INX()
        {
            return 5;
        }

        private int Op_DCX()
        {
            return 5;
        }

        private int Op_INR()
        {
            return 5;
        }

        private int Op_DCR()
        {
            return 5;
        }

        private int Op_MVI()
        {
            return 7;
        }

        private int Op_Immediate()
        {
            return 7;
        }

        private int Op_MOV()
        {
            return 5;
        }

        private int Op_MOVM()
        {
            return 7;
        }

        private int Op_ADD( byte b )
        {
            _regs.RegA = Add( _regs.RegA, b );
            return 4;
        }

        private int Op_ADDMemory( byte b )
        {
            _regs.RegA = Add( _regs.RegA, b );
            return 7;
        }

        private int Op_ADC( byte b )
        {
            _regs.RegA = AddWithCarry( _regs.RegA, b );
            return 4;
        }

        private int Op_ADCMemory( byte b )
        {
            _regs.RegA = AddWithCarry( _regs.RegA, b );
            return 7;
        }

        private int Op_SUB( byte b )
        {
            _regs.RegA = Subtract( _regs.RegA, b );
            return 4;
        }

        private int Op_SUBMemory( byte b )
        {
            _regs.RegA = Subtract( _regs.RegA, b );
            return 7;
        }

        private int Op_SBB( byte b )
        {
            _regs.RegA = SubtractWithCarry( _regs.RegA, b );
            return 4;
        }

        private int Op_SBBMemory( byte b )
        {
            _regs.RegA = SubtractWithCarry( _regs.RegA, b );
            return 7;
        }

        private int Op_ANA( byte b )
        {
            _regs.RegA = LogicalAND( _regs.RegA, b );
            return 4;
        }

        private int Op_ANAMemory( byte b )
        {
            _regs.RegA = LogicalAND( _regs.RegA, b );
            return 7;
        }

        private int Op_XRA( byte b )
        {
            _regs.RegA = LogicalXOR( _regs.RegA, b );
            return 4;
        }

        private int Op_XRAMemory( byte b )
        {
            _regs.RegA = LogicalXOR( _regs.RegA, b );
            return 7;
        }

        private int Op_ORA( byte b )
        {
            _regs.RegA = LogicalOR( _regs.RegA, b );
            return 4;
        }

        private int Op_ORAMemory( byte b )
        {
            _regs.RegA = LogicalOR( _regs.RegA, b );
            return 7;
        }

        private int Op_CMP( byte b )
        {
            Compare( _regs.RegA, b );
            return 4;
        }

        private int OpCMPMemory( byte b )
        {
            Compare( _regs.RegA, b );
            return 7;
        }

        private int Op_DAD( UInt16 num )
        {
            _regs.RegHL = DoubleAdd( _regs.RegHL, num );
            return 10;
        }

        private int Op_Rotate()
        {
            return 4;
        }

        private int Op_Push(byte hi, byte lo)
        {
            PushBytes(hi, lo);
            return 11;
        }

        private int Op_Push(UInt16 num)
        {
            PushRegister(num);
            return 11;
        }

        private int Op_Pop()
        {
            return 10;
        }

        private int Op_Jump(bool jump)
        {
            if (jump)
            {
                _regs.ProgramCounter.Value = GetImmediateAddr();
            }
            return 10;
        }

        private int Op_Call(bool call)
        {
            // push the current program counter and load
            // program counter with immediate

            if (call)
            {
                // Push the program counter for the *next* instruction.
                PushRegister((UInt16)(_regs.ProgramCounter.Value + 3));
                _regs.ProgramCounter.Value = GetImmediateAddr();
                return 17;
            }
            else
            {
                // fewer clock cycles if no jump
                return 11;
            }

        }

        private int Op_Return(bool condition)
        {
            if(condition)
            {
                _regs.ProgramCounter.Value = PopRegister();
                return 11;
            }
            else
            {
                return 5;
            }
        }

        private int Op_RST(byte interrupt_num)
        {

            // push the ProgramCounter to the stack
            PushRegister(_regs.ProgramCounter.Value);

            // replace program counter with the special interrupt location
            _regs.ProgramCounter.Value = (UInt16)(interrupt_num * 8);

            return 11;
        }

        #endregion

        #region Utility and Math Functions

        private byte Add( byte a, byte b )
        {
            UInt16 answer = ( UInt16 ) ( a + b );
            _regs.CondReg.CalcCarryFlag( answer );
            _regs.CondReg.CalcZeroFlag( answer );
            _regs.CondReg.CalcParityFlag( answer );
            _regs.CondReg.CalcSignFlag( answer );
            _regs.CondReg.CalcAuxCarryFlag( a, b );
            return ( byte ) ( answer & 0xff );
        }

        private byte Increment( byte a )
        {
            UInt16 answer = ( UInt16 ) ( a + 1 );

            // increment instruction does not alter the carry flag
            _regs.CondReg.CalcZeroFlag( answer );
            _regs.CondReg.CalcParityFlag( answer );
            _regs.CondReg.CalcSignFlag( answer );
            _regs.CondReg.CalcAuxCarryFlag( a, 1 );
            return ( byte ) ( answer & 0xff );
        }

        private byte Decrement( byte a )
        {
            byte answer = ( byte ) ( a - 1 );

            // decrement instruction does not alter the carry flag
            _regs.CondReg.CalcZeroFlag( answer );
            _regs.CondReg.CalcParityFlag( answer );
            _regs.CondReg.CalcSignFlag( answer );
            //_regs.CondReg.CalcAuxCarryFlag( a, 1 );
            _regs.CondReg.AuxCarryFlag = false; // not sure if this is right
            return answer;
        }

        private UInt16 DoubleAdd( UInt16 a, UInt16 b )
        {
            UInt32 answer = ( UInt32 ) ( a + b );
            _regs.CondReg.CarryFlag = ( answer > 0xffff );
            return ( UInt16 ) ( answer & 0xffff );
        }

        private byte AddWithCarry( byte a, byte b )
        {
            UInt16 answer = ( UInt16 ) ( a + b );
            if( _regs.CondReg.CarryFlag )
            {
                answer += 1;
                _regs.CondReg.CalcAuxCarryFlag(a, b, 1);
            }
            else
            {
                _regs.CondReg.CalcAuxCarryFlag(a, b);
            }
            _regs.CondReg.CalcCarryFlag( answer );
            _regs.CondReg.CalcZeroFlag( answer );
            _regs.CondReg.CalcParityFlag( answer );
            _regs.CondReg.CalcSignFlag( answer );
            return ( byte ) ( answer & 0xff );
        }

        private byte Subtract( byte a, byte b )
        {
            // Subtract using 2's compliment
            UInt16 answer = (UInt16)(a + (~b & 0xff) + 1);

            // On subtraction no carry out sets the carry bit
            // this is opposite from the normal calculation           
            _regs.CondReg.CalcCarryFlag( answer );
            _regs.CondReg.CarryFlag = !_regs.CondReg.CarryFlag;

            _regs.CondReg.CalcZeroFlag( answer );
            _regs.CondReg.CalcParityFlag( answer );
            _regs.CondReg.CalcSignFlag( answer );
            _regs.CondReg.CalcAuxCarryFlag( a, (byte)(~b & 0xff), 1 );
            return ( byte ) ( answer & 0xff );
        }

        private byte SubtractWithCarry( byte a, byte b )
        {
            if( _regs.CondReg.CarryFlag )
                b += 1;
            return Subtract( a, b );
        }

        private byte  LogicalAND( byte a, byte b )
        {
            byte answer = ( byte ) ( a & b );
            _regs.CondReg.CarryFlag = false;
            _regs.CondReg.AuxCarryFlag = false;
            _regs.CondReg.CalcZeroFlag(answer);
            _regs.CondReg.CalcParityFlag( answer );
            _regs.CondReg.CalcSignFlag( answer );
            return answer;
        }

        private byte LogicalXOR( byte a, byte b )
        {
            byte answer = ( byte ) ( a ^ b );
            _regs.CondReg.CarryFlag = false;
            _regs.CondReg.AuxCarryFlag = false;
            _regs.CondReg.CalcZeroFlag(answer);
            _regs.CondReg.CalcParityFlag( answer );
            _regs.CondReg.CalcSignFlag( answer );
            return answer;
        }

        private byte LogicalOR( byte a, byte b )
        {
            byte answer = ( byte ) ( a | b );
            _regs.CondReg.CarryFlag = false;
            _regs.CondReg.AuxCarryFlag = false;
            _regs.CondReg.CalcZeroFlag(answer);
            _regs.CondReg.CalcParityFlag( answer );
            _regs.CondReg.CalcSignFlag( answer );
            return answer;
        }

        private void Compare( byte a, byte b )
        {
            // This operation only alters the conditional register.
            // Comparision is calculated by subtraction.  The
            // zero flag indicates equality

            UInt16 answer = Subtract( a, b );
        }

        private void ZeroArray( ref byte[] array )
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = 0x00;
            }
        }

        private void PushBytes( byte hi, byte lo )
        {
            _memory[ (UInt16)(_regs.StackPointer - 1) ] = hi;
            _memory[ (UInt16)(_regs.StackPointer - 2) ] = lo;
            _regs.StackPointer -= 2;
        }

        public UInt16 PopRegister()
        {
            UInt16 result = i8080Registers.GetValue16(_memory[_regs.StackPointer + 1], _memory[_regs.StackPointer]);
            _regs.StackPointer += 2;
            return result;
        }

        public void PushRegister( UInt16 num )
        {
            PushBytes( i8080Registers.GetHiValue( num ), i8080Registers.GetLoValue( num ) );
        }

        public UInt16 GetImmediateAddr()
        {
            return i8080Registers.GetValue16((byte)(_memory[_regs.ProgramCounter.Value + 2]), (byte)(_memory[_regs.ProgramCounter.Value + 1]));
        }
        #endregion
    }
}
