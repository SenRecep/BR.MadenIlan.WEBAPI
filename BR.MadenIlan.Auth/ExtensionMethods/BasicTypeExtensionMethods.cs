namespace BR.MadenIlan.Auth.ExtensionMethods
{
    public static class BasicTypeExtensionMethods
    {
        public static bool IsEmpty(this string str)=> string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str);
        public static bool IsNull(this object obj) => obj is null;
        public static bool IsNotNull(this object obj) => obj != null;
        public static T Cast<T>(this object obj) => (T)obj;
        public static bool IsTrue(this bool con) => con;
        public static bool IsFalse(this bool con) => !con;
    }
}
