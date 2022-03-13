using System;

public class Program
{
    public static void Main()
    {
        Console.WriteLine(Tribonacci(new double[] { 2, 7, 14 }, 1));
    }
    public static double[] Tribonacci(double[] signature, int n)
    {
        // hackonacci me

        //for (int i = 0; i < n; i++)
        //{
        //    var length = signature.Length;
        //    var sum = signature[length - 1] + signature[length - 2] + signature[length - 3];

        //    var newSignature = new double[length + 1];

        //    for (int y = 0; y < signature.Length; y++)
        //    {
        //        newSignature[y] = signature[y];
        //    }

        //    newSignature[length] = sum;

        //    signature = newSignature;
        //}

        //var tribonacci = new double[signature.Length - 3];

        //for (int i = signature.Length ; i > 0; i--)
        //{
        //    tribonacci[i - 4] = signature[i];
        //}

        //return n > 0 ? signature : tribonacci;


        // hackonacci me
        var tribonacci = new List<double>(signature);

        for (int i = 0; i < n - signature.Length; i++)
        {
            var sum = tribonacci.TakeLast(3).Sum();
            tribonacci.Add(sum);
        }

        if (n < 3)
        {
            var a = tribonacci.GetRange(0, n);
            return a.ToArray();
        }

        return n > 0 ? tribonacci.ToArray() : new double[1];
    }
}
