CREATE OR ALTER 
VIEW MatchBetsView
AS (
	
	SELECT 
		bet.Id as BetId,
		bet.HomeScore as HomeScoreBet,
		bet.AwayScore as AwayScoreBet,
		home_team.Name as HomeTeamName,
		away_team.Name as AwayTeamName,
		match.HomeScore as HomeScore,
		match.AwayScore as AwayScore,
		match.DateTime as MatchDateTime,
		bet.userId as userId
	FROM
		Bets as bet INNER JOIN
		Matches as match 
			ON (bet.MatchId = match.Id)
			INNER JOIN
		Teams as home_team
			ON (match.HomeTeamId = home_team.Id)
			INNER JOIN
		Teams as away_team
			ON (match.AwayTeamId = away_team.Id)
);