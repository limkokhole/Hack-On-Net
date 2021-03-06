﻿using HackLinks_Server.Daemons.Types.Mission.Goals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackLinks_Server.Daemons.Types.Mission
{
    class MissionListing
    {
        public int id;
        public string missionName;
        public string description;
        public int requiredRanking;
        public Difficulty difficulty;
        public Status status;
        public string employer;
        public string claimedBy;
        public string startDescription;
        public List<MissionGoal> goals;
        public List<int> completedGoals;

        public MissionListing(int id, string missionName, string description, int requiredRanking, Difficulty difficulty, Status status, string employer, string claimedBy, string startDescription, List<MissionGoal> goals, List<int> completedGoals)
        {
            this.id = id;
            this.missionName = missionName;
            this.description = description;
            this.requiredRanking = requiredRanking;
            this.difficulty = difficulty;
            this.status = status;
            this.employer = employer;
            this.claimedBy = claimedBy;
            this.startDescription = startDescription;
            this.goals = goals;
            this.completedGoals = completedGoals;
        }

        public enum Status
        {
            Unpublished,
            Unclaimed,
            InProgress,
            Complete,
            Failed
        }

        public enum Difficulty
        {
            Beginner,
            Basic,
            Intermediate,
            Advanced,
            Expert,
            Extreme,
            Impossible
        }
    }
}
