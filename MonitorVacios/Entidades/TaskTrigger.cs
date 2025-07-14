using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace MonitorVacios.Entidades
{
    internal class Tasker
    {
        public Int64? taskUniqueID { get; set; }
        public string taskName { get; set; }
        public Task taskPrograming { get; set; }
        public CancellationTokenSource fuente { get; set; }
        public System.Timers.Timer time { get; set; }
        public string status { get; set; }
        public Tasker(Int64 uk, string name, Task task, CancellationTokenSource fu, System.Timers.Timer tim)
        {
            this.taskUniqueID = uk;
            this.taskName = name;
            this.taskPrograming = task;
            this.fuente = fu;
            this.time = tim;
        }
        public Tasker(Int64 uk, string name, Task task, CancellationTokenSource fu, System.Timers.Timer tim, string st)
        {
            this.taskUniqueID = uk;
            this.taskName = name;
            this.taskPrograming = task;
            this.fuente = fu;
            this.time = tim;
            this.status = st;
        }
    }

    public class TaskTrigger
    {
        //voy guardando la lista de tareas que voy creando.
        //para tener un inventario
        private static Dictionary<Int64, Tasker> TaskInventory;

        private static string GetAllTask()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var t in TaskInventory)
            {
                var te = t.Value.taskPrograming != null;
                var ti = t.Value.time != null;
                sb.AppendLine(string.Format("ID:{0} |NAME:{1} |STATUS:{2} |TIMER:{3};", t.Key, t.Value.taskName, t.Value.status, ti ? t.Value.time.Enabled : false));
            }
            if (sb.Length <= 0)
            {
                sb.AppendLine(string.Format("{0}- No existen tareas en inventario", DateTime.Now.ToString("dd/MM/yyyy HH:mm")));
            }
            return sb.ToString();
        }

        public static void CheckInventory(IProgress<string> reporter, CancellationTokenSource fuente, CancellationToken token)
        {
            ParallelOptions parOpts = new ParallelOptions();
            parOpts.CancellationToken = token;
            parOpts.MaxDegreeOfParallelism = 1;
            TaskCreationOptions tco = new TaskCreationOptions();
            tco = TaskCreationOptions.PreferFairness;
            Task task = null;
            System.Timers.Timer timer = new System.Timers.Timer(5000);
            timer.Elapsed += (source, e) =>
            {
                task = Task.Factory.StartNew(() =>
                {

                    if (fuente.IsCancellationRequested)
                    {
                        //me aseguro que exista
                        if (timer != null)
                        {
                            timer.Enabled = false;
                            timer.Stop();
                            timer.Dispose();
                        }
                    }
                    else
                    {
                        reporter.Report(GetAllTask());
                    }
                }, parOpts.CancellationToken, tco, parOpts.TaskScheduler);
                task.ContinueWith((a) => { a.Dispose(); });
            };
            timer.Enabled = true;
            timer.Start();
        }


        public static void TaskLiteAlter(int tiempo, Action<Progress<string>, int, string> metodo, Progress<string> reporter, int maxreg, string cod, CancellationTokenSource fuente, CancellationToken token, string metodoNm)
        {
            ParallelOptions parOpts = new ParallelOptions();
            parOpts.CancellationToken = token;
            parOpts.MaxDegreeOfParallelism = 1;
            parOpts.TaskScheduler = TaskScheduler.Current;
            TaskCreationOptions tco = new TaskCreationOptions();
            tco = TaskCreationOptions.PreferFairness;
            Task task = null;
            if (TaskInventory == null)
            {
                TaskInventory = new Dictionary<long, Tasker>();
            }
            task = Task.Factory.StartNew(() =>
            {
                System.Timers.Timer timer = new System.Timers.Timer(tiempo * 1000);
                timer.Enabled = true;
                timer.Start();
                timer.Elapsed += (source, e) =>
                {
                    if (fuente.IsCancellationRequested)
                    {
                        if (timer != null)
                        {
                            timer.Enabled = false;
                            timer.Stop();
                            timer.Dispose();
                            //actualice inventatrio
                            var tk = TaskInventory.Where(g => g.Key == task.Id).FirstOrDefault();
                            if (tk.Key != 0) { tk.Value.status = "Canceled"; }
                        }
                        task.Dispose();
                    }
                    else
                    {
                        metodo(reporter, maxreg, cod);
                        TaskInventory.Add(task.Id, new Tasker(task.Id, metodoNm, task, fuente, timer, "Started"));
                    }

                };

                if (task.IsCompleted)
                {
                    var tk = TaskInventory.Where(g => g.Key == task.Id).FirstOrDefault();
                    if (tk.Key != 0) { tk.Value.status = "Completed"; }
                }

            }, parOpts.CancellationToken, tco, parOpts.TaskScheduler);

            task.ContinueWith((a) =>
            {
                a.Dispose();
            });

        }

    }
}
