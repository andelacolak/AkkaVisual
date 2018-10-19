using Akka.Actor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Zdk01
{
    public partial class Form1 : Form
    {
        IActorRef mainActor;
        public Form1()
        {
            #region postavljanje komponenti sucelja - mora biti prvo
            InitializeComponent();
            CreateTextBoxes();
            #endregion

            Props actorProps = Props.Create(() => new MainActor(lblRezultat))
                .WithDispatcher("akka.actor.synchronized-dispatcher");
            mainActor = Program.ActSystem.ActorOf(actorProps);

        }

        #region programski kod za stvaranje textboxova i popunjavanje istih ispravno rijesenim sudokom
        private void IspuniTextBoxe(List<int> vrijednosti)
        {
            int i = 0;

            foreach (var control in gb.Controls)
            {
                TextBox txt = (TextBox)control;

                txt.Text = vrijednosti[i].ToString();

                i++;
            }
        }

        private int[][] DobaviVrijednostiIzTextboxa()
        {
            int[][] rezultat = new int[9][];

            for (int z = 0; z < 9; z++)
            {
                rezultat[z] = new int[9];
            }


            int i = 0;
            int j = 0;

            foreach (var control in gb.Controls)
            {
                TextBox txt = (TextBox)control;
                if (txt.Text != "")
                {
                    rezultat[i][j] = int.Parse(txt.Text);

                    j++;

                    if (j % 9 == 0)
                    {
                        j = 0;
                        i++;
                    }
                }
            }

            return rezultat;
        }

        private void btnIspuni_Click(object sender, EventArgs e)
        {
            var vrijednosti = new List<int>
            {
                5, 3, 4, 6, 7, 8, 9, 1, 2,
                6, 7, 2, 1, 9, 5, 3, 4, 8,
                1, 9, 8, 3, 4, 2, 5, 6, 7,
                8, 5, 9, 7, 6, 1, 4, 2, 3,
                4, 2, 6, 8, 5, 3, 7, 9, 1,
                7, 1, 3, 9, 2, 4, 8, 5, 6,
                9, 6, 1, 5, 3, 7, 2, 8, 4,
                2, 8, 7, 4, 1, 9, 6, 3, 5,
                3, 4, 5, 2, 8, 6, 1, 7, 9
            };
            IspuniTextBoxe(vrijednosti);
        }
        #endregion

        private void btnProvjeriIspravnost_Click(object sender, EventArgs e)
        {
            mainActor.Tell(new CreateChildren(DobaviVrijednostiIzTextboxa()));
        }
    }
}
