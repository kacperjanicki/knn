
using System.Globalization;

namespace knn
{
    public class Neighbour
    {
        public double Distance { get; set; }
        public string Label { get; set; }

        public Neighbour(double distance, string label)
        {
            Distance = distance;
            Label = label;
        }
    }
    
    public class KNearestNeighbours
    {
        private int k;
        private List<IrisObservation> trainDataset;

        public KNearestNeighbours(int k, List<IrisObservation> trainDataset)
        {
            this.k = k;
            this.trainDataset = trainDataset;
        }
        public double EuclideanDistance(double[] a, double[] b)
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

        public List<Neighbour> SortDistances(List<Neighbour> neighbours)
        {
            int n = neighbours.Count;

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                    if (neighbours[j].Distance > neighbours[j + 1].Distance)
                    {
                        (neighbours[j], neighbours[j+1]) = (neighbours[j+1], neighbours[j]);
                    }
                }
            }
            return neighbours;
        }

        public string FindPredictedClass(List<Neighbour> sortedNeighbours)
        {
            List<Neighbour> kNearest = new List<Neighbour>();
            for (int i = 0; i < k; i++)
            {
                kNearest.Add(sortedNeighbours[i]);
            }

            return FindMode(kNearest);
        }

        private string FindMode(List<Neighbour> kNearest)
        {
            Dictionary<string, int> counts = new Dictionary<string, int>();

            foreach (var neighbour in kNearest)
            {
                if (counts.ContainsKey(neighbour.Label))
                {
                    counts[neighbour.Label]++;
                }
                else
                {
                    counts[neighbour.Label] = 1;
                }
            }

            // szukamy który gatunek ma największy count
            int max = 0;
            foreach (var count in counts.Values)
            {
                if (count > max) max = count;
            }
            // tworzymy listę gatunków o max count
            List<string> bestCandidates = new List<string>();
            foreach (var candidate in counts)
            {
                if(candidate.Value == max) bestCandidates.Add(candidate.Key);
            }
            // jezeli jest wiecej niz jeden gatunek o max count to zwracamy losowy zgodnie z opisem zadania
            if (bestCandidates.Count > 1)
            {
                Random rd = new Random();
                return bestCandidates[rd.Next(bestCandidates.Count)];
            }

            return bestCandidates[0];
        }

        public string classify(double[] vector)
        {
            List<Neighbour> neighbors = new List<Neighbour>();
            foreach (var obs in trainDataset)
            {
                double dist = EuclideanDistance(vector, obs.Features);
                neighbors.Add(new Neighbour(dist, obs.Label));
            }

            SortDistances(neighbors);
            return FindPredictedClass(neighbors);
        }
    }

    class Program
    {
        public static void RunUI(KNearestNeighbours knn)
        {
            while (true)
            {
                Console.WriteLine("Enter 4 features of your flower, space separated ('q' to quit)");
                string input = Console.ReadLine();

                if (input?.ToLower() == "q") break;

                try
                {
                    string[] inputData = input.Split(' ');
                    if (inputData.Length != 4)
                    {
                        Console.WriteLine("Error: you need to specify exactly 4 features");
                        continue;
                    }

                    double[] features = new double[4];
                    for (int i = 0; i < 4; i++)
                    {
                        features[i] = double.Parse(inputData[i].Replace(',', '.'), CultureInfo.InvariantCulture);
                    }

                    string prediction = knn.classify(features);
                    Console.WriteLine($"Expected species is: {prediction.ToUpper()}");
                }
                catch (FormatException ex)
                {
                    Console.WriteLine($"Error: incorrect data format. {ex.Message} Input should look like: 1.2 4.2 6.2 7.1");
                }
            }
        }
        static void Main(string[] args)
        {
            PrepareDataset dataset = new PrepareDataset();
            List<IrisObservation> observations = dataset.LoadCsv("iris.csv");
            (List<IrisObservation> train, List<IrisObservation> test) = dataset.trainTestSplit(observations);
            
            KNearestNeighbours knn = new KNearestNeighbours(7, train);
            
            
            RunUI(knn);
            // EvaluationMetrics metrics = new EvaluationMetrics();
            // for (int currentK = 1; currentK <= 15; currentK += 2)
            // {
            //     KNearestNeighbours knnModel = new KNearestNeighbours(currentK, train);
            //     List<string> predictions = new List<string>();
            //     List<string> realValues = new List<string>();
            //
            //     foreach (var testObs in test)
            //     {
            //         string result = knnModel.classify(testObs.Features);
            //         predictions.Add(result);
            //         realValues.Add(testObs.Label);
            //     }
            //
            //     double acc = metrics.MeasureAccuracy(realValues, predictions);
            //     Console.WriteLine($"Dokladnosc dla k={currentK}: {acc:P2}");
            // }

   

        }
    }
}







