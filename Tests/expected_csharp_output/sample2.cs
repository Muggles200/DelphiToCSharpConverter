using System.Collections;

class Program
{
    static void Main()
    {
        ArrayList list = new ArrayList();
        try
        {
            list.Add(null);
        }
        finally
        {
            // In C#, ArrayList does not have a Free method. This is implicit in the .NET garbage collection.
        }
    }
}
