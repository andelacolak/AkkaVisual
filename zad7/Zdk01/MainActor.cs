using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Zdk01
{
    class MainActor : ReceiveActor
    {
        private List<Task<JobDone>> tasks = new List<Task<JobDone>>();

        public MainActor(Label label)
        {
            Receive<CreateChildren>(x => 
            {
                CreateRowChild(x.Matrix);
                CreateColumnChild(x.Matrix);
                CreateDiagonalChild(x.Matrix);
            });
            Receive<JobDone[]>(x => JobDone(x));
            Receive<Print>(x => Print(x.isMagic, label));
        }

        private void CreateRowChild(int[][] matrix)
        {
            Props actorProps = Props.Create(() => new ChildActor());
            IActorRef child = Program.ActSystem.ActorOf(actorProps);
            tasks.Add(child.Ask<JobDone>(new SumRows(matrix)));
            WaitForAnswer();
        }

        private void CreateColumnChild(int[][] matrix)
        {
            Props actorProps = Props.Create(() => new ChildActor());
            IActorRef child = Program.ActSystem.ActorOf(actorProps);
            tasks.Add(child.Ask<JobDone>(new SumColumns(matrix)));
            WaitForAnswer();
        }

        private void CreateDiagonalChild(int[][] matrix)
        {
            Props actorProps = Props.Create(() => new ChildActor());
            IActorRef child = Program.ActSystem.ActorOf(actorProps);
            tasks.Add(child.Ask<JobDone>(new SumDiagonals(matrix)));
            WaitForAnswer();
        }

        private void WaitForAnswer()
        {
            Task.WhenAll(tasks).PipeTo(Self, Self);
        }

        private void JobDone(object x)
        {
            List<int> list = new List<int>();
            foreach (Task<JobDone> task in tasks)
            {
                foreach(int number in task.Result.Results)
                {
                    list.Add(number);
                }
            }
            Self.Tell(new Print(IsMagical(list)));
        }

        private bool IsMagical(List<int> list)
        {
            for (int i = 1; i < list.Count(); i++)
            {
                if (list[i] != list[0])
                {
                    return false;
                }
            }
            return true;
        }

        private void Print(bool result, Label label)
        {
            label.Text = result == true ? "da" : "ne";
        }
    }
}
