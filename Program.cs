Random rd = new Random();

static double EuclideanDistance(double[] a, double[] b)
{
    if (a.Length != b.Length)
    {
        throw new Exception("Vectors must have equal dimensions");
    }

    double sum = 0;
    for (int i = 0; i < a.Length; i++)
    {
        double diff = a[i] - b[i];
        sum += (diff * diff);
    }
    
    return Math.Sqrt(sum);
}

Console.WriteLine();

double[] jed = new double[] { 2.4, 2.1, 6.6 };
double[] dwa = new double[] { 4.2, 1.2, 5.5 };


Console.WriteLine(EuclideanDistance(jed,dwa));