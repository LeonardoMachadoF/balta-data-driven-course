using System.Text;

namespace DataDriven;
public static class Settings
{
    private static string Secret = "8435aa81-46d5-43a9-be4b-21801e978645";

    public static byte[] GetKeyInBytes() => Encoding.ASCII.GetBytes(Secret);

}