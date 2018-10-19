using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zdk01
{
    class CreateChildren
    {
        public int[][] Matrix { get; private set; }

        public CreateChildren(int[][] matrix)
        {
            Matrix = matrix;
        }
    }

    class JobDone 
    {
        public List<int> Results;

        public JobDone(List<int> results)
        {
            Results = results;
        }
    }

    class Sums
    {
        public int[][] Matrix;

        protected Sums(int[][] matrix)
        {
            Matrix = matrix;
        }
    }

    class SumRows : Sums
    {
        public SumRows(int[][] matrix) : base(matrix) { }
    }

    class SumColumns : Sums
    {
        public SumColumns(int[][] matrix) : base(matrix) { }
    }

    class SumDiagonals : Sums
    {
        public SumDiagonals(int[][] matrix) : base(matrix) { }
    }

    class Print
    {
        public bool isMagic { get; private set; }

        public Print( bool magic)
        {
            isMagic = magic;
        }
    }
}
