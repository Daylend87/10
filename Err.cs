namespace Компилятор
{
    public struct Err
    {
        public TextPosition ErrorPosition { get; set; }
        public byte ErrorCode { get; set; }

        public Err(TextPosition errorPosition, byte errorCode)
        {
            this.ErrorPosition = errorPosition;
            this.ErrorCode = errorCode;
        }
    }
}
