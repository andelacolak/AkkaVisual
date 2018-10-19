using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zdk01
{
    class ChildActor : ReceiveActor
    {
        public ChildActor()
        {
            Receive<SumRows>(x => SumRows(x.Matrix));
            Receive<SumColumns>(x => SumColumns(x.Matrix));
            Receive<SumDiagonals>(x => SumDiagonals(x.Matrix));
        }

        private void SumRows(int[][] matrix)
        {
            List<int> rowSums = new List<int>();
            for (int i = 0; i < matrix.Length; i++)
            {
                int rowSum = 0;
                for (int j = 0; j < matrix.Length; j++)
                {
                    rowSum += matrix[i][j];
                }
                rowSums.Add(rowSum);
            }
            Sender.Tell(new JobDone(rowSums));
        }

        private void SumColumns(int[][] matrix)
        {
            List<int> columnsSums = new List<int>();
            for (int j = 0; j < matrix.Length; j++)
            {
                int columnSum = 0;
                for (int i = 0; i < matrix.Length; i++)
                {
                    columnSum += matrix[i][j];
                }
                columnsSums.Add(columnSum);
            }
            Sender.Tell(new JobDone(columnsSums));
        }

        private void SumDiagonals(int[][] matrix)
        {
            //sredi ovo kasnije
            List<int> diagonalsSums = new List<int>();
            int diagonalSum = 0;
            for (int j = 0; j < matrix.Length; j++)
            {
                diagonalSum += matrix[j][j];
            }
            diagonalsSums.Add(diagonalSum);

            diagonalSum = 0;
            for (int j = matrix.Length - 1; j >= 0; j--)
            {
                diagonalSum += matrix[matrix.Length -1 - j][j];
            }
            diagonalsSums.Add(diagonalSum);

            Sender.Tell(new JobDone(diagonalsSums));
        }
    }

    
}
