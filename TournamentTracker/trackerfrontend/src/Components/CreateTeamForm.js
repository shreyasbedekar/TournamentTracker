import React, { useState, useEffect } from 'react';
import axios from 'axios';
import '../Css/CreateTeamForm.css';

const CreateTeamForm = () => {
  const [teamName, setTeamName] = useState('');
  const [availableMembers, setAvailableMembers] = useState([]);
  const [selectedMembers, setSelectedMembers] = useState([]);
  const [memberSelection, setMemberSelection] = useState('');
  const [newMember, setNewMember] = useState({ firstName: '', lastName: '', email: '', phone: '' });
  const [validationMessage, setValidationMessage] = useState(''); // For form validation

  useEffect(() => {
    axios.get('https://localhost:7159/api/People')
      .then(response => setAvailableMembers(Array.isArray(response.data) ? response.data : []))
      .catch(() => setAvailableMembers([]));
  }, []);

  const addMember = () => {
    const selectedMember = availableMembers.find(m => m.id === parseInt(memberSelection));
    if (selectedMember && !selectedMembers.includes(selectedMember)) {
      setSelectedMembers([...selectedMembers, selectedMember]);
      setAvailableMembers(availableMembers.filter(m => m.id !== selectedMember.id));
      setMemberSelection('');
    }
  };

  const removeMember = (id) => {
    const memberToRemove = selectedMembers.find(m => m.id === id);
    if (memberToRemove) {
      setSelectedMembers(selectedMembers.filter(m => m.id !== id));
      setAvailableMembers([...availableMembers, memberToRemove]);
    }
  };

  const handleNewMemberChange = (e) => {
    const { name, value } = e.target;
    setNewMember({ ...newMember, [name]: value });
  };

  const addNewMember = () => {
    if (!newMember.firstName || !newMember.lastName || !newMember.email || !newMember.phone) {
      setValidationMessage('Please fill in all fields for the new member.');
      return;
    }

    const newId = Math.max(...availableMembers.map(m => m.id), 0) + 1;
    const newMemberWithId = { id: newId, ...newMember, fullName: `${newMember.firstName} ${newMember.lastName}` };
    setAvailableMembers([...availableMembers, newMemberWithId]);
    setNewMember({ firstName: '', lastName: '', email: '', phone: '' });
    setValidationMessage(''); // Clear validation message
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (!teamName.trim()) {
      setValidationMessage('Please enter a team name.');
      return;
    }
    if (selectedMembers.length === 0) {
      setValidationMessage('Please select at least one team member.');
      return;
    }

    const teamData = {
      teamName,
      teamMembers: selectedMembers.map(m => ({
        firstName: m.firstName,
        lastName: m.lastName,
        emailAddress: m.email,
        cellphoneNumber: m.phone
      }))
    };

    try {
      await axios.post('https://localhost:7159/api/Teams', teamData);
      alert('Team created successfully!');
      setTeamName('');
      setSelectedMembers([]);
      setValidationMessage(''); // Clear validation message
    } catch (error) {
      alert('There was an error creating the team.');
    }
  };

  return (
    <div className="form-container">
      <h1>Create Team</h1>
      <form onSubmit={handleSubmit}>
        <div className="form-group">
          <label>Team Name</label>
          <input
            type="text"
            value={teamName}
            onChange={(e) => setTeamName(e.target.value)}
            placeholder="Enter team name"
            className="form-input"
          />
        </div>

        <div className="form-group">
          <label>Select Team Member</label>
          <div className="member-selection">
            <select
              value={memberSelection}
              onChange={(e) => setMemberSelection(e.target.value)}
              className="form-input"
            >
              <option value="">--Select a Member--</option>
              {availableMembers.map(member => (
                <option key={member.id} value={member.id}>
                  {member.firstName} {member.lastName}
                </option>
              ))}
            </select>
            <button type="button" onClick={addMember} className="btn-add">Add Member</button>
          </div>
        </div>

        <div className="selected-members">
          <h3>Selected Members</h3>
          {selectedMembers.length > 0 ? (
            <ul className="member-list">
              {selectedMembers.map(member => (
                <li key={member.id} className="member-item">
                  {member.fullName}
                  <button type="button" onClick={() => removeMember(member.id)} className="btn-remove">Remove</button>
                </li>
              ))}
            </ul>
          ) : (
            <p>No members selected yet.</p>
          )}
        </div>

        <div className="form-group new-member-section">
          <h3>Add New Member</h3>
          <input
            type="text"
            name="firstName"
            value={newMember.firstName}
            onChange={handleNewMemberChange}
            placeholder="First Name"
            className="form-input"
          />
          <input
            type="text"
            name="lastName"
            value={newMember.lastName}
            onChange={handleNewMemberChange}
            placeholder="Last Name"
            className="form-input"
          />
          <input
            type="email"
            name="email"
            value={newMember.email}
            onChange={handleNewMemberChange}
            placeholder="Email"
            className="form-input"
          />
          <input
            type="text"
            name="phone"
            value={newMember.phone}
            onChange={handleNewMemberChange}
            placeholder="Phone No."
            className="form-input"
          />
          <button type="button" onClick={addNewMember} className="btn-add">Create Member</button>
        </div>

        {validationMessage && <p className="validation-message">{validationMessage}</p>} {/* Validation message */}

        <button type="submit" className="btn-submit" disabled={!teamName || selectedMembers.length === 0}>Create Team</button>
      </form>
    </div>
  );
};

export default CreateTeamForm;
