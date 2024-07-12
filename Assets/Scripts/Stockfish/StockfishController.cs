using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Chessed.Logic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Chessed
{
    public class StockfishController : MonoBehaviour
    {
        public MoveSquares BestMoveSquares { get; private set; }
        
        [FormerlySerializedAs("searchDepth")] [SerializeField] private float searchTime = 5f;
        [SerializeField] private string stockfishPath;
        [SerializeField] private GameManager manager;
        [SerializeField] private BoardDisplay boardDisplay;

        private Process process;
        private StreamWriter input;
        private StreamReader output;

        private string StateFEN => manager.GameState.StateFEN;
        private int SearchTimeMillis => (int)(searchTime * 1000f);

        private void Start()
        {
            string path = Application.dataPath + "/" + stockfishPath;

            process = new Process();
            process.StartInfo.FileName = path;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.Start();

            input = process.StandardInput;
            output = process.StandardOutput;

            SendCommand("uci");
            SendCommand("isready");
            SendCommand("ucinewgame");
            SendCommand($"position fen {StateFEN}");
            SendCommand($"go movetime {SearchTimeMillis}");

            Task.Run(ReadStockfishOutputAsync);
        }

        private void OnApplicationQuit()
        {
            if (process == null || process.HasExited) return;

            process.Kill();
            process.Dispose();
        }

        private void SendCommand(string command)
        {
            input.WriteLine(command);
            input.Flush();
        }

        private async Task ReadStockfishOutputAsync()
        {
            while (!process.HasExited)
            {
                if (manager.GameState.IsGameOver) break;
                
                string outputStr = await output.ReadLineAsync();
                if (string.IsNullOrEmpty(outputStr) || !outputStr.StartsWith("bestmove")) continue;

                string bestMoveStr = outputStr.Split(' ')[1];
                Square from = new Square(bestMoveStr[..2]);
                Square to = new Square(bestMoveStr[2..4]);

                BestMoveSquares = new MoveSquares(from, to);
            }
        }

        public void OnMove(MoveSquares moveSquares)
        {
            SendCommand($"position fen {StateFEN} moves {moveSquares.From.Algebraic}{moveSquares.To.Algebraic}");
            SendCommand($"go movetime {SearchTimeMillis}");
        }
    }
}
