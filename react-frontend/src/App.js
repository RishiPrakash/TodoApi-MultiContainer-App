import React, { useState } from 'react';

const App = () => {
  const [dataInput, setDataInput] = useState('');
  const [typeInput, setTypeInput] = useState('');
  const [message, setMessage] = useState('');
  const [result, setResult] = useState('');

  const backendUrl = "http://localhost:8081";

  const fetchData = async () => {
    try {
      const response = await fetch(`${backendUrl}/get`);
      const data = await response.json();
      setResult('Data from backend: ' + JSON.stringify(data, null, 2));
    } catch (error) {
      console.error('Error fetching data:', error);
      setResult('');
    }
  };

  const addData = async () => {
    try {
      const response = await fetch(`${backendUrl}/add`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({ Name: dataInput, type: typeInput })
      });
      const result = await response.json();
      setMessage('Data added successfully.');
      console.log('Result from backend:', result);
    } catch (error) {
      console.error('Error adding data:', error);
      setMessage('');
    }
  };

  return (
    <div style={styles.container}>
      <h1>Frontend Layer</h1>
      
      <label htmlFor="dataInput">Enter Data:</label>
      <input
        type="text"
        id="dataInput"
        placeholder="Enter data to add"
        value={dataInput}
        onChange={(e) => setDataInput(e.target.value)}
        style={styles.input}
      />
      
      <label htmlFor="typeInput">Enter Type:</label>
      <input
        type="text"
        id="typeInput"
        placeholder="Enter type to categorize"
        value={typeInput}
        onChange={(e) => setTypeInput(e.target.value)}
        style={styles.input}
      />
      
      <button onClick={fetchData} style={styles.button}>Get Data</button>
      <button onClick={addData} style={styles.button}>Add Data</button>
      
      {message && <div style={styles.message}>{message}</div>}
      {result && <div style={styles.result}>{result}</div>}
    </div>
  );
};

const styles = {
  container: {
    fontFamily: 'Arial, sans-serif',
    backgroundColor: '#f5f5f5',
    margin: '0',
    padding: '20px',
    display: 'flex',
    justifyContent: 'center',
    alignItems: 'center',
    height: '100vh',
    flexDirection: 'column',
    backgroundColor: '#ffffff',
    borderRadius: '8px',
    boxShadow: '0px 0px 10px 0px rgba(0,0,0,0.1)',
    maxWidth: '400px',
    width: '100%',
    boxSizing: 'border-box'
  },
  input: {
    width: 'calc(100% - 10px)',
    padding: '8px',
    border: '1px solid #cccccc',
    borderRadius: '4px',
    marginBottom: '20px',
    boxSizing: 'border-box'
  },
  button: {
    padding: '10px 20px',
    backgroundColor: '#007bff',
    color: '#ffffff',
    border: 'none',
    borderRadius: '4px',
    cursor: 'pointer',
    transition: 'background-color 0.3s ease',
    marginTop: '10px'
  },
  message: {
    color: '#28a745',
    marginBottom: '10px'
  },
  result: {
    color: '#333333',
    marginBottom: '20px',
    whiteSpace: 'pre-wrap'
  }
};

export default App;