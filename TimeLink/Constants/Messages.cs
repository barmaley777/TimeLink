using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeLink.Constants
{
    static public class Messages
    {
        public const string goalName = "The goal name: ";
        public const string errorMissingGoalID = "impossible to get goal (missing goal ID)";
        public const string errorIncorrectGoalID = "impossible to find goal in database (incorrect GoalID)";
        public const string errorMailExists ="email already exists";
    }
}