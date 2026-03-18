namespace knn;

public class EvaluationMetrics
{
    public double MeasureAccuracy(List<string> realLabels, List<string> predictedLabels)
    {
        if (realLabels.Count == 0) return 0;

        int correct = 0;
        for (int i = 0; i < realLabels.Count; i++)
        {
            if (realLabels[i] == predictedLabels[i])
            {
                correct++;
            }
        }

        return (double)correct / realLabels.Count;
    }
}