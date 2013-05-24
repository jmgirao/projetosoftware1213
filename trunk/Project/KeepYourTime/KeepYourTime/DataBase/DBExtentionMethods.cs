
namespace KeepYourTime.DataBase
{
    /// <summary>
    /// Extention Methods to use with Database
    /// </summary>
    /// <remarks>CREATED BY Rui Ganhoto</remarks>
    public static class DBExtentionMethods
    {
        /// <summary>
        /// Converts a Boolean to DataBase Query
        /// </summary>
        /// <param name="BooleanValue">Boolean to be converted.</param>
        /// <returns>if <c>true</c> returns "1" else returns "0"</returns>
        public static string ToDB(this bool BooleanValue)
        {
            return BooleanValue ? "1" : "0";
        }
    }

}
