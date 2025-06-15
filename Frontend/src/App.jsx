// src/App.jsx
import React, { useState } from 'react';
import FolderManager from './components/folderManager';
import FileManager from './components/fileManager';
import ApiServices from './services/apiServices';
import './App.css';

function App() {
  const [selectedFolder, setSelectedFolder] = useState(null);
  const [authToken, setAuthToken] = useState(localStorage.getItem('authToken'));

  // Simple auth check - you'll need to implement proper authentication
  if (!authToken) {
    return (
      <div className="auth-container">
        <h1>Dropbox Clone</h1>
        <p>Please authenticate to continue</p>
        <button 
          onClick={() => {
            // This is just for demo - implement proper auth
            const token = prompt('Enter your auth token:');
            if (token) {
              setAuthToken(token);
              ApiService.setAuthToken(token);
              localStorage.setItem('authToken', token);
            }
          }}
        >
          Login
        </button>
      </div>
    );
  }

  // Set the token in apiService when component mounts
  if (authToken && !ApiServices.token) {
    ApiServices.setAuthToken(authToken);
  }

  const handleFolderSelect = (folder) => {
    setSelectedFolder(folder);
  };

  return (
    <div className="app">
      <header className="app-header">
        <h1>Dropbox Clone</h1>
        <button 
          className="btn-secondary"
          onClick={() => {
            setAuthToken(null);
            ApiServices.removeAuthToken();
            localStorage.removeItem('authToken');
          }}
        >
          Logout
        </button>
      </header>

      <div className="app-content">
        <div className="sidebar">
          <FolderManager onFolderSelect={handleFolderSelect} />
        </div>
        
        <div className="main-content">
          <FileManager 
            folderId={selectedFolder?.id} 
            folderName={selectedFolder?.name}
          />
        </div>
      </div>
    </div>
  );
}

export default App;