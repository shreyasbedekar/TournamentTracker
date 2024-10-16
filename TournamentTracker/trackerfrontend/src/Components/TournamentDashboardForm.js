import React, { useState } from 'react';
import '../Css/TouranmentDashboardForm.css'; // Import CSS file

const TournamentDashboardForm = () => {
  const [selectedTournament, setSelectedTournament] = useState('');

  // Handle loading the selected tournament
  const handleLoadTournament = () => {
    if (!selectedTournament) {
      alert('Please select a tournament to load.');
      return;
    }

    console.log('Loaded Tournament:', selectedTournament);
    // Logic for loading the tournament
  };

  // Handle creating a new tournament
  const handleCreateTournament = () => {
    console.log('Navigating to Create Tournament Form');
    // Logic for navigating to the create tournament form
  };

  return (
    <div className="dashboard-form-container">
      <h1 className="tracker-heading">Tournament Tracker</h1>
      <h2 className="form-heading">Tournament Dashboard</h2>

      <div className="form-group">
        <label>Load Existing Tournament</label>
        <select
          value={selectedTournament}
          onChange={(e) => setSelectedTournament(e.target.value)}
          className="form-input"
        >
          <option value="">Select Tournament</option>
          <option value="Tournament 1">Tournament 1</option>
          <option value="Tournament 2">Tournament 2</option>
          <option value="Tournament 3">Tournament 3</option>
        </select>
      </div>

      <button className="load-tournament-btn" onClick={handleLoadTournament}>
        Load Tournament
      </button>

      <div className="or-separator">OR</div>

      <button className="create-tournament-btn" onClick={handleCreateTournament}>
        Create New Tournament
      </button>
    </div>
  );
};

export default TournamentDashboardForm;
