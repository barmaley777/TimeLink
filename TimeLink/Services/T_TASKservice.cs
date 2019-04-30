using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeLink.Models;

namespace TimeLink.Services
{
    public class T_TASKservice
    {
        public static IQueryable<T_TASK> GetTasksByGoalID(MyDataModel db, int goalID)
        {
            return db.T_TASK.Where(n => n.GoalID == goalID);
        }

        public static T_TASK GetTaskByGoalID(MyDataModel db, int goalID)
        {
            return db.T_TASK.FirstOrDefault(n => n.GoalID == goalID);
        }

        public static IQueryable<T_TASK> GetTasksByFilters(IQueryable<T_TASK> tasks, string taskName)
        {
            return tasks.Where(n => n.TaskName.Contains(taskName));
        }

        public static IQueryable<T_TASK> GetTasksByFilters(IQueryable<T_TASK> tasks, string taskName, bool isDone)
        {
            return GetTasksByFilters(tasks, taskName).Where(n => n.Done == isDone);
        }

        public static T_TASK GetTaskByID(MyDataModel db, int taskID)
        {
            return db.T_TASK.FirstOrDefault(n => n.TaskID == taskID);
        }

        public static void RemoveTaskByID(MyDataModel db, int taskID)
        {
            T_TASK taskForDelete = GetTaskByID(db, taskID);
            db.T_TASK.Remove(taskForDelete);
            db.SaveChanges();
        }

        public static IQueryable<T_TASK> GetIncompliteTasks(IQueryable<T_TASK> tasks, bool isDone) {
            return tasks.Where(t => t.Done == false);
        }

        public static bool IsTaskExists(MyDataModel db, int taskID) {
            return db.T_TASK.Any(n => n.TaskID == taskID);
        }
    }
}