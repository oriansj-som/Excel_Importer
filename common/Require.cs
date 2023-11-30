namespace common
{
    public static class Require
    {
        public static void True(bool t, string? message)
        {
            if (!t)
            {
                if (null == message) throw new Exception("unknown error has occurred");
                else throw new Exception(message);
            }
        }

        public static void False(bool t, string? message)
        {
            if (t)
            {
                if (null == message) throw new Exception("unknown error has occurred");
                else throw new Exception(message);
            }
        }
    }
}
