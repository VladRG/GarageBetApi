﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageBet.Api.Models
{
    public class MatchModel
    {
        public long Id { get; set; }

        public int HomeScore { get; set; }

        public int AwayScore { get; set; }

        public string HomeTeamName { get; set; }

        public string AwayTeamName { get; set; }

        public string ChampionshipName { get; set; }

        public string CompetitiveYear { get; set; }

        public DateTime DateTime { get; set; }
    }
}
