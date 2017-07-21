using BenchmarkDotNet.Attributes;

namespace FillWordGameSolver.Performance
{
    [Config(typeof(BaseBenchmarkConfig))]
    public class GameSolverBenchmark
    {
        public void GlobalSetup()
        {
        }
    }
}
