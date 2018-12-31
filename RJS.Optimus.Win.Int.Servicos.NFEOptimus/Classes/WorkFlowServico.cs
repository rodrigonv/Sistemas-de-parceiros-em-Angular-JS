using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Diagnostics;

namespace RJS.Optimus.Win.Int.Servicos.NFEOptimus.Classes
{
    public class WorkFlowServico
    {
        public Stopwatch time { get; set; }

        private StringBuilder eventos { get; set; }

        private string NomeClasse { get; set; }

        public WorkFlowServico(string NomeClasse)
        {
            time = new Stopwatch();
            time.Start();
            eventos = new StringBuilder();
            this.NomeClasse = NomeClasse;

        }

        public void AddEvento(string nomeEvento)
        {
            eventos.AppendLine(string.Format("[{0}] {1} DATA:{2} TEMPO(ms):{3}", NomeClasse, nomeEvento, DateTime.Now, time.Elapsed.TotalSeconds));
        }

        public string FinishWorkFlow()
        {
            time.Stop();
            eventos.AppendLine(string.Format("[{0}] {1} DATA:{2} TEMPO TOTAL(ms):{3}", NomeClasse, "PROCESSO FINALIZADO", DateTime.Now, time.Elapsed.TotalSeconds));
            return eventos.ToString();
        }
    }
}
