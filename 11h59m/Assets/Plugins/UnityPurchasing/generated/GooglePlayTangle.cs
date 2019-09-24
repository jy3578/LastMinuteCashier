#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("Y4HRj5ZRkMRxPE5gXnxWuWEvPr06iAsoOgcMAyCMQoz9BwsLCw8KCUPrYzCO/9Z7ZX21jOYGx4ZWl6RzZ5R1M87umTemTCizwyxFMmUYcDhIILqD5XHD2Gswq5dPwbs+5CcRpvpWsvvqTrPvFX0HmicJBJOLxb3M0/Yj3RicPv3PPgEwPWqgEGT1/gXFvhC6pGqXJcScqmTisxj+0LVf1JDgZuoi6R+PecDJuxZuuodNJXq3Yt8dDB2v2lVCMHHJC9PtnEB9iAYPzwHujcmgEfVE+oYW3iTKQw7SQYgLBQo6iAsACIgLCwqfo+rAFRRqnedq0mz6Ivv9DkF01mAdxAZnJ0TkUmnk0uZRbE8tBX2Sd0O6cBsxTO89W1ZxjqwnDQgJCwoL");
        private static int[] order = new int[] { 9,9,4,10,10,10,10,7,13,12,11,12,13,13,14 };
        private static int key = 10;

        public static byte[] Data() {
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
