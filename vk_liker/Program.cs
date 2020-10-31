using vk_liker.Structure;

namespace vk_liker
{
    public class Program
    {
        public static void Main()
        {
            using (var liker = new Liker(VkUserHelper.Users))
            {
                liker.ExecuteForVkUserId(440554969);
            }
        }
    }
}