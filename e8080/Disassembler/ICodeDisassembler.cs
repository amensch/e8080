using KDS.Loader;

namespace KDS.e8080
{
    interface ICodeDisassembler
    {
        string Disassemble(ICodeLoader loader);
    }
}
