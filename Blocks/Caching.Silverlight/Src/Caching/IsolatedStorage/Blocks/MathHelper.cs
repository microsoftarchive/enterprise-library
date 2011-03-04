namespace Microsoft.Practices.EnterpriseLibrary.Caching.IsolatedStorage
{
    internal static class MathHelper
    {
        public static int SafeDivision(long x, long y)
        {
            long quotient = x / y;

            long retVal = (x % y) == 0 ? quotient : quotient + 1;
            return checked((int)retVal);
        }
    }
}
