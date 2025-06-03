namespace Компилятор
{
    public struct TextPosition
    {
        public uint LineNumber { get; set; }
        public byte CharNumber { get; set; }

        public TextPosition(uint ln = 0, byte c = 0)
        {
            LineNumber = ln;
            CharNumber = c;
        }
    }
}