using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using vk_liker.DTO;

namespace vk_liker.Structure
{
    static class VkUserHelper
    {
        public static VkUserInfo[] Users
        {
            get
            {
                var users = new Collection<VkUserInfo>();

                foreach (var fileName in Directory.GetFiles("UserInfos"))
                {
                    var lines = File.ReadAllLines(fileName);
                    if (lines != null && lines.Count() > 0)
                    {
                        foreach (var line in lines)
                        {
                            var arr = line.Split(':');
                            users.Add(new VkUserInfo
                            {
                                Login = arr[0],
                                Pass = arr[1]
                            });
                        }
                    }
                }

                return users.ToArray();
            }
        }
    }
}