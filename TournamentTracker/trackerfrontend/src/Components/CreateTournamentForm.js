import React, { useState } from 'react';
import '../Css/CreateTournamentForm.css'; // Import CSS file

const CreateTournamentForm = () => {
  const [tournamentName, setTournamentName] = useState('');
  const [entryFee, setEntryFee] = useState('');
  const [teamCount, setTeamCount] = useState('');
  const [prize, setPrize] = useState('');
  const [prizes, setPrizes] = useState([]);

  // Handler for adding a prize
  const handleAddPrize = () => {
    if (!prize) {
      alert('Please select a prize.');
      return;
    }
    setPrizes([...prizes, prize]);
    setPrize('');
  };

  // Handler for removing a prize
  const handleRemovePrize = (index) => {
    setPrizes(prizes.filter((_, i) => i !== index));
  };

  // Handler for form submission
  const handleSubmit = (e) => {
    e.preventDefault();
    if (!tournamentName || !entryFee || !teamCount || prizes.length === 0) {
      alert('Please fill in all fields and add at least one prize.');
      return;
    }

    const tournamentData = {
      tournamentName,
      entryFee,
      teamCount,
      prizes,
    };

    console.log('Tournament Created:', tournamentData);

    // Reset form
    setTournamentName('');
    setEntryFee('');
    setTeamCount('');
    setPrizes([]);
  };

  return (
    <div className="tournament-form-container">
      <h1 className="tracker-heading">Tournament Tracker</h1>
      <h2 className="form-heading">Create Tournament</h2>
      <form onSubmit={handleSubmit} className="tournament-form">
        <div className="form-group">
          <label>Tournament Name</label>
          <input
            type="text"
            value={tournamentName}
            onChange={(e) => setTournamentName(e.target.value)}
            placeholder="Enter tournament name"
            className="form-input"
          />
        </div>

        <div className="form-group">
          <label>Entry Fee</label>
          <input
            type="number"
            value={entryFee}
            onChange={(e) => setEntryFee(e.target.value)}
            placeholder="Enter entry fee"
            className="form-input"
          />
        </div>

        <div className="form-group">
          <label>Number of Teams</label>
          <input
            type="number"
            value={teamCount}
            onChange={(e) => setTeamCount(e.target.value)}
            placeholder="Enter number of teams"
            className="form-input"
          />
        </div>

        <div className="form-group">
          <label>Add Prize</label>
          <input
            type="text"
            value={prize}
            onChange={(e) => setPrize(e.target.value)}
            placeholder="Enter prize"
            className="form-input"
          />
          <button type="button" className="add-prize-btn" onClick={handleAddPrize}>
            Add Prize
          </button>
        </div>

        <h3 className="subheading">Prizes</h3>
        {prizes.length > 0 ? (
          <ul className="prizes-list">
            {prizes.map((prize, index) => (
              <li key={index} className="prize-item">
                {prize}
                <button className="remove-prize-btn" onClick={() => handleRemovePrize(index)}>
                  Remove
                </button>
              </li>
            ))}
          </ul>
        ) : (
          <p className="no-prizes">No prizes added yet.</p>
        )}

        <button type="submit" className="submit-btn">
          Create Tournament
        </button>
      </form>
    </div>
  );
};

export default CreateTournamentForm;
