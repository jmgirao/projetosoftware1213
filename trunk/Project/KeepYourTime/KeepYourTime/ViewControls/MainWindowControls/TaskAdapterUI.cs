using KeepYourTime.DataBase.Adapters;
using KeepYourTime.DataBase.Connectors;
using KeepYourTime.ViewWindows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeepYourTime.ViewControls.MainWindowControls
{
    class TaskAdapterUI : TaskAdapter
    {
        public string TotalTime { get { return "XXX:XX"; } }
        public string TodayTime { get { return "XX:XX"; } }
        public string StopDate { get { return "12/41/1231"; } }

        public bool ActiveChange
        {
            get
            {
                return this.Active;
            }
            set
            {
                var mhResult = new MethodHandler();
                try
                {
                    mhResult = TaskConnector.ActivateTask(this.TaskId, value);
                    if (mhResult.Exits) return;
                    this.Active = value;

                    if (this.Active == false)
                        if (OnTaskDeactivated != null)
                            OnTaskDeactivated((object)this, new EventArgs());
                    
                }
                catch (Exception ex)
                {
                    mhResult.Exception(ex);
                }
                finally
                {
                    MessageWindow.ShowMethodHandler(mhResult, false);
                }
            }
        }

        public TaskAdapterUI()
        {


        }

        public TaskAdapterUI(TaskAdapter taskBase)
        {
            this.TaskId = taskBase.TaskId;
            this.TaskName = taskBase.TaskName;
            this.Active = taskBase.Active;
            this.Description = taskBase.Description;
        }

        public event EventHandler OnTaskDeactivated;
    }
}
