import React, { useState } from 'react';
import axios from 'axios';
import '../Css/CreatePrizeForm.css';

const CreatePrizeForm = () => {
  const [placeNumber, setPlaceNumber] = useState('');
  const [placeName, setPlaceName] = useState('');
  const [prizeAmount, setPrizeAmount] = useState('');
  const [prizePercentage, setPrizePercentage] = useState('');

  const handleSubmit = async (e) => {
    e.preventDefault();

    // Ensure that either prizeAmount or prizePercentage is provided, but not both
    if (!placeNumber || !placeName || (prizeAmount && prizePercentage)) {
      alert('Please fill in all required fields and ensure either Prize Amount or Prize Percentage is filled, but not both.');
      return;
    }

    const prizeData = { 
      placeNumber: parseInt(placeNumber, 10), 
      placeName, 
      prizeAmount: prizeAmount ? parseFloat(prizeAmount) : 0, 
      prizePercentage: prizePercentage ? parseFloat(prizePercentage) : 0 
    };

    console.log('Sending Prize Data:', prizeData);

    try {
      // Post to the backend API
      const response = await axios.post('https://localhost:7159/api/Prize', prizeData);

      console.log('Prize Data Submitted:', response.data);
      alert('Prize created successfully!');

      // Reset the form
      setPlaceNumber('');
      setPlaceName('');
      setPrizeAmount('');
      setPrizePercentage('');
    } catch (error) {
      console.error('Error submitting prize data:', error);
      alert('There was an error creating the prize.');
    }
  };

  return (
    <div className="prize-form-container">
      <h1 className="tracker-heading">Tournament Tracker</h1>
      <h2 className="form-heading">Create Prize</h2>
      <form onSubmit={handleSubmit} className="prize-form">
        <div className="form-group">
          <label>Place Number</label>
          <input 
            type="text" 
            value={placeNumber} 
            onChange={(e) => setPlaceNumber(e.target.value)} 
            placeholder="Enter place number" 
            className="form-input"
          />
        </div>
        <div className="form-group">
          <label>Place Name</label>
          <input 
            type="text" 
            value={placeName} 
            onChange={(e) => setPlaceName(e.target.value)} 
            placeholder="Enter place name" 
            className="form-input"
          />
        </div>
        <div className="form-group">
          <label>Prize Amount</label>
          <input 
            type="text" 
            value={prizeAmount} 
            onChange={(e) => setPrizeAmount(e.target.value)} 
            placeholder="Enter prize amount" 
            className="form-input"
          />
        </div>
        <div className="or-label">OR</div>
        <div className="form-group">
          <label>Prize Percentage</label>
          <input 
            type="text" 
            value={prizePercentage} 
            onChange={(e) => setPrizePercentage(e.target.value)} 
            placeholder="Enter prize percentage" 
            className="form-input"
          />
        </div>
        <button type="submit" className="submit-btn">Create Prize</button>
      </form>
    </div>
  );
};

export default CreatePrizeForm;
