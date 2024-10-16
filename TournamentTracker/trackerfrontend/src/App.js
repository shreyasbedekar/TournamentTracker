import logo from './logo.svg';
import './App.css';
import React from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import PrizeForm from './Components/CreatePrizeForm';
import TeamForm from './Components/CreateTeamForm';
import TournamentForm from './Components/CreateTournamentForm';
import TournamentDashboardForm from './Components/TournamentDashboardForm';
import TournamentViewerForm from './Components/TouranmentViewerForm';

function App() {
  return (
    <Router>
        <Routes>
          <Route path="/create-prize" element={<PrizeForm/>} />
          <Route path="/create-team" element={<TeamForm/>} />
          <Route path="/create-tournament" element={<TournamentForm/>} />
          <Route path="/"  element={<TournamentDashboardForm/>} />
          <Route path="/tournament-viewer" element={<TournamentViewerForm/>} />
        </Routes>
    </Router>
  );
}

export default App;
