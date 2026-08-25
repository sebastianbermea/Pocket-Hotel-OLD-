// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("iNYZmLwqtm8i19dYNFVWoQO/nLEr/VafvwQkJ5QFyM6Xtf0FVSt0CtdUWlVl11RfV9dUVFXMyBbIWvGNZddUd2VYU1x/0x3TolhUVFRQVVa7n/EhJvkJixzdvt8xWIRkssVmcx0pNJamh0CEycaYcLL7YdnjnWYeqHUJIp0H/85/SV/PBMmVq4E15J6Rr7kOJ65kSplRTiDwsfdI9wOzefAGS7g09FSCtoDOFSFhWi7+GmtvQiBuLToTIJtDeKsWLnrCaqWdC7/AoMmEK8NNLdUvI2g+18m3rNzOrzOavKoIyP9tCBi9obvvyxyfjO1uhSkMEcXkTfUHwWl0ZlCQgjEzF2FuWs4lW+VNquU8jgYawpksF4AMCqiFbhz9rRaqTFdWVFVU");
        private static int[] order = new int[] { 9,6,6,9,13,7,10,11,9,10,11,13,13,13,14 };
        private static int key = 85;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
