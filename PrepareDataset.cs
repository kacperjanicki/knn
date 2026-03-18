namespace knn;

public class IrisObservation
{
    public double[] Features { get; set; }
    public string Label { get; set; }

    public IrisObservation(double[] features, string label)
    {
        Features = features;
        Label = label;
    }
    
    public override string ToString()
    {
        return $"[{string.Join(", ", Features)}] -> {Label}";
    }
}

public class PrepareDataset
{
    public List<IrisObservation> LoadCsv(string path)
    {
        List<IrisObservation> observations = new List<IrisObservation>();
        string[] lines = File.ReadAllLines(path);

        // od i = 1, bo i = 0 to nagłówki
        for (int i = 1; i < lines.Length; i++)
        {
            string[] splitted = lines[i].Split(',');

            double[] features = new double[4];
            for (int j = 0; j < 4; j++)
            {
                features[j] = double.Parse(splitted[j]);
            }

            string label = splitted[4].Trim();
            observations.Add(new IrisObservation(features, label));
        }

        return observations;
    }

    public (List<IrisObservation> train, List<IrisObservation> test) trainTestSplit(List<IrisObservation> dataset)
    {
        List<IrisObservation> train = new List<IrisObservation>();
        List<IrisObservation> test = new List<IrisObservation>();

        Dictionary<string, List<IrisObservation>> grouped = new Dictionary<string, List<IrisObservation>>();
        foreach (var obs in dataset)
        {
            if (!grouped.ContainsKey(obs.Label))
            {
                grouped[obs.Label] = new List<IrisObservation>();
            }
            grouped[obs.Label].Add(obs);
        }
        /*
         * OTRZYMAMY SŁOWNIK:
         *  {
         *      setosa: [obs1, obs2, obs3, ... , obs50],
         *      versicolor: [obs1, obs2, obs3, ... , obs50],
         *      virginica: [obs1, obs2, obs3, ... , obs50],
         */
        foreach (var label in grouped.Keys)
        {
            List<IrisObservation> species = grouped[label];
            int splitBoundary = (int)(species.Count * 0.66);
            // z kazdego gatunku pierwsze 66% obserwacji zostanie dodane do train, a ostatnie 33% do test
            
            for (int i = 0; i < species.Count; i++)
            {
                if (i < splitBoundary)
                {
                    train.Add(species[i]);
                }
                else
                {
                    test.Add(species[i]);
                }
            }
        }
        // foreach (var VARIABLE in test)
        // {
        //     Console.WriteLine(VARIABLE);
        // }
        return (train, test);
    }
}