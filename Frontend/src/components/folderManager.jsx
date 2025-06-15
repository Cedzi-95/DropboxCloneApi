// src/components/FolderManager.jsx
import React, { useState } from 'react';
import { useFolders } from '../hooks/useApi';


const FolderManager = ({ onFolderSelect }) => {
  const { folders, loading, error, createFolder, deleteFolder } = useFolders();
  const [showCreateForm, setShowCreateForm] = useState(false);
  const [newFolderName, setNewFolderName] = useState('');
  const [creating, setCreating] = useState(false);

  const handleCreateFolder = async (e) => {
    e.preventDefault();
    if (!newFolderName.trim()) return;

    try {
      setCreating(true);
      await createFolder({ name: newFolderName });
      setNewFolderName('');
      setShowCreateForm(false);
    } catch (err) {
      console.error('Failed to create folder:', err);
    } finally {
      setCreating(false);
    }
  };

  const handleDeleteFolder = async (id, name) => {
    if (window.confirm(`Are you sure you want to delete "${name}"?`)) {
      try {
        await deleteFolder(id);
      } catch (err) {
        console.error('Failed to delete folder:', err);
      }
    }
  };

  if (loading) return <div className="loading">Loading folders...</div>;
  if (error) return <div className="error">Error: {error}</div>;

  return (
    <div className="folder-manager">
      <div className="folder-header">
        <h2>My Folders</h2>
        <button 
          className="btn-primary"
          onClick={() => setShowCreateForm(true)}
        >
          Create Folder
        </button>
      </div>

      {showCreateForm && (
        <div className="create-form">
          <form onSubmit={handleCreateFolder}>
            <input
              type="text"
              value={newFolderName}
              onChange={(e) => setNewFolderName(e.target.value)}
              placeholder="Folder name"
              required
            />
            <button type="submit" disabled={creating}>
              {creating ? 'Creating...' : 'Create'}
            </button>
            <button 
              type="button" 
              onClick={() => {
                setShowCreateForm(false);
                setNewFolderName('');
              }}
            >
              Cancel
            </button>
          </form>
        </div>
      )}

      <div className="folders-grid">
        {/* FIXED: Added safety check to prevent the .map error */}
        {(folders && Array.isArray(folders) ? folders : []).map(folder => (
          <div 
            key={folder.id} 
            className="folder-card"
            onClick={() => onFolderSelect && onFolderSelect(folder)}
          >
            <div className="folder-icon">📁</div>
            <h3>{folder.name}</h3>
            <p>Files: {folder.fileCount}</p>
            <p>Created: {new Date(folder.createdAt).toLocaleDateString()}</p>
            <p>By: {folder.createdByUsername}</p>
            <div className="folder-actions">
              <button 
                className="btn-danger"
                onClick={(e) => {
                  e.stopPropagation(); // Prevent folder selection when clicking delete
                  handleDeleteFolder(folder.id, folder.name);
                }}
              >
                Delete
              </button>
            </div>
          </div>
        ))}
      </div>

      {(!folders || folders.length === 0) && !loading && (
        <div className="empty-state">
          <p>No folders yet. Create your first folder to get started!</p>
        </div>
      )}
    </div>
  );
};

export default FolderManager;