using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeLink.Models;

namespace TimeLink.Services
{
    public class T_GOALservice
    {
        public static IQueryable<T_GOAL> GetGoalsByEmail(MyDataModel db, string email)
        {
            return db.T_GOAL.Where(n => n.Email == email);
        }

        public static int GetMaxTasksCount(IQueryable<T_GOAL> goals) {
            return goals.Select(n => n.T_TASK.Count()).Max();
        }

        public static IQueryable<T_GOAL> GetGoalsByFilters(IQueryable<T_GOAL> goals, string goalName, int minTasksCount, int maxTasksCount) {
            return goals.Where(n => n.GoalName.Contains(goalName))
                    .Where(n => n.T_TASK.Count >= minTasksCount)
                    .Where(n => n.T_TASK.Count <= maxTasksCount);
        }

        public static IQueryable<T_GOAL> GetGoalsByFilters(IQueryable<T_GOAL> goals, string goalName, int minTasksCount, int maxTasksCount, bool isDone)
        {
            return GetGoalsByFilters(goals, goalName, minTasksCount, maxTasksCount)
                .Where(n => n.Done == isDone);
        }

        public static T_GOAL AddNewGoal(MyDataModel db, string email, T_ACCOUNT account ) {
            T_GOAL newGoal = new T_GOAL { Email = email, T_ACCOUNT = account, Done = false, GoalName = "New Goal", CreateDate = DateTime.Now };
            db.T_GOAL.Add(newGoal);
            db.SaveChanges();
            return newGoal;
        }

        public static T_GOAL GetGoalByID(MyDataModel db, int goalID) {
            return db.T_GOAL.FirstOrDefault(n => n.GoalID == goalID);
        }

    }
}