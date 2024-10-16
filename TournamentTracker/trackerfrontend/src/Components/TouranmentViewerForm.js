import React, { useState } from 'react';
import '../Css/TournamentViewerForm.css'; // Import CSS file

const TournamentViewerForm = () => {
  const [round, setRound] = useState('');
  const [unplayedOnly, setUnplayedOnly] = useState(false);
  const [teamOneScore, setTeamOneScore] = useState('');
  const [teamTwoScore, setTeamTwoScore] = useState('');

  // Handle the selection of a round
  const handleRoundChange = (e) => {
    setRound(e.target.value);
    console.log('Round selected:', e.target.value);
  };

  // Handle the unplayed-only checkbox toggle
  const handleUnplayedOnlyChange = () => {
    setUnplayedOnly(!unplayedOnly);
    console.log('Unplayed only:', !unplayedOnly);
  };

  // Handle scoring
  const handleScoreSubmit = () => {
    if (!teamOneScore || !teamTwoScore) {
      alert('Please enter scores for both teams.');
      return;
    }
    console.log('Scores submitted:', { teamOneScore, teamTwoScore });
    // Logic for submitting scores goes here
  };

  return (
    <div className="viewer-form-container">
      <h1 className="tracker-heading">Tournament Tracker</h1>
      <h2 className="form-heading">Tournament Viewer</h2>
      
      <label className="tournament-name">Tournament Name</label>

      <div className="form-group">
        <label htmlFor="round">Round</label>
        <select id="round" value={round} onChange={handleRoundChange} className="form-input">
          <option value="">Select Round</option>
          <option value="Round 1">Round 1</option>
          <option value="Round 2">Round 2</option>
          <option value="Round 3">Round 3</option>
        </select>
      </div>

      <div className="form-group-checkbox">
        <input
          type="checkbox"
          id="unplayedOnly"
          checked={unplayedOnly}
          onChange={handleUnplayedOnlyChange}
        />
        <label htmlFor="unplayedOnly">Unplayed Only</label>
      </div>

      <div className="matchup-list">
        <h3>Matchups</h3>
        <ul>
          <li>
            <span>Team One vs Team Two</span>
            <div className="matchup-scores">
              <label>Team One Score:</label>
              <input
                type="text"
                value={teamOneScore}
                onChange={(e) => setTeamOneScore(e.target.value)}
                className="score-input"
              />
            </div>
            <div className="matchup-scores">
              <label>Team Two Score:</label>
              <input
                type="text"
                value={teamTwoScore}
                onChange={(e) => setTeamTwoScore(e.target.value)}
                className="score-input"
              />
            </div>
            <button className="submit-score-btn" onClick={handleScoreSubmit}>
              Submit Score
            </button>
          </li>
          {/* More matchups can be listed here */}
        </ul>
      </div>
    </div>
  );
};

export default TournamentViewerForm;
